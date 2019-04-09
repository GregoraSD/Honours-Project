using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisibility : MonoBehaviour
{
    private void Awake()
    {
        UnlockCursor();
    }

    private void OnEnable()
    {
        UnlockCursor();
    }

    private void Start()
    {
        UnlockCursor();
    }

    private static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}