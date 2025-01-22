using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D normalCursor; // 평소 커서 스프라이트
    public Texture2D clickCursor;  // 클릭 시 커서 스프라이트
    public CursorMode cursorMode = CursorMode.Auto; // 커서 모드

    private void Start()
    {
        // 평소 커서를 중심 기준으로 설정
        Vector2 normalHotSpot = new Vector2(normalCursor.width / 2, normalCursor.height / 2);
        Cursor.SetCursor(normalCursor, normalHotSpot, cursorMode);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            Vector2 clickHotSpot = new Vector2(clickCursor.width / 2, clickCursor.height / 2);
            Cursor.SetCursor(clickCursor, clickHotSpot, cursorMode);
        }
        else if (Input.GetMouseButtonUp(0)) // 마우스 버튼에서 손을 뗄 때
        {
            Vector2 normalHotSpot = new Vector2(normalCursor.width / 2, normalCursor.height / 2);
            Cursor.SetCursor(normalCursor, normalHotSpot, cursorMode);
        }
    }
}
