using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Heap
    {
        private int[,] matrix = new int[20,10]; 

        public int[,] Matrix => matrix;

        public bool IsOverlap(Figure figure)
        {
            foreach (Point point in figure.Points)
            {
                if (point.Y >= 0 && point.X >=0 && point.X < matrix.GetLength(1) && point.Y < matrix.GetLength(0))
                {
                    if (matrix[point.Y, point.X] != 0)
                        return true;
                }
            }

            return false;
        }

        public bool IsOverlapOrOverBorder(Figure figure) 
        { 
            Point offsetPoint = GetOverBorderOffset(figure);
            bool isOverBorder = offsetPoint.X != 0 || offsetPoint.Y != 0;
            return IsOverlap(figure) || isOverBorder;
        }

        public Point GetOverBorderOffset(Figure figure)
        {
            Point offsetPoint = new Point();

            Point leftmostPoint = figure.Points.OrderBy(a => a.X).First();
            Point lowestPoint = figure.Points.OrderBy(a => a.Y).Last(); ;
            Point rightmostPoint = figure.Points.OrderBy(a => a.X).Last();
            
            if(leftmostPoint.X < 0)
            {
                offsetPoint.X = leftmostPoint.X;
            }

            if (rightmostPoint.X >= matrix.GetLength(1))
            {
                offsetPoint.X = matrix.GetLength(1) - 1 - rightmostPoint.X;
            }

            if(lowestPoint.Y >= matrix.GetLength(0))
            {
                offsetPoint.Y = matrix.GetLength(0) - 1 - lowestPoint.Y;
            }


            return offsetPoint;
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
