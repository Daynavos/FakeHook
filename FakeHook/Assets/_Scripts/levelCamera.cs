using UnityEngine;

public class levelCamera : MonoBehaviour
{
    public GameObject player;
    public float minY = 0f;      
    public float maxY = 10f;     

    void Update()
    {
        Vector3 camPos = transform.position;   
        camPos.y = Mathf.Clamp(player.transform.position.y, minY, maxY);
        
        transform.position = camPos;  
    }
}
