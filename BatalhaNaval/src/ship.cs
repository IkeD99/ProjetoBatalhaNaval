using System;
using System.Collections.Generic;

namespace BatalhaNaval
{
    public class Ship
    {
        public string Name { get; set; }
        public char Code { get; set; }
        public int Size { get; set; }
        public int Quantity { get; set; }

        public Ship(string name, char code, int size, int quantity)
        {
            Name = name;
            Code = code;
            Size = size;
            Quantity = quantity;
        }

        public static List<Ship> CreateFleet()
        {
            return new List<Ship>
            {
                new Ship("Lancha", 'L', 1, 4),         // 4 lanchas, tamanho 1
                new Ship("Submarino", 'S', 2, 3),      // 3 submarinos, tamanho 2
                new Ship("Fragata", 'F', 3, 2),        // 2 fragatas, tamanho 3
                new Ship("Cruzador", 'C', 4, 1),       // 1 cruzador, tamanho 4
                new Ship("Porta-Aviões", 'P', 5, 1)    // 1 porta-aviões, tamanho 5
            };
        }
    }
}
