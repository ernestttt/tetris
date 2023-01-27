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
        
        internal int[] Pos => pos;

        internal Point[] Points => new Point[] { Point1, Point2, Point3, Point4 };

        internal int TopmostY => Points.OrderBy(a => a.Y).First().Y;
        internal int LowestY => Points.OrderBy(a => a.Y).Last().Y;
        internal int LeftmostX => Points.OrderBy(a => a.X).First().X;
        internal int RightmostX => Points.OrderBy(a => a.X).Last().X;

        internal event Action? GameOver;

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

            isEmpty = false;
        }

        internal bool Move(MovementType movement)
        {
            if (isEmpty)
            {
                Spawn();
            }

            int posIndex = movement == MovementType.Down ? 1 : 0;
            int delta = movement == MovementType.Left ? -1 : 1;
            bool result = true;

            pos[posIndex] += delta;

            if (heap.IsOverlapOrOverBorder(this))
            {
                pos[posIndex] -= delta;
                result = false;

                // figure touch the heap 
                if (movement == MovementType.Down)
                {
                    // check for game over
                    if (TopmostY < 0)
                    {
                        GameOver?.Invoke();
                    }

                    heap.Add(this);
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

            Point offset = heap.GetOverBorderOffset(this);
            if (offset.X != 0 || offset.Y != 0)
            {
                // try to adjust position
                pos[0] -= offset.X;
                pos[1] -= offset.Y;

                // figure can't be rotated
                if (heap.IsOverlap(this))
                {
                    Matrix = oldMatrix;
                    currentRotation = oldRotation;

                    return false;
                }
            }

            return true;
        }

        // used for spawn
        private Random random = new Random();
        private const int SPAWNER_HEIGHT = 4;
        private const int SPAWNER_WIDTH = 10;

        private int[] pos = new int[2] { 0, 0 };
        private int[] offset = new int[2] { 0, 0 };
        private int[] innerSize = new int[2] { 0, 0 };

        private int[,] matrix = new int[4,4];
        // points with figure inner indexis, [i,0] - x, [i,1] - y
        private int[,] points = new int[4, 2];

        private Point Point1 => new Point() { X = points[0, 0] + pos[0], Y = points[0, 1] + pos[1] - SPAWNER_HEIGHT };
        private Point Point2 => new Point() { X = points[1, 0] + pos[0], Y = points[1, 1] + pos[1] - SPAWNER_HEIGHT };
        private Point Point3 => new Point() { X = points[2, 0] + pos[0], Y = points[2, 1] + pos[1] - SPAWNER_HEIGHT };
        private Point Point4 => new Point() { X = points[3, 0] + pos[0], Y = points[3, 1] + pos[1] - SPAWNER_HEIGHT };

        private Heap heap;

        private bool isEmpty = true;
        private int currentRotation;
        private TetrisFigure currentFigure;

        private void Clear()
        {
            isEmpty = true;
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
