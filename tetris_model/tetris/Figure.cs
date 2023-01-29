using System;
using System.Drawing;
using System.Linq;

namespace Tetris
{
    internal class Figure
    {
        internal int[,] Matrix
        {
            get
            {
                return matrix;
            }
            private set
            {
                matrix = value;
                CalculatePoints();
            }
        }

        internal bool IsEmpty { get; private set; } = true;

        internal int[] Pos => pos;

        private int[,] pointsWithSize = new int[4, 2];
        internal int[,] PointsWithSize
        {
            get
            {
                pointsWithSize[0, 0] = points[0, 0] + pos[0];
                pointsWithSize[0, 1] = points[0, 1] + pos[1] - SPAWNER_HEIGHT;
                pointsWithSize[1, 0] = points[1, 0] + pos[0];
                pointsWithSize[1, 1] = points[1, 1] + pos[1] - SPAWNER_HEIGHT;
                pointsWithSize[2, 0] = points[2, 0] + pos[0];
                pointsWithSize[2, 1] = points[2, 1] + pos[1] - SPAWNER_HEIGHT;
                pointsWithSize[3, 0] = points[3, 0] + pos[0];
                pointsWithSize[3, 1] = points[3, 1] + pos[1] - SPAWNER_HEIGHT;

                return pointsWithSize;
            }
        }

        private int[,] extremePoints = new int[2,2];
        // 2 [x, y] points of the figure, [0] is top left, [1] is bottom right point
        internal int[,] ExtremePointsOfFigure
        {
            get
            {
                int topY = int.MaxValue;
                int bottomY = int.MinValue;
                int leftX = int.MaxValue;
                int rightX = int.MinValue;

                for (int i = 0; i < PointsWithSize.GetLength(0); i++)
                {
                    if (PointsWithSize[i,0] < leftX)
                    {
                        leftX = PointsWithSize[i, 0];
                    }
                    if (PointsWithSize[i,0] > rightX)
                    {
                        rightX = PointsWithSize[i, 0];
                    }
                    if (PointsWithSize[i,1] > bottomY)
                    {
                        bottomY = PointsWithSize[i,1];
                    }
                    if (PointsWithSize[i,1] < topY)
                    {
                        topY = PointsWithSize[i,1];
                    }
                }

                extremePoints[0, 0] = leftX;
                extremePoints[0, 1] = topY;
                extremePoints[1, 0] = rightX;
                extremePoints[1, 1] = bottomY;

                return extremePoints;
            }
        }

        internal event Action CantMoveOnSpawnEvent;


        internal bool Move(MovementType movement, Func<int[,], int[,], bool> checkForOverlap)
        {
            bool result = true;

            if (IsEmpty)
            {
                Spawn();
            }

            int posIndex = movement == MovementType.Down ? 1 : 0;
            int delta = movement == MovementType.Left ? -1 : 1;
            pos[posIndex] += delta;

            if (checkForOverlap(PointsWithSize, ExtremePointsOfFigure))
            {
                // reverse position
                pos[posIndex] -= delta;
                result = false;
            }

            return result;
        }


        internal bool Rotate(Func<int[,], int[]> getOverBorderOffset, Func<int[,], bool> isOverlap)
        {
            // remember old state
            int[,] oldMatrix = Matrix;
            int oldRotation = currentRotation;

            currentRotation++;
            currentRotation %= 4;
            Matrix = TetrisFigures.GetFigure(currentFigure, currentRotation);

            int[] offset = getOverBorderOffset(ExtremePointsOfFigure);

            // try to adjust position
            pos[0] -= offset[0];
            pos[1] -= offset[1];

            // figure can't be rotated, return to old state
            if (isOverlap(PointsWithSize))
            {
                pos[0] += offset[0];
                pos[1] += offset[1];
                Matrix = oldMatrix;
                currentRotation = oldRotation;

                return false;
            }

            return true;
        }


        // used in spawn
        private readonly System.Random random = new System.Random();

        private const int SPAWNER_HEIGHT = 4;
        private const int SPAWNER_WIDTH = 10;

        // pos of 4x4 matrix which contains figure
        private int[] pos = new int[2] { 0, 0 };

        // offset from top left corner of 4x4 matrix to top left corner of smallest possible rectangular of figure of this square
        private int[] offset = new int[2] { 0, 0 };

        // inner size of smallest rectangular of figure in 4x4 matrix
        private int[] innerSize = new int[2] { 0, 0 };

        // 4x4 matrix which contains figure
        private int[,] matrix = new int[4,4];

        // points with figure inner indexis, [i,0] - x, [i,1] - y
        private int[,] points = new int[4, 2];

        private TetrisFigure currentFigure;
        private int currentRotation;

        

        public void Spawn()
        {
            currentFigure = (TetrisFigure)random.Next(7);
            currentRotation = random.Next(4);
            Matrix = TetrisFigures.GetFigure(currentFigure, currentRotation);

            // place figure in random position
            pos[0] = random.Next(0, SPAWNER_WIDTH - innerSize[0]) - offset[0];
            pos[1] = SPAWNER_HEIGHT - innerSize[1] - offset[1];

            IsEmpty = false;
        }

        private void CalculatePoints()
        {
            CalculateInnerSizeAndOffset();

            int pointsIndex = 0;
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    if (Matrix[i, j] != 0)
                    {
                        points[pointsIndex, 0] = j;
                        points[pointsIndex, 1] = i;
                        pointsIndex++;
                    }
                }
            }
        }


        private void CalculateInnerSizeAndOffset()
        {
            int startVerticalIndex = int.MaxValue;
            int endVerticalIndex = int.MinValue;
            int startHorizontalIndex = int.MaxValue;
            int endHorizontalIndex = int.MinValue;

            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    if (Matrix[i, j] == 0)
                    {
                        continue;
                    }

                    if (startVerticalIndex > i)
                    {
                        startVerticalIndex = i;
                    }
                    if (endVerticalIndex < i)
                    {
                        endVerticalIndex = i;
                    }
                    if (startHorizontalIndex > j)
                    {
                        startHorizontalIndex = j;
                    }
                    if (endHorizontalIndex < j)
                    {
                        endHorizontalIndex = j;
                    }
                }
            }

            innerSize[0] = endHorizontalIndex - startHorizontalIndex + 1;
            innerSize[1] = endVerticalIndex - startVerticalIndex + 1;

            offset[0] = startHorizontalIndex;
            offset[1] = startVerticalIndex;
        }
    }
}
