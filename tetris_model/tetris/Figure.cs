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

        internal Point[] Points => new Point[] { Point1, Point2, Point3, Point4 };

        // extreme x and y coordinates of figure in 4x4 matrix
        internal ExtremePoints ExtremePointsOfFigure
        {
            get
            {
                var orderedByX = Points.OrderBy(a => a.X).Select(a => a.X);
                var orderedByY = Points.OrderBy(a => a.Y).Select(a => a.Y); 

                return new ExtremePoints()
                {
                    TopmostY = orderedByY.First(),
                    LowestY = orderedByY.Last(),
                    LeftmostX = orderedByX.First(),
                    RightmostX = orderedByX.Last(),
                };
            }
        }

        internal event Action? CantMoveOnSpawnEvent;


        internal Figure(Heap heap)
        {
            this.heap = heap;
        }


        internal void Spawn()
        {
            currentFigure = (TetrisFigure)random.Next(7);
            currentRotation = random.Next(4);
            Matrix = TetrisFigures.GetFigure(currentFigure, currentRotation);

            // place figure in random position
            pos[0] = random.Next(0, SPAWNER_WIDTH - innerSize[0]) - offset[0];
            pos[1] = SPAWNER_HEIGHT - innerSize[1] - offset[1];

            IsEmpty = false;
        }


        internal bool Move(MovementType movement)
        {
            bool result = true;

            if (IsEmpty)
            {
                Spawn();
            }

            int posIndex = movement == MovementType.Down ? 1 : 0;
            int delta = movement == MovementType.Left ? -1 : 1;
            pos[posIndex] += delta;

            if (heap.IsOverlapOrOverBorder(Points, ExtremePointsOfFigure))
            {
                // reverse position
                pos[posIndex] -= delta;

                result = false;

                // figure touch the heap on moving down
                if (movement == MovementType.Down)
                {
                    // check for game over
                    if (ExtremePointsOfFigure.TopmostY < 0)
                    {
                        CantMoveOnSpawnEvent?.Invoke();
                    }

                    heap.Add(Points);
                    Clear();
                }
            }

            return result;
        }


        internal bool Rotate()
        {
            // remember old state
            int[,] oldMatrix = Matrix;
            int oldRotation = currentRotation;

            currentRotation++;
            currentRotation %= 4;
            Matrix = TetrisFigures.GetFigure(currentFigure, currentRotation);

            Point offset = heap.GetOverBorderOffset(ExtremePointsOfFigure);
            if (offset.X != 0 || offset.Y != 0)
            {
                // try to adjust position
                pos[0] -= offset.X;
                pos[1] -= offset.Y;

                // figure can't be rotated, return to old state
                if (heap.IsOverlap(Points))
                {
                    Matrix = oldMatrix;
                    currentRotation = oldRotation;

                    return false;
                }
            }

            return true;
        }


        // used in spawn
        private readonly Random random = new Random();

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

        private readonly Heap heap;

        private TetrisFigure currentFigure;
        private int currentRotation;

        // 4 points of figure
        private Point Point1 => new Point() { X = points[0, 0] + pos[0], Y = points[0, 1] + pos[1] - SPAWNER_HEIGHT };
        private Point Point2 => new Point() { X = points[1, 0] + pos[0], Y = points[1, 1] + pos[1] - SPAWNER_HEIGHT };
        private Point Point3 => new Point() { X = points[2, 0] + pos[0], Y = points[2, 1] + pos[1] - SPAWNER_HEIGHT };
        private Point Point4 => new Point() { X = points[3, 0] + pos[0], Y = points[3, 1] + pos[1] - SPAWNER_HEIGHT };


        private void Clear()
        {
            IsEmpty = true;
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

        internal struct ExtremePoints
        {
            internal int TopmostY { get; init; }
            internal int LowestY { get; init; }
            internal int LeftmostX { get; init; }
            internal int RightmostX { get; init; }
        }
    }
}
