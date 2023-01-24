using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal static class TetrisFigures
    {
        private static Random random = new Random();

        private static readonly byte[,] square = new byte[,]
        {
            { 1,1 },
            { 1,1 },
        };

        private static readonly byte[,] line = new byte[,]
        {
            {1 },
            {1 },
            {1 },
            {1 }
        };

        private static readonly byte[,] s_left = new byte[,]
        {
            {1,1,0},
            {0,1,1},
        };

        private static readonly byte[,] s_right = new byte[,]
        {
            {0,1,1},
            {1,1,0},
        };

        private static readonly byte[,] l_left = new byte[,]
        {
            {0,1},
            {0,1},
            {1,1},
        };

        private static readonly byte[,] l_right = new byte[,]
        {
            {1,0},
            {1,0},
            {1,1},
        };

        private static readonly byte[,] t = new byte[,]
        {
            { 0, 1, 0 },
            { 1, 1, 1 },
        };

        private static readonly Dictionary<TetrisFigure, TetrisMatrix> figures;

        static TetrisFigures()
        {
            figures = new Dictionary<TetrisFigure, TetrisMatrix>
            {
                { TetrisFigure.Square, new TetrisMatrix(square)},
                { TetrisFigure.Line, new TetrisMatrix(line)},
                { TetrisFigure.S_Left, new TetrisMatrix(s_left)},
                { TetrisFigure.S_Right, new TetrisMatrix(s_right)},
                { TetrisFigure.L_Left, new TetrisMatrix(l_left)},
                { TetrisFigure.L_Right, new TetrisMatrix(l_right)},
                { TetrisFigure.T, new TetrisMatrix(t)},
            };
        }

        public static TetrisMatrix GetFigure(TetrisFigure figureType)
        {
            TetrisFigure innerFigureType = figureType;

            if (figureType == TetrisFigure.Random)
            {
                int randomIndex = random.Next(figures.Keys.Count());
                innerFigureType = figures.Keys.ToArray()[randomIndex];
            }

            return figures[innerFigureType];
        }
    }
}
