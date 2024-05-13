using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreenText : MonoBehaviour
{
    private TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        text.text = $"And it only took you {(GameManager.CompleteTime - GameManager.StartTime):F2} seconds :O";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
