using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA
{
    internal class HPP
    {
        public int Width { get; }
        public int Height { get; }
        public bool isPBC { get; }

        public List<Site> Sites { get; set; }

        public HPP(int width, int height, bool isPBC)
        {
            Width = width;
            Height = height;
            this.isPBC = isPBC;
            Sites = new List<Site>();
        }

        public HPP(string path)
        {
            //TODO
        }

        public void Step()
        {
            Transport();
            Collision();
        }

        private void Collision()
        {
            foreach (Site s in Sites)
            {
                //reflection
                if (!isPBC)
                {
                    if (s.X > Width) s.Particles = "0010";
                    else if (s.Y > Height) s.Particles = "0001";
                    else if (s.X < 0) s.Particles = "1000";
                    else if (s.Y < 0) s.Particles = "0100";
                }

                //collision
                if (s.Particles == "1111") continue;
                if (s.Particles[0] == '1' && s.Particles[2] == '1') s.Particles = "0101";
                if (s.Particles[1] == '1' && s.Particles[3] == '1') s.Particles = "1010";
            }
        }

        private void Transport()
        {
            List<Site> newSites = new List<Site>();
            foreach (Site s in Sites)
            {
                if (s.Particles[0] == '1') newSites.Add(new Site(s.X + 1, s.Y, "1000"));
                if (s.Particles[1] == '1') newSites.Add(new Site(s.X, s.Y + 1, "0100"));
                if (s.Particles[2] == '1') newSites.Add(new Site(s.X - 1, s.Y, "0010"));
                if (s.Particles[3] == '1') newSites.Add(new Site(s.X, s.Y - 1, "0001"));
            }

            if (isPBC)
            {
                foreach (Site s in newSites)
                {
                    if (s.X > Width) s.X = 0;
                    if (s.Y > Height) s.Y = 0;
                    if (s.X < 0) s.X = Width;
                    if (s.Y < 0) s.Y = Height;
                }
            }

            Sites = MergeSites(newSites);
        }

        private List<Site> MergeSites(List<Site> sites)
        {
            for (int i = 0; i < sites.Count-1; i++)
            {
                string particles = sites[i].Particles;
                for (int j = i + 1; j < sites.Count; j++)
                {
                    if (sites[i].X == sites[j].X && sites[i].Y == sites[j].Y)
                    {
                        string newParticles = "";
                        for (int k = 0; k < 4; k++)
                        {
                            if (particles[k] == '0' && sites[j].Particles[k] == '0') newParticles += '0';
                            else newParticles += '1';
                        }
                        sites.RemoveAt(j);
                    }
                }
            }
            return sites;
        }
    }

    class Site
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Particles { get; set; }
        public Site(int x, int y, string particles = "0000")
        {
            X = x;
            Y = y;
            Particles = particles;
            /*
             * 0: (1; 0)  >
             * 1: (0; 1)  ʌ
             * 2: (-1; 0) <
             * 3: (0; -1) v
             */
        }
    }
}
