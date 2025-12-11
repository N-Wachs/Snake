namespace Snake
{
    public class Input
    {
        #region Fields
        private Output _conOutput; // Output handler for displaying messages
        private ConsoleKeyInfo _pressed; // Stores the last pressed key
        #endregion

        #region Properties
        private Output OHandler { get => _conOutput; set => _conOutput = value; }
        private ConsoleKeyInfo Pressed { get => _pressed; set => _pressed = value;  }

        public ConsoleKeyInfo LastPressedKey => _pressed;
        public int KeyChar => _pressed.KeyChar;
        #endregion

        #region Constructors
        // Default constructor
        public Input()
        {
            _conOutput = new Output();
            _pressed = new ConsoleKeyInfo();
        }

        // Copy constructor
        public Input(Input other)
        {
            _conOutput = new Output(other._conOutput);
            _pressed = other._pressed;
        }
        #endregion

        #region Methods
        // Method to read a key press from the console
        public ConsoleKeyInfo ReadKey(bool intercept = true) 
        { 
            Pressed = Console.ReadKey(intercept); 
            return Pressed;
        }

        public bool IsKeyAvailable() => Console.KeyAvailable;

        public void ClearInputBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        public void WaitOrKey(int timeoutMs)
        {
            DateTime waitTime = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            while (waitTime > DateTime.UtcNow)
            {
                if (Console.KeyAvailable) return; 
                Thread.Sleep(20); // Small delay to prevent busy waiting
            }
        }
        #endregion
    }
}
