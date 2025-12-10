using System.Numerics;
using Snake.Interfaces;

namespace Snake;

/// <summary>
/// Standard food item that appears regularly in the game
/// </summary>
public class Apple : IConsumable
{
    #region Fields
    private Vector2 _position;
    private const int GROWTH_AMOUNT = 3;
    #endregion

    #region IConsumable Implementation
    public (int X, int Y) Position => ((int)_position.X, (int)_position.Y);
    public ConsoleColor Color => ConsoleColor.Red;
    public int GrowthAmount => GROWTH_AMOUNT;
    #endregion

    #region Constructors
    public Apple()
    {
        _position = Vector2.Zero;
    }

    public Apple(Vector2 position)
    {
        _position = position;
    }
    #endregion

    #region IConsumable Methods
    public void Respawn((int xStart, int xEnd, int yStart, int yEnd) gameSize, Random random)
    {
        _position = new Vector2(
            random.Next(gameSize.xStart, gameSize.xEnd + 1),
            random.Next(gameSize.yStart, gameSize.yEnd + 1)
        );
    }

    public bool OnConsume(Snake snake)
    {
        // Standard apple just grows the snake
        snake.Grow(GrowthAmount);
        return true; // Remove after consumption
    }

    public bool ShouldSpawn(DateTime currentTime)
    {
        // Normal apples are always present (managed by FoodManager)
        return true;
    }

    public void Update(DateTime currentTime)
    {
        // Normal apples don't need updates
    }
    #endregion
}
