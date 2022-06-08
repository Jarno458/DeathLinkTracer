using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;

namespace DeathLinkTracer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Server: ");
            var server = Console.ReadLine();

            Console.Write("Player: ");
            var player = Console.ReadLine();

            Console.Write("Game: ");
            var game = Console.ReadLine();

            var session = ArchipelagoSessionFactory.CreateSession(new Uri("ws://" + server));
            session.TryConnectAndLogin(game, player, new Version(0, 3, 0), ItemsHandlingFlags.NoItems, new[] { "IgnoreGame", "TextOnly" });

            PrintEnterCommand();

            while (true)
            {
                int slot;
                var command = Console.ReadLine();

                if (command == "failed" || command == "f")
                {
                    var failedDeathLinks = session.DataStorage["FailedDeathLinks"].To<string[]>();
                    Console.WriteLine("\"FailedDeathLinks\": [");
                    for (int i = 0; i < failedDeathLinks.Length; i++)
                        Console.WriteLine($"\t[{i}]: {failedDeathLinks[i]},");
                    Console.WriteLine("]");
                }
                else if ((command.StartsWith("slot ") && int.TryParse(command.Substring(5), out slot)) 
                      || (command.StartsWith("received ") && int.TryParse(command.Substring(9), out slot)))
                {
                    var received = session.DataStorage[$"Slot:{slot}:DeathLinkReceived"].To<string[]>();
                    if (received == null)
                    {
                        Console.WriteLine($"\"Slot:{slot}:DeathLinkReceived\": null");
                    }
                    else
                    {
                        Console.WriteLine($"\"Slot:{slot}:DeathLinkReceived\": [");
                        for (int i = 0; i < received.Length; i++)
                            Console.WriteLine($"[{i}]: {received[i]},");
                        Console.WriteLine("]");
                    }
                }
                else if (command.StartsWith("send ") && int.TryParse(command.Substring(5), out slot))
                {
                    var send = session.DataStorage[$"Slot:{slot}:DeathLinkSend"].To<string[]>();
                    if (send == null)
                    {
                        Console.WriteLine($"\"Slot:{slot}:DeathLinkSend\": null");
                    }
                    else
                    {
                        Console.WriteLine($"\"Slot:{slot}:DeathLinkSend\": [");
                        for (int i = 0; i < send.Length; i++)
                            Console.WriteLine($"[{i}]: {send[i]},");
                        Console.WriteLine("]");
                    }
                }

                PrintEnterCommand();
            }
        }

        static void PrintEnterCommand() => Console.WriteLine("Plz enter an command (failed, received #, send #)");
    }
}
