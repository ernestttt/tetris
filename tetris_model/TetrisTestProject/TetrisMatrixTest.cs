using Tetris;

namespace TetrisTestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void TetrisMatrixRotation()
        {
            TetrisMatrix s_matrix = new TetrisMatrix(new byte[3,2] { { 1, 0 }, { 1, 1 }, { 0, 1 } });
            TetrisMatrix s_matrixRotated = new TetrisMatrix(new byte[2, 3] { { 0, 1, 1 }, {1, 1, 0 }});

            s_matrix.Rotate(Rotation.Left);

            Assert.IsTrue(CheckMatrixEquality(s_matrix, s_matrixRotated));

        }

        private static bool CheckMatrixEquality(TetrisMatrix matrix1, TetrisMatrix matrix2)
        {
            if (matrix1.Width != matrix2.Width)
                return false;

            if (matrix1.Height != matrix2.Height)
                return false;

            for (int i = 0; i < matrix1.Height; i++)
            {
                for (int j = 0; j < matrix2.Width; j++)
                {
                    bool value1 = matrix1[i, j] != 0;
                    bool value2 = matrix2[i, j] != 0;
                    if (value1 != value2)
                        return false;
                }
            }

            return true;
        }
    }
}