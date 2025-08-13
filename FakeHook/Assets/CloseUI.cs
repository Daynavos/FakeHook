using UnityEngine;

public class CloseUI : MonoBehaviour
{
    [Header("UI to Disable")]
    public GameObject uiPanel; // Drag your UI GameObject here

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (uiPanel != null)
            {
                uiPanel.SetActive(false); // Hide UI
            }
        }
    }
}