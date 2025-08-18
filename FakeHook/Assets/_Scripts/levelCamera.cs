using UnityEngine;

public class levelCamera : MonoBehaviour
{
    public  GameObject player;
    void Update()
    {
        Vector3 camPos = transform.position;   
        camPos.y = player.transform.position.y;
        
        transform.position = camPos;  
    }
}
