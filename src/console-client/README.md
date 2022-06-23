# Console Client

A secure and discrete terminal based client that imitates old-world console based UI where users were keyboard focused (TAB key, Function keys, etc.).

To secure communications, avoiding MITM attacks or sniffing, it establishes an SSH Tunnel. Comms to/from the server, gets forwarded through there, remaining obfuscated to the common eye.
> SSH.NET Library: https://github.com/sshnet/SSH.NET/

For rendering the UI inside a Console, getting the look & feel we're after while keeping a modern and extensible backend, we're using a cross platform UI library.
> Terminal.Gui Library: https://github.com/migueldeicaza/gui.cs

## A note on securing comms

Initially, SSH'ing comms might seem far fetched. These days, it'd be easy to 'hack' into most SMB locations, by connecting to their local network, and sniff around. Given that most common retails spare too little in IT complexity & security.

It requires just an intentioned person, not a 'rocket scientist'. We're proposing a no-brainer, out-of-the-box solution that doesn't get in the way. On one end, the 'server' is SSH capable, and on the other, we're proposing a client that 'tunnels' its way up, in order to secure back and forth comms.
