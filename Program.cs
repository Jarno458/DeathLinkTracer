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
            Console.WriteLine();

            Console.Write("Player: ");
            var player = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Game: ");
            var game = Console.ReadLine();
            Console.WriteLine();


            var session = ArchipelagoSessionFactory.CreateSession(new Uri("ws://" + server));
            session.TryConnectAndLogin(game, player, new Version(0, 3, 0), ItemsHandlingFlags.NoItems, new[] { "IgnoreGame", "TextOnly" });

            PrintEnterCommand();

            while (true)
            {
                var command = Console.ReadLine();

                if (command == "failed")
                {
                    var failedDeathLinks = session.DataStorage["FailedDeathLinks"].To<string[]>();
                    Console.WriteLine("\"FailedDeathLinks\": [");
                    for (int i = 0; i < failedDeathLinks.Length; i++)
                        Console.WriteLine($"\t[{i}]: {failedDeathLinks[i]},");
                    Console.WriteLine("]");
                }
                else if (command.StartsWith("slot ") && int.TryParse(command.Substring(5), out var slot))
                {
                    var received = session.DataStorage[$"Slot:{slot}:DeathLinkReceived"].To<string[]>();
                    Console.WriteLine($"\"Slot:{slot}:DeathLinkReceived\": [");
                    for (int i = 0; i < received.Length; i++)
                        Console.WriteLine($"[{i}]: {received[i]},");
                    Console.WriteLine("]");
                }
                else if (command.StartsWith("send ") && int.TryParse(command.Substring(5), out var slot2))
                {
                    var send = session.DataStorage[$"Slot:{slot2}:DeathLinkSend"].To<string[]>();
                    Console.WriteLine($"\"Slot:{slot2}:DeathLinkSend\": [");
                    for (int i = 0; i < send.Length; i++)
                        Console.WriteLine($"[{i}]: {send[i]},");
                    Console.WriteLine("]");
                }

                PrintEnterCommand();
            }
        }

        static void PrintEnterCommand() => Console.WriteLine("Plz enter an command (f, slot #, send #)");
    }
}
