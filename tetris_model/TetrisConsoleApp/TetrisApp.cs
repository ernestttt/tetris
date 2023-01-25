using Tetris;
using System.Threading;


public class TetrisApp
{
    private const int delay = 1000/30;

    private static void Main()
    {
        Field field = new Field();
        ConsoleView view = new ConsoleView();

        _ = ListenToInput(field);
        field.Step();
        view.Draw(field.ViewMatrix);

        return;
        while (true)
        {
            
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
                //field.MoveLeft();
            }
            else if(key.Key == ConsoleKey.RightArrow)
            {
                //field.MoveRight();
            }
            else if(key.Key == ConsoleKey.UpArrow)
            {

            }
            else if(key.Key == ConsoleKey.DownArrow)
            {

            }
        }  
    }
}