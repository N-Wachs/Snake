
namespace Snake
{
    class Output
    {
        #region Properties
        private string _displayText;     // Text to be displayed
        private ConsoleColor _textColor; // Color of the text
        #endregion

        #region Fields
        private string DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; }
        }

        private ConsoleColor TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }
        #endregion

        #region Constructors
        // Default constructor
        public Output()
        {
            _displayText = "";
            _textColor = ConsoleColor.White;
        }

        // Copy constructor
        public Output(Output other)
        {
            _displayText = other._displayText;
            _textColor = other._textColor;
        }
        #endregion

        #region Methods
        // Method to display text with the specified color
        public void WriteText(bool clear = false)
        {
            Console.CursorVisible = false;
            if (clear)
            {
                Console.Clear();
            }
            Console.ForegroundColor = TextColor;
            Console.WriteLine(DisplayText);
            Console.ResetColor();
        }

        // Method to set the display text and color
        public void SetOutput(string text, ConsoleColor color = ConsoleColor.White)
        {
            DisplayText = text;
            TextColor = color;
        }

        public void ClearConsole()
        {
            Console.CursorVisible = false;
            Console.Clear();
        }

        // The Welcomescreen
        public void WelcomeScreen()
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WindowHeight = 25;
            Console.WindowWidth = 80;
            Console.SetCursorPosition(10, 6);
            Console.WriteLine(Styles.WelcomeArt);

            Console.SetCursorPosition(0, 20);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Styles.PressAnyKeyArt);
            Console.ResetColor();
        }

        public void MainMenue()
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Styles.MainMenueArt);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Styles.StartGameMenueOption);
            Console.WriteLine(Styles.HighScoresMenueOption);
            Console.WriteLine(Styles.ExitMenueOption);
        }

        public void UpdateSnake(List<(int X, int Y)> bodySegments, (int X, int Y) delSegment, ConsoleColor snakeColor = ConsoleColor.Green)
        {
            Console.CursorVisible = false;
            for (int i = 0; i <= 1; i++)
            {
                if (i == 0)
                    Console.ForegroundColor = ConsoleColor.White; // Head color
                else
                    Console.ForegroundColor = snakeColor; // Body color
                Console.SetCursorPosition(bodySegments[i].X, bodySegments[i].Y);
                Console.Write("█");
            }

            // Clear the last segment
            Console.SetCursorPosition(delSegment.X, delSegment.Y);
            Console.Write(" ");
            Console.ResetColor();
        }

        public void GameOutline()
        {
            Console.CursorVisible = false;
            Console.Clear();

            Console.SetCursorPosition(0, 1);
            Console.Write("┌" + new string('─', 78) + "┐");
            for (int i = 2; i < 24; i++)
            {
                if (i == 5)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write(Styles.PressAnyKeyToStart);
                }
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(79, i);
                Console.Write("│");
            }
            Console.Write("└" + new string('─', 78) + "┘");
        }

        public void SpawnFood((int X, int Y) position, ConsoleColor foodColor = ConsoleColor.Red)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = foodColor;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write("█");
            Console.ResetColor();
        }

        public void GameOver()
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.SetCursorPosition(0, 5);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Styles.GameOverArt);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(0, 20);
            Console.WriteLine(Styles.PressAnyKeyArt);
            Console.ResetColor();
        }
        #endregion
    }
}
