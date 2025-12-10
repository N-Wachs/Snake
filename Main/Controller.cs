using System.Numerics;

namespace Snake
{
    class Controller
    {
        #region Fields
        private Input _kbdInput; // Input handler for managing user input
        private Output _conOutput; // Output handler for displaying messages
        private MenuController _menuHandler; // Menu controller for handling menu operations
        private GameController _gameHandler; // Game controller for managing game logic
        #endregion

        #region Properties
        private Input KbdInput
        {
            get { return _kbdInput; }
            set { _kbdInput = value; }
        }
        private Output ConOutput
        {
            get { return _conOutput; }
            set { _conOutput = value; }
        }
        private MenuController MenuHandler
        {
            get { return _menuHandler; }
            set { _menuHandler = value; }
        }
        private GameController GameHandler
        {
            get { return _gameHandler; }
            set { _gameHandler = value; }
        }
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
            CheckForUpdatesAsync();
            KbdInput.ReadKey();

            Thread.Sleep(500);

            // Main menu loop
            do
            {
                int option = ConOutput.MainMenu();

                if (option == 0)
                {
                    // Start Game
                    GameHandler.StartGame(tickLengthMs, gameSize, snakeColor);
                }
                else if (option == 1)
                {
                    // Settings Menu
                    MenuHandler.SettingsMenu(ref tickLengthMs, ref gameSize, ref snakeColor);
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
                        Thread.Sleep(2500);
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
        }


        private async Task CheckForUpdatesAsync()
        {
            string versionUrl = "https://raw.githubusercontent.com/N-Wachs/Snake/main/version.txt";
            string currentVersion = "1.2.1";

            try
            {
                using HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5); // Timeout nach 5 Sekunden

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
}
