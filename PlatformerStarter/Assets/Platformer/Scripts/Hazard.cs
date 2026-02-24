using UnityEngine;

public class Hazard : MonoBehaviour
{
    private LevelParser _level;

    void Start()
    {
        _level = FindFirstObjectByType<LevelParser>();
    }

    private void OnTriggerEnter(Collider other)
    { 
        Debug.Log("YOU DIED!");
        _level.ReloadLevel();
    }
}