namespace DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;

public class Coordinate2D
{
    public override string ToString()
    {
        return $"{X},{Y}";
    }

    public long X { get; set; }
    public long Y { get; set; }

    public CoordinateKey Key { get; set; }

    public Coordinate2D(long x, long y)
    {
        X = x;
        Y = y;
        Key = new(X, Y);
    }

    public Coordinate2D(string input)
    {
        var parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
        X = long.Parse(parts[0]);
        Y = long.Parse(parts[1]);
        Key = new(X, Y);
    }

    public Coordinate2D GetAbsoluteDistance(Coordinate2D toPoint)
    {
        return new Coordinate2D(
            Math.Abs(toPoint.X - X),
            Math.Abs(toPoint.Y - Y)
        );
    }

    public double GetDistanceSquared(Coordinate2D toPoint)
    {
        return Math.Pow(X - toPoint.X, 2) + Math.Pow(Y - toPoint.Y, 2);
    }

    public long GetArea(Coordinate2D toPoint)
    {
        return Math.Abs((X - toPoint.X) * (Y - toPoint.Y));
    }

    public long GetOffsetArea(Coordinate2D toPoint)
    {
        var rectangle = GetAbsoluteDistance(toPoint);
        return (rectangle.X + 1) * (rectangle.Y + 1);
    }

    public struct CoordinateKey(long X, long Y)
    {
        public readonly long X = X;
        public readonly long Y = Y;

        public override string ToString()
        {
            return $"{X},{Y}";
        }
        public override bool Equals(object? obj)
        {
            return obj is CoordinateKey other && Equals(other)
                || obj is Coordinate2D coord && Equals(coord.Key);
        }

        public bool Equals(CoordinateKey other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(CoordinateKey left, CoordinateKey right) => left.Equals(right);
        public static bool operator !=(CoordinateKey left, CoordinateKey right) => !left.Equals(right);
    }
}
