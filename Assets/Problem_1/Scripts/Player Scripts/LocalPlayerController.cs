using UnityEngine;
using TMPro;


public class LocalPlayerController : MonoBehaviour
{
    public float speed = 5f;
    public NetworkBridge networkdBridge;
    public TMP_Text locationText;

    
    [Header("Optimization")]
    public float movementThreshold = 0.1f;
    private Vector3 lastSentPosition;

    void Start()
    {
        lastSentPosition = transform.localPosition;
        networkdBridge.SendPosition(transform.localPosition);
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        transform.localPosition += move * speed * Time.deltaTime;

        if (locationText != null)
        {
            locationText.text = $"Position: {transform.localPosition}";
        }


        // Optimization: Only send if moved 
        if (Vector3.Distance(transform.localPosition, lastSentPosition) > movementThreshold)
        {
            networkdBridge.SendPosition(transform.localPosition);
            lastSentPosition = transform.localPosition;
        }
    }
}
