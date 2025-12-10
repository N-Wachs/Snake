using System.Numerics;
using Snake.Interfaces;

namespace Snake;

/// <summary>
/// Special food item that spawns periodically and gives more growth
/// </summary>
public class SuperApple : IConsumable
{
    #region Fields
    private Vector2 _position;
    private DateTime _spawnTime;
    private DateTime _nextSpawnTime;
    private bool _isActive;
    private const int GROWTH_AMOUNT = 9;
    private const int RESPAWN_SECONDS = 30;
    #endregion

    #region IConsumable Implementation
    public (int X, int Y) Position => ((int)_position.X, (int)_position.Y);
    public ConsoleColor Color => ConsoleColor.Magenta;
    public int GrowthAmount => GROWTH_AMOUNT;
    #endregion

    #region Properties
    public bool IsActive => _isActive;
    #endregion

    #region Constructors
    public SuperApple()
    {
        _position = Vector2.Zero;
        _isActive = false;
        _nextSpawnTime = DateTime.UtcNow.AddSeconds(RESPAWN_SECONDS);
    }

    public SuperApple(Vector2 position)
    {
        _position = position;
        _isActive = true;
        _spawnTime = DateTime.UtcNow;
        _nextSpawnTime = DateTime.UtcNow.AddSeconds(RESPAWN_SECONDS);
    }
    #endregion

    #region IConsumable Methods
    /// <summary>
    /// Respawns the consumable at a random position within the specified game area and marks it as active.
    /// </summary>
    /// <remarks>Calling this method resets the consumable's position and activation state. The spawn time is
    /// updated to the current UTC time.</remarks>
    /// <param name="gameSize">A tuple specifying the inclusive start and end coordinates for the x and y axes of the game area. The consumable
    /// will be placed within these bounds.</param>
    /// <param name="random">The random number generator used to select the new position within the specified area. Cannot be null.</param>
    public void Respawn((int xStart, int xEnd, int yStart, int yEnd) gameSize, Random random)
    {
        _position = new Vector2(
            random.Next(gameSize.xStart, gameSize.xEnd + 1),
            random.Next(gameSize.yStart, gameSize.yEnd + 1)
        );
        _isActive = true;
        _spawnTime = DateTime.UtcNow;
    }

    /// <summary>
    /// Consumes the super apple, applying its effects to the specified snake and removing the apple from play.
    /// </summary>
    /// <remarks>After consumption, the snake grows by a predefined amount and any special effects associated
    /// with the super apple are triggered. The apple becomes inactive and is scheduled to respawn after a set
    /// interval.</remarks>
    /// <param name="snake">The snake instance that will receive the growth and special effects upon consuming the super apple. Cannot be
    /// null.</param>
    /// <returns>true if the super apple was successfully consumed and should be removed from play; otherwise, false.</returns>
    public bool OnConsume(Snake snake)
    {
        // Super apple grows snake more and triggers special effect
        snake.Grow(GrowthAmount);
        _isActive = false;
        _nextSpawnTime = DateTime.UtcNow.AddSeconds(RESPAWN_SECONDS);
        return true; // Remove after consumption
    }

    /// <summary>
    /// Determines whether spawning should occur based on the current time and the object's active state.
    /// </summary>
    /// <param name="currentTime">The current time to evaluate against the next scheduled spawn time.</param>
    /// <returns>true if the object is inactive and the current time is greater than or equal to the next spawn time; otherwise,
    /// false.</returns>
    public bool ShouldSpawn(DateTime currentTime)
    {
        return !_isActive && currentTime >= _nextSpawnTime;
    }

    /// <summary>
    /// Updates the state of the object based on the specified current time.
    /// </summary>
    /// <remarks>Emtpy for now</remarks>
    /// <param name="currentTime">The current time used to evaluate and update the object's state.</param>
    public void Update(DateTime currentTime)
    {
        // SuperApple could have time-based mechanics like disappearing after X seconds
        // For now, it stays until consumed
    }
    #endregion

    #region Public Methods
    public void Deactivate()
    {
        _isActive = false;
        _nextSpawnTime = DateTime.UtcNow.AddSeconds(RESPAWN_SECONDS);
    }
    #endregion
}
