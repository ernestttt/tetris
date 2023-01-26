using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Figure
    {
        private const int SPAWNER_SPACE = 4;


        private Random random = new Random();
        private int[] pos = new int[2] { 0, 0 };
        private int[] offset = new int[2] { 0, 0 };
        private int[] innerSize = new int[2] { 0, 0 };

        private int[,] matrix;

        private int rotation;
        private TetrisFigures.TetrisFigure figureType;

        public int[,] Matrix => matrix;
        public int[] Pos => pos;

        private int[,] points = new int[4, 2];

        public Point Point1 => new Point() { X = points[0, 0] + pos[0], Y = points[0, 1] + pos[1] - SPAWNER_SPACE };
        public Point Point2 => new Point() { X = points[1, 0] + pos[0], Y = points[1, 1] + pos[1] - SPAWNER_SPACE };
        public Point Point3 => new Point() { X = points[2, 0] + pos[0], Y = points[2, 1] + pos[1] - SPAWNER_SPACE };
        public Point Point4 => new Point() { X = points[3, 0] + pos[0], Y = points[3, 1] + pos[1] - SPAWNER_SPACE };

        public Point[] Points => new Point[] {Point1, Point2, Point3, Point4};

        private Heap heap;

        private bool isEmpty = true;
        private bool IsEmpty => isEmpty;

        private int currentRotation;
        private TetrisFigures.TetrisFigure currentFigure;

        public Figure(Heap heap)
        {
            this.heap = heap;
        }

        

        public void Spawn()
        {
            currentFigure = (TetrisFigures.TetrisFigure)random.Next(7);
            currentRotation = random.Next(4);
            matrix = TetrisFigures.GetFigure(currentFigure, rotation);

            CalculateInnerSizeAndOffset();
            CalculatePoints();

            pos[0] = random.Next(-offset[0], 10 - innerSize[0] - offset[0]);
            pos[1] = 4 - innerSize[1] - offset[1];
            isEmpty = false;
        }

        public bool Move(MovementType movement)
        {
            if (IsEmpty)
            {
                Spawn();
            }
            if (MovementType.Down == movement)
            {
                pos[1]++;
                if (heap.IsOverlap(this) || heap.GetOverBorderOffset(this).Y != 0)
                {
                    pos[1]--;
                    heap.Add(this);
                    Clear();
                    return false;
                }
            }
            else if(MovementType.Left == movement)
            {
                pos[0]--;
                if (heap.IsOverlapOrOverBorder(this))
                {
                    pos[0]++;
                    return false;
                }
            }
            else if(MovementType.Right == movement)
            {
                pos[0]++;
                if (heap.IsOverlapOrOverBorder(this))
                {
                    pos[0]--;
                    return false;
                }
            }

            return true;
        }

        public bool Rotate()
        {
            // remember old state
            int[,] oldMatrix = matrix;
            int oldRotation = currentRotation;

            currentRotation++;
            currentRotation %= 4;
            matrix = TetrisFigures.GetFigure(currentFigure, currentRotation);
            CalculateInnerSizeAndOffset();
            CalculatePoints();

            if(heap.IsOverlap(this))
            {
                matrix = oldMatrix;
                currentRotation = oldRotation;
                CalculateInnerSizeAndOffset();
                CalculatePoints();

                return false;
            }

            Point offset = heap.GetOverBorderOffset(this);
            if(offset.X != 0 || offset.Y != 0)
            {
                pos[0] += offset.X;
                pos[1] += offset.Y;

                if (heap.IsOverlap(this))
                {
                    matrix = oldMatrix;
                    currentRotation = oldRotation;
                    CalculateInnerSizeAndOffset();
                    CalculatePoints();

                    return false;
                }
            }

            return true;
        }

        private void Clear()
        {
            isEmpty = true;
        }

        private void CalculatePoints()
        {
            int pointsIndex = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != 0)
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

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
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
