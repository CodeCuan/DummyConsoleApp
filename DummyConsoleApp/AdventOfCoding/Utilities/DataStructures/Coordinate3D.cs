namespace DummyConsoleApp.AdventOfCoding.Utilities.DataStructures;

public class Coordinate3D
{
    public override string ToString()
    {
        return $"{X},{Y},{Z}";
    }

    public long X { get; set; }
    public long Y { get; set; }
    public long Z { get; set; }

    public Coordinate3D(long x, long y, long z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Coordinate3D(string input, char seperator = ',')
    {
        var parts = input.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
        X = long.Parse(parts[0]);
        Y = long.Parse(parts[1]);
        Z = long.Parse(parts[2]);
    }

    public bool ContainsPoint(Coordinate3D point)
    {
        return point.X <= X && point.Y <= Y && point.Z <= Z;
    }

    public Coordinate3D GetAbsoluteDistance(Coordinate3D toPoint) { 
        return new Coordinate3D(
            Math.Abs(toPoint.X - X),
            Math.Abs(toPoint.Y - Y),
            Math.Abs(toPoint.Z - Z)
        );
    }

    public double GetDistanceSquared(Coordinate3D toPoint)
    {
        return Math.Pow(X - toPoint.X, 2) + Math.Pow(Y - toPoint.Y, 2) + Math.Pow(Z - toPoint.Z, 2);
    }

    public long GetVolume()
    {
        return X * Y * Z;
    }
}
