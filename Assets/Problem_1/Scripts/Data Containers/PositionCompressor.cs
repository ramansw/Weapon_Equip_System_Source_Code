using UnityEngine;

public static class PositionCompressor
{
    private const float precision = 100f; 

    public static short Compress(float value)
    {
        return (short)Mathf.RoundToInt(value * precision);
    }

    public static float Decompress(short value)
    {
        return value / precision;
    }

}
