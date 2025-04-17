using System;
using System.Collections.Generic;

namespace BatalhaNaval
{
    public class BattleShip
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        private Player currentTurn;
        public Player CurrentPlayerPlacingShips { get; set; }
        public bool GameInProgress { get; set; }
        public bool CombatStarted { get; set; }
        public bool IsPlacingShips { get; set; }
        private int turn;
        public int CurrentPlayerShipIndex { get; set; }

        public BattleShip()
        {
            turn = 0;
            IsPlacingShips = false;
            currentTurn = null;
            GameInProgress = false;
            CombatStarted = false;
        }

        // Registro de Jogadores
        public string RegisterPlayer(string name)
        {
            if (Player1 == null)
            {
                Player1 = new Player(name);
                return $"Player {name} registrado com sucesso.";
            }
            else if (Player2 == null)
            {
                Player2 = new Player(name);
                return $"Player {name} registrado com sucesso.";
            }
            else
            {
                return "Limite de 2 jogadores atingido.";
            }
        }

        // Remover Jogador
        public string RemovePlayer(string name)
        {
            if (Player1 != null && Player1.Name == name)
            {
                Player1 = null;
                return $"Player {name} foi removido.";
            }
            else if (Player2 != null && Player2.Name == name)
            {
                Player2 = null;
                return $"Player {name} foi removido.";
            }
            else
            {
                return $"Player {name} não encontrado.";
            }
        }

        // Listar Jogadores
        public string ListPlayers()
        {
            if (Player1 == null && Player2 == null)
                return "Nenhum jogador registrado.";
            
            string result = "";
            if (Player1 != null)
                result += $"Player 1: {Player1.Name}\n";
            if (Player2 != null)
                result += $"Player 2: {Player2.Name}\n";
            
            return result;
        }

        // Iniciar Jogo
        public string StartGame(string name1, string name2)
        {
            if (Player1 == null || Player2 == null)
                return "Os jogadores devem ser registrados antes de iniciar o jogo.";

            Player1 = GetPlayerByName(name1);
            Player2 = GetPlayerByName(name2);

            if (Player1 == null || Player2 == null)
                return "Jogador não encontrado.";

            GameInProgress = true;
            IsPlacingShips = true;
            CurrentPlayerPlacingShips = Player1;
            CurrentPlayerShipIndex = 0;

            return "O jogo foi iniciado. Comece a posicionar seus navios.";
        }

        // Iniciar Combate
        public string StartCombat()
        {
            if (Player1 == null || Player2 == null)
                return "Os jogadores devem ser registrados antes de iniciar o jogo.";

            if (!GameInProgress)
                return "Nenhum jogo em andamento.";

            CombatStarted = true;
            IsPlacingShips = false;
            return "Combate iniciado!";
        }

        // Surrender
        public string Surrender(string playerName)
        {
            Player player = GetPlayerByName(playerName);
            if (player == null)
                return "Jogador não encontrado.";
            
            if (player == Player1)
                return $"{Player2.Name} venceu por rendição!";
            else
                return $"{Player1.Name} venceu por rendição!";
        }

        // Atirar
        public string Shoot(Player shooter, Player target, int row, int col)
        {
            if (row < 0 || row >= 10 || col < 0 || col >= 10)
                return "Coordenadas inválidas.";

            if (target.OwnBoard[row, col] != '~')
            {
                target.OwnBoard[row, col] = 'X'; // Mark hit
                return "Acerto!";
            }

            target.OwnBoard[row, col] = 'O'; // Mark miss
            return "Tiro na água!";
        }

        // Exibir Resultado
        public void DisplayResult()
        {
            if (Player1 == null || Player2 == null)
                Console.WriteLine("O jogo ainda não foi iniciado.");
            else
                Console.WriteLine($"Player 1: {Player1.Name} | Player 2: {Player2.Name}");
        }

        // Acessar Jogador por Nome
        public Player GetPlayerByName(string name)
        {
            if (Player1 != null && Player1.Name == name)
                return Player1;
            if (Player2 != null && Player2.Name == name)
                return Player2;
            return null;
        }

        // Método para validar o posicionamento do navio (excluindo sobreposição, limites, etc.)
        public string PlaceNextShip(string playerName, int startRow, int startCol, char direction = ' ')
        {
            if (!GameInProgress)
                return "O jogo ainda não foi iniciado.";

            Player player = GetPlayerByName(playerName);
            if (player == null)
                return "Jogador não encontrado.";

            if (CurrentPlayerPlacingShips != player)
                return $"Não é a vez de {playerName} posicionar os navios.";

            if (CurrentPlayerShipIndex >= player.Fleet.Count)
                return "Todos os navios já foram posicionados.";

            Ship ship = player.Fleet[CurrentPlayerShipIndex];

            if (ship.Quantity <= 0)
            {
                CurrentPlayerShipIndex++;
                return PlaceNextShip(playerName, startRow, startCol, direction); // Recursivamente tentar o próximo
            }

            // Validação de direção
            int dRow = 0, dCol = 0;
            if (ship.Size > 1)
            {
                switch (char.ToUpper(direction))
                {
                    case 'N': dRow = -1; break;
                    case 'S': dRow = 1; break;
                    case 'E': dCol = 1; break;
                    case 'O': dCol = -1; break;
                    default: return "Direção inválida. Use N, S, E ou O.";
                }
            }

            // Verificação de limites e sobreposição
            for (int i = 0; i < ship.Size; i++)
            {
                int row = startRow + i * dRow;
                int col = startCol + i * dCol;

                if (row < 0 || row >= 10 || col < 0 || col >= 10)
                    return "Navio ultrapassa os limites do tabuleiro.";

                if (player.OwnBoard[row, col] != '~')
                    return "Já existe um navio nessa posição.";
            }

            // Posicionamento do navio
            for (int i = 0; i < ship.Size; i++)
            {
                int row = startRow + i * dRow;
                int col = startCol + i * dCol;
                player.OwnBoard[row, col] = ship.Code;
            }

            ship.Quantity--;
            if (ship.Quantity <= 0)
                CurrentPlayerShipIndex++;

            // Verifica se todos os navios foram posicionados
            if (CurrentPlayerShipIndex >= player.Fleet.Count)
            {
                if (CurrentPlayerPlacingShips == Player1 && Player2 != null)
                {
                    CurrentPlayerPlacingShips = Player2;
                    CurrentPlayerShipIndex = 0;
                    Player2.Fleet = Ship.CreateFleet(); // Resetar frota para o segundo jogador
                    return $"{playerName} finalizou o posicionamento. Agora é a vez de {Player2.Name}.";
                }
                else
                {
                    IsPlacingShips = false;
                    CurrentPlayerPlacingShips = null;
                    return $"{playerName} finalizou o posicionamento. Todos os navios foram posicionados.";
                }
            }

            return $"Navio {ship.Name} posicionado com sucesso.";
        }
    }
}
