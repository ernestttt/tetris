using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Field
    {
        private Random random = new Random();

        public byte[,] ViewMatrix { get; init; }

        private TetrisMatrix matrix_24x10;
        private TetrisMatrix figureMatrix_24x10;
        private TetrisMatrix heapMatrix_24x10;

        public Field()
        {
            figureMatrix_24x10 = new TetrisMatrix(24, 10);
            matrix_24x10 = new TetrisMatrix(24, 10);
            heapMatrix_24x10 = new TetrisMatrix(24, 10);

            ViewMatrix = new byte[20, 10];
        }

        public void MoveDown()
        {
            matrix_24x10.Clear();

            if (figureMatrix_24x10.IsEmpty())
            {
                Spawn();
            }

            if (!figureMatrix_24x10.Move(MovementType.Down, heapMatrix_24x10))
            {
                heapMatrix_24x10.CombineWith(figureMatrix_24x10, 0, 0);
                figureMatrix_24x10.Clear();
            }
            matrix_24x10.CombineWith(figureMatrix_24x10, 0, 0, PivotType.TopLeft);
            matrix_24x10.CombineWith(heapMatrix_24x10, 0, 0);
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            for (int i = 0; i < ViewMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < ViewMatrix.GetLength(1); j++)
                {
                    ViewMatrix[i, j] = matrix_24x10.GetElementAt(i + 4, j);
                }
            }
        }

        public void Spawn()
        {
            TetrisMatrix figure = TetrisFigures.GetFigure(TetrisFigure.Random);
            figure.Rotate(random.Next(2) == 0 ? Rotation.Right : Rotation.Left, random.Next(3));

            figureMatrix_24x10.CombineWith(figure, 4 - figure.Height, random.Next(figureMatrix_24x10.Width - figure.Width));
        }
    }
}
