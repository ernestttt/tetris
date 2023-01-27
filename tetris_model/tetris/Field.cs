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

            figure.CantMoveOnSpawnEvent += () => GameOverEvent?.Invoke();
            heap.CompletedRowsEvent += (rows) => CompletedRows += rows;
        }


        public void MoveDown()
        {
            Move(MovementType.Down);
        }


        public void MoveLeft()
        {
            Move(MovementType.Left);
        }

        public void MoveRight()
        {
            Move(MovementType.Right);
        }


        public void Rotate()
        {
            figure.Rotate();
        }


        private readonly Figure figure;

        private readonly Heap heap = new();


        private void Move(MovementType movementType)
        {
            figure.Move(movementType);
            UpdateViewMatrix();
        }


        private void UpdateViewMatrix()
        {
            ClearViewMatrix();
            AddFigureToMatrix();
            AddHeapToViewMatrix();
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


        private void AddFigureToMatrix()
        {
            if (!figure.IsEmpty)
            {
                foreach (Point point in figure.Points)
                {
                    if (point.X >= 0 && point.X < ViewMatrix.GetLength(1) && point.Y >= 0 && point.Y < ViewMatrix.GetLength(0))
                    {
                        ViewMatrix[point.Y, point.X] = 1;
                    }
                }
            }
        }


        private void AddHeapToViewMatrix()
        {
            for (int i = 0; i < ViewMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < ViewMatrix.GetLength(1); j++)
                {
                    if (heap.Matrix[i, j] != 0)
                    {
                        ViewMatrix[i, j] = 1;
                    }
                }
            }
        }
    }
}
