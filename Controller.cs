using System.Net.Sockets;
using System.Numerics;

namespace Snake
{
    class Controller
    {
        #region Properties
        private Input _kbdInput; // Input handler for managing user input
        private Output _conOutput; // Output handler for displaying messages
        #endregion

        #region Fields
        private Input KbdInput
        {
            get { return _kbdInput; }
            set { _kbdInput = value; }
        }
        private Output ConOutput
        {
            get { return _conOutput; }
            set { _conOutput = value; }
        }
        #endregion

        #region Constructors
        // Default constructor
        public Controller()
        {
            _kbdInput = new Input();
            _conOutput = new Output();
        }
        #endregion

        #region Methods
        // Method to start the controller operations
        public void Run()
        {
            #region Variables
            bool repeat = true;
            bool exit = false;
            ConsoleKeyInfo pressed = new ConsoleKeyInfo();
            int tickLengthMs = 75; // Length of each game tick in milliseconds
            (int Xstart, int Xend, int Ystart, int Yend) gameSize = (1, 78, 2, 23); // Default game size
            ConsoleColor snakeColor = ConsoleColor.Green; // Default snake color
            #endregion

            ConOutput.WelcomeScreen();
            KbdInput.ReadKey();

            // Main menu loop
            do
            {
                repeat = true;
                ConOutput.MainMenue();
                int option = 0;

                do
                {
                    pressed = KbdInput.ReadKey();

                    if (pressed.Key == ConsoleKey.UpArrow)
                    {
                        // Erasing the current option highlight
                        Console.SetCursorPosition(0, 7 + (option * 4));
                        if (option == 0)
                            ConOutput.SetOutput(Styles.StartGameMenueOption, ConsoleColor.White);
                        else if (option == 1)
                            ConOutput.SetOutput(Styles.SettingsMenueOption, ConsoleColor.White);
                        else if (option == 2)
                            ConOutput.SetOutput(Styles.HighScoresMenueOption, ConsoleColor.White);
                        else if (option == 3)
                            ConOutput.SetOutput(Styles.ExitMenueOption, ConsoleColor.White);
                        ConOutput.WriteText();

                        // Setting the new option with wrap-around
                        option = (option - 1 + 4) % 4;

                        // Highlighting the new option
                        Console.SetCursorPosition(0, 7 + (option * 4));
                        if (option == 0)
                            ConOutput.SetOutput(Styles.StartGameMenueOption, ConsoleColor.Green);
                        else if (option == 1)
                            ConOutput.SetOutput(Styles.SettingsMenueOption, ConsoleColor.Green);
                        else if (option == 2)
                            ConOutput.SetOutput(Styles.HighScoresMenueOption, ConsoleColor.Green);
                        else if (option == 3)
                            ConOutput.SetOutput(Styles.ExitMenueOption, ConsoleColor.Green);
                        ConOutput.WriteText();

                    }
                    else if (pressed.Key == ConsoleKey.DownArrow)
                    {
                        // Erasing the current option highlight
                        Console.SetCursorPosition(0, 7 + (option * 4));
                        if (option == 0)
                            ConOutput.SetOutput(Styles.StartGameMenueOption, ConsoleColor.White);
                        else if (option == 1)
                            ConOutput.SetOutput(Styles.SettingsMenueOption, ConsoleColor.White);
                        else if (option == 2)
                            ConOutput.SetOutput(Styles.HighScoresMenueOption, ConsoleColor.White);
                        else if (option == 3)
                            ConOutput.SetOutput(Styles.ExitMenueOption, ConsoleColor.White);
                        ConOutput.WriteText();

                        // Setting the new option with wrap-around
                        option = (option + 1) % 4; // Wrap around the options

                        // Highlighting the new option
                        Console.SetCursorPosition(0, 7 + (option * 4));
                        if (option == 0)
                            ConOutput.SetOutput(Styles.StartGameMenueOption, ConsoleColor.Green);
                        else if (option == 1)
                            ConOutput.SetOutput(Styles.SettingsMenueOption, ConsoleColor.Green);
                        else if (option == 2)
                            ConOutput.SetOutput(Styles.HighScoresMenueOption, ConsoleColor.Green);
                        else if (option == 3)
                            ConOutput.SetOutput(Styles.ExitMenueOption, ConsoleColor.Green);
                        ConOutput.WriteText();

                    }
                    else if (pressed.Key == ConsoleKey.Enter)
                    {
                        // Select the current option
                        if (option == 0)
                        {
                            // Start Game
                            StartGame(tickLengthMs, gameSize, snakeColor);
                            repeat = false;
                        }
                        else if (option == 1)
                        {
                            // Change Settings
                            SettingsMenu(ref tickLengthMs, ref gameSize, ref snakeColor);
                            repeat = false;
                        }
                        else if (option == 2)
                        {
                            // Show Highscores
                            ShowHighscores();
                            repeat = false;
                        }
                        else if (option == 3)
                        {
                            // Exit
                            repeat = false;
                            exit = true;
                        }
                        else
                        {
                            // Error handling for unexpected option values
                            // Setting repeat to false to avoid infinite loop
                            repeat = false;
                        }
                    }

                } while (repeat);
            } while (!exit);
        }

        private void ShowHighscores()
        {
            // Writing the highscores header
            ConOutput.SetOutput("Highscores:\n", ConsoleColor.Yellow);
            ConOutput.WriteText(true);

            if (File.Exists("Highscores/highscores.txt"))
            {
                // Reading highscores from the file and saving them into their variables
                string[] scores = File.ReadAllText("Highscores/highscores.txt").Split(';');
                BigInteger highscoreTicks = BigInteger.Parse(scores[0]);
                int highscoreFood = int.Parse(scores[1]);

                // Converting ticks to a readable time format
                TimeSpan timePlayed = TimeSpan.FromMilliseconds((double)highscoreTicks);

                // Displaying the highscores
                ConOutput.SetOutput($"Longest Time Survived: {timePlayed.Minutes}m {timePlayed.Seconds}s {timePlayed.Milliseconds}ms\n", ConsoleColor.Cyan);
                ConOutput.WriteText();
                ConOutput.SetOutput($"Most Food Eaten: {highscoreFood}\n", ConsoleColor.Cyan);
                ConOutput.WriteText();
            }
            else
            {
                ConOutput.SetOutput("No highscores recorded yet.\n", ConsoleColor.Red);
                ConOutput.WriteText();
            }
            ConOutput.SetOutput("\nPress any key to return to the main menu.", ConsoleColor.Green);
            ConOutput.WriteText();
            KbdInput.ReadKey();
        }

        private void SettingsMenu(ref int tickLengthMs, ref (int Xstart, int Xend, int Ystart, int Yend) gameSize, ref ConsoleColor snakeColor)
        {
            int option = 0;
            ConsoleKeyInfo pressed;
            bool repeat = true;

            #region Initial Settings Menu Display
            ConOutput.ClearConsole();
            ConOutput.SetOutput("SETTINGS MENUE\n", ConsoleColor.Yellow);
            ConOutput.WriteText();

            ConOutput.SetOutput("Tick Speed:", ConsoleColor.Cyan);
            ConOutput.WriteText();
            ConOutput.SetOutput(tickLengthMs.ToString() + "ms ", ConsoleColor.Green);
            ConOutput.WriteText();

            Console.SetCursorPosition(13, 2);
            ConOutput.SetOutput("Game Size:");
            ConOutput.WriteText();
            Console.SetCursorPosition(13, 3);
            ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1}");
            ConOutput.WriteText();
            Console.SetCursorPosition(19, 3);
            ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1}");
            ConOutput.WriteText();

            Console.SetCursorPosition(25, 2);
            ConOutput.SetOutput("Snake Color:");
            ConOutput.WriteText();
            Console.SetCursorPosition(25, 3);
            ConOutput.SetOutput(snakeColor.ToString());
            ConOutput.WriteText();

            Console.SetCursorPosition(0, 7);
            ConOutput.SetOutput("Use Left/Right arrows to navigate options.\n", ConsoleColor.Yellow);
            ConOutput.WriteText();
            ConOutput.SetOutput("Use Up/Down arrows to change values.\n", ConsoleColor.Yellow);
            ConOutput.WriteText();
            ConOutput.SetOutput("Press Enter to return to the main menu.", ConsoleColor.Yellow);
            ConOutput.WriteText();
            #endregion

            // Menu interaction loop
            do
            {
                pressed = KbdInput.ReadKey();

                // If Left or Right arrow is pressed, change the option
                if (pressed.Key == ConsoleKey.LeftArrow)
                {
                    #region Erase current option highlight
                    if (option == 0)
                    {
                        Console.SetCursorPosition(0, 2);
                        ConOutput.SetOutput("Tick Speed:");
                        ConOutput.WriteText();
                        ConOutput.SetOutput(tickLengthMs.ToString() + "ms ");
                        ConOutput.WriteText();
                    }
                    else if (option == 1)
                    {
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:");
                        ConOutput.WriteText();
                        Console.SetCursorPosition(13, 3);
                        ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1}");
                        ConOutput.WriteText();
                    }
                    else if (option == 2)
                    {
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:");
                        ConOutput.WriteText();
                        Console.SetCursorPosition(19, 3);
                        ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1}");
                        ConOutput.WriteText();
                    }
                    else if (option == 3)
                    {
                        Console.SetCursorPosition(25, 2);
                        ConOutput.SetOutput("Snake Color:");
                        ConOutput.WriteText();
                        Console.SetCursorPosition(25, 3);
                        ConOutput.SetOutput(snakeColor.ToString());
                        ConOutput.WriteText();
                    }
                    #endregion

                    option = (option - 1 + 4) % 4; // Wrap around the options

                    #region Highlight new option
                    if (option == 0)
                    {
                        Console.SetCursorPosition(0, 2);
                        ConOutput.SetOutput("Tick Speed:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        ConOutput.SetOutput(tickLengthMs.ToString() + "ms ", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 1)
                    {
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(13, 3);
                        ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1}", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 2)
                    {
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(19, 3);
                        ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1}", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 3)
                    {
                        Console.SetCursorPosition(25, 2);
                        ConOutput.SetOutput("Snake Color:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(25, 3);
                        ConOutput.SetOutput(snakeColor.ToString(), snakeColor);
                        ConOutput.WriteText();
                    }
                    #endregion
                }
                else if (pressed.Key == ConsoleKey.RightArrow)
                {
                    #region Erase current option highlight
                    if (option == 0)
                    {
                        Console.SetCursorPosition(0, 2);
                        ConOutput.SetOutput("Tick Speed:");
                        ConOutput.WriteText();
                        ConOutput.SetOutput(tickLengthMs.ToString() + "ms ");
                        ConOutput.WriteText();
                    }
                    else if (option == 1)
                    {
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:");
                        ConOutput.WriteText();
                        Console.SetCursorPosition(13, 3);
                        ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1}");
                        ConOutput.WriteText();
                    }
                    else if (option == 2)
                    {
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:");
                        ConOutput.WriteText();
                        Console.SetCursorPosition(19, 3);
                        ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1}");
                        ConOutput.WriteText();
                    }
                    else if (option == 3)
                    {
                        Console.SetCursorPosition(25, 2);
                        ConOutput.SetOutput("Snake Color:");
                        ConOutput.WriteText();
                        Console.SetCursorPosition(25, 3);
                        ConOutput.SetOutput(snakeColor.ToString());
                        ConOutput.WriteText();
                    }
                    #endregion

                    option = (option + 1) % 4; // Wrap around the options

                    #region Highlight new option
                    if (option == 0)
                    {
                        Console.SetCursorPosition(0, 2);
                        ConOutput.SetOutput("Tick Speed:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        ConOutput.SetOutput(tickLengthMs.ToString() + "ms ", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 1)
                    {
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(13, 3);
                        ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1}", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 2)
                    {
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(19, 3);
                        ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1}", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 3)
                    {
                        Console.SetCursorPosition(25, 2);
                        ConOutput.SetOutput("Snake Color:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(25, 3);
                        ConOutput.SetOutput(snakeColor.ToString(), snakeColor);
                        ConOutput.WriteText();
                    }
                    #endregion

                }
                else if (pressed.Key == ConsoleKey.UpArrow)
                {
                    if (option == 0)
                    {
                        // Increase Tick Speed
                        if (tickLengthMs < 250)
                            tickLengthMs += 25;
                        Console.SetCursorPosition(0, 2);
                        ConOutput.SetOutput("Tick Speed:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        ConOutput.SetOutput(tickLengthMs.ToString() + "ms ", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 1)
                    {
                        // Increase Game Size X
                        if (gameSize.Xend < 78)
                            gameSize.Xend += 1;
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(13, 3);
                        ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1}", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 2)
                    {
                        // Increase Game Size Y
                        if (gameSize.Yend < 23)
                            gameSize.Yend += 1;
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(19, 3);
                        ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1}", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 3)
                    {
                        // Change Snake Color to the next color
                        snakeColor = (ConsoleColor)(((int)snakeColor + 1) % 16);
                        Console.SetCursorPosition(25, 2);
                        ConOutput.SetOutput("Snake Color:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(25, 3);
                        ConOutput.SetOutput(snakeColor.ToString() + "        ", snakeColor);
                        ConOutput.WriteText();
                    }
                }
                else if (pressed.Key == ConsoleKey.DownArrow)
                {
                    if (option == 0)
                    {
                        // Decrease Tick Speed
                        if (tickLengthMs > 25)
                            tickLengthMs -= 25;
                        Console.SetCursorPosition(0, 2);
                        ConOutput.SetOutput("Tick Speed:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        ConOutput.SetOutput(tickLengthMs.ToString() + "ms ", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 1)
                    {
                        // Decrease Game Size X
                        if (gameSize.Xend > 10)
                            gameSize.Xend -= 1;
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(13, 3);
                        ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1} ", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 2)
                    {
                        // Decrease Game Size Y
                        if (gameSize.Yend > 7)
                            gameSize.Yend -= 1;
                        Console.SetCursorPosition(13, 2);
                        ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(19, 3);
                        ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1} ", ConsoleColor.Green);
                        ConOutput.WriteText();
                    }
                    else if (option == 3)
                    {
                        // Change Snake Color to the previous color
                        snakeColor = (ConsoleColor)(((int)snakeColor - 1 + 16) % 16);
                        Console.SetCursorPosition(25, 2);
                        ConOutput.SetOutput("Snake Color:", ConsoleColor.Cyan);
                        ConOutput.WriteText();
                        Console.SetCursorPosition(25, 3);
                        ConOutput.SetOutput(snakeColor.ToString() + "      ", snakeColor);
                        ConOutput.WriteText();
                    }
                }
                else if (pressed.Key == ConsoleKey.Escape || pressed.Key == ConsoleKey.Enter)
                {
                    repeat = false; // Exit settings menu
                }

            } while (repeat);
        }

        // Method to start the game
        private void StartGame(int tickLengthMs, (int Xstart, int Xend, int Ystart, int Yend) gameSize, ConsoleColor snakeColor)
        {
            #region Game Variables
            Random rand = new Random();
            DateTime startTime = DateTime.Now;
            DateTime nextTick = startTime; // Game ticks
            DateTime nextSuperFood = startTime;
            DateTime gameTime = startTime;
            Snake playerSnake = new Snake(5, (gameSize.Xend / 2 + gameSize.Xstart, gameSize.Yend / 2 + gameSize.Ystart));
            (int X, int Y) lastBodyPosition = (0, 0);
            (int X, int Y) direction = (1, 0); // Initial direction to the right
            List<(int X, int Y)> superFoodPosition = new List<(int X, int Y)> { (rand.Next(gameSize.Xstart, gameSize.Xend), rand.Next(gameSize.Ystart, gameSize.Yend)) };
            (int X, int Y) foodPosition = (rand.Next(gameSize.Xstart, gameSize.Xend), rand.Next(gameSize.Ystart, gameSize.Yend));
            BigInteger ticksPlayed = 0;
            int superFoodCount = 1;
            int length = 5;
            int visibleLength = 2;
            ConsoleKeyInfo pressedKey;
            bool gameOver = false;
            #endregion

            #region Pre-Game Setup and start
            // Setting up the console for the game
            ConOutput.SetSnakeColor(snakeColor);
            ConOutput.GameOutline(gameSize);
            ConOutput.UpdateSnake(playerSnake.GetBodySegments(), (gameSize.Xend / 2 - 2, gameSize.Yend / 2 + gameSize.Ystart));
            KbdInput.ReadKey();

            // Game starts here
            if (gameSize.Xend == 78 && gameSize.Yend == 23)
            {
                // Removing "Press any key to start" text if default game size is used
                for (int i = 5; i < 16; i++)
                {
                    Console.SetCursorPosition(1, i);
                    Console.Write(new string(' ', 76));
                }
            }
            else
            {
                Console.SetCursorPosition(36, 0);
                ConOutput.SetOutput("                           ");
                ConOutput.WriteText();
            }
            startTime = DateTime.Now;
            nextTick = startTime;
            nextSuperFood = startTime.AddSeconds(30);
            ConOutput.SpawnFood(foodPosition);
            ConOutput.SpawnFood(superFoodPosition.Last(), ConsoleColor.Magenta);
            #endregion

            // Main game loop
            do
            {
                #region Moving the snake and handling input
                // Moving the snake based on user input
                playerSnake.Move(direction);

                // Updating the console output
                ConOutput.UpdateSnake(playerSnake.GetBodySegments(), lastBodyPosition);

                // Increasing the visible length of the snake until it reaches its actual length
                if (visibleLength < length)
                {
                    visibleLength++;
                    ConOutput.UpdateSnakeSize(visibleLength);
                }

                // Waiting for the next tick
                while (DateTime.Now < nextTick) ;
                ticksPlayed++;

                if (gameTime.AddSeconds(1) < DateTime.Now)
                {
                    // Updating the game time every second
                    gameTime = gameTime.AddSeconds(1);
                    ConOutput.UpdateGameTime(DateTime.Now - startTime);
                }

                // The next tick
                nextTick = nextTick.AddMilliseconds(tickLengthMs); // Game tick every 75 ms rn

                // Saving the last body position for erasing
                lastBodyPosition = playerSnake.GetBodySegments().Last();

                // Checking for user input to change direction
                if (KbdInput.IsKeyAvailable())
                {
                    pressedKey = KbdInput.ReadKey();
                    switch (pressedKey.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (direction != (0, 1)) // Prevent reversing direction
                                direction = (0, -1);
                            break;
                        case ConsoleKey.DownArrow:
                            if (direction != (0, -1))
                                direction = (0, 1);
                            break;
                        case ConsoleKey.LeftArrow:
                            if (direction != (1, 0))
                                direction = (-1, 0);
                            break;
                        case ConsoleKey.RightArrow:
                            if (direction != (-1, 0))
                                direction = (1, 0);
                            break;
                        case ConsoleKey.Escape:
                            gameOver = true; // Exit the game
                            break;
                        case ConsoleKey.Spacebar:
                            // Pause the game
                            while (!KbdInput.IsKeyAvailable() || KbdInput.ReadKey().Key != ConsoleKey.Spacebar)
                            {
                                // Continuing the Ticks in the pause so the game doesn't speed up after unpausing
                                while (DateTime.Now < nextTick) ;
                                nextTick = nextTick.AddMilliseconds(75); // Game tick every 75 ms
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region Detecting collisions
                // Predicting future head position
                (int X, int Y) futureHeadPosition = playerSnake.GetHead();
                futureHeadPosition.X += direction.X;
                futureHeadPosition.Y += direction.Y;

                // Checking for wall collisions
                if (futureHeadPosition.X < gameSize.Xstart || futureHeadPosition.X > gameSize.Xend || futureHeadPosition.Y < gameSize.Ystart || futureHeadPosition.Y > gameSize.Yend)
                {
                    gameOver = true;
                }

                // Checking if super food timer has expired (every 3 seconds)
                if (DateTime.Now > nextSuperFood)
                {
                    // Spawn new super food and reset timer
                    superFoodPosition.Add((rand.Next(gameSize.Xstart, gameSize.Xend), rand.Next(gameSize.Ystart, gameSize.Yend)));
                    for (int i = 0; i < length; i++)
                    {
                        // Ensure super food does not spawn on the snake or regular food
                        while (superFoodPosition.Last() == playerSnake.GetBodySegments()[i] || superFoodPosition.Last() == foodPosition)
                        {
                            superFoodPosition.RemoveAt(superFoodPosition.Count - 1);
                            superFoodPosition.Add((rand.Next(gameSize.Xstart, gameSize.Xend), rand.Next(gameSize.Ystart, gameSize.Yend)));
                            i = 0; // Restart checking from the beginning
                        }
                    }
                    ConOutput.SpawnFood(superFoodPosition.Last(), ConsoleColor.Magenta);
                    nextSuperFood = DateTime.Now.AddSeconds(30);
                    superFoodCount++;
                }

                // Detecting food consumption
                if (futureHeadPosition == foodPosition)
                {
                    playerSnake.Grow();
                    length += 3;

                    // Spawn new food in the game area
                    foodPosition = (rand.Next(gameSize.Xstart, gameSize.Xend), rand.Next(gameSize.Ystart, gameSize.Yend));
                    for (int i = 0; i < length; i++)
                    {
                        // Ensure food does not spawn on the snake
                        while (foodPosition == playerSnake.GetBodySegments()[i])
                        {
                            foodPosition = (rand.Next(gameSize.Xstart, gameSize.Xend), rand.Next(gameSize.Ystart, gameSize.Yend));
                            i = 0; // Restart checking from the beginning
                        }
                    }
                    ConOutput.SpawnFood(foodPosition);
                }
                else if (superFoodCount > 0)
                {
                    // If there is super food on the field, check for its consumption
                    // Detecting super food consumption
                    for (int i = 0; i < superFoodCount; i++)
                    {
                        if (futureHeadPosition == superFoodPosition[i])
                        {
                            playerSnake.Grow();
                            playerSnake.Grow(); // Super food grows the snake by 3*3 segments
                            playerSnake.Grow();
                            length += 9;

                            // Remove the consumed super food from the list
                            superFoodPosition.RemoveAt(i);
                            superFoodCount--;
                            break;
                        }
                    }
                }

                // Detecting self-collision
                for (int i = 3; i < length; i++)
                {
                    if (futureHeadPosition == playerSnake.GetBodySegments()[i])
                    {
                        gameOver = true;
                        break;
                    }
                }
                #endregion
            } while (!gameOver);

            #region Post-Game Operations
            // Saving the score
            ticksPlayed *= tickLengthMs; // Total time played in milliseconds

            // Waiting for 300ms before showing Game Over screen
            nextTick = nextTick.AddMilliseconds(300);
            while (DateTime.Now < nextTick) ;

            // Ensure Highscores directory and file exist
            if (!Directory.Exists("Highscores"))
            {
                Directory.CreateDirectory("Highscores");
                File.WriteAllText("Highscores/highscores.txt", "0;0");
            }
            else if (!File.Exists("Highscores/highscores.txt"))
            {
                File.WriteAllText("Highscores/highscores.txt", "0;0");
            }

            // Reading existing highscores
            BigInteger highscoreTicks = BigInteger.Parse(File.ReadAllText("Highscores/highscores.txt").Split(';')[0]);
            int highscoreFood = int.Parse(File.ReadAllText("Highscores/highscores.txt").Split(';')[1]);

            // Updating highscores if beaten
            if (highscoreFood < visibleLength)
            {
                highscoreFood = visibleLength;
            }
            if (highscoreTicks < ticksPlayed)
            {
                highscoreTicks = ticksPlayed;
            }

            // Writing updated highscores back to the file
            File.WriteAllText("Highscores/highscores.txt", highscoreTicks.ToString() + ";" + highscoreFood.ToString(), System.Text.Encoding.UTF8);

            ConOutput.GameOver();
            KbdInput.ReadKey();

            // Waiting for 300ms before ending Game Over screen
            nextTick = nextTick.AddMilliseconds(300);
            while (DateTime.Now < nextTick) ;
            #endregion
        }
        #endregion
    }
}
