namespace Snake;

public class MenuController
{
    #region Fields
    private Output _conOutput; // Output handler for displaying messages
    private Input _kbdInput; // Input handler for reading key presses
    #endregion

    #region Properties
    private Output OHandler { get => _conOutput; set => _conOutput = value; }
    private Input IHandler { get => _kbdInput; set => _kbdInput = value; }
    #endregion

    #region Constructors
    // Default constructor
    public MenuController()
    {
        _conOutput = new Output();
        _kbdInput = new Input();
    }

    // With parameters constructor
    public MenuController(Output outputHandler, Input inputHandler)
    {
        _conOutput = new Output(outputHandler);
        _kbdInput = new Input(inputHandler);
    }

    // Copy constructor
    public MenuController(MenuController other)
    {
        _conOutput = new Output(other._conOutput);
        _kbdInput = new Input(other._kbdInput);
    }
    #endregion

    #region Methods
    public void SettingsMenu(ref int tickLengthMs, ref (int Xstart, int Xend, int Ystart, int Yend) gameSize,
                             ref ConsoleColor snakeColor, List<IConsumable> consumables)
    {
        #region Local Variables
        bool leftSide = true;
        bool inSettingsMenu = true;
        int option = 0;
        
        // Right side sub-options (for game size X/Y separate navigation)
        int rightSubOption = 0; // 0 = Tick Length, 1 = Game Size X, 2 = Game Size Y, 3 = Snake Color
        
        // Available colors for snake
        ConsoleColor[] availableColors = 
        {
            ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow,
            ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Gray,
            ConsoleColor.DarkRed, ConsoleColor.DarkGreen, ConsoleColor.DarkBlue, ConsoleColor.DarkYellow,
            ConsoleColor.DarkCyan, ConsoleColor.DarkMagenta, ConsoleColor.DarkGray, ConsoleColor.Black
        };
        #endregion

        OHandler.Clear();
        #region Short animation
        OHandler.WriteAt(0, 11, Styles.SettingsMenueOption + "\n" + new string(' ', 40), ConsoleColor.Green);
        Thread.Sleep(100);
        OHandler.WriteAt(0, 10, Styles.SettingsMenueOption + "\n" + new string(' ', 40), ConsoleColor.Green);
        Thread.Sleep(100);
        OHandler.WriteAt(0, 9, Styles.SettingsMenueOption + "\n" + new string(' ', 40), ConsoleColor.Green);
        Thread.Sleep(100);
        OHandler.WriteAt(0, 8, Styles.SettingsMenueOption + "\n" + new string(' ', 40), ConsoleColor.Green);
        Thread.Sleep(100);
        OHandler.WriteAt(0, 7, Styles.SettingsMenueOption + "\n" + new string(' ', 40), ConsoleColor.Green);
        Thread.Sleep(100);
        OHandler.WriteAt(0, 6, Styles.SettingsMenueOption + "\n" + new string(' ', 40), ConsoleColor.Green);
        Thread.Sleep(100);
        OHandler.WriteAt(0, 5, Styles.SettingsMenueOption + "\n" + new string(' ', 40), ConsoleColor.Green);
        Thread.Sleep(100);
        OHandler.WriteAt(0, 4, Styles.SettingsMenueOption + "\n" + new string(' ', 40), ConsoleColor.Green);
        Thread.Sleep(100);
        #endregion
        OHandler.WriteAt(7, 3, Styles.SettingsMenueOption + "\n" + new string(' ', 40), ConsoleColor.Green);

        #region Menu Headers
        OHandler.TextColor = ConsoleColor.Blue;
        OHandler.SetPos(20, 8);
        OHandler.WriteWithAnimation("█ Consumables █ ", 5);
        OHandler.TextColor = ConsoleColor.White;
        OHandler.WriteWithAnimation("── █ Game Settings █", 5);
        OHandler.SetPos(20, 10);
        OHandler.WriteWithAnimation("Use W/S to navigate, A/D to change values", 2);
        OHandler.SetPos(20, 11);
        OHandler.WriteWithAnimation("Enter/Space to toggle, Tab to switch sides", 2);
        #endregion

        // Initial render
        RenderLeftSide(consumables, option, leftSide);
        RenderRightSide(tickLengthMs, gameSize, snakeColor, rightSubOption, !leftSide);

        #region Main Loop
        do
        {
            ConsoleKeyInfo key = IHandler.ReadKey();
            
            #region Input Handling
            switch (key.Key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    if (!leftSide)
                    {
                        // Adjust value on right side
                        AdjustRightSideValue(ref tickLengthMs, ref gameSize, ref snakeColor, 
                                            rightSubOption, availableColors, false);
                        RenderRightSide(tickLengthMs, gameSize, snakeColor, rightSubOption, true);
                    }
                    break;
                    
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    if (!leftSide)
                    {
                        // Adjust value on right side
                        AdjustRightSideValue(ref tickLengthMs, ref gameSize, ref snakeColor, 
                                            rightSubOption, availableColors, true);
                        RenderRightSide(tickLengthMs, gameSize, snakeColor, rightSubOption, true);
                    }
                    break;
                    
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    if (leftSide)
                    {
                        option = (option - 1 + 3) % 3;
                        RenderLeftSide(consumables, option, true);
                    }
                    else
                    {
                        RenderRightSide(tickLengthMs, gameSize, snakeColor, rightSubOption, false);
                        rightSubOption = (rightSubOption - 1 + 4) % 4;
                        RenderRightSide(tickLengthMs, gameSize, snakeColor, rightSubOption, true);
                    }
                    break;
                    
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    if (leftSide)
                    {
                        option = (option + 1) % 3;
                        RenderLeftSide(consumables, option, true);
                    }
                    else
                    {
                        RenderRightSide(tickLengthMs, gameSize, snakeColor, rightSubOption, false);
                        rightSubOption = (rightSubOption + 1) % 4;
                        RenderRightSide(tickLengthMs, gameSize, snakeColor, rightSubOption, true);
                    }
                    break;
                    
                case ConsoleKey.Escape:
                case ConsoleKey.Backspace:
                    inSettingsMenu = false;
                    break;
                    
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    if (leftSide)
                    {
                        // Toggle consumable on/off
                        ToggleConsumable(consumables, option);
                        RenderLeftSide(consumables, option, true);
                    }
                    break;
                    
                case ConsoleKey.Tab:
                    if (leftSide)
                    {
                        RenderLeftSide(consumables, option, false);
                        leftSide = false;
                        RenderRightSide(tickLengthMs, gameSize, snakeColor, rightSubOption, true);
                    }
                    else
                    {
                        RenderRightSide(tickLengthMs, gameSize, snakeColor, rightSubOption, false);
                        leftSide = true;
                        RenderLeftSide(consumables, option, true);
                    }
                    break;
            }
            #endregion
        } while (inSettingsMenu);
        #endregion
    }

    private void RenderLeftSide(List<IConsumable> consumables, int selectedOption, bool isActive)
    {
        // Header
        OHandler.TextColor = ConsoleColor.Blue;
        OHandler.SetPos(20, 8);
        OHandler.Write("█ Consumables █ ");
        OHandler.TextColor = ConsoleColor.White;
        OHandler.Write("── █ Game Settings █");

        // Option 0: Normal Apple
        OHandler.SetPos(18, 13);
        bool hasApple = FindMatchInList(consumables, new Apple());
        if (isActive && selectedOption == 0)
        {
            OHandler.TextColor = hasApple ? new Apple().Color : ConsoleColor.White;
            OHandler.Write("> Normal Apple <");
        }
        else
        {
            OHandler.TextColor = hasApple ? new Apple().Color : ConsoleColor.White;
            OHandler.Write("  Normal Apple  ");
        }
        OHandler.ResetColor();

        // Option 1: Golden Apple
        OHandler.SetPos(18, 15);
        bool hasGolden = FindMatchInList(consumables, new GoldenApple());
        if (isActive && selectedOption == 1)
        {
            OHandler.TextColor = hasGolden ? new GoldenApple().Color : ConsoleColor.White;
            OHandler.Write("> Golden Apple <");
        }
        else
        {
            OHandler.TextColor = hasGolden ? new GoldenApple().Color : ConsoleColor.White;
            OHandler.Write("  Golden Apple  ");
        }
        OHandler.ResetColor();

        // Option 2: Super Apple
        OHandler.SetPos(18, 17);
        bool hasSuper = FindMatchInList(consumables, new SuperApple());
        if (isActive && selectedOption == 2)
        {
            OHandler.TextColor = hasSuper ? new SuperApple().Color : ConsoleColor.White;
            OHandler.Write("> Super Apple <");
        }
        else
        {
            OHandler.TextColor = hasSuper ? new SuperApple().Color : ConsoleColor.White;
            OHandler.Write("  Super Apple  ");
        }
        OHandler.ResetColor();
    }

    private void RenderRightSide(int tickLengthMs, (int Xstart, int Xend, int Ystart, int Yend) gameSize, 
                                 ConsoleColor snakeColor, int selectedSubOption, bool isActive)
    {
        int gameSizeX = gameSize.Xend - gameSize.Xstart;
        int gameSizeY = gameSize.Yend - gameSize.Ystart;

        // Header
        OHandler.TextColor = ConsoleColor.White;
        OHandler.SetPos(20, 8);
        OHandler.Write("█ Consumables █ ──");
        OHandler.TextColor = ConsoleColor.Blue;
        OHandler.Write(" █ Game Settings █");


        // Option 0: Tick Length
        OHandler.SetPos(48, 13);
        if (isActive && selectedSubOption == 0)
        {
            OHandler.TextColor = ConsoleColor.Green;
            OHandler.Write($"Tick Length: > {tickLengthMs,3} ms <  ");
        }
        else
        {
            OHandler.TextColor = ConsoleColor.White;
            OHandler.Write($"Tick Length:   {tickLengthMs,3} ms    ");
        }
        OHandler.ResetColor();

        // Option 1: Game Size X
        OHandler.SetPos(48, 15);
        if (isActive && selectedSubOption == 1)
        {
            OHandler.TextColor = ConsoleColor.Green;
            OHandler.Write($"  Game Size X: > {gameSizeX,3} <     ");
        }
        else
        {
            OHandler.TextColor = ConsoleColor.White;
            OHandler.Write($"  Game Size X:   {gameSizeX,3}       ");
        }
        OHandler.ResetColor();

        // Option 2: Game Size Y
        OHandler.SetPos(48, 17);
        if (isActive && selectedSubOption == 2)
        {
            OHandler.TextColor = ConsoleColor.Green;
            OHandler.Write($"  Game Size Y: > {gameSizeY,3} <     ");
        }
        else
        {
            OHandler.TextColor = ConsoleColor.White;
            OHandler.Write($"  Game Size Y:   {gameSizeY,3}       ");
        }
        OHandler.ResetColor();

        // Option 3: Snake Color
        OHandler.SetPos(48, 19);
        ConsoleColor displayColor = snakeColor == ConsoleColor.Black ? ConsoleColor.DarkGray : snakeColor;
        if (isActive && selectedSubOption == 3)
        {
            OHandler.TextColor = displayColor;
            OHandler.Write($"Snake Color: > {snakeColor,-12} <");
        }
        else
        {
            OHandler.TextColor = displayColor;
            OHandler.Write($"Snake Color:   {snakeColor,-12}  ");
        }
        OHandler.ResetColor();
    }

    private void AdjustRightSideValue(ref int tickLengthMs, ref (int Xstart, int Xend, int Ystart, int Yend) gameSize,
                                     ref ConsoleColor snakeColor, int subOption, ConsoleColor[] availableColors, bool increase)
    {
        switch (subOption)
        {
            case 0: // Tick Length
                if (increase)
                {
                    tickLengthMs = Math.Min(250, tickLengthMs + 25);
                }
                else
                {
                    tickLengthMs = Math.Max(25, tickLengthMs - 25);
                }
                break;

            case 1: // Game Size X
                int currentX = gameSize.Xend - gameSize.Xstart;
                int maxX = Console.WindowWidth - 3; // -3 for borders
                if (increase)
                {
                    currentX = Math.Min(maxX, currentX + 1);
                }
                else
                {
                    currentX = Math.Max(10, currentX - 1);
                }
                gameSize.Xend = gameSize.Xstart + currentX;
                break;

            case 2: // Game Size Y
                int currentY = gameSize.Yend - gameSize.Ystart;
                int maxY = Console.WindowHeight - 4; // -4 for borders and status line
                if (increase)
                {
                    currentY = Math.Min(maxY, currentY + 1);
                }
                else
                {
                    currentY = Math.Max(10, currentY - 1);
                }
                gameSize.Yend = gameSize.Ystart + currentY;
                break;

            case 3: // Snake Color
                int currentColorIndex = Array.IndexOf(availableColors, snakeColor);
                if (currentColorIndex == -1) currentColorIndex = 0;
                
                if (increase)
                {
                    currentColorIndex = (currentColorIndex + 1) % availableColors.Length;
                }
                else
                {
                    currentColorIndex = (currentColorIndex - 1 + availableColors.Length) % availableColors.Length;
                }
                snakeColor = availableColors[currentColorIndex];
                break;
        }
    }

    private void ToggleConsumable(List<IConsumable> consumables, int option)
    {
        IConsumable targetType = option switch
        {
            0 => new Apple(),
            1 => new GoldenApple(),
            2 => new SuperApple(),
            _ => null
        };

        if (targetType == null) return;

        // Check if already in list
        IConsumable existing = null;
        foreach (var item in consumables)
        {
            if (item.GetType() == targetType.GetType())
            {
                existing = item;
                break;
            }
        }

        if (existing != null)
        {
            // Remove from list (deactivate)
            consumables.Remove(existing);
        }
        else
        {
            // Add to list (activate)
            consumables.Add(targetType);
        }
    }

    private bool FindMatchInList(List<IConsumable> list, object search)
    {
        foreach (var item in list)
        {
            if (item.GetType() == search.GetType())
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
