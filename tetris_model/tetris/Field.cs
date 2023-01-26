﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Field
    {
        public byte[,] ViewMatrix { get; init; }
        private Figure figure;
        private Heap heap = new Heap();

        public int CompletedRows { get; private set; } = 0;

        public event Action GameOverEvent;

        public Field()
        {
            ViewMatrix = new byte[20, 10];
            figure = new Figure(heap);
            heap.FigureAdded += CheckRows;
            figure.GameOver += GameOver;
            heap.CompletedRows += UpdateScore;
        }

        public void UpdateScore(int rows)
        {
            CompletedRows += rows;
        }

        public void CheckRows()
        {
            heap.CompleteRows();
        }

        public void MoveDown()
        {
            ClearViewMatrix();
            figure.Move(MovementType.Down);
            UpdateViewMatrix();
        }

        private void GameOver()
        {
            GameOverEvent?.Invoke();
        }

        public void MoveLeft()
        {
            figure.Move(MovementType.Left);
        }

        public void MoveRight() 
        {
            figure.Move(MovementType.Right);
        }

        public void Rotate()
        {
            figure.Rotate();
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
            // figure
            foreach(Point point in figure.Points)
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
