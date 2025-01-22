using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D normalCursor; // ��� Ŀ�� ��������Ʈ
    public Texture2D clickCursor;  // Ŭ�� �� Ŀ�� ��������Ʈ
    public CursorMode cursorMode = CursorMode.Auto; // Ŀ�� ���

    private void Start()
    {
        // ��� Ŀ���� �߽� �������� ����
        Vector2 normalHotSpot = new Vector2(normalCursor.width / 2, normalCursor.height / 2);
        Cursor.SetCursor(normalCursor, normalHotSpot, cursorMode);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ��
        {
            Vector2 clickHotSpot = new Vector2(clickCursor.width / 2, clickCursor.height / 2);
            Cursor.SetCursor(clickCursor, clickHotSpot, cursorMode);
        }
        else if (Input.GetMouseButtonUp(0)) // ���콺 ��ư���� ���� �� ��
        {
            Vector2 normalHotSpot = new Vector2(normalCursor.width / 2, normalCursor.height / 2);
            Cursor.SetCursor(normalCursor, normalHotSpot, cursorMode);
        }
    }
}
