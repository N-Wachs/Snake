using System.Numerics;

namespace Snake;

public class GameController
{
    #region Fields
    private Output _conOutput; // Output handler for displaying messages
    private Input _conInput; // Input handler for reading user input
    #endregion

    #region Properties
    private Output ConOutput
    {
        get { return _conOutput; }
        set { _conOutput = value; }
    }
    private Input KbdInput
    {
        get { return _conInput; }
        set { _conInput = value; }
    }
    #endregion

    #region Constructors
    // Default constructor
    public GameController()
    {
        _conOutput = new Output();
        _conInput = new Input();
    }

    // Parameterized constructor
    public GameController(Output output, Input input)
    {
        _conOutput = output;
        _conInput = input;
    }

    // Copy constructor
    public GameController(GameController other)
    {
        _conOutput = new Output(other._conOutput);
        _conInput = new Input(other._conInput);
    }
    #endregion

    #region Methods
    // Method to start the game
    public void StartGame(int tickLengthMs, (int Xstart, int Xend, int Ystart, int Yend) gameSize, ConsoleColor snakeColor)
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
                ConOutput.ClearArea(1, i, 76);
            }
        }
        else
        {
            ConOutput.WriteAt(36, 0, "                           ");
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
                    case ConsoleKey.W:
                        if (direction != (0, 1)) // Prevent reversing direction
                            direction = (0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction != (0, -1))
                            direction = (0, 1);
                        break;
                    case ConsoleKey.S:
                        if (direction != (0, -1))
                            direction = (0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction != (1, 0))
                            direction = (-1, 0);
                        break;
                    case ConsoleKey.A:
                        if (direction != (1, 0))
                            direction = (-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction != (-1, 0))
                            direction = (1, 0);
                        break;
                    case ConsoleKey.D:
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
