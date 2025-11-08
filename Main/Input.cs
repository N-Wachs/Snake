namespace Snake
{
    class Input
    {
        #region Fields
        private Output _conOutput; // Output handler for displaying messages
        private ConsoleKeyInfo _pressed; // Stores the last pressed key
        #endregion

        #region Properties
        private Output OHandler
        {
            get { return _conOutput; }
            set { _conOutput = value; }
        }
        private ConsoleKeyInfo Pressed
        {
            get { return _pressed; }
            set { _pressed = value; }
        }
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
            if (!intercept)
            {
                Console.CursorVisible = true;
            }
            else
            {
                Console.CursorVisible = false;
            }
            Pressed = Console.ReadKey(intercept);
            return Pressed;
        }

        // Method to get the last pressed key
        public ConsoleKeyInfo GetPressedKey()
        {
            return Pressed;
        }

        public bool IsKeyAvailable()
        {
            return Console.KeyAvailable;
        }
        #endregion
    }
}
