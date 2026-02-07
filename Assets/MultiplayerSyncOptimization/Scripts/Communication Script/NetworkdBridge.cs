using UnityEngine;

public class NetworkBridge : MonoBehaviour
{
    public RemotePlayerController remotePlayer;
    public bool includeY = false;

    public void SendPosition(Vector3 position)
    {
        short x = PositionCompressor.Compress(position.x);
        short z = PositionCompressor.Compress(position.z);
        short y = PositionCompressor.Compress(position.y);

        byte[] packet;

        if (includeY)
        {
            packet = PositionPacket.Pack(x, y, z);
            remotePlayer.ReceivePacket(packet, true);
        }
        else
        {
            packet = PositionPacket.Pack(x, z);
            remotePlayer.ReceivePacket(packet, false);
        }

        int sizeBits = packet.Length * 8;

        Debug.Log(
            $"Sent Position: {position} | Payload Size: {packet.Length} bytes ({sizeBits} bits)"
        );
    }

}
