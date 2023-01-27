namespace Tetris
{
    internal class Heap
    {
        public int[,] Matrix => matrix;

        public event Action? FigureAdded;
        public event Action<int>? CompletedRows;

        public bool IsOverlap(Figure figure)
        {
            foreach (Point point in figure.Points)
            {
                if (point.Y >= 0 && point.X >= 0 && point.X < matrix.GetLength(1) && point.Y < matrix.GetLength(0))
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

            if (figure.LeftmostX < 0)
            {
                offsetPoint.X = figure.LeftmostX;
            }

            if (figure.RightmostX >= matrix.GetLength(1))
            {
                offsetPoint.X = matrix.GetLength(1) - 1 - figure.RightmostX;
            }

            if (figure.LowestY >= matrix.GetLength(0))
            {
                offsetPoint.Y = matrix.GetLength(0) - 1 - figure.LowestY;
            }

            return offsetPoint;
        }

        public void Add(Figure figure)
        {
            foreach (Point point in figure.Points)
            {
                if (point.X >= 0 && point.Y >= 0 && point.X < matrix.GetLength(1) && point.Y < matrix.GetLength(0))
                {
                    matrix[point.Y, point.X] = 1;
                }
            }
            FigureAdded?.Invoke();
        }

        public void CompleteRows()
        {
            // top row border to avoid recalculate entire 20x10 matrix
            int topRowBorder = 0;
            int[] completedRows = GetCompletedRows(out topRowBorder);
            if (completedRows.Length > 0)
            {
                CompleteRows(completedRows, topRowBorder);
                CompletedRows?.Invoke(completedRows.Length);
            }
        }

        private int[,] matrix = new int[20, 10];

        private int[] GetCompletedRows(out int topRowBorder)
        {
            bool rowCompleted = false;
            bool emptyRow = false;
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
