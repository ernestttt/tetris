﻿using System;

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
        /// invoked when figure can't move down after spawn
        /// </summary>
        public event Action GameOverEvent;


        public TetrisCore()
        {
            ViewMatrix = new byte[20, 10];
            figure = new Figure(heap);

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
            figure.Rotate();
        }


        private readonly Figure figure;

        private readonly Heap heap = new Heap();


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