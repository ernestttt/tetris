using static Tetris.Figure;

namespace Tetris
{
    internal class Heap
    {
        public int[,] Matrix => matrix;

        public event Action<int>? CompletedRowsEvent;


        public bool IsOverlapOrOverBorder(Point[] points, ExtremePoints extremePoints)
        {
            Point offsetPoint = GetOverBorderOffset(extremePoints);
            bool isOverBorder = offsetPoint.X != 0 || offsetPoint.Y != 0;
            return IsOverlap(points) || isOverBorder;
        }


        public bool IsOverlap(Point[] points)
        {
            foreach (Point point in points)
            {
                if (point.Y >= 0 && point.X >= 0 && point.X < matrix.GetLength(1) && point.Y < matrix.GetLength(0))
                {
                    if (matrix[point.Y, point.X] != 0)
                        return true;
                }
            }

            return false;
        }


        public Point GetOverBorderOffset(ExtremePoints extremePoints)
        {
            Point offsetPoint = new Point();

            if (extremePoints.LeftmostX < 0)
            {
                offsetPoint.X = extremePoints.LeftmostX;
            }

            if (extremePoints.RightmostX >= matrix.GetLength(1))
            {
                offsetPoint.X = matrix.GetLength(1) - 1 - extremePoints.RightmostX;
            }

            if (extremePoints.LowestY >= matrix.GetLength(0))
            {
                offsetPoint.Y = matrix.GetLength(0) - 1 - extremePoints.LowestY;
            }

            return offsetPoint;
        }


        public void Add(Point[] points)
        {
            foreach (Point point in points)
            {
                if (point.X >= 0 && point.Y >= 0 && point.X < matrix.GetLength(1) && point.Y < matrix.GetLength(0))
                {
                    matrix[point.Y, point.X] = 1;
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
