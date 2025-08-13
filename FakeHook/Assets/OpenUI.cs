using UnityEngine;

public class OpenUI : MonoBehaviour
{
    [Header("UI to Open")]
    public GameObject uiPanel; // Drag your UI GameObject here

    private void Start()
    {
        /*if (uiPanel != null)
        {
            uiPanel.SetActive(false); // Hide UI at start
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (uiPanel != null)
            {
                uiPanel.SetActive(true); // Show UI
            }

            //Destroy(gameObject); // Remove this object
        }
    }
}