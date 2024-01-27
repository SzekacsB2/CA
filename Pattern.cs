using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA
{
    internal class Pattern
    {
        public HashSet<Choord> AliveCells { get; set; }
        private Dictionary<Choord, int> Grid { get; set; }
        public Pattern()
        {
            AliveCells = new HashSet<Choord>();
            Grid = new Dictionary<Choord, int>();
        }

        public Pattern(string path)
        {
            AliveCells = new HashSet<Choord>();
            Grid = new Dictionary<Choord, int>();
            StreamReader sr = new StreamReader(path);
            while (!sr.EndOfStream)
            {
                string[] data = sr.ReadLine().Split(' ');
                AliveCells.Add(new Choord(Convert.ToInt32(data[0]), Convert.ToInt32(data[1])));
            }
            sr.Close();
        }

        public void Update()
        {
            foreach (Choord cell in AliveCells)
            {
                CountNeighbours(cell);
            }
            foreach (Choord choord in Grid.Keys)
            {
                int count = Grid[choord];
                if (AliveCells.Contains(choord))
                {
                    if (count < 2 || count > 3)
                    {
                        AliveCells.Remove(choord);
                    }
                }
                else if (count == 3)
                {
                    AliveCells.Add(choord);
                }
            }
            Grid.Clear();
        }

        private void CountNeighbours(Choord cell)
        {
            (int x, int y)[] neighbourhood =
            {
                (-1, -1),
                (-1, 0),
                (-1, 1),
                (0, 1),
                (1, 1),
                (1, 0),
                (1, -1),
                (0, -1)
            };
            foreach (var neighbour in neighbourhood)
            {
                Choord choord = new Choord(cell.X + neighbour.x, cell.Y + neighbour.y);
                if (Grid.ContainsKey(choord))
                {
                    Grid[choord] += 1;
                }
                else
                {
                    Grid.Add(choord, 1);
                }
            }

        }
    }

    class Choord
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Choord(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
