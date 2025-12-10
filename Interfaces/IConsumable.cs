using System.Numerics;

namespace Snake.Interfaces;

/// <summary>
/// Interface for all consumable items in the game (food, power-ups, etc.)
/// </summary>
public interface IConsumable
{
    /// <summary>
    /// Gets the position of the consumable item
    /// </summary>
    (int X, int Y) Position { get; }

    /// <summary>
    /// Gets the display color of the consumable
    /// </summary>
    ConsoleColor Color { get; }

    /// <summary>
    /// Gets the number of segments to add when consumed
    /// </summary>
    int GrowthAmount { get; }

    /// <summary>
    /// Sets the position of the consumable within game bounds
    /// </summary>
    void Respawn((int xStart, int xEnd, int yStart, int yEnd) gameSize, Random random);

    /// <summary>
    /// Called when the consumable is eaten by the snake
    /// </summary>
    /// <returns>True if the consumable should be removed after consumption</returns>
    bool OnConsume(Snake snake);

    /// <summary>
    /// Checks if the consumable should spawn/respawn
    /// </summary>
    bool ShouldSpawn(DateTime currentTime);

    /// <summary>
    /// Updates the consumable state (for time-based mechanics)
    /// </summary>
    void Update(DateTime currentTime);
}
