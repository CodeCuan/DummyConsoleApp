namespace DummyConsoleApp.AdventOfCoding.Constants
{
    public static class CardinalDirectionConstants
    {
        public static Dictionary<CardinalDirection, List<CardinalDirection>> LeftOrdering = new Dictionary<CardinalDirection, List<CardinalDirection>>() {
            { CardinalDirection.Up, [CardinalDirection.Left, CardinalDirection.Down, CardinalDirection.Right, CardinalDirection.Up] },
            { CardinalDirection.Left, [ CardinalDirection.Down, CardinalDirection.Right, CardinalDirection.Up, CardinalDirection.Left] },
            { CardinalDirection.Down, [ CardinalDirection.Right, CardinalDirection.Up, CardinalDirection.Left, CardinalDirection.Down] },
            { CardinalDirection.Right, [CardinalDirection.Up, CardinalDirection.Left, CardinalDirection.Down, CardinalDirection.Right] }
        };

        public static Dictionary<CardinalDirection, CardinalDirection> InvertDirection = new Dictionary<CardinalDirection, CardinalDirection>() {
            { CardinalDirection.Up, CardinalDirection.Down },
            { CardinalDirection.Down, CardinalDirection.Up },
            { CardinalDirection.Left, CardinalDirection.Right },
            { CardinalDirection.Right, CardinalDirection.Left }
        };
    }
}
