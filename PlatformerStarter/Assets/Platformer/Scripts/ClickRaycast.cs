using UnityEngine;
using UnityEngine.InputSystem;

public class ClickRaycast : MonoBehaviour
{
    public Camera rayCamera;
    public Transform debugSphere;

    public GameManager gameManager;

    public int brickScore = 100;
    public int questionScore = 100;
    public int coinScore = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rayCamera == null)
        {
            rayCamera = Camera.main;
        }
    }
    
    public void DestroyBrick(GameObject hitObj)
    {
        if (hitObj.CompareTag("Brick"))
        { 
            gameManager.AddScore(brickScore);
            Destroy(hitObj);
        }
        
        if (hitObj.CompareTag("Question"))
        { 
            gameManager.AddScore(questionScore);
            gameManager.AddCoin(1);
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
                    gameManager.AddScore(questionScore);
                }
            }
        }
    }
}
