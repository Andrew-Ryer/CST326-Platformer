using UnityEngine;

public class Goal : MonoBehaviour
{
    private LevelParser _level;

    void Start()
    {
        _level = FindFirstObjectByType<LevelParser>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("YOU WIN!");
        //_level.ReloadLevel();
    }
}