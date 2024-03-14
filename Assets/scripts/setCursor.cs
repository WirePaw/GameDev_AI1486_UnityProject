using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    void Start()
    {
        Vector2 cursorOffset = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorOffset, CursorMode.Auto);
    }
}
