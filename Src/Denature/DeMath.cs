using System;
using System.Collections.Generic;

namespace Osiris.Src.Denature;

public static class DeMath
{
    public readonly struct Vec2(float x, float y)
    {
        public float X { get; init; } = x;
        public float Y { get; init; } = y;

        public static Vec2 Max(Vec2 a, Vec2 b)
        {
            return new(MathF.Max(a.X, b.X), MathF.Max(a.Y, b.Y));
        }
        public static Vec2 Max(IEnumerable<Vec2> vs)
        {
            float mx = float.NegativeInfinity;
            float my = float.NegativeInfinity;
            foreach (var v in vs)
            {
                mx = MathF.Max(mx, v.X);
                my = MathF.Max(my, v.Y);
            }
            return new(mx, my);
        }
        public static Vec2 Min(Vec2 a, Vec2 b)
        {
            return new(MathF.Min(a.X, b.X), MathF.Min(a.Y, b.Y));
        }
        public static Vec2 Min(IEnumerable<Vec2> vs)
        {
            float mx = float.PositiveInfinity;
            float my = float.PositiveInfinity;
            foreach (var v in vs)
            {
                mx = MathF.Min(mx, v.X);
                my = MathF.Min(my, v.Y);
            }
            return new(mx, my);
        }
        public static Vec2 Grow(Vec2 vec, (float, float, float, float) area)
        {
            return new(vec.X + area.Item3 + area.Item4, vec.Y + area.Item1 + area.Item2);
        }
        public static Vec2 Shrink(Vec2 vec, (float, float, float, float) area)
        {
            return new(vec.X - area.Item3 - area.Item4, vec.Y - area.Item1 - area.Item2);
        }
    }
}
