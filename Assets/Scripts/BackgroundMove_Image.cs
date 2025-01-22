using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMove_Image : MonoBehaviour
{
    private Material material; // ����� Material ����
    private Vector2 offset;    // �ؽ�ó ������
    float scrollSpeed; // ��ũ�� �ӵ� (���� ����)

    private void Start()
    {
        // SpriteRenderer���� Material�� ������
        material = GetComponent<Image>().material;

        // �ʱ� ������ ����
        offset = material.mainTextureOffset;

        scrollSpeed = 0.2f;
    }

    private void Update()
    {
        // �ð��� ���� x �������� ������ �̵�
        offset.x += scrollSpeed * Time.deltaTime;

        // Material�� Offset ������Ʈ
        material.mainTextureOffset = offset;
    }
}
