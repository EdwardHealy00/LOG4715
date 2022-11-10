using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D m_Pointer;
    [SerializeField] private Texture2D m_Busy;
    [SerializeField] private Texture2D m_Grab;
    [SerializeField] private Texture2D m_Grabbing;


    private void Start()
    {
        Cursor.SetCursor(m_Pointer, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursor(CursorState state)
    {
        switch (state)
        {
            case CursorState.Pointer:
                Cursor.SetCursor(m_Pointer, Vector2.zero, CursorMode.Auto);
                break;
            case CursorState.Busy:
                Cursor.SetCursor(m_Busy, new Vector2(16, 16), CursorMode.Auto);
                break;
            case CursorState.Grab:
                Cursor.SetCursor(m_Grab, new Vector2(16, 16), CursorMode.Auto);
                break;
            case CursorState.Grabbing:
                Cursor.SetCursor(m_Grabbing, new Vector2(16, 16), CursorMode.Auto);
                break;
        }
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

}

public enum CursorState
{
    Pointer,
    Busy,
    Grab,
    Grabbing
}
