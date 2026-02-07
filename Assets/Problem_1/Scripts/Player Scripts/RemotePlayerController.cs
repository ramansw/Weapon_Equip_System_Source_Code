using UnityEngine;
using TMPro;


public class RemotePlayerController : MonoBehaviour
{
    public float smoothSpeed = 8f;
    public TMP_Text locationText;
    private Vector3 targetPosition;


    public void ReceivePosition(short x, short z)
    {
        float realX = PositionCompressor.Decompress(x);
        float realZ = PositionCompressor.Decompress(z);

        targetPosition = new Vector3(realX, transform.localPosition.y, realZ);

        Debug.Log($"Received Position: {targetPosition}");
    }

    public void ReceivePosition(short x, short y, short z)
    {
        float realX = PositionCompressor.Decompress(x);
        float realY = PositionCompressor.Decompress(y);
        float realZ = PositionCompressor.Decompress(z);

        targetPosition = new Vector3(realX, realY, realZ);

        Debug.Log($"Received Position: {targetPosition}");
    }
        public void ReceivePacket(byte[] packet, bool includeY)
        {
            short x, y, z;

            if (includeY)
            {
                PositionPacket.Unpack(packet, out x, out y, out z);

                targetPosition = new Vector3(
                    PositionCompressor.Decompress(x),
                    PositionCompressor.Decompress(y),
                    PositionCompressor.Decompress(z)
                );
            }
            else
            {
                PositionPacket.Unpack(packet, out x, out z);

                targetPosition = new Vector3(
                    PositionCompressor.Decompress(x),
                    transform.localPosition.y,
                    PositionCompressor.Decompress(z)
                );
            }

            Debug.Log($"Received Position: {targetPosition}");
        }


    void Update()
    {
        transform.localPosition =
            Vector3.Lerp(transform.localPosition, targetPosition, smoothSpeed * Time.deltaTime);

        if (locationText != null)
        {
            locationText.text = $"Position: {transform.localPosition}";
        }
    }

}
