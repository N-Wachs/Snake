using System.Numerics;

namespace Snake;

/// <summary>
/// Represents a special consumable item that appears in the game and provides significant growth and bonus points when
/// collected.
/// </summary>
/// <remarks>A golden apple is a rare item that can be triggered to spawn under certain game events, such as when
/// a normal apple is eaten. It remains active for a limited time before disappearing if not collected. Consuming a
/// golden apple grants the snake a large growth boost and may award additional bonus points or trigger special effects,
/// depending on game implementation.</remarks>
public class GoldenApple : IConsumable
{
    #region Fields
    private Vector2 _position;
    private DateTime _despawnTime;
    private DateTime _spawnTime;
    private bool _isActive;
    private bool _shouldSpawn;
    private const int GROWTH_AMOUNT = 50;
    private const int LIFETIME_SECONDS = 10; // Disappears after 10 seconds
    private const int SPAWN_CHANCE_PERCENT = 2; // 2% chance to spawn when normal apple is eaten
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
        
        snake.SetColor(ConsoleColor.Yellow); // Temporary color change effect

        _isActive = false;
        return true; // Remove after consumption
    }

    public bool ShouldSpawn(DateTime currentTime)
    {
        // Golden apples don't auto-spawn, they're triggered by game events
        bool value = _shouldSpawn;
        _shouldSpawn = false; // Reset after check
        return value;
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
    public void RollForSpawn(Random random)
    {
        _shouldSpawn = random.Next(100) < SPAWN_CHANCE_PERCENT;
    }

    public void Deactivate()
    {
        _isActive = false;
    }
    #endregion
}
