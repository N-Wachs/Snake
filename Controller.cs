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
            bool repeat = true;
            bool exit = false;
            ConsoleKeyInfo pressed = new ConsoleKeyInfo();

            ConOutput.WelcomeScreen();
            KbdInput.ReadKey();

            do
            {
                repeat = true;
                ConOutput.MainMenue();

                do
                {
                    pressed = KbdInput.ReadKey();

                    if (pressed.KeyChar == '1')
                    {
                        // Start Game
                        StartGame();
                        repeat = false;
                    }
                    else if (pressed.KeyChar == '2')
                    {
                        // Show Highscores
                        ShowHighscores();
                        repeat = false;
                    }
                    else if (pressed.KeyChar == '3')
                    {
                        // Exit
                        repeat = false;
                        exit = true;
                    }
                    else
                    {
                        // Invalid Input
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

        // Method to start the game
        private void StartGame()
        {
            #region Game Variables
            DateTime startTime = DateTime.Now;
            DateTime nextTick = startTime.AddMilliseconds(350); // Game tick every 75 ms
            Snake playerSnake = new Snake(5, (40, 17));
            (int X, int Y) lastBodyPosition = (0, 0);
            (int X, int Y) direction = (1, 0); // Initial direction to the right
            bool gameOver = false;
            ConsoleKeyInfo pressedKey;
            Random rand = new Random();
            (int X, int Y) foodPosition = (rand.Next(1, 78), rand.Next(2, 23));
            BigInteger ticksPlayed = 0;
            int eatenFood = 0;
            #endregion

            ConOutput.GameOutline();
            ConOutput.UpdateSnake(playerSnake.GetBodySegments(), (10, 17));
            KbdInput.ReadKey();

            // Game starts here
            for (int i = 5; i < 16; i++)
            {
                // Removing "Press any key to start" text
                Console.SetCursorPosition(1, i);
                Console.Write(new string(' ', 76));
            }
            startTime = DateTime.Now;
            nextTick = startTime;
            ConOutput.SpawnFood(foodPosition);

            // Main game loop
            do
            {
                // Moving the snake based on user input
                playerSnake.Move(direction);

                // Updating the console output
                ConOutput.UpdateSnake(playerSnake.GetBodySegments(), lastBodyPosition);

                // Waiting for the next tick
                while (DateTime.Now < nextTick) ;
                ticksPlayed++;


                // The next tick
                nextTick = nextTick.AddMilliseconds(75); // Game tick every 75 ms

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

                // Detecting collisions with walls
                // Predicting future head position
                (int X, int Y) futureHeadPosition = playerSnake.GetHead();
                futureHeadPosition.X += direction.X;
                futureHeadPosition.Y += direction.Y;

                // Checking for wall collisions
                if (futureHeadPosition.X < 1 || futureHeadPosition.X > 78 || futureHeadPosition.Y < 2 || futureHeadPosition.Y > 23)
                {
                    gameOver = true;
                }

                // Detecting food consumption
                if (futureHeadPosition == foodPosition)
                {
                    playerSnake.Grow();
                    eatenFood++;

                    // Spawn new food
                    foodPosition = (rand.Next(1, 78), rand.Next(2, 23));
                    for (int i = 0; i < playerSnake.GetBodySegments().Count; i++)
                    {
                        // Ensure food does not spawn on the snake
                        while (foodPosition == playerSnake.GetBodySegments()[i])
                        {
                            foodPosition = (rand.Next(1, 78), rand.Next(2, 23));
                            i = 0; // Restart checking from the beginning
                        }
                    }
                    ConOutput.SpawnFood(foodPosition);
                }

                // Detecting self-collision
                for (int i = 3; i < playerSnake.GetBodySegments().Count; i++)
                {
                    if (futureHeadPosition == playerSnake.GetBodySegments()[i])
                    {
                        gameOver = true;
                        break;
                    }
                }
            } while (!gameOver);

            // Game's over
            // Saving the score
            ticksPlayed *= 75; // Total time played in milliseconds

            // Ensure Highscores directory and file exist
            if (!Directory.Exists("Highscores"))
            {
                Directory.CreateDirectory("Highscores");
                File.Create("Highscores/highscores.txt").Close();
            }
            else if (!File.Exists("Highscores/highscores.txt"))
            {
                File.Create("Highscores/highscores.txt").Close();
            }
            else
            {
                // Reading existing highscores
                BigInteger highscoreTicks = BigInteger.Parse(File.ReadAllText("Highscores/highscores.txt").Split(';')[0]);
                int highscoreFood = int.Parse(File.ReadAllText("Highscores/highscores.txt").Split(';')[1]);

                // Updating highscores if beaten
                if (highscoreFood < eatenFood)
                {
                    highscoreFood = eatenFood;
                }
                if (highscoreTicks < ticksPlayed)
                {
                    highscoreTicks = ticksPlayed;
                }

                // Writing updated highscores back to the file
                File.WriteAllText("Highscores/highscores.txt", highscoreTicks.ToString() + ";" + highscoreFood.ToString(), System.Text.Encoding.UTF8);
            }

            ConOutput.GameOver();
            KbdInput.ReadKey();
        }
        #endregion
    }
}
