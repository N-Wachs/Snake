using System.Numerics;

namespace Snake;

public class SpecialApple
{
    #region Fields
    private Vector2 _position; // Position of the apple on the game grid
    private DateTime _timeForRespawn; // Time when the apple should respawn
    private int _respawnTimeSeconds; // Time between apple respawns in seconds
    private ConsoleColor _appleColor; // Color of the special apple
    private bool _isVisible; // Visibility status of the apple
    #endregion

    #region Properties
    private Vector2 Position
    {
        get { return _position; }
        set { _position = value; }
    }
    private int X
    {
        get { return (int)_position.X; }
        set { _position = new Vector2(value, _position.Y); }
    }
    private int Y
    {
        get { return (int)_position.Y; }
        set { _position = new Vector2(_position.X, value); }
    }
    private DateTime TimeForRespawn
    {
        get { return _timeForRespawn; }
        set { _timeForRespawn = value; }
    }
    public ConsoleColor AppleColor => _appleColor; // Color of the special apple
    private bool IsVisible
    {
        get { return _isVisible; }
        set { _isVisible = value; }
    }
    #endregion

    #region Constructors
    // Default constructor
    public SpecialApple()
    {
        _position = new Vector2(0, 0);
        _respawnTimeSeconds = 30;
        _appleColor = ConsoleColor.White;
        _isVisible = false;
        _timeForRespawn = DateTime.Now.AddSeconds(_respawnTimeSeconds);
    }
    // Parameterized constructor
    public SpecialApple(Vector2 position, int respawnTime)
    {
        _position = position;
        _respawnTimeSeconds = respawnTime;
        _appleColor = ConsoleColor.White;
        _isVisible = true;
        _timeForRespawn = DateTime.Now.AddSeconds(_respawnTimeSeconds);
    }
    // Parameterized constructor with color
    public SpecialApple(Vector2 position, int respawnTime, ConsoleColor color)
    {
        _position = position;
        _respawnTimeSeconds = respawnTime;
        _appleColor = color;
        _isVisible = true;
        _timeForRespawn = DateTime.Now.AddSeconds(_respawnTimeSeconds);
    }
    // Copy constructor
    public SpecialApple(SpecialApple other)
    {
        _position = other._position;
        _respawnTimeSeconds = other._respawnTimeSeconds;
        _appleColor = other._appleColor;
        _isVisible = other._isVisible;
        _timeForRespawn = other._timeForRespawn;
    }
    #endregion

    #region Methods
    // Method to spawn the apple at a random position within the specified bounds
    public void SetApple((int xStart, int xEnd, int yStart, int yEnd) gameSize, Random random = null)
    {
        if (random == null)
            random = new Random();
        X = random.Next(gameSize.xStart, gameSize.xEnd + 1);
        Y = random.Next(gameSize.yStart, gameSize.yEnd + 1);
        _isVisible = true;
    }

    // Method to get the position of the apple
    public Vector2 GetPosition() => Position;

    // Method to get position as tuple
    public (int X, int Y) GetPositionTuple() => ((int)Position.X, (int)Position.Y);

    // Method to check if it's time to respawn
    public bool IsTimeToRespawn() => DateTime.Now >= TimeForRespawn;

    // Method to reset respawn timer
    public void ResetRespawnTimer()
    {
        _timeForRespawn = DateTime.Now.AddSeconds(_respawnTimeSeconds);
    }

    // Method to set visibility
    public void SetVisibility(bool visible) => _isVisible = visible;

    // Method to check visibility
    public bool GetVisibility() => _isVisible;
    #endregion
}
