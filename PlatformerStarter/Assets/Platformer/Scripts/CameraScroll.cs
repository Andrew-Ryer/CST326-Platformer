using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScroll : MonoBehaviour
{
    public float scrollSpeed = 6f;

    // bounds
    public bool useBounds = false;
    public float minX = 0f;
    public float maxX = 100f;

    void Update()
    {
        float dir = 0f;

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            dir = -1f;
        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            dir = 1f;
        }

        Vector3 pos = transform.position;
        pos.x += dir * scrollSpeed * Time.deltaTime;

        if (useBounds)
        {
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
        }

        transform.position = pos;
    }
}