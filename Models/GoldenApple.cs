using System.Numerics;

namespace Snake;

/// <summary>
/// Example of an extendable special apple - Golden Apple that gives bonus points
/// This demonstrates how easy it is to add new consumable types
/// </summary>
public class GoldenApple : IConsumable
{
    #region Fields
    private Vector2 _position;
    private DateTime _spawnTime;
    private DateTime _despawnTime;
    private bool _isActive;
    private const int GROWTH_AMOUNT = 5;
    private const int LIFETIME_SECONDS = 10; // Disappears after 10 seconds
    private const int SPAWN_CHANCE_PERCENT = 5; // 5% chance to spawn when normal apple is eaten
    #endregion

    #region IConsumable Implementation
    public (int X, int Y) Position => ((int)_position.X, (int)_position.Y);
    public ConsoleColor Color => ConsoleColor.Yellow;
    public int GrowthAmount => GROWTH_AMOUNT;
    #endregion

    #region Properties
    public bool IsActive => _isActive;
    public int BonusPoints => 50; // Example: could give bonus points
    #endregion

    #region Constructors
    public GoldenApple()
    {
        _position = Vector2.Zero;
        _isActive = false;
    }
    #endregion

    #region IConsumable Methods
    public void Respawn((int xStart, int xEnd, int yStart, int yEnd) gameSize, Random random)
    {
        _position = new Vector2(
            random.Next(gameSize.xStart, gameSize.xEnd + 1),
            random.Next(gameSize.yStart, gameSize.yEnd + 1)
        );
        _isActive = true;
        _spawnTime = DateTime.UtcNow;
        _despawnTime = _spawnTime.AddSeconds(LIFETIME_SECONDS);
    }

    public bool OnConsume(Snake snake)
    {
        // Golden apple gives growth and could trigger special effects
        snake.Grow(GrowthAmount);
        
        // Here you could add:
        // - Bonus points
        // - Speed boost
        // - Temporary invincibility
        // - Any other special effect
        
        _isActive = false;
        return true; // Remove after consumption
    }

    public bool ShouldSpawn(DateTime currentTime)
    {
        // Golden apples don't auto-spawn, they're triggered by game events
        // This would be handled by the game logic
        return false;
    }

    public void Update(DateTime currentTime)
    {
        // Golden apple disappears after lifetime expires
        if (_isActive && currentTime >= _despawnTime)
        {
            _isActive = false;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Attempts to spawn a golden apple based on chance
    /// Call this when a normal apple is eaten
    /// </summary>
    public static bool RollForSpawn(Random random)
    {
        return random.Next(100) < SPAWN_CHANCE_PERCENT;
    }

    public void Deactivate()
    {
        _isActive = false;
    }
    #endregion
}
