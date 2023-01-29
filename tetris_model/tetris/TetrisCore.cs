using System;
using System.Drawing;

namespace Tetris
{
    /// <summary>
    /// Main class with tetris logic
    /// </summary>
    public class TetrisCore
    {
        /// <summary>
        /// Number of completed rows
        /// </summary>
        public int CompletedRows { get; private set; }

        /// <summary>
        /// 20x10 tetrix matrix
        /// </summary>
        public byte[,] ViewMatrix { get; private set; }

        /// <summary>
        /// Invoked when figure can't move down after spawn
        /// </summary>
        public event Action GameOverEvent;


        public TetrisCore()
        {
            ViewMatrix = new byte[20, 10];
            figure = new Figure();
            figure.Spawn();
            heap = new Heap();
            figure.CantMoveOnSpawnEvent += () => GameOverEvent?.Invoke();
            heap.CompletedRowsEvent += (rows) => CompletedRows += rows;
        }

        /// <summary>
        /// Move figure down
        /// </summary>
        public void MoveDown()
        {
            Move(MovementType.Down);
        }

        /// <summary>
        /// Move figure to left side
        /// </summary>
        public void MoveLeft()
        {
            Move(MovementType.Left);
        }


        /// <summary>
        /// Move figure to right side
        /// </summary>
        public void MoveRight()
        {
            Move(MovementType.Right);
        }

        /// <summary>
        /// try to rotate figure, if it impossible, figure will stay in old position
        /// </summary>
        public void Rotate()
        {
            figure.Rotate(heap.GetOverBorderOffset, heap.IsOverlap);
        }


        private readonly Figure figure;

        private readonly Heap heap;


        private void Move(MovementType movementType)
        {
            bool result = figure.Move(movementType, heap.IsOverlapOrOverBorder);

            // figure touch the heap on moving down
            if (!result && movementType == MovementType.Down)
            {
                // check for game over
                if (figure.ExtremePointsOfFigure[0, 1] < 0)
                {
                    GameOverEvent?.Invoke();
                }

                heap.Add(figure.PointsWithSize);
                figure.Spawn();
            }

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
                int[,] pointsWithSize = figure.PointsWithSize;
                for (int i = 0; i < pointsWithSize.GetLength(0); i++)
                {
                    if (pointsWithSize[i,0] >= 0 && pointsWithSize[i, 1] >= 0 && pointsWithSize[i, 0] < ViewMatrix.GetLength(1) && pointsWithSize[i, 1] < ViewMatrix.GetLength(0))
                    {
                        ViewMatrix[pointsWithSize[i, 1], pointsWithSize[i,0]] = 1;
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
