using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Field
    {
        public byte[,] ViewMatrix { get; init; }

        private Figure figure = new Figure();

        public Field()
        {
            ViewMatrix = new byte[20, 10];
        }

        public void Step()
        {
            ClearViewMatrix();
            figure.Spawn();
            figure.Move(MovementType.Down);
            UpdateViewMatrix();
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
            for (int i = 0; i < figure.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < figure.Matrix.GetLength(1); j++)
                {
                    int viewMatrixPosHor = j + figure.Pos[0];
                    int viewMatrixPosVert = i + figure.Pos[1] - 4;

                    if (viewMatrixPosHor >= 0 && viewMatrixPosHor < ViewMatrix.GetLength(1) && viewMatrixPosVert >= 0 && viewMatrixPosVert < ViewMatrix.GetLength(0))
                    {
                        ViewMatrix[viewMatrixPosVert, viewMatrixPosHor] = (byte)figure.Matrix[i, j];
                    }
                }
            }
        }
    }
}
