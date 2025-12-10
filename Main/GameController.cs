using System.Numerics;

namespace Snake;

public class GameController
{
    #region Fields
    private Output _conOutput; // Output handler for displaying messages
    private Input _conInput; // Input handler for reading user input
    private Random _random; // Random number generator for object pooling
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
        _random = new Random();
    }

    // Parameterized constructor
    public GameController(Output output, Input input)
    {
        _conOutput = output;
        _conInput = input;
        _random = new Random();
    }

    // Copy constructor
    public GameController(GameController other)
    {
        _conOutput = new Output(other._conOutput);
        _conInput = new Input(other._conInput);
        _random = new Random();
    }
    #endregion

    #region Methods
    // Method to start the game
    public void StartGame(int tickLengthMs, (int Xstart, int Xend, int Ystart, int Yend) gameSize, ConsoleColor snakeColor)
    {
        #region Game Variables
        DateTime startTime = DateTime.UtcNow;
        DateTime nextTick = startTime;
        DateTime nextSuperFood = startTime;
        DateTime gameTime = startTime;
        Snake playerSnake = new Snake(5, (gameSize.Xend / 2 + gameSize.Xstart, gameSize.Yend / 2 + gameSize.Ystart));
        (int X, int Y) lastBodyPosition = (0, 0);
        (int X, int Y) direction = (1, 0); // Initial direction to the right
        List<SuperApple> specialFoods = new List<SuperApple>();
        specialFoods.Add(new SuperApple());
        specialFoods[0].SetApple(gameSize, _random);
        List<SuperApple> superApplePool = new List<SuperApple>();
        Apple normalFood = new Apple();
        normalFood.SetApple(gameSize, _random);
        BigInteger ticksPlayed = 0;
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
        startTime = DateTime.UtcNow;
        nextTick = startTime;
        nextSuperFood = startTime.AddSeconds(30);
        ConOutput.SpawnFood(normalFood.GetPositionTuple(), normalFood.AppleColor);
        ConOutput.SpawnFood(specialFoods[0].GetPositionTuple(), specialFoods[0].AppleColor);
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
            while (DateTime.UtcNow < nextTick) ;
            ticksPlayed++;

            if (gameTime.AddSeconds(1) < DateTime.UtcNow)
            {
                // Updating the game time every second
                gameTime = gameTime.AddSeconds(1);
                ConOutput.UpdateGameTime(DateTime.UtcNow - startTime);
            }

            // The next tick
            nextTick = nextTick.AddMilliseconds(tickLengthMs);

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
                            while (DateTime.UtcNow < nextTick) ;
                            nextTick = nextTick.AddMilliseconds(tickLengthMs);
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

            // Checking if super food timer has expired (every 30 seconds)
            if (DateTime.UtcNow > nextSuperFood)
            {
                // Spawn new super food and reset timer - use pooled object if available
                SuperApple newSuperApple;
                if (superApplePool.Count > 0)
                {
                    newSuperApple = superApplePool[superApplePool.Count - 1];
                    superApplePool.RemoveAt(superApplePool.Count - 1);
                }
                else
                {
                    newSuperApple = new SuperApple();
                }
                
                newSuperApple.SetApple(gameSize, _random);
                
                // Ensure super food does not spawn on the snake or regular food
                bool validPosition = false;
                while (!validPosition)
                {
                    validPosition = true;
                    for (int i = 0; i < length; i++)
                    {
                        if (newSuperApple.GetPositionTuple() == playerSnake.GetBodySegments()[i] || 
                            newSuperApple.GetPositionTuple() == normalFood.GetPositionTuple())
                        {
                            newSuperApple.SetApple(gameSize, _random);
                            validPosition = false;
                            break;
                        }
                    }
                }
                
                specialFoods.Add(newSuperApple);
                ConOutput.SpawnFood(newSuperApple.GetPositionTuple(), newSuperApple.AppleColor);
                nextSuperFood = DateTime.UtcNow.AddSeconds(30);
            }

            // Detecting normal food consumption
            if (futureHeadPosition == normalFood.GetPositionTuple())
            {
                playerSnake.Grow();
                length += normalFood.GetLengthAdded();

                // Spawn new food in the game area
                normalFood.SetApple(gameSize, _random);
                
                // Ensure food does not spawn on the snake
                bool validPosition = false;
                while (!validPosition)
                {
                    validPosition = true;
                    for (int i = 0; i < length; i++)
                    {
                        if (normalFood.GetPositionTuple() == playerSnake.GetBodySegments()[i])
                        {
                            normalFood.SetApple(gameSize, _random);
                            validPosition = false;
                            break;
                        }
                    }
                }
                ConOutput.SpawnFood(normalFood.GetPositionTuple(), normalFood.AppleColor);
            }

            // Detecting super food consumption
            if (specialFoods.Count > 0)
            {
                for (int i = specialFoods.Count - 1; i >= 0; i--)
                {
                    if (futureHeadPosition == specialFoods[i].GetPositionTuple())
                    {
                        playerSnake.Grow();
                        playerSnake.Grow(); // Super food grows the snake by 3*3 segments
                        playerSnake.Grow();
                        length += specialFoods[i].GetLengthAdded();

                        // Return consumed super food to pool instead of discarding
                        superApplePool.Add(specialFoods[i]);
                        specialFoods.RemoveAt(i);
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
        while (DateTime.UtcNow < nextTick) ;

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

        // Waiting for 300ms before ending Game Over screen
        Thread.Sleep(300);
        KbdInput.ClearInputBuffer();
        KbdInput.ReadKey();
        while (DateTime.UtcNow < nextTick) ;
        #endregion
    }
    #endregion
}
