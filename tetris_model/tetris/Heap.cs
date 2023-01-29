using System;
using System.Collections.Generic;
using System.Numerics;

namespace Tetris
{
    internal class Heap
    {
        internal int[,] Matrix => matrix;

        internal event Action<int> CompletedRowsEvent;

        // x, y topleft offset
        private int[] offset = new int[2];

        internal bool IsOverlapOrOverBorder(int[,] points, int[,] extremePoints)
        {
            int[] offset = GetOverBorderOffset(extremePoints);
            bool isOverBorder = offset[0] != 0 || offset[1] != 0;
            return IsOverlap(points) || isOverBorder;
        }


        internal bool IsOverlap(int[,] points)
        {
            for(int i = 0; i < points.GetLength(0); i++)
            {
                if (points[i,0] >= 0 && points[i,1] >= 0 && points[i,0] < matrix.GetLength(1) && points[i,1] < matrix.GetLength(0))
                {
                    if (matrix[points[i,1], points[i,0]] != 0)
                        return true;
                }
            }

            return false;
        }


        internal int[] GetOverBorderOffset(int[,] extremePoints)
        {
            int offsetX = 0;
            int offsetY = 0;

            if (extremePoints[0,0] < 0)
            {
                offsetX = extremePoints[0, 0];
            }

            if (extremePoints[1,0] >= matrix.GetLength(1))
            {
                offsetX = matrix.GetLength(1) - 1 - extremePoints[1, 0];
            }

            if (extremePoints[1,1] >= matrix.GetLength(0))
            {
                offsetY = matrix.GetLength(0) - 1 - extremePoints[1, 1];
            }

            offset[0] = offsetX;
            offset[1] = offsetY;


            return offset;
        }


        internal void Add(int[,] points)
        {
            for(int i = 0; i < points.GetLength(0); i++)
            {
                if (points[i,0] >= 0 && points[i,1] >= 0 && points[i,0] < matrix.GetLength(1) && points[i,1] < matrix.GetLength(0))
                {
                    matrix[points[i, 1], points[i, 0]] = 1;
                }
            }

            CheckRows();
        }


        private int[,] matrix = new int[20, 10];


        private void CheckRows()
        {
            // top row border to avoid recalculate entire 20x10 matrix every time
            int[] completedRows = GetCompletedRows(out int topRowBorder);

            if (completedRows.Length > 0)
            {
                CompleteRows(completedRows, topRowBorder);
                CompletedRowsEvent?.Invoke(completedRows.Length);
            }
        }


        private int[] GetCompletedRows(out int topRowBorder)
        {
            bool rowCompleted;
            bool emptyRow;
            topRowBorder = 0;

            List<int> completedRows = new List<int>();

            for (int i = matrix.GetLength(0) - 1; i >= 0; i--)
            {
                emptyRow = true;
                rowCompleted = true;

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != 0)
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
                    topRowBorder = i;
                    break;
                }

                if (rowCompleted)
                {
                    completedRows.Add(i);
                }
            }

            return completedRows.ToArray();
        }


        private void CompleteRows(int[] rows, int topRow)
        {
            for (int i = 0; i < rows.Length; i++)
            {
                CompleteRow(rows[i] + i, topRow);
            }
        }


        private void CompleteRow(int row, int topRow)
        {
            for (int i = row; i >= topRow && i > 0; i--)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = matrix[i - 1, j];
                }
            }

            // if top row less than 2 we were very close to top line
            if(topRow < 2)
            {
                for (int i = 0; i < matrix.GetLength(1); i++)
                {
                    matrix[0, i] = 0;
                }
            }
        }
    }
}
