using System.Numerics;

namespace Snake;

public class SuperApple : SpecialApple
{
    #region Fields
    private int _lengthAdded; // Length added to the snake when this apple is eaten
    #endregion

    #region Properties
    private int LengthAdded
    {
        get { return _lengthAdded; }
        set { _lengthAdded = value; }
    }
    public ConsoleColor AppleColor => ConsoleColor.Magenta; // Color of the super apple
    #endregion

    #region Constructors
    // Default constructor
    public SuperApple() : base()
    {
        _lengthAdded = 9; // Default length added
    }
    // Parameterized constructor
    public SuperApple(Vector2 position) : base(position, 30)
    {
        _lengthAdded = 9; // Default length added
    }
    // Copy constructor
    public SuperApple(SuperApple other) : base(other)
    {
        _lengthAdded = 9;
    }
    #endregion

    #region Methods
    // Method to get the length added by this super apple
    public int GetLengthAdded() => LengthAdded;
    #endregion
}
