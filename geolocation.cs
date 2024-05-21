using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IPGeolocator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter IP address to locate:");
            string ipAddress = Console.ReadLine();

            // Make API request
            string apiUrl = "http://ip-api.com/json/" + ipAddress + "?fields=country,city,lat,lon,isp,query,zip,countryCode,proxy";
            string responseJson;

            using (HttpClient client = new HttpClient())
            {
                responseJson = await client.GetStringAsync(apiUrl);
            }

            // response
            if (!string.IsNullOrEmpty(responseJson))
            {
                IpApiResponse response = JsonConvert.DeserializeObject<IpApiResponse>(responseJson);

                // Display results
                Console.WriteLine("Country: " + response.country);
                Console.WriteLine("Country Code: " + response.countryCode);
                Console.WriteLine("City: " + response.city);
                Console.WriteLine("ZIP Code: " + response.zip);
                Console.WriteLine("Latitude: " + response.lat);
                Console.WriteLine("Longitude: " + response.lon);
                Console.WriteLine("ISP: " + response.isp);
                Console.WriteLine("IP Version: " + response.ipVersion);
                Console.WriteLine("IP Address: " + response.query);
                Console.WriteLine("VPN: " + response.isVPN);
            }
            else
            {
                Console.WriteLine("Failed to retrieve geolocation data.");
            }

            Console.ReadLine();
        }
    }

    public class IpApiResponse
    {
        public string country { get; set; } = "";
        public string countryCode { get; set; } = "";
        public string city { get; set; } = "";
        public string zip { get; set; } = "";
        public double lat { get; set; }
        public double lon { get; set; }
        public string isp { get; set; } = "";
        public string ipVersion
        {
            get
            {
                if (query.Contains(":"))
                {
                    return "IPv6";
                }
                else
                {
                    return "IPv4";
                }
            }
        }
        public string query { get; set; } = "";
        public string proxy { get; set; } = "";

        // Property to determine if IP address belongs to a VPN
        public string isVPN
        {
            get
            {
                return proxy.ToLower() == "yes" || proxy.ToLower() == "true" ? "Yes" : "No";
            }
        }
    }
}
//goelocation tool, compile it yourself if you wish