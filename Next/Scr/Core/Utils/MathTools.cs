using System;

namespace SkySwordKill.Next.Utils;

public static class MathTools
{
    public static double Clamp(double value, double min, double max)
    {
        return Math.Min(Math.Max(value, min), max);
    }
    
    public static float Clamp(float value, float min, float max)
    {
        return Math.Min(Math.Max(value, min), max);
    }

    public static int Clamp(int value, int min, int max)
    {
        return Math.Min(Math.Max(value, min), max);
    }
    
    public static long Clamp(long value, long min, long max)
    {
        return Math.Min(Math.Max(value, min), max);
    }
}