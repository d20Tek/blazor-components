//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

namespace D20Tek.BlazorComponents
{
    using System;

    public struct ValueRange : IEquatable<ValueRange>
    {
        public ValueRange(int min, int? max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(
                    "Invalid range: minimum value must be less or equal to maximum.");
            }

            this.Min = min;
            this.Max = max;
        }

        public int Min { get; set; }

        public int? Max { get; set; }

        public static bool operator ==(ValueRange lhs, ValueRange rhs) =>
            lhs.Equals(rhs);

        public static bool operator !=(ValueRange lhs, ValueRange rhs) =>
            !lhs.Equals(rhs);

        public bool InRange(int value) =>
            (value >= this.Min && value <= (this.Max ?? int.MaxValue));

        public bool Equals(ValueRange other) =>
            (this.Min == other.Min && this.Max == other.Max);

        public override bool Equals(object? obj)
        {
            if (obj is ValueRange range)
            {
                return this.Equals(range);
            }

            return false;
        }

        public override int GetHashCode() =>
            this.Min.GetHashCode() ^ this.Max.GetHashCode();
    }
}
