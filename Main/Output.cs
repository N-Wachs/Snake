namespace Snake
{
    class Output
    {
        #region Fields
        private string _displayText;      // Text to be displayed
        private ConsoleColor _textColor;  // Color of the text
        private ConsoleColor _snakeColor; // Color of the snake
        #endregion

        #region Properties
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

        private ConsoleColor SnakeColor
        {
            get { return _snakeColor; }
            set { _snakeColor = value; }
        }
        #endregion

        #region Constructors
        // Default constructor
        public Output()
        {
            _displayText = "";
            _textColor = ConsoleColor.White;
            _snakeColor = ConsoleColor.Green;
        }

        // Copy constructor
        public Output(Output other)
        {
            _displayText = other._displayText;
            _textColor = other._textColor;
            _snakeColor = other._snakeColor;
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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Styles.StartGameMenueOption);
            Console.ResetColor();
            Console.WriteLine(Styles.SettingsMenueOption);
            Console.WriteLine(Styles.HighScoresMenueOption);
            Console.WriteLine(Styles.ExitMenueOption);
        }

        public void SetSnakeColor(ConsoleColor color)
        {
            SnakeColor = color;
        }

        public void UpdateSnake(List<(int X, int Y)> bodySegments, (int X, int Y) delSegment)
        {
            Console.CursorVisible = false;
            for (int i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    if (SnakeColor == ConsoleColor.White || SnakeColor == ConsoleColor.Gray)
                        Console.ForegroundColor = ConsoleColor.DarkGray; // Head color
                    else
                        Console.ForegroundColor = ConsoleColor.White; // Head color
                }
                else if (SnakeColor == ConsoleColor.Black)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray; // Body color
                }
                else
                {
                    Console.ForegroundColor = SnakeColor; // Body color
                }
                Console.SetCursorPosition(bodySegments[i].X, bodySegments[i].Y);
                Console.Write("█");
            }

            // Clear the last segment
            Console.SetCursorPosition(delSegment.X, delSegment.Y);
            Console.Write(" ");
            Console.ResetColor();
        }

        public void GameOutline((int Xstart, int Xend, int Ystart, int Yend) gameSize)
        {
            Console.CursorVisible = false;
            Console.Clear();
            bool defaultSize = false;
            ConsoleColor color = (ConsoleColor)(DateTime.Now.Ticks % 14);
            if (color == ConsoleColor.Black)
                color = ConsoleColor.DarkGray;

            // Check if the game size is the default size
            if (gameSize == (1, 78, 2, 23))
            {
                defaultSize = true;
            }

            Console.Write("Time Played: 00:00:00   Size: 005");
            if (!defaultSize)
                Console.Write("    Contine with any Key...");

            Console.ForegroundColor = color;
            Console.SetCursorPosition(0, gameSize.Ystart - 1);
            Console.Write("┌" + new string('─', gameSize.Xend) + "┐");
            for (int i = gameSize.Ystart; i < gameSize.Yend + 1; i++)
            {
                if (i == 5 && defaultSize)
                {
                    // Display "Press Any Key to Start" in the middle of the game area if the gamesize is the default size
                    Console.SetCursorPosition(0, i);
                    Console.Write(Styles.PressAnyKeyToStart);
                }
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(gameSize.Xend + 1, i);
                Console.Write("│");
            }
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top + 1);
            Console.Write("└" + new string('─', gameSize.Xend) + "┘");
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

        public void UpdateGameTime(TimeSpan timeSpan)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Time Played: {timeSpan:hh\\:mm\\:ss}   ");
        }

        public void UpdateSnakeSize(int size)
        {
            Console.SetCursorPosition(24, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Size: ");
            if (size < 10)
                Console.Write($"00{size}");
            else if (size < 100)
                Console.Write($"0{size}");
            else
                Console.Write($"{size}");
        }
        #endregion
    }
}
