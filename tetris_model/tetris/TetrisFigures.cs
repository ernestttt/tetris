using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal static class TetrisFigures
    {
        public enum TetrisFigure
        {
            Square = 0,
            Line,
            S_Left,
            S_Right,
            L_Left,
            L_Right,
            T,
            Random,
        }


        private static readonly int[,] square = new int[,]
        {
            { 0,0,0,0 },
            { 0,1,1,0 },
            { 0,1,1,0 },
            { 0,0,0,0 },
        };

        private static readonly int[,] line_1 = new int[,]
        {
            { 0,1,0,0 },
            { 0,1,0,0 },
            { 0,1,0,0 },
            { 0,1,0,0 },
        };

        private static readonly int[,] line_2 = new int[,]
        {
            { 0,0,0,0 },
            { 1,1,1,1 },
            { 0,0,0,0 },
            { 0,0,0,0 },
        };

        private static readonly int[,] line_3 = new int[,]
        {
            { 0,0,1,0 },
            { 0,0,1,0 },
            { 0,0,1,0 },
            { 0,0,1,0 },
        };

        private static readonly int[,] line_4 = new int[,]
        {
            { 0,0,0,0 },
            { 1,1,1,1 },
            { 0,0,0,0 },
            { 0,0,0,0 },
        };

        private static readonly int[,] s_left_1 = new int[,]
        {
            { 0,0,0,0 },
            { 1,1,0,0 },
            { 0,1,1,0 },
            { 0,0,0,0 },
        };

        private static readonly int[,] s_left_2 = new int[,]
        {
            { 0,0,0,0 },
            { 0,0,1,0 },
            { 0,1,1,0 },
            { 0,1,0,0 },
        };

        private static readonly int[,] s_right_1 = new int[,]
        {
            { 0,0,0,0 },
            { 0,1,1,0 },
            { 1,1,0,0 },
            { 0,0,0,0 },
        };

        private static readonly int[,] s_right_2 = new int[,]
        {
            { 0,0,0,0 },
            { 0,1,0,0 },
            { 0,1,1,0 },
            { 0,0,1,0 },
        };

        private static readonly int[,] l_left_1 = new int[,]
        {
            { 0,0,0,0 },
            { 0,0,1,0 },
            { 0,0,1,0 },
            { 0,1,1,0 },
        };

        private static readonly int[,] l_left_2 = new int[,]
        {
            { 0,0,0,0 },
            { 1,0,0,0 },
            { 1,1,1,0 },
            { 0,0,0,0 },
        };

        private static readonly int[,] l_left_3 = new int[,]
        {
            { 0,0,0,0 },
            { 0,1,1,0 },
            { 0,1,0,0 },
            { 0,1,0,0 },
        };

        private static readonly int[,] l_left_4 = new int[,]
        {
            { 0,0,0,0 },
            { 1,1,1,0 },
            { 0,0,1,0 },
            { 0,0,0,0 },
        };

        private static readonly int[,] l_right_1 = new int[,]
        {
            { 0,0,0,0 },
            { 0,1,0,0 },
            { 0,1,0,0 },
            { 0,1,1,0 },
        };

        private static readonly int[,] l_right_2 = new int[,]
        {
            { 0,0,0,0 },
            { 1,1,1,0 },
            { 1,0,0,0 },
            { 0,0,0,0 },
        };

        private static readonly int[,] l_right_3 = new int[,]
        {
            { 0,0,0,0 },
            { 0,1,1,0 },
            { 0,0,1,0 },
            { 0,0,1,0 },
        };

        private static readonly int[,] l_right_4 = new int[,]
        {
            { 0,0,0,0 },
            { 0,0,1,0 },
            { 1,1,1,0 },
            { 0,0,0,0 },
        };

        private static readonly int[,] t_1 = new int[,]
        {
            { 0,0,0,0 },
            { 0,0,0,0 },
            { 0,1,0,0 },
            { 1,1,1,0 },
        };

        private static readonly int[,] t_2 = new int[,]
        {
            { 0,0,0,0 },
            { 0,1,0,0 },
            { 0,1,1,0 },
            { 0,1,0,0 },
        };

        private static readonly int[,] t_3 = new int[,]
        {
            { 0,0,0,0 },
            { 0,0,0,0 },
            { 1,1,1,0 },
            { 0,1,0,0 },
        };

        private static readonly int[,] t_4 = new int[,]
        {
            { 0,0,0,0 },
            { 0,0,1,0 },
            { 0,1,1,0 },
            { 0,0,1,0 },
        };

        private static readonly Dictionary<TetrisFigure, int[][,]> figures;

        static TetrisFigures()
        {
            figures = new Dictionary<TetrisFigure, int[][,]>
            {
                {
                    TetrisFigure.Square,
                    new int[][,]
                    {
                        square, square, square, square,
                    }
                },
                {
                    TetrisFigure.Line, 
                    new int[][,]
                    {
                        line_1, line_2, line_3, line_4,
                    }
                },
                {
                    TetrisFigure.S_Left, 
                    new int[][,]
                    {
                        s_left_1, s_left_2, s_left_1, s_left_2,
                    }
                },
                {
                    TetrisFigure.S_Right, 
                    new int[][,]
                    {
                        s_right_1, s_right_2, s_right_1, s_right_2,
                    }
                },
                {
                    TetrisFigure.L_Left, 
                    new int[][,]
                    {
                        l_left_1, l_left_2, l_left_3, l_left_4,
                    }
                },
                {
                    TetrisFigure.L_Right, 
                    new int[][,]
                    {
                        l_right_1, l_right_2, l_right_3, l_right_4,
                    }
                },
                {
                    TetrisFigure.T, 
                    new int[][,]
                    {
                        t_1, t_2, t_3, t_4,
                    }
                }
            };
        }

        public static int[,] GetFigure(TetrisFigure figure, int rotation)
        {
            return figures[figure][rotation];
        }
    }
}
