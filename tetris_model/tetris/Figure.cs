using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Figure
    {
        private Random random = new Random();
        private int[] pos = new int[2] { 0, 0 };
        private int[] offset = new int[2] { 0, 0 };
        private int[] innerSize = new int[2] { 0, 0 };

        private int[,] matrix;

        private int rotation;
        private TetrisFigures.TetrisFigure figureType;

        public int[,] Matrix => matrix;
        public int[] Pos => pos;

        public void Spawn()
        {
            figureType = (TetrisFigures.TetrisFigure)random.Next(7);
            rotation = random.Next(4);
            matrix = TetrisFigures.GetFigure(figureType, rotation);

            CalculateInnerSizeAndOffset();

            pos[0] = random.Next(-offset[0], 10 - innerSize[0] - offset[0]);
            pos[1] = 4 - innerSize[1] - offset[1];
        }

        public void Move(MovementType movement)
        {
            if(MovementType.Down == movement)
            {
                pos[1]++;
            }
        }

        private void CalculateInnerSizeAndOffset()
        {
            int startVerticalIndex = int.MaxValue;
            int endVerticalIndex = int.MinValue;
            int startHorizontalIndex = int.MaxValue;
            int endHorizontalIndex = int.MinValue;

            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        continue;
                    }

                    if(startVerticalIndex > i)
                    {
                        startVerticalIndex = i;
                    }
                    if(endVerticalIndex < i) 
                    { 
                        endVerticalIndex = i;
                    }
                    if(startHorizontalIndex > j)
                    {
                        startHorizontalIndex = j;
                    }
                    if(endHorizontalIndex < j)
                    {
                        endHorizontalIndex = j;
                    }
                }
            }

            innerSize[0] = endHorizontalIndex - startHorizontalIndex + 1;
            innerSize[1] = endVerticalIndex - startVerticalIndex + 1;

            offset[0] = startHorizontalIndex;
            offset[1] = startVerticalIndex;
        }
    }
}
