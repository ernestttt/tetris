using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class ConsoleView
{
    public void Draw(byte[,] viewMatrix)
    {
        Console.Clear();
        for(int i = 0; i < viewMatrix.GetLength(1) + 2; i++)
        {
            Console.Write('*');
        }
        Console.WriteLine();
        for(int i = 0; i < viewMatrix.GetLength(0); i++)
        {
            Console.Write("*");
            for(int j = 0; j < viewMatrix.GetLength(1); j++)
            {
                Console.Write(viewMatrix[i,j] == 0 ? " " : "*");
            }
            Console.WriteLine("*");
        }
        for (int i = 0; i < viewMatrix.GetLength(1) + 2; i++)
        {
            Console.Write('*');
        }
    }
}
