using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Heap
    {
        private int[,] matrix = new int[20,10]; 

        public int[,] Matrix => matrix;

        public bool IsOverlap(Figure figure, MovementType movementType)
        {
            foreach (Point point in figure.Points)
            {
                if (MovementType.Down == movementType && point.Y >= matrix.GetLength(0))
                {
                    return true;
                }
                if (MovementType.Left == movementType && point.X < 0)
                {
                    return true;
                }
                if (MovementType.Right == movementType && point.X >= matrix.GetLength(1))
                {
                    return true;
                }
                if (point.Y >= 0 && point.X >=0 && point.X < matrix.GetLength(1))
                {
                    if (matrix[point.Y, point.X] != 0)
                        return true;
                }
            }

            return false;
        }

        public void Add(Figure figure)
        {
            foreach (Point point in figure.Points)
            {
                if (point.X >= 0 && point.X < matrix.GetLength(1) && point.Y >= 0 && point.Y < matrix.GetLength(0))
                {
                    matrix[point.Y, point.X] = 1;
                }
            }
        }
    }
}
