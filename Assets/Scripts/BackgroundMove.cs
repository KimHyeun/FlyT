using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    private Material material; // ����� Material ����
    private Vector2 offset;    // �ؽ�ó ������
   // float scrollSpeed; // ��ũ�� �ӵ� (���� ����)

    private void Start()
    {
        // SpriteRenderer���� Material�� ������
        material = GetComponent<SpriteRenderer>().material;

        // �ʱ� ������ ����
        offset = material.mainTextureOffset;

        
    }

    private void Update()
    {
        // �ð��� ���� x �������� ������ �̵�
        offset.x += GameManager.Instance.scrollSpeed * Time.deltaTime;

        // Material�� Offset ������Ʈ
        material.mainTextureOffset = offset;
    }
}
