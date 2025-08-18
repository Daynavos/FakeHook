
using System;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public LayerMask hooks;
    private bool _onHook;
    public GameObject player;
    public float maxDistanceToPlayer = 5f;
    void Start()
    {
        UnityEngine.Cursor.visible = false;
    }

    void Update()
    {
        if (Camera.main == null) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cursorPos = transform;
        mousePos.z = cursorPos.position.z;
        Vector3 direction = mousePos - player.transform.position;
        if (direction.magnitude>maxDistanceToPlayer)
        {
            direction = direction.normalized * maxDistanceToPlayer;
        }
        transform.position = player.transform.position + direction;
    }
    //Old Logic
    public bool HookAvailable()
    {
        return Physics2D.OverlapCircle(transform.position, 1, hooks);
    }
}
