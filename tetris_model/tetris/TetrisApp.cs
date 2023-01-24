using Tetris;
using System.Threading;


public class TetrisApp
{
    private const int delay = 70;

    private static void Main()
    {
        Field field = new Field();
        ConsoleView view = new ConsoleView();

        _ = ListenToInput(field);
        while (true)
        {
            field.MoveDown();
            view.Draw(field.ViewMatrix);
            Task.Delay(delay).Wait();
        }
    }

    private async static Task ListenToInput(Field field)
    {
        while(true)
        {
            await Task.Delay(delay);
            var key = Console.ReadKey();
            if(key.Key == ConsoleKey.LeftArrow)
            {

            }
            else if(key.Key == ConsoleKey.RightArrow)
            {

            }
        }  
    }
}