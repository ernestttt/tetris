using Tetris;

internal class TetrisApp
{
    private const int delay = 1000/5;
    private static bool isPlaying = false;

    private static void Main()
    {
        Field field = new Field();
        ConsoleView view = new ConsoleView();

        field.GameOverEvent += () => isPlaying = false;

        isPlaying = true;

        _ = ListenToInput(field);

        while (isPlaying)
        {
            field.MoveDown();
            view.Draw(field.ViewMatrix);
            Console.WriteLine("\n\n\n{0}", field.CompletedRows);
            Task.Delay(delay).Wait();
        }

        Console.Clear();
        Console.WriteLine("GAME OVER");
    }

    private async static Task ListenToInput(Field field)
    {
        while(isPlaying)
        {
            await Task.Delay(1000/250);
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