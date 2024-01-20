using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO

namespace CA
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class Pattern
    {
        public Dictionary<Choord, int> AliveCells { get; set; }
        private Dictionary<Choord, int> Grid { get; set; }
        public Pattern()
        {
            AliveCells = new Dictionary<Choord, int>();
            Grid = new Dictionary<Choord, int>();
        }

        public Pattern(string path)
        {
            AliveCells = new Dictionary<Choord, int>();
            Grid = new Dictionary<Choord, int>();
            StreamReader sr = new StreamReader(path);
            while(!sr.EndOfStream)
            {
                string[] data = sr.ReadLine().Split(' ');
                AliveCells.Add(new Choord(Convert.ToInt32(data[0]), Convert.ToInt32(data[1])), 0);
            }
            sr.Close();
        }

        public void Update()
        {
            foreach (Choord cell in  AliveCells.Keys)
            {
                CountNeighbours(cell);
            }
            foreach (Choord choord in Grid.Keys)
            {
                int count = Grid[choord];
                if (AliveCells.ContainsKey(choord))
                {
                    if (count < 2 || count > 3)
                    {
                        AliveCells.Remove(choord);
                    }
                }
                else if(count == 3)
                {
                    AliveCells.Add(choord, 0);
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
                Choord choord = new Choord( cell.X + neighbour.x, cell.Y + neighbour.y);
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

    struct Choord
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
