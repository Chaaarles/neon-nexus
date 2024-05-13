using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipCanvasMovement : MonoBehaviour
{
    public Transform PlayerPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = PlayerPosition.position + new Vector3(0, 1, 0);
    }
}
