using TMPro;
using UnityEngine;

public class TimeControlller : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private float timeLeft = 360;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        timeText.text = $"TIME\n{timeLeft.ToString("#")}";
    }
}
