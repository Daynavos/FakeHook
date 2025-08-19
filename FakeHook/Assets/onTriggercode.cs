using UnityEngine;

public class onTriggercode : MonoBehaviour
{

    public GameObject tutorial;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            tutorial.SetActive(true);
        }
    }
}
