using Tetris;


Field field = new Field();
ConsoleView view = new ConsoleView();

field.MoveDown();
view.Draw(field.ViewMatrix);
