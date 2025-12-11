
namespace Snake;

/// <summary>
/// Manages all consumable items in the game with efficient collision detection
/// </summary>
public class FoodManager
{
    #region Fields
    private readonly List<IConsumable> _activeConsumables;
    private readonly List<IConsumable> _inactiveConsumables;
    private readonly HashSet<(int, int)> _occupiedPositions;
    private readonly Random _random;
    private readonly (int xStart, int xEnd, int yStart, int yEnd) _gameSize;
    #endregion

    #region Constructors
    public FoodManager((int xStart, int xEnd, int yStart, int yEnd) gameSize, Random random)
    {
        _activeConsumables = new List<IConsumable>();
        _inactiveConsumables = new List<IConsumable>();
        _occupiedPositions = new HashSet<(int, int)>();
        _random = random;
        _gameSize = gameSize;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Registers a consumable to be managed
    /// </summary>
    public void RegisterConsumable(IConsumable consumable)
    {
        if (consumable.ShouldSpawn(DateTime.UtcNow))
        {
            SpawnConsumable(consumable);
        }
        else
        {
            _inactiveConsumables.Add(consumable);
        }
    }

    /// <summary>
    /// Updates all consumables and checks for spawning
    /// </summary>
    public void Update(DateTime currentTime)
    {
        // Check inactive consumables for spawning
        for (int i = _inactiveConsumables.Count - 1; i >= 0; i--)
        {
            var consumable = _inactiveConsumables[i];
            if (consumable.ShouldSpawn(currentTime))
            {
                _inactiveConsumables.RemoveAt(i);
                SpawnConsumable(consumable);
            }
        }

        // Update active consumables
        foreach (var consumable in _activeConsumables)
        {
            consumable.Update(currentTime);
        }
    }

    /// <summary>
    /// Checks if a position has a consumable and returns it
    /// </summary>
    public IConsumable CheckCollision((int X, int Y) position)
    {
        foreach (var consumable in _activeConsumables)
        {
            if (consumable.Position == position)
            {
                return consumable;
            }
        }
        return null;
    }

    /// <summary>
    /// Consumes an item and handles its removal/respawn
    /// </summary>
    public void ConsumeItem(IConsumable consumable, Snake snake)
    {
        bool shouldRemove = consumable.OnConsume(snake);
        
        if (shouldRemove)
        {
            _activeConsumables.Remove(consumable);
            _occupiedPositions.Remove(consumable.Position);

            // Check if it should be tracked for respawning
            if (consumable is SuperApple)
            {
                _inactiveConsumables.Add(consumable);
            }
            else
            {
                // Regular apple - respawn immediately
                SpawnConsumable(consumable);
            }
        }
    }

    /// <summary>
    /// Gets all active consumables for rendering
    /// </summary>
    public IReadOnlyList<IConsumable> GetActiveConsumables() => _activeConsumables.AsReadOnly();

    /// <summary>
    /// Updates occupied positions (should be called after snake moves)
    /// </summary>
    public void UpdateOccupiedPositions(HashSet<(int, int)> snakePositions)
    {
        _occupiedPositions.Clear();
        foreach (var pos in snakePositions)
        {
            _occupiedPositions.Add(pos);
        }
        foreach (var consumable in _activeConsumables)
        {
            _occupiedPositions.Add(consumable.Position);
        }
    }

    /// <summary>
    /// Checks if a position is occupied
    /// </summary>
    public bool IsPositionOccupied((int X, int Y) position) => _occupiedPositions.Contains(position);
    #endregion

    #region Private Methods
    private void SpawnConsumable(IConsumable consumable)
    {
        int maxAttempts = 100;
        int attempts = 0;

        do
        {
            consumable.Respawn(_gameSize, _random);
            attempts++;
        } while (_occupiedPositions.Contains(consumable.Position) && attempts < maxAttempts);

        if (attempts < maxAttempts)
        {
            _activeConsumables.Add(consumable);
            _occupiedPositions.Add(consumable.Position);
        }
    }
    #endregion
}
