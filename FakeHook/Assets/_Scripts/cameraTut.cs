using UnityEngine;

public class cameraTut : MonoBehaviour
{
    public GameObject player;
    
    public float minX = -2.96f;
    public float maxX =  28.6f;
    public float minY =  0.7f;
    public float maxY =  20.57f;

    void Update()
    {
        Vector3 camPos = transform.position;

        // Clamp X and Y to stay within bounds
        camPos.x = Mathf.Clamp(player.transform.position.x, minX, maxX);
        camPos.y = Mathf.Clamp(player.transform.position.y, minY, maxY);

        transform.position = camPos;
    }
}
