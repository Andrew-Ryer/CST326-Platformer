using UnityEngine;

public class Hazard : MonoBehaviour
{
    //PT2
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