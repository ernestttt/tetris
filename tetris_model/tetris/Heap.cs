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

        public event Action FigureAdded;
        public event Action<int> CompletedRows;

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
            FigureAdded?.Invoke();
        }

        public void CompleteRows()
        {
            int[] completedRows = GetCompletedRows();
            if(completedRows.Length > 0)
            {
                CompletedRows?.Invoke(completedRows.Length);
            }
            CompleteRows(completedRows);
        }

        public int[] GetCompletedRows()
        {
            bool rowCompleted = false;
            bool emptyRow = false;

            List<int> completedRows = new List<int>();

            for(int i = matrix.GetLength(0) - 1; i >= 0; i--)
            {
                emptyRow = true;
                rowCompleted = true;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i,j] != 0)
                    {
                        emptyRow = false;
                    }
                    else
                    {
                        rowCompleted = false;
                    }
                }

                if (emptyRow)
                {
                    break;
                }

                if(rowCompleted)
                {
                    completedRows.Add(i);
                }
            }

            return completedRows.ToArray();
        }

        public void CompleteRows(int[] rows)
        {
            for(int i = 0; i < rows.Length; i++)
            {
                CompleteRow(rows[i] - i);
            }
        }

        public void CompleteRow(int row)
        {
            for(int i = row; i > 0; i--)
            {
                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i,j] = matrix[i-1,j];
                }
            }

            for(int i = 0; i < matrix.GetLength(1); i++)
            {
                matrix[0, i] = 0;
            }
        }
    }
}
