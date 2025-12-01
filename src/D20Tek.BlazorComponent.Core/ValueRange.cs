namespace D20Tek.BlazorComponents;

public struct ValueRange : IEquatable<ValueRange>
{
    public ValueRange(int min, int? max)
    {
        if (min > max)
        {
            throw new ArgumentOutOfRangeException(
                nameof(min),
                "Invalid range: minimum value must be less or equal to maximum.");
        }

        Min = min;
        Max = max;
    }

    public int Min { get; set; }

    public int? Max { get; set; }

    public static bool operator ==(ValueRange lhs, ValueRange rhs) => lhs.Equals(rhs);

    public static bool operator !=(ValueRange lhs, ValueRange rhs) => !lhs.Equals(rhs);

    public bool InRange(int value) => value >= Min && value <= (Max ?? int.MaxValue);

    public void AssertInRange(int value, string parameterName = "ValueRange.Value")
    {
        if (!InRange(value)) throw new ArgumentOutOfRangeException(parameterName);
    }

    public readonly bool Equals(ValueRange other) => Min == other.Min && Max == other.Max;

    public override readonly bool Equals(object? obj) => obj is ValueRange range && Equals(range);

    public override readonly int GetHashCode() => Min.GetHashCode() ^ Max.GetHashCode();
}
