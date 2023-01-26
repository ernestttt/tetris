using Tetris;
using System.Threading;


public class TetrisApp
{
    private const int delay = 1000/10;

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
            await Task.Delay(1000/60);
            var key = Console.ReadKey();
            if(key.Key == ConsoleKey.LeftArrow)
            {
                field.MoveLeft();
            }
            else if(key.Key == ConsoleKey.RightArrow)
            {
                field.MoveRight();
            }
            else if(key.Key == ConsoleKey.UpArrow)
            {
                field.Rotate();
            }
            else if(key.Key == ConsoleKey.DownArrow)
            {
                field.MoveDown();
            }
        }  
    }
}