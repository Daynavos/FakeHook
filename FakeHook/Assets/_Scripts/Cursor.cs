
using System;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public LayerMask hooks;
    private bool _onHook;
    void Start()
    {
        UnityEngine.Cursor.visible = false;
    }

    void Update()
    {
        if (Camera.main == null) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var transform1 = transform;
        mousePos.z = transform1.position.z;
        transform1.position = mousePos;
    }
    //Old Logic
    public bool HookAvailable()
    {
        return Physics2D.OverlapCircle(transform.position, 1, hooks);
    }
}
