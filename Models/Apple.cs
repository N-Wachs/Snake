using System.Numerics;

namespace Snake;

public class Apple
{
    #region Fields
    private Vector2 _position; // Position of the apple on the game grid
    private ConsoleColor _appleColor; // Color of the apple
    private int _lengthAdded; // Length added to the snake when this apple is eaten
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
    public ConsoleColor AppleColor => _appleColor; // Color of the apple
    private int LengthAdded
    {
        get { return _lengthAdded; }
        set { _lengthAdded = value; }
    }
    #endregion

    #region Constructors
    // Default constructor
    public Apple()
    {
        _position = new Vector2(0, 0);
        _appleColor = ConsoleColor.Red;
        _lengthAdded = 3;
    }
    // Parameterized constructor
    public Apple(Vector2 position)
    {
        _position = position;
        _appleColor = ConsoleColor.Red;
        _lengthAdded = 3;
    }
    // Copy constructor
    public Apple(Apple other)
    {
        _position = other._position;
        _appleColor = other._appleColor;
        _lengthAdded = other._lengthAdded;
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
    }

    // Method to get the position of the apple
    public Vector2 GetPosition() => Position;

    // Method to get position as tuple
    public (int X, int Y) GetPositionTuple() => ((int)Position.X, (int)Position.Y);

    // Method to get the length added by this apple
    public int GetLengthAdded() => LengthAdded;
    #endregion
}
