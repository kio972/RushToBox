using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualJoystick : MonoBehaviour
{
    [SerializeField]
    private RectTransform joyStickGroup;
    [SerializeField]
    private RectTransform joyStickHandle;

    public float joyStickLength = 1f;

    public void SetActive(bool value, Vector2 position)
    {
        if (value)
            joyStickGroup.position = position;
        joyStickGroup.gameObject.SetActive(value);
    }

    public void SetHandlePos(Vector2 curPos, Vector2 originPos)
    {
        Vector2 dir = curPos - originPos;
        if (dir.magnitude > joyStickLength)
        {
            // 방향을 가리키는 길이가 1인 벡터
            dir.Normalize();
            dir *= joyStickLength;
            curPos = originPos + dir;
        }

        joyStickHandle.position = curPos;
    }

    public void Start()
    {
        SetActive(false, joyStickGroup.position);
    }
}
