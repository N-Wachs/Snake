using System.Numerics;

namespace Snake;

class Controller
{
    #region Fields
    private Input _kbdInput; // Input handler for managing user input
    private Output _conOutput; // Output handler for displaying messages
    private MenuController _menuHandler; // Menu controller for handling menu operations
    private GameController _gameHandler; // Game controller for managing game logic
    private List<IConsumable> _consumables = new List<IConsumable>
    {
        new Apple(),
        new GoldenApple(),
        new SuperApple()
    }; // List of all the Consumables
    #endregion

    #region Properties
    private Input KbdInput { get => _kbdInput; set => _kbdInput = value; }
    private Output ConOutput { get => _conOutput; set => _conOutput = value; }
    private MenuController MenuHandler { get => _menuHandler; set => _menuHandler = value; }
    private GameController GameHandler { get => _gameHandler; set => _gameHandler = value; }
    private List<IConsumable> Consumables { get => _consumables; set => _consumables = value; }
    #endregion

    #region Constructors
    // Default constructor
    public Controller()
    {
        _kbdInput = new Input();
        _conOutput = new Output();
        _menuHandler = new MenuController(_conOutput, _kbdInput);
        _gameHandler = new GameController(_conOutput, _kbdInput);
    }
    #endregion

    #region Methods
    // Method to start the controller operations
    public void Run()
    {
        #region Variables
        bool exit = false;
        int tickLengthMs = 75; // Length of each game tick in milliseconds
        (int Xstart, int Xend, int Ystart, int Yend) gameSize = (1, 78, 2, 23); // Default game size
        ConsoleColor snakeColor = ConsoleColor.Green; // Default snake color
        #endregion

        ConOutput.WelcomeScreen();
        CheckForUpdatesAsync(); // Check for updates asynchronously (not awaited to avoid blocking)
        KbdInput.ReadKey();

        Thread.Sleep(500);

        // Main menu loop
        do
        {
            int option = ConOutput.MainMenu();

            if (option == 0)
            {
                // Start Game
                GameHandler.StartGame(tickLengthMs, gameSize, snakeColor, Consumables);
            }
            else if (option == 1)
            {
                // Settings Menu
                MenuHandler.SettingsMenu(ref tickLengthMs, ref gameSize, ref snakeColor, Consumables);
            }
            else if (option == 2)
            {
                // Show Highscores
                if (File.Exists("Highscores/highscores.txt"))
                {
                    // Reading highscores from the file and saving them into their variables
                    string[] scores = File.ReadAllText("Highscores/highscores.txt").Split(';');
                    BigInteger highscoreTicks = BigInteger.Parse(scores[0]);
                    int highscoreFood = int.Parse(scores[1]);

                    // Converting ticks to a readable time format
                    TimeSpan timePlayed = TimeSpan.FromMilliseconds((double)highscoreTicks);
                    ConOutput.ShowHighscores(timePlayed, highscoreFood);
                }
                else
                {
                    ConOutput.Clear();
                    ConOutput.Warning("No Highscores recorded yet!");
                    KbdInput.WaitOrKey(2500);
                }
            }
            else if (option == 3)
            {
                // Exit
                ConOutput.TextColor = ConsoleColor.Cyan;
                ConOutput.WriteLine("Goodbye!", true);
                exit = true;
            }
        } while (!exit);

        KbdInput.WaitOrKey(3000);
    }


    /// <summary>
    /// Checks asynchronously for a newer version of the application and notifies the user if an update is
    /// available.
    /// </summary>
    /// <remarks>This method retrieves the latest version information from a remote server. If a newer
    /// version is detected, a warning message is displayed to the user with a download link. Network errors and
    /// timeouts are ignored, allowing the application to continue running without interruption.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CheckForUpdatesAsync()
    {
        string versionUrl = "https://raw.githubusercontent.com/N-Wachs/Snake/main/version.txt";
        string currentVersion = "1.4.0";

        try
        {
            using HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(1); // Timeout nach 1 Sekunde

            string latestVersion = await client.GetStringAsync(versionUrl);
            latestVersion = latestVersion.Trim();

            if (latestVersion != currentVersion)
            {
                ConOutput.Warning($"\nUpdate verfügbar: {latestVersion} (du hast {currentVersion})\nDownload: https://github.com/N-Wachs/Snake/releases/latest");
            }
        }
        catch (HttpRequestException)
        {
            // Netzwerkfehler ignorieren - Spiel läuft trotzdem
        }
        catch (TaskCanceledException)
        {
            // Timeout ignorieren
        }
    }
    #endregion
}

