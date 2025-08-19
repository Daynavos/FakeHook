using UnityEngine;

public class OpenUI : MonoBehaviour
{
    [Header("UI to Open")]
    public GameObject uiPanel;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (uiPanel != null)
            {
                uiPanel.SetActive(true);
            }
        }
    }
}