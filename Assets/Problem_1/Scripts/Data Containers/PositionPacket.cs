using System;

public static class PositionPacket
{
    public static byte[] Pack(short x, short z)
    {
        byte[] buffer = new byte[4]; 

        Buffer.BlockCopy(BitConverter.GetBytes(x), 0, buffer, 0, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(z), 0, buffer, 2, 2);

        return buffer;
    }

    public static byte[] Pack(short x, short y, short z)
    {
        byte[] buffer = new byte[6]; 

        Buffer.BlockCopy(BitConverter.GetBytes(x), 0, buffer, 0, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(y), 0, buffer, 2, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(z), 0, buffer, 4, 2);

        return buffer;
    }

    public static void Unpack(byte[] buffer, out short x, out short z)
    {
        x = BitConverter.ToInt16(buffer, 0);
        z = BitConverter.ToInt16(buffer, 2);
    }

    public static void Unpack(byte[] buffer, out short x, out short y, out short z)
    {
        x = BitConverter.ToInt16(buffer, 0);
        y = BitConverter.ToInt16(buffer, 2);
        z = BitConverter.ToInt16(buffer, 4);
    }
}
