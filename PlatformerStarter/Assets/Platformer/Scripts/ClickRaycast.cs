using UnityEngine;
using UnityEngine.InputSystem;

public class ClickRaycast : MonoBehaviour
{
    public Camera rayCamera;
    public Transform debugSphere;

    public GameManager gameManager;

    public int brickScore = 50;
    public int questionScore = 200;
    public int coinScore = 200;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rayCamera == null)
        {
            rayCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse -> Ray
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray screenRay = rayCamera.ScreenPointToRay(mousePosition);

        // Always draw a debug ray
        Debug.DrawRay(screenRay.origin, screenRay.direction * 100f, Color.blue);

        // Raycast hit?
        if (Physics.Raycast(screenRay, out RaycastHit hitInfo, 200f))
        {
            Debug.DrawLine(screenRay.origin, hitInfo.point, Color.magenta);

            // Only act on click
            if (!Mouse.current.leftButton.wasPressedThisFrame)
            {
                return;
            }

            if (debugSphere != null)
            {
                debugSphere.position = hitInfo.point;
            }

            // Use the collider's GameObject (or root if collider is on a child)
            GameObject hitObj = hitInfo.collider.gameObject;

            // If the collider is on a child but the tag is on the parent prefab:
            if (!hitObj.CompareTag("Brick") && !hitObj.CompareTag("Question"))
                hitObj = hitObj.transform.root.gameObject;

            if (hitObj.CompareTag("Brick"))
            {
                if (gameManager != null)
                    gameManager.AddScore(brickScore);

                Destroy(hitObj);
            }
            else if (hitObj.CompareTag("Question"))
            {
                if (gameManager != null)
                {
                    gameManager.AddCoin(1);
                    gameManager.AddScore(questionScore); // optional
                }
            }
        }
    }
}
