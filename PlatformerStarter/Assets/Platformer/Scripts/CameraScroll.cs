using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScroll : MonoBehaviour
{
    [Header("Manual Scroll (Arrow Keys)")]
    public float scrollSpeed = 6f;
    public bool allowManualScroll = true;

    [Header("Follow Player")]
    public bool followPlayer = true;
    [Tooltip("Camera will follow the first object found with this tag.")]
    public string playerTag = "Player";
    public Vector3 followOffset = new Vector3(0f, 0f, -10f);
    [Tooltip("Higher values = snappier follow. 0 = instant.")]
    public float followSmooth = 8f;
    public bool followX = true;
    public bool followY = false;

    [Header("Bounds")]
    public bool useBounds = false;
    public float minX = 0f;
    public float maxX = 100f;

    private Transform _target;

    void Start()
    {
        if (followPlayer)
            TryFindTarget();
    }

    void Update()
    {
        // Manual scroll only if enabled AND not currently following a target
        if (!allowManualScroll || (followPlayer && _target != null))
            return;

        float dir = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed) dir = -1f;
            else if (Keyboard.current.rightArrowKey.isPressed) dir = 1f;
        }

        Vector3 pos = transform.position;
        pos.x += dir * scrollSpeed * Time.deltaTime;

        if (useBounds)
            pos.x = Mathf.Clamp(pos.x, minX, maxX);

        transform.position = pos;
    }

    void LateUpdate()
    {
        if (!followPlayer)
            return;

        if (_target == null)
            TryFindTarget();

        if (_target == null)
            return;

        Vector3 desired = transform.position;
        Vector3 targetPos = _target.position + followOffset;

        if (followX) desired.x = targetPos.x;
        if (followY) desired.y = targetPos.y;
        desired.z = targetPos.z; // keep depth consistent

        if (useBounds)
            desired.x = Mathf.Clamp(desired.x, minX, maxX);

        if (followSmooth <= 0f)
        {
            transform.position = desired;
        }
        else
        {
            // Exponential smoothing (stable across framerates)
            float t = 1f - Mathf.Exp(-followSmooth * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desired, t);
        }
    }

    private void TryFindTarget()
    {
        GameObject go = GameObject.FindGameObjectWithTag(playerTag);
        _target = (go != null) ? go.transform : null;
    }
}