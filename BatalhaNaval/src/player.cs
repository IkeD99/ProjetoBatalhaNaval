using System;
using System.Collections.Generic;
using System.Linq;

namespace BatalhaNaval
{
    public class Player
    {
        public string Name { get; set; }
        public char[,] OwnBoard { get; set; }
        public char[,] EnemyBoard { get; set; }
        public List<Ship> Fleet { get; set; }

        public Player(string name)
        {
            Name = name;
            OwnBoard = InitializeBoard();
            EnemyBoard = InitializeBoard();
            Fleet = Ship.CreateFleet();
        }

        private char[,] InitializeBoard()
        {
            char[,] board = new char[10, 10];
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    board[i, j] = '~';  // '~' representa água
            return board;
        }

        public void DisplayBoard(char[,] board)
        {
            Console.WriteLine("  A B C D E F G H I J");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((i + 1).ToString().PadLeft(2) + " ");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public Ship GetShipByCode(char code)
        {
            return Fleet.FirstOrDefault(s => s.Code == code);
        }
    }
}
