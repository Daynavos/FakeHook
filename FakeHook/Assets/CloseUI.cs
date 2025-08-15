using UnityEngine;

public class CloseUI : MonoBehaviour
{
    [Header("UI to Disable")]
    public GameObject uiPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (uiPanel != null)
            {
                uiPanel.SetActive(false);
            }
        }
    }
}