namespace Tetris
{
    public class Field
    {
        public int CompletedRows { get; private set; } = 0;
        public byte[,] ViewMatrix { get; init; }
        public event Action? GameOverEvent;

        public Field()
        {
            ViewMatrix = new byte[20, 10];
            figure = new Figure(heap);
            heap.FigureAdded += CheckRows;
            figure.GameOver += GameOver;
            heap.CompletedRows += UpdateScore;
        }

        public void MoveDown()
        {
            figure.Move(MovementType.Down);
            UpdateViewMatrix();
        }

        public void MoveLeft()
        {
            figure.Move(MovementType.Left);
            UpdateViewMatrix();
        }

        public void MoveRight()
        {
            figure.Move(MovementType.Right);
            UpdateViewMatrix();
        }

        public void Rotate()
        {
            figure.Rotate();
        }

        private Figure figure;
        private Heap heap = new Heap();

        private void UpdateScore(int rows)
        {
            CompletedRows += rows;
        }

        private void CheckRows()
        {
            heap.CompleteRows();
        }

        private void GameOver()
        {
            GameOverEvent?.Invoke();
        }

        private void ClearViewMatrix()
        {
            for (int i = 0; i < ViewMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < ViewMatrix.GetLength(1); j++)
                {
                    ViewMatrix[i, j] = 0;
                }
            }
        }

        private void UpdateViewMatrix()
        {
            ClearViewMatrix();
            // figure
            foreach (Point point in figure.Points)
            {
                if (point.X >= 0 && point.X < ViewMatrix.GetLength(1) && point.Y >= 0 && point.Y < ViewMatrix.GetLength(0))
                {
                    ViewMatrix[point.Y, point.X] = 1;
                }
            }

            // heap
            for (int i = 0; i < ViewMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < ViewMatrix.GetLength(1); j++)
                {
                    if (heap.Matrix[i,j] != 0)
                    {
                        ViewMatrix[i,j] = 1;
                    }
                }
            }
        }
    }
}
