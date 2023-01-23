using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class ConsoleView
{
    public void Draw(byte[,] viewMatrix)
    {
        for(int i = 0; i < viewMatrix.GetLength(0); i++)
        {
            for(int j = 0; j < viewMatrix.GetLength(1); j++)
            {
                Console.Write(viewMatrix[i,j] == 0 ? " " : "*");
            }
            Console.WriteLine();
        }
    }
}
