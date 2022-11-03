using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar_TD : AStarScript
{
    // Start is called before the first frame update
    protected override void Start()
    {
        gridScript = FindObjectOfType<GridScript>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
