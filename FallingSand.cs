using System;

namespace CA
{
    internal class FallingSand
    {
        public int RowSize { get; set; }
        public int ColSize { get; set; }
        public int[,] Grid { get; set; }

        public double Gravity { get; set; }
        public double VelocityTransfer { get; set; }
        public bool Reactive { get; set; }

        public FallingSand(int rowSize, int colSize) 
        {
            RowSize = rowSize;
            ColSize = colSize;
            Grid = new int[rowSize, colSize];
            Gravity = 1;
            VelocityTransfer = 1;
            Reactive = false;
        }

        public FallingSand(int rowSize, int colSize, double gravity, double velocityTransfer, bool reactive)
        {
            RowSize = rowSize;
            ColSize = colSize;
            Grid = new int[rowSize, colSize];
            Gravity = gravity;
            VelocityTransfer = velocityTransfer;
            Reactive = reactive;
        }
        
        public void Tick()
        {
            int[,] newGrid = new int[RowSize, ColSize];
            for (int i = 0; i < RowSize; i++)
            {
                for (int j = 0; j < ColSize; j++)
                {
                    if (Grid[i, j] != 0)
                    {
                        double velocity = 1;
                        int newRow = i;
                        int newCol = j;
                        if (i < RowSize - 1 && Grid[i + 1, j] == 0)
                        {
                            velocity = Fall(ref newRow, j);
                        }
                        velocity = Slide(newRow, ref newCol, velocity);
                        while(Reactive &&  velocity != 0)
                        {
                            if (i < RowSize - 1 && Grid[i + 1, j] == 0)
                            {
                                velocity = Fall(ref newRow, j);
                            }
                            velocity = Slide(newRow, ref newCol, velocity);
                        }
                    }
                }
            }
        }

        private double Fall(ref int row, int col)
        {
            double velocity = Gravity;
            while (row < RowSize - 1 && velocity >= 1 && Grid[row + 1, col] == 0)
            {
                row++;
                velocity--;
            }
            return velocity * VelocityTransfer;
        }

        private double Slide(int row, ref int col, double velocity)
        {
            bool left = false;
            bool right = false;
            int direction = 0;
            if (col > 0 && Grid[row, col - 1] == 0) left = true;
            if (col < ColSize - 1 && Grid[row, col + 1] == 0) right = true;

            if (left && right)
            {
                Random r = new Random();
                direction = r.NextDouble() > 0.5 ? -1 : 1;
            }
            if ((left && direction == 0) || direction == -1)
            {
                while (velocity > 0 && col > 0 && Grid[row, col - 1] == 0 && (row ==  RowSize - 1 || Grid[row + 1, col] != 0))
                {
                    col--;
                    velocity--;
                }
                return velocity * VelocityTransfer;
            }
            if (right)
            {
                while (velocity > 0 && col < ColSize - 1 && Grid[row, col + 1] == 0 && (row == RowSize - 1 || Grid[row + 1, col] != 0))
                {
                    col++;
                    velocity--;
                }
                return velocity * VelocityTransfer;
            }

            return 0;
        }
    }
}
