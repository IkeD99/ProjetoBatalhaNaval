using System;

namespace BatalhaNaval
{
    public class GameCommands
    {
        private BattleShip battleShip;

        public GameCommands(BattleShip battleShip)
        {
            this.battleShip = battleShip;
        }

        public void ProcessCommand(string command)
        {
            string[] args = command.Split(' ');

            switch (args[0].ToUpper())
            {
                case "RJ": // Registrar Jogador
                    if (args.Length < 2)
                        Console.WriteLine("Uso correto: RJ <nome>");
                    else
                        Console.WriteLine(battleShip.RegisterPlayer(args[1]));
                    break;

                case "EJ": // Remover Jogador
                    if (args.Length < 2)
                        Console.WriteLine("Uso correto: EJ <nome>");
                    else
                        Console.WriteLine(battleShip.RemovePlayer(args[1]));
                    break;

                case "LJ": // Listar Jogadores
                    Console.WriteLine(battleShip.ListPlayers());
                    break;

                case "IJ": // Iniciar Jogo
                    if (args.Length < 3)
                        Console.WriteLine("Uso correto: IJ <jogador1> <jogador2>");
                    else
                        Console.WriteLine(battleShip.StartGame(args[1], args[2]));
                    break;

                case "IC": // Iniciar Combate
                    Console.WriteLine(battleShip.StartCombat());
                    break;

                case "D": // Desistir
                    if (args.Length < 2)
                        Console.WriteLine("Uso correto: D <nome>");
                    else
                        Console.WriteLine(battleShip.Surrender(args[1]));
                    break;

                case "CN": // Colocar Navio
                    if (args.Length < 5)
                    {
                        Console.WriteLine("Uso correto: CN <nome> <código do navio> <linha> <coluna> [orientação]");
                        break;
                    }

                    string name = args[1];
                    string shipCodeStr = args[2];
                    string lineStr = args[3];
                    string columnStr = args[4];
                    string orientation = args.Length >= 6 ? args[5] : "";

                    if (!int.TryParse(lineStr, out int row))
                    {
                        Console.WriteLine("A linha deve ser um número válido.");
                        break;
                    }

                    int col = columnStr.ToUpper()[0] - 'A';

                    if (col < 0 || col >= 10)
                    {
                        Console.WriteLine("A coluna deve estar entre A e J.");
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(shipCodeStr) || shipCodeStr.Length != 1)
                    {
                        Console.WriteLine("Código do navio inválido.");
                        break;
                    }

                    char shipCode = char.ToUpper(shipCodeStr[0]);
                    char dir = orientation.Length > 0 ? char.ToUpper(orientation[0]) : ' ';

                    string result = battleShip.PlaceShip(name, shipCode, row - 1, col, dir);
                    Console.WriteLine(result);
                    break;

                case "VT": // Visualizar Tabuleiro
                    if (args.Length < 2) // Verifica se o nome do jogador não foi passado
                    {
                        Console.WriteLine("Uso correto: VT <nome>");
                    }
                    else
                    {
                        string playerName = args[1];
                        Player player = battleShip.GetPlayerByName(playerName);

                        if (player != null)
                        {
                            player.DisplayBoard(player.OwnBoard); // Exibe o tabuleiro do jogador
                        }
                        else
                        {
                            Console.WriteLine("Jogador não encontrado.");
                        }
                    }
                    break;

                case "T": // Disparar Tiro
                    if (args.Length < 4)
                        Console.WriteLine("Uso correto: T <nome> <linha> <coluna>");
                    else
                    {
                        int row, col;
                        if (!int.TryParse(args[2], out row) || !int.TryParse(args[3], out col))
                        {
                            Console.WriteLine("As coordenadas devem ser números válidos.");
                        }
                        else
                        {
                            Player shooter = battleShip.GetPlayerByName(args[1]);
                            if (shooter != null)
                            {
                                Player target = shooter == battleShip.Player1 ? battleShip.Player2 : battleShip.Player1;
                                Console.WriteLine(battleShip.Shoot(shooter, target, row - 1, col - 1));
                            }
                            else
                            {
                                Console.WriteLine("Jogador não encontrado.");
                            }
                        }
                    }
                    break;

                case "V": // Visualizar Resultados
                    battleShip.DisplayResult();
                    break;

                default:
                    Console.WriteLine("Comando inválido.");
                    break;
            }
        }
    }
}
