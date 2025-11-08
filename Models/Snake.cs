namespace Snake
{
    class Snake
    {
        #region Fields
        private List<(int X, int Y)> _bodySegments; // List of body segments represented by their coordinates
        private ConsoleColor _snakeColor;          // Color of the snake
        #endregion

        #region Properties
        private List<(int X, int Y)> BodySegments
        {
            get { return _bodySegments; }
            set { _bodySegments = value; }
        }
        private ConsoleColor SnakeColor
        {
            get { return _snakeColor; }
            set { _snakeColor = value; }
        }
        #endregion

        #region Constructors
        // Default constructor
        public Snake()
        {
            _bodySegments = new List<(int X, int Y)>();
            _snakeColor = ConsoleColor.Green;
        }

        // Constructor with length and initial position
        public Snake(int length, (int X, int Y) initialPosition)
        {
            _bodySegments = new List<(int X, int Y)>();
            for (int i = 0; i < length; i++)
            {
                _bodySegments.Add((initialPosition.X - i, initialPosition.Y));
            }
            _snakeColor = ConsoleColor.Green;
        }

        // Copy constructor
        public Snake(Snake other)
        {
            _bodySegments = new List<(int X, int Y)>(other._bodySegments);
            _snakeColor = other._snakeColor;
        }
        #endregion

        #region Methods
        // Method to move the snake in a specified direction
        public void Move((int DeltaX, int DeltaY) direction)
        {
            // Move each segments X and Y to the position of the previous segment
            for (int i = _bodySegments.Count - 1; i > 0; i--)
            {
                _bodySegments[i] = _bodySegments[i - 1];
            }

            // Move the head in the specified direction
            _bodySegments[0] = (_bodySegments[0].X + direction.DeltaX, _bodySegments[0].Y + direction.DeltaY);
        }

        // Method to grow the snake by adding a new segment at the tail
        public void Grow()
        {
            var tail = _bodySegments[_bodySegments.Count - 1];
            _bodySegments.Add(tail); // Add a new segment at the tail's position
            _bodySegments.Add(tail); // Add a new segment at the tail's position
            _bodySegments.Add(tail); // Add a new segment at the tail's position
        }

        // Method to set the snake's color
        public void SetColor(ConsoleColor color)
        {
            SnakeColor = color;
        }

        // Method to get the snake's Head segment
        public (int X, int Y) GetHead()
        {
            return _bodySegments[0];
        }

        public List<(int X, int Y)> GetBodySegments()
        {
            return _bodySegments;
        }
        #endregion
    }
}
