using Tetris;
using System.Threading;


public class TetrisApp
{
    private static void Main()
    {
        Field field = new Field();
        ConsoleView view = new ConsoleView();

        while (true)
        {
            field.MoveDown();
            view.Draw(field.ViewMatrix);
            Task.Delay(1000).Wait();
        }
    }
}