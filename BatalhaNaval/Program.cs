using System;

namespace BatalhaNaval
{
    class Program
    {
        static void Main(string[] args)
        {
            BattleShip battleShip = new BattleShip();
            GameCommands commands = new GameCommands(battleShip);

            Console.WriteLine("=== BATALHA NAVAL ===");
            while (true)
            {
                Console.WriteLine("Digite um comando (ou 'sair' para encerrar):");
                Console.Write("> ");
                string input = Console.ReadLine();

                if (input.ToLower() == "sair") break;
                commands.ProcessCommand(input);
            }
        }
    }
}