import { Client } from 'discord.js';
const client = new Client();
const prefix = '/'; // change prefix here

client.once('ready', () => {
    console.log('Ticket Bot is online!');
});

client.on('message', async message => {
    if (!message.guild || message.author.bot) return;

    const args = message.content.slice(prefix.length).trim().split(/ +/);
    const command = args.shift().toLowerCase();

    if (command === 'ticket') {
        // limits to one channel
        if (message.guild.channels.cache.some(channel => channel.name === `ticket-${message.author.id}`)) {
            return message.channel.send('You already have a ticket open!');
        }

        // makes ticket channel
        const ticketChannel = await message.guild.channels.create(`ticket-${message.author.id}`, {
            type: 'text',
            permissionOverwrites: [
                {
                    id: message.author.id,
                    allow: ['SEND_MESSAGES', 'VIEW_CHANNEL']
                },
                {
                    id: message.guild.roles.everyone,
                    deny: ['VIEW_CHANNEL']
                }
            ]
        });

        // pings the person for the ticket
        message.channel.send(`Your ticket has been created! Click here to access: ${ticketChannel}`);
    }

    if (command === 'close') {
        // people with adminstrator/manage channels to check for permissionms
        if (!message.member.hasPermission('MANAGE_CHANNELS')) {
            return message.channel.send('You do not have permission to use this command!');
        }

        // delete command
        const ticketChannel = message.guild.channels.cache.find(channel => channel.name === `ticket-${message.author.id}`);
        if (!ticketChannel) return message.channel.send('Ticket channel not found!');

        ticketChannel.delete();
    }

    if (command === 'solved') {
        // permissions change if you want
        if (!message.member.hasPermission('MANAGE_CHANNELS')) {
            return message.channel.send('You do not have permission to use this command!');
        }

        // marks ticket as solved
        const ticketChannel = message.guild.channels.cache.find(channel => channel.name === `ticket-${message.author.id}`);
        if (!ticketChannel) return message.channel.send('Ticket channel not found!');

        
    }

    if (command === 'claim') {
        // claim command
        const ticketChannel = message.guild.channels.cache.find(channel => channel.name === `ticket-${message.author.id}`);
        if (!ticketChannel) return message.channel.send('Ticket channel not found!');

        // Your logic for claiming the ticket
    }
});

client.login('token here');
// change bot token 