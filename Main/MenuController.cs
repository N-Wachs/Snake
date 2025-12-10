namespace Snake;

public class MenuController
{
    #region Fields
    private Output _conOutput; // Output handler for displaying messages
    private Input _kbdInput; // Input handler for reading key presses
    #endregion

    #region Properties
    private Output ConOutput
    {
        get { return _conOutput; }
        set { _conOutput = value; }
    }
    private Input KbdInput
    {
        get { return _kbdInput; }
        set { _kbdInput = value; }
    }
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
    public void SettingsMenu(ref int tickLengthMs, ref (int Xstart, int Xend, int Ystart, int Yend) gameSize, ref ConsoleColor snakeColor)
    {
        int option = 0;
        ConsoleKeyInfo pressed;
        bool repeat = true;

        ConOutput.SettingsMenu(tickLengthMs, gameSize, snakeColor);

        // Menu interaction loop
        do
        {
            pressed = KbdInput.ReadKey();

            // If Left or Right arrow is pressed, change the option
            if (pressed.Key == ConsoleKey.LeftArrow || pressed.Key == ConsoleKey.A)
            {
                UpdateOptionHighlight(ref option, tickLengthMs, gameSize, snakeColor, false);
            }
            else if (pressed.Key == ConsoleKey.RightArrow || pressed.Key == ConsoleKey.D)
            {
                UpdateOptionHighlight(ref option, tickLengthMs, gameSize, snakeColor);
            }
            else if (pressed.Key == ConsoleKey.UpArrow || pressed.Key == ConsoleKey.W)
            {
                if (option == 0)
                {
                    // Increase Tick Speed
                    if (tickLengthMs < 250)
                        tickLengthMs += 25;
                    ConOutput.SetPos(0, 2);
                    ConOutput.SetOutput("Tick Speed:", ConsoleColor.Cyan);
                    ConOutput.WriteText();
                    ConOutput.SetOutput(tickLengthMs.ToString() + "ms ", ConsoleColor.Green);
                    ConOutput.WriteText();
                }
                else if (option == 1)
                {
                    // Increase Game Size X
                    if (gameSize.Xend < 78)
                        gameSize.Xend += 1;
                    ConOutput.SetPos(13, 2);
                    ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                    ConOutput.WriteText();
                    ConOutput.SetPos(13, 3);
                    ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1}", ConsoleColor.Green);
                    ConOutput.WriteText();
                }
                else if (option == 2)
                {
                    // Increase Game Size Y
                    if (gameSize.Yend < 23)
                        gameSize.Yend += 1;
                    ConOutput.SetPos(13, 2);
                    ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                    ConOutput.WriteText();
                    ConOutput.SetPos(19, 3);
                    ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1}", ConsoleColor.Green);
                    ConOutput.WriteText();
                }
                else if (option == 3)
                {
                    // Change Snake Color to the next color
                    snakeColor = (ConsoleColor)(((int)snakeColor + 1) % 16);
                    ConOutput.SetPos(25, 2);
                    ConOutput.SetOutput("Snake Color:", ConsoleColor.Cyan);
                    ConOutput.WriteText();
                    ConOutput.SetPos(25, 3);
                    ConOutput.SetOutput(snakeColor.ToString() + "        ", snakeColor);
                    ConOutput.WriteText();
                }
            }
            else if (pressed.Key == ConsoleKey.DownArrow || pressed.Key == ConsoleKey.S)
            {
                if (option == 0)
                {
                    // Decrease Tick Speed
                    if (tickLengthMs > 25)
                        tickLengthMs -= 25;
                    ConOutput.SetPos(0, 2);
                    ConOutput.SetOutput("Tick Speed:", ConsoleColor.Cyan);
                    ConOutput.WriteText();
                    ConOutput.SetOutput(tickLengthMs.ToString() + "ms ", ConsoleColor.Green);
                    ConOutput.WriteText();
                }
                else if (option == 1)
                {
                    // Decrease Game Size X
                    if (gameSize.Xend > 10)
                        gameSize.Xend -= 1;
                    ConOutput.SetPos(13, 2);
                    ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                    ConOutput.WriteText();
                    ConOutput.SetPos(13, 3);
                    ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1} ", ConsoleColor.Green);
                    ConOutput.WriteText();
                }
                else if (option == 2)
                {
                    // Decrease Game Size Y
                    if (gameSize.Yend > 7)
                        gameSize.Yend -= 1;
                    ConOutput.SetPos(13, 2);
                    ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
                    ConOutput.WriteText();
                    ConOutput.SetPos(19, 3);
                    ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1} ", ConsoleColor.Green);
                    ConOutput.WriteText();
                }
                else if (option == 3)
                {
                    // Change Snake Color to the previous color
                    snakeColor = (ConsoleColor)(((int)snakeColor - 1 + 16) % 16);
                    ConOutput.SetPos(25, 2);
                    ConOutput.SetOutput("Snake Color:", ConsoleColor.Cyan);
                    ConOutput.WriteText();
                    ConOutput.SetPos(25, 3);
                    ConOutput.SetOutput(snakeColor.ToString() + "      ", snakeColor);
                    ConOutput.WriteText();
                }
            }
            else if (pressed.Key == ConsoleKey.Escape || pressed.Key == ConsoleKey.Enter)
            {
                repeat = false; // Exit settings menu
            }

        } while (repeat);
    }

    private void UpdateOptionHighlight(ref int option, int tickLengthMs, (int Xstart, int Xend, int Ystart, int Yend) gameSize, ConsoleColor snakeColor, bool add = true)
    {
        #region Erase current option highlight
        if (option == 0)
        {
            ConOutput.SetPos(0, 2);
            ConOutput.SetOutput("Tick Speed:");
            ConOutput.WriteText();
            ConOutput.SetOutput(tickLengthMs.ToString() + "ms ");
            ConOutput.WriteText();
        }
        else if (option == 1)
        {
            ConOutput.SetPos(13, 2);
            ConOutput.SetOutput("Game Size:");
            ConOutput.WriteText();
            ConOutput.SetPos(13, 3);
            ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1}");
            ConOutput.WriteText();
        }
        else if (option == 2)
        {
            ConOutput.SetPos(13, 2);
            ConOutput.SetOutput("Game Size:");
            ConOutput.WriteText();
            ConOutput.SetPos(19, 3);
            ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1}");
            ConOutput.WriteText();
        }
        else if (option == 3)
        {
            ConOutput.SetPos(25, 2);
            ConOutput.SetOutput("Snake Color:");
            ConOutput.WriteText();
            ConOutput.SetPos(25, 3);
            ConOutput.SetOutput(snakeColor.ToString());
            ConOutput.WriteText();
        }
        #endregion

        if (add)
            option = (option + 1) % 4; // Wrap around the options
        else
            option = (option - 1) % 4; // Wrap around the options

        #region Highlight new option
        if (option == 0)
        {
            ConOutput.SetPos(0, 2);
            ConOutput.SetOutput("Tick Speed:", ConsoleColor.Cyan);
            ConOutput.WriteText();
            ConOutput.SetOutput(tickLengthMs.ToString() + "ms ", ConsoleColor.Green);
            ConOutput.WriteText();
        }
        else if (option == 1)
        {
            ConOutput.SetPos(13, 2);
            ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
            ConOutput.WriteText();
            ConOutput.SetPos(13, 3);
            ConOutput.SetOutput($"X: {gameSize.Xend - gameSize.Xstart + 1}", ConsoleColor.Green);
            ConOutput.WriteText();
        }
        else if (option == 2)
        {
            ConOutput.SetPos(13, 2);
            ConOutput.SetOutput("Game Size:", ConsoleColor.Cyan);
            ConOutput.WriteText();
            ConOutput.SetPos(19, 3);
            ConOutput.SetOutput($"Y: {gameSize.Yend - gameSize.Ystart + 1}", ConsoleColor.Green);
            ConOutput.WriteText();
        }
        else if (option == 3)
        {
            ConOutput.SetPos(25, 2);
            ConOutput.SetOutput("Snake Color:", ConsoleColor.Cyan);
            ConOutput.WriteText();
            ConOutput.SetPos(25, 3);
            ConOutput.SetOutput(snakeColor.ToString(), snakeColor);
            ConOutput.WriteText();
        }
        #endregion
    }
    #endregion
}
