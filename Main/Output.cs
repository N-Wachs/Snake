namespace Snake
{
    class Output
    {
        #region Fields
        private string _displayText;      // Text to be displayed
        private ConsoleColor _textColor;  // Color of the text
        private ConsoleColor _snakeColor; // Color of the snake
        private bool _cursorVisible;      // Cursor visibility
        private Input _kbInput;           // Keyboard input handler
        #endregion

        #region Properties
        public string DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; }
        }

        public ConsoleColor TextColor
        {
            get { return _textColor; }
            set { _textColor = value; Console.ForegroundColor = value; }
        }

        public ConsoleColor SnakeColor
        {
            get { return _snakeColor; }
            set { _snakeColor = value; }
        }

        public bool CursorVisible
        {
            get { return _cursorVisible; }
            set { _cursorVisible = value; Console.CursorVisible = value; }
        }

        private Input KbdInput
        {
            get
            {
                if (_kbInput == null)
                {
                    _kbInput = new Input();
                }
                return _kbInput;
            }
            set { _kbInput = value; }
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
        public void WriteText(string output = "", bool clear = false)
        {
            CursorVisible = false;
            if (clear)
            {
                Clear();
            }
            if (output == "")
                output = DisplayText;

            Console.WriteLine(output);
            ResetColor();
        }
        public void WriteLine(string output = "", bool clear = false)
        {
            CursorVisible = false;
            if (clear)
            {
                Clear();
            }
            if (output == "")
                output = DisplayText;

            Console.WriteLine(output);
            ResetColor();
        }

        public void ResetColor() => TextColor = ConsoleColor.White;
        public void SetPos(int x, int y) => Console.SetCursorPosition(x, y);
        public void SetPos((int x, int y) pos) => Console.SetCursorPosition(pos.x, pos.y);
        public (int x, int y) GetPos() => Console.GetCursorPosition();

        // Method to set the display text and color
        public void SetOutput(string text, ConsoleColor color = ConsoleColor.White)
        {
            DisplayText = text;
            TextColor = color;
        }

        public void Clear() => Console.Clear();

        // The Welcomescreen
        public void WelcomeScreen()
        {
            CursorVisible = false;
            Clear();
            TextColor = ConsoleColor.Green;
            Console.WindowHeight = 25;
            Console.WindowWidth = 80;
            SetPos(10, 6);
            WriteLine(Styles.WelcomeArt);

            SetPos(0, 20);
            TextColor = ConsoleColor.Cyan;
            WriteLine(Styles.PressAnyKeyArt);
            ResetColor();
        }

        public int MainMenu()
        {
            CursorVisible = false;
            Clear();
            TextColor = ConsoleColor.Yellow;
            WriteLine(Styles.MainMenueArt);

            TextColor = ConsoleColor.Green;
            WriteLine(Styles.StartGameMenueOption);
            ResetColor();
            WriteLine(Styles.SettingsMenueOption);
            WriteLine(Styles.HighScoresMenueOption);
            WriteLine(Styles.ExitMenueOption);

            bool repeat = true;
            int option = 0;

            do
            {
                ConsoleKeyInfo pressed = KbdInput.ReadKey();

                if (pressed.Key == ConsoleKey.UpArrow || pressed.Key == ConsoleKey.W)
                {
                    // Erasing the current option highlight
                    SetPos(0, 7 + (option * 4));
                    if (option == 0)
                        SetOutput(Styles.StartGameMenueOption, ConsoleColor.White);
                    else if (option == 1)
                        SetOutput(Styles.SettingsMenueOption, ConsoleColor.White);
                    else if (option == 2)
                        SetOutput(Styles.HighScoresMenueOption, ConsoleColor.White);
                    else if (option == 3)
                        SetOutput(Styles.ExitMenueOption, ConsoleColor.White);
                    WriteText();

                    // Setting the new option with wrap-around
                    option = (option - 1 + 4) % 4;

                    // Highlighting the new option
                    SetPos(0, 7 + (option * 4));
                    if (option == 0)
                        SetOutput(Styles.StartGameMenueOption, ConsoleColor.Green);
                    else if (option == 1)
                        SetOutput(Styles.SettingsMenueOption, ConsoleColor.Green);
                    else if (option == 2)
                        SetOutput(Styles.HighScoresMenueOption, ConsoleColor.Green);
                    else if (option == 3)
                        SetOutput(Styles.ExitMenueOption, ConsoleColor.Green);
                    WriteText();

                }
                else if (pressed.Key == ConsoleKey.DownArrow || pressed.Key == ConsoleKey.S)
                {
                    // Erasing the current option highlight
                    SetPos(0, 7 + (option * 4));
                    if (option == 0)
                        SetOutput(Styles.StartGameMenueOption, ConsoleColor.White);
                    else if (option == 1)
                        SetOutput(Styles.SettingsMenueOption, ConsoleColor.White);
                    else if (option == 2)
                        SetOutput(Styles.HighScoresMenueOption, ConsoleColor.White);
                    else if (option == 3)
                        SetOutput(Styles.ExitMenueOption, ConsoleColor.White);
                    WriteText();

                    // Setting the new option with wrap-around
                    option = (option + 1) % 4; // Wrap around the options

                    // Highlighting the new option
                    SetPos(0, 7 + (option * 4));
                    if (option == 0)
                        SetOutput(Styles.StartGameMenueOption, ConsoleColor.Green);
                    else if (option == 1)
                        SetOutput(Styles.SettingsMenueOption, ConsoleColor.Green);
                    else if (option == 2)
                        SetOutput(Styles.HighScoresMenueOption, ConsoleColor.Green);
                    else if (option == 3)
                        SetOutput(Styles.ExitMenueOption, ConsoleColor.Green);
                    WriteText();

                }
                else if (pressed.Key == ConsoleKey.Enter)
                {
                    // Select the current option
                    if (option >= 0 && option <= 3)
                    {
                        return option; // Exit the method to proceed with the selected option
                    }
                    else
                    {
                        // Error handling for unexpected option values
                        // Setting repeat to false to avoid infinite loop
                        throw new InvalidOperationException("Invalid menu option selected. HOW???");
                    }
                }

            } while (repeat);

            return option;
        }

        public void SetSnakeColor(ConsoleColor color) => SnakeColor = color;

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
