using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float StartTime;
    public static float CompleteTime;
    
    public static void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }

    public static void Win()
    {
        CompleteTime = Time.time;
        SceneManager.LoadScene("Win");
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Start()
    {
        StartTime = Time.time;
    }
}
