using System.Numerics;
using Snake.Managers;

namespace Snake;

public class GameController
{
    #region Fields
    private readonly Output _conOutput;
    private readonly Input _conInput;
    private readonly Random _random;
    #endregion

    #region Constructors
    public GameController()
    {
        _conOutput = new Output();
        _conInput = new Input();
        _random = new Random();
    }

    public GameController(Output output, Input input)
    {
        _conOutput = output;
        _conInput = input;
        _random = new Random();
    }
    #endregion

    #region Methods
    public void StartGame(int tickLengthMs, (int Xstart, int Xend, int Ystart, int Yend) gameSize, ConsoleColor snakeColor)
    {
        #region Game Variables
        DateTime startTime = DateTime.UtcNow;
        DateTime nextTick = startTime;
        DateTime gameTime = startTime;
        
        Snake playerSnake = new Snake(5, (gameSize.Xend / 2 + gameSize.Xstart, gameSize.Yend / 2 + gameSize.Ystart));
        (int X, int Y) lastBodyPosition = (0, 0);
        (int X, int Y) direction = (1, 0);
        
        // Initialize Food Manager
        FoodManager foodManager = new FoodManager(gameSize, _random);
        
        // Register consumables
        foodManager.RegisterConsumable(new Apple()); // Normal apple always present
        foodManager.RegisterConsumable(new SuperApple()); // Super apple spawns periodically
        
        BigInteger ticksPlayed = 0;
        int visibleLength = 2;
        bool gameOver = false;
        #endregion

        #region Pre-Game Setup
        _conOutput.SetSnakeColor(snakeColor);
        _conOutput.GameOutline(gameSize);
        _conOutput.UpdateSnake(playerSnake.GetBodySegments(), (gameSize.Xend / 2 - 2, gameSize.Yend / 2 + gameSize.Ystart));
        _conInput.ReadKey();

        // Clear start message
        if (gameSize.Xend == 78 && gameSize.Yend == 23)
        {
            for (int i = 5; i < 16; i++)
            {
                _conOutput.ClearArea(1, i, 76);
            }
        }
        else
        {
            _conOutput.WriteAt(36, 0, "                           ");
        }
        
        startTime = DateTime.UtcNow;
        nextTick = startTime;
        gameTime = startTime;
        
        // Initial food spawn
        foodManager.Update(DateTime.UtcNow);
        foreach (var consumable in foodManager.GetActiveConsumables())
        {
            _conOutput.SpawnFood(consumable.Position, consumable.Color);
        }
        #endregion

        // Main game loop
        do
        {
            #region Snake Movement
            playerSnake.Move(direction);
            _conOutput.UpdateSnake(playerSnake.GetBodySegments(), lastBodyPosition);

            if (visibleLength < playerSnake.Length)
            {
                visibleLength++;
                _conOutput.UpdateSnakeSize(visibleLength);
            }

            // Timing
            var delayTime = nextTick - DateTime.UtcNow;
            if (delayTime > TimeSpan.Zero)
            {
                Thread.Sleep(delayTime);
            }
            ticksPlayed++;

            if (gameTime.AddSeconds(1) < DateTime.UtcNow)
            {
                gameTime = gameTime.AddSeconds(1);
                _conOutput.UpdateGameTime(DateTime.UtcNow - startTime);
            }

            nextTick = nextTick.AddMilliseconds(tickLengthMs);
            lastBodyPosition = playerSnake.GetBodySegments().Last();
            #endregion

            #region Input Handling
            if (_conInput.IsKeyAvailable())
            {
                var pressedKey = _conInput.ReadKey();
                
                if (pressedKey.Key == ConsoleKey.Escape)
                {
                    gameOver = true;
                }
                else if (pressedKey.Key == ConsoleKey.Spacebar)
                {
                    HandlePause(ref nextTick, tickLengthMs);
                }
                else
                {
                    direction = pressedKey.Key switch
                    {
                        ConsoleKey.UpArrow or ConsoleKey.W when direction != (0, 1) => (0, -1),
                        ConsoleKey.DownArrow or ConsoleKey.S when direction != (0, -1) => (0, 1),
                        ConsoleKey.LeftArrow or ConsoleKey.A when direction != (1, 0) => (-1, 0),
                        ConsoleKey.RightArrow or ConsoleKey.D when direction != (-1, 0) => (1, 0),
                        _ => direction
                    };
                }
            }
            #endregion

            #region Collision Detection
            var futureHead = (X: playerSnake.GetHead().X + direction.X, Y: playerSnake.GetHead().Y + direction.Y);

            // Wall collision
            if (futureHead.X < gameSize.Xstart || futureHead.X > gameSize.Xend || 
                futureHead.Y < gameSize.Ystart || futureHead.Y > gameSize.Yend)
            {
                gameOver = true;
                continue;
            }

            // Self collision
            if (playerSnake.CheckSelfCollision(futureHead))
            {
                gameOver = true;
                continue;
            }

            // Food collision
            var consumedItem = foodManager.CheckCollision(futureHead);
            if (consumedItem != null)
            {
                foodManager.ConsumeItem(consumedItem, playerSnake);
            }

            // Update food manager (for timed spawns)
            foodManager.UpdateOccupiedPositions(playerSnake.GetBodySegmentsSet());
            foodManager.Update(DateTime.UtcNow);
            
            // Render any newly spawned food
            foreach (var consumable in foodManager.GetActiveConsumables())
            {
                _conOutput.SpawnFood(consumable.Position, consumable.Color);
            }
            #endregion
        } while (!gameOver);

        // Game over handling
        HandleGameOver(ticksPlayed * tickLengthMs, visibleLength, nextTick);
    }

    private (int, int) HandlePause(ref DateTime nextTick, int tickLengthMs)
    {
        while (!_conInput.IsKeyAvailable() || _conInput.ReadKey().Key != ConsoleKey.Spacebar)
        {
            var delayTime = nextTick - DateTime.UtcNow;
            if (delayTime > TimeSpan.Zero)
            {
                Thread.Sleep(delayTime);
            }
            nextTick = nextTick.AddMilliseconds(tickLengthMs);
        }
        return (0, 0); // Return dummy direction (not used)
    }

    private void HandleGameOver(BigInteger totalTicks, int finalScore, DateTime nextTick)
    {
        // Wait before showing game over
        var delayTime = nextTick.AddMilliseconds(300) - DateTime.UtcNow;
        if (delayTime > TimeSpan.Zero)
        {
            Thread.Sleep(delayTime);
        }

        // Handle highscores
        EnsureHighscoreFileExists();
        UpdateHighscores(totalTicks, finalScore);

        _conOutput.GameOver();
        Thread.Sleep(300);
        _conInput.ClearInputBuffer();
        _conInput.ReadKey();
    }

    private void EnsureHighscoreFileExists()
    {
        if (!Directory.Exists("Highscores"))
        {
            Directory.CreateDirectory("Highscores");
        }
        
        if (!File.Exists("Highscores/highscores.txt"))
        {
            File.WriteAllText("Highscores/highscores.txt", "0;0");
        }
    }

    private void UpdateHighscores(BigInteger totalTicks, int finalScore)
    {
        string[] scores = File.ReadAllText("Highscores/highscores.txt").Split(';');
        BigInteger highscoreTicks = BigInteger.Parse(scores[0]);
        int highscoreFood = int.Parse(scores[1]);

        if (finalScore > highscoreFood)
        {
            highscoreFood = finalScore;
        }
        if (totalTicks > highscoreTicks)
        {
            highscoreTicks = totalTicks;
        }

        File.WriteAllText("Highscores/highscores.txt", 
            $"{highscoreTicks};{highscoreFood}", 
            System.Text.Encoding.UTF8);
    }
    #endregion
}
