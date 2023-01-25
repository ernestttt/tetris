using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public enum PivotType
    {
        TopLeft = 0,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    public enum Rotation
    {
        Left,
        Right,
    }

    public enum MovementType
    {
       Down = 0,
       Up,
       Left,
       Right,
    }

    public enum Axis
    {
        Vertical,
        Horizontal,
    }

    public class TetrisMatrix
    {
        private byte[,] innerMatrix;
        private byte[,] transposeMatrix;

        public int Height => innerMatrix.GetLength(0);
        public int Width => innerMatrix.GetLength(1);

        public int VerticalIndexOfLastElement => Height - 1;
        public int HorizontalIndexOfLastElement => Width - 1;

        public TetrisMatrix(int vertical, int horizontal)
        {
            innerMatrix = new byte[vertical, horizontal];
            Clear();
            transposeMatrix = new byte[Width, Height];
            PrecalculateTranspose();
        }

        public TetrisMatrix(byte[,] matrix)
        {
            innerMatrix = matrix;
            transposeMatrix = new byte[Width, Height];
            PrecalculateTranspose();
        }

        public byte GetElementAt(int rowIndex, int columnIndex)
        {
            return innerMatrix[rowIndex, columnIndex];
        }

        public bool IsEmpty()
        {
            for (int i = 0; i < innerMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < innerMatrix.GetLength(1); j++)
                {
                    if (innerMatrix[i, j] != 0)
                        return false;
                }
            }

            return true;
        }

        public void CombineWith(TetrisMatrix otherMatrix, int verticalOffset, int horizontalOffset, PivotType pivot = PivotType.TopLeft)
        {
            // adjust offsets
            int verticalOffsetTop = verticalOffset;
            int horizontalOffsetLeft = horizontalOffset;

            if (pivot == PivotType.TopRight || pivot == PivotType.BottomRight)
            {
                horizontalOffsetLeft = Width - otherMatrix.Width + horizontalOffset;
            }

            if (pivot == PivotType.BottomLeft || pivot == PivotType.BottomRight)
            {
                verticalOffsetTop = Height - Height + verticalOffset;
            }

            CombineWith_LeftPivot(otherMatrix, verticalOffsetTop, horizontalOffsetLeft);
        }

        private void CombineWith_LeftPivot(TetrisMatrix otherMatrix, int verticalOffset, int horizontalOffset)
        {
            int startHorizontalIndex = horizontalOffset;
            int endHorizontalIndex = horizontalOffset + otherMatrix.Width - 1;
            int startVerticalIndex = verticalOffset;
            int endVerticalIndex = verticalOffset + otherMatrix.Height - 1;

            // check out of bounds
            if (startHorizontalIndex >= Width || endHorizontalIndex < 0 || startHorizontalIndex >= Height || endVerticalIndex < 0)
                return;

            // clamp bounds
            startHorizontalIndex = Math.Clamp(startHorizontalIndex, 0, Width - 1);
            endHorizontalIndex = Math.Clamp(endHorizontalIndex, 0, Width - 1);

            startVerticalIndex = Math.Clamp(startVerticalIndex, 0, Height - 1);
            endVerticalIndex = Math.Clamp(endVerticalIndex, 0, Height - 1);

            for (int vertIndex = startVerticalIndex; vertIndex <= endVerticalIndex; vertIndex++)
            {
                for (int horIndex = startHorizontalIndex; horIndex <= endHorizontalIndex; horIndex++)
                {
                    //calculate indices for otherMatrix
                    int otherVertIndex = vertIndex - verticalOffset;
                    int otherHorIndex = horIndex - horizontalOffset;

                    if (otherMatrix.GetElementAt(otherVertIndex, otherHorIndex) != 0)
                    {
                        innerMatrix[vertIndex, horIndex] = otherMatrix.GetElementAt(otherVertIndex, otherHorIndex);
                    }
                }
            }
        }

        public void Rotate(Rotation rotation, int times = 1)
        {
            Rotation currentRotation = rotation;
            for (int i = 0; i < times; i++)
            {
                Rotate(rotation);
                currentRotation = currentRotation == Rotation.Left ? Rotation.Right : Rotation.Left;
            }
        }

        private void Rotate(Rotation rotation)
        {
            MirrorAroundAxis(rotation == Rotation.Right ? Axis.Horizontal : Axis.Vertical);
            TransposeMatrix();
        }

        private void MirrorAroundAxis(Axis axis)
        {
            if (axis == Axis.Vertical)
            {
                MirrorAroundVertical();
            }
            else
            {
                MirrorAroundHorizontal();
            }
        }

        private void MirrorAroundVertical()
        {
            int startColumn = 0;
            int endColumn = innerMatrix.GetUpperBound(1);

            while (startColumn < endColumn)
            {
                for (int i = 0; i < innerMatrix.GetLength(0); i++)
                {
                    byte temp = innerMatrix[i, startColumn];
                    innerMatrix[i, startColumn] = innerMatrix[i, endColumn];
                    innerMatrix[i, endColumn] = temp;
                }
                startColumn++;
                endColumn--;
            }
        }

        private void MirrorAroundHorizontal()
        {
            int startRow = 0;
            int endRow = innerMatrix.GetUpperBound(0);

            while (startRow < endRow)
            {
                for (int i = 0; i < innerMatrix.GetLength(1); i++)
                {
                    byte temp = innerMatrix[startRow, i];
                    innerMatrix[startRow, i] = innerMatrix[endRow, i];
                    innerMatrix[endRow, i] = temp;
                }
                startRow++;
                endRow--;
            }
        }

        private void PrecalculateTranspose()
        {
            for (int i = 0; i < innerMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < innerMatrix.GetLength(1); j++)
                {
                    transposeMatrix[j, i] = innerMatrix[i, j];
                }
            }
        }

        private void TransposeMatrix()
        {
            PrecalculateTranspose();
            // swap matrices
            byte[,] tempMatrix = innerMatrix;
            innerMatrix = transposeMatrix;
            transposeMatrix = tempMatrix;
        }

        public byte[,] GetValueBlock()
        {
            MatrixBounds bounds = MatrixBounds.CalculateBounds(this);

            int height = bounds.EndVerticalIndex - bounds.StartVerticalIndex;
            int width = bounds.EndHorizontalIndex - bounds.StartHorizontalIndex;

            byte[,] valueBlock = new byte[height, width];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    valueBlock[row, col] = innerMatrix[row + bounds.StartVerticalIndex, col + bounds.StartHorizontalIndex];
                }
            }

            return valueBlock;
        }

        public void Clear()
        {
            for (int i = 0; i < innerMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < innerMatrix.GetLength(1); j++)
                {
                    innerMatrix[i, j] = 0;
                }
            }
        }

        public bool IsItPossibleToMove(MovementType movement)
        {
            bool isItPossibleToMove = true;
            MatrixBounds bounds = MatrixBounds.CalculateBounds(this);

            if (movement == MovementType.Left)
            {
                isItPossibleToMove = bounds.StartHorizontalIndex != 0;
            }
            if (movement == MovementType.Right)
            {
                isItPossibleToMove = bounds.EndHorizontalIndex != Width - 1;
            }
            if (movement == MovementType.Down)
            {
                isItPossibleToMove = bounds.EndVerticalIndex != Height - 1;
            }

            return isItPossibleToMove;
        }

        public byte this[int i, int j]
        {
            get
            {
                return innerMatrix[i, j];
            }
        }


        public bool IsItPossibleToMove(MovementType movement, TetrisMatrix another)
        {
            throw new NotImplementedException();

            if(this.Width != another.Width || this.Height != another.Height)
            {
                throw new ArgumentException("dimensions of matricies should be the same");
            }

            bool isItPossibleToMove = IsItPossibleToMove(movement);
            MatrixBounds bounds = MatrixBounds.CalculateBounds(this);

            if (movement == MovementType.Left)
            {
                isItPossibleToMove = bounds.StartHorizontalIndex != 0;
            }
            if (movement == MovementType.Right)
            {
                isItPossibleToMove = bounds.EndHorizontalIndex != 0;
            }
            if (movement == MovementType.Down)
            {
                isItPossibleToMove = bounds.EndVerticalIndex != 0;
            }

            return isItPossibleToMove;
        }

        private int[,] GetLeftIndices(MatrixBounds bounds)
        {
            int[,] indices = new int[bounds.EndVerticalIndex - bounds.StartVerticalIndex, 2];

            for(int row = bounds.StartVerticalIndex, i = 0; row <= bounds.EndVerticalIndex; row++, i++) 
            {
                for(int col = bounds.StartHorizontalIndex; col <= bounds.EndHorizontalIndex; col++)
                {
                    if(this.GetElementAt(row, col) != 0)
                    {
                        indices[i, 0] = row;
                        indices[i, 1] = col;
                        break;
                    }
                }
            }

            return indices;
        }

        private int[,] GetRightIndices(MatrixBounds bounds)
        {
            int[,] indices = new int[bounds.EndVerticalIndex - bounds.StartVerticalIndex, 2];

            for (int row = bounds.StartVerticalIndex, i = 0; row <= bounds.EndVerticalIndex; row++, i++)
            {
                for (int col = bounds.EndHorizontalIndex; col >= bounds.StartHorizontalIndex; col--)
                {
                    if (this.GetElementAt(row, col) != 0)
                    {
                        indices[i, 0] = row;
                        indices[i, 1] = col;
                        break;
                    }
                }
            }

            return indices;
        }

        private int[,] GetDownIndices(MatrixBounds bounds)
        {
            throw new NotImplementedException();
        }

        // refactor this method
        public bool Move(MovementType movement, TetrisMatrix otherMatrix = null)
        {
            if (!IsItPossibleToMove(movement))
            {
                return false;
            }

            if (movement == MovementType.Down)
            {
                for(int i = innerMatrix.GetLength(0) - 2; i >= 0; i--)
                {
                    for (int j = 0; j < innerMatrix.GetLength(1); j++)
                    {
                        innerMatrix[i + 1, j] = innerMatrix[i, j];
                    }
                }

                for(int i = 0; i < innerMatrix.GetLength(1); i++)
                {
                    innerMatrix[0, i] = 0;
                }

                if (otherMatrix != null && IsOverlap(otherMatrix))
                {
                    for (int i = 0; i < innerMatrix.GetLength(0) - 1; i++)
                    {
                        for(int j = 0; j < innerMatrix.GetLength(1); j++)
                        {
                            innerMatrix[i, j] = innerMatrix[i+1, j];
                        }
                    }

                    for (int i = 0; i < innerMatrix.GetLength(1); i++)
                    {
                        innerMatrix[0, i] = 0;
                    }

                    return false;
                }
            }
            else if (movement == MovementType.Left)
            {
                for (int i = 0; i < innerMatrix.GetLength(0); i++)
                {
                    for(int j = 0; j < innerMatrix.GetLength(1) - 1; j++)
                    {
                        innerMatrix[i, j] = innerMatrix[i, j + 1];
                    }
                }

                for (int i = 0; i < innerMatrix.GetLength(0); i++)
                {
                    innerMatrix[i, Width - 1] = 0;
                }

                if (otherMatrix != null && IsOverlap(otherMatrix))
                {
                    for (int i = 0; i < innerMatrix.GetLength(0); i++)
                    {
                        for (int j = innerMatrix.GetLength(1) - 1; j > 0; j--)
                        {
                            innerMatrix[i, j] = innerMatrix[i, j - 1];
                        }
                    }

                    for (int i = 0; i < innerMatrix.GetLength(0); i++)
                    {
                        innerMatrix[i, 0] = 0;
                    }

                    return false;
                }
            }
            else if (movement == MovementType.Right)
            {
                for (int i = 0; i < innerMatrix.GetLength(0); i++)
                {
                    for (int j = innerMatrix.GetLength(1) - 1; j > 0; j--)
                    {
                        innerMatrix[i, j] = innerMatrix[i, j - 1];
                    }
                }

                for (int i = 0; i < innerMatrix.GetLength(0); i++)
                {
                    innerMatrix[i, 0] = 0;
                }

                if (otherMatrix != null && IsOverlap(otherMatrix))
                {
                    for (int i = 0; i < innerMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < innerMatrix.GetLength(1) - 1; j++)
                        {
                            innerMatrix[i, j] = innerMatrix[i, j + 1];
                        }
                    }

                    for (int i = 0; i < innerMatrix.GetLength(0); i++)
                    {
                        innerMatrix[i, Width - 1] = 0;
                    }
                    
                    return false;
                }
            }

            return true;
        }

        private bool IsOverlap(TetrisMatrix other)
        {
            for(int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    if (innerMatrix[i, j] != 0 && other.GetElementAt(i,j) != 0) 
                    { 
                        return true; 
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for(int i = 0; i < innerMatrix.GetLength(0); i++)
            {
                for(int j = 0; j < innerMatrix.GetLength(1); j++)
                {
                    stringBuilder.Append(innerMatrix[i, j].ToString());
                }
                stringBuilder.Append('\n');
            }
            

            return stringBuilder.ToString();
        }
    }

    public struct MatrixBounds
    {
        public int StartVerticalIndex { get; init; }
        public int EndVerticalIndex { get; init; }
        public int StartHorizontalIndex { get; init; }
        public int EndHorizontalIndex { get; init; }

        public static MatrixBounds CalculateBounds(TetrisMatrix matrix)
        {
            int startIndexVertical = int.MaxValue;
            int endIndexVertical = int.MinValue;
            int startIndexHorizontal = int.MaxValue;
            int endIndexHorizontal = int.MinValue;

            // get bounds
            for (int row = 0; row < matrix.Height; row++)
            {
                for (int col = 0; col < matrix.Width; col++)
                {
                    if (matrix.GetElementAt(row, col) != 0)
                    {
                        if (row > endIndexVertical)
                        {
                            endIndexVertical = row;
                        }
                        if (row < startIndexVertical)
                        {
                            startIndexVertical = row;
                        }
                        if (col > endIndexHorizontal)
                        {
                            endIndexHorizontal = col;
                        }
                        if (col < startIndexHorizontal)
                        {
                            startIndexHorizontal = col;
                        }
                    }
                }
            }

            return new MatrixBounds { StartVerticalIndex = startIndexVertical,EndVerticalIndex = endIndexVertical,StartHorizontalIndex = startIndexHorizontal,EndHorizontalIndex = endIndexHorizontal };
        }
    }
}
