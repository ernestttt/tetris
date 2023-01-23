using Tetris;
using System.Threading;


public class TetrisApp
{
    private const int delay = 70;

    private static void Main()
    {
        Field field = new Field();
        ConsoleView view = new ConsoleView();

        while (true)
        {
            field.MoveDown();
            view.Draw(field.ViewMatrix);
            Task.Delay(delay).Wait();
        }
    }
}