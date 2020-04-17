
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPlayerMovement : MonoBehaviour
{
    private bool canMove=false;
    private float cellSize;
    private Vector3 currentPos;
    private Vector3 nextPos;
    private Vector3 moveDir;
    private int node = 0;


    void Start()
    {
        transform.position=Vector3.zero + new Vector3(SimplePathfinding.cellSize/2, SimplePathfinding.cellSize/2 + 1.4f);
        //transform.position = Vector3.zero + new Vector3(Testing.p[0].x, Testing.p[0].y) * cellSize + Vector3.one * cellSize / 2;

        cellSize = SimplePathfinding.cellSize;
        currentPos = transform.position;
        nextPos = transform.position;
    }


    void Update()
    {
        if (Testing.move && node < Testing.p.Count-1)
        {

            currentPos = new Vector3(Testing.p[node].x, Testing.p[node].y) * cellSize + new Vector3(cellSize / 2, cellSize / 2);
            nextPos = new Vector3(Testing.p[node+1].x, Testing.p[node+1].y) * cellSize + new Vector3(cellSize / 2, cellSize / 2);
            moveDir = (nextPos - currentPos).normalized;
            //transform.position += moveDir * 7f * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, nextPos + new Vector3(0f,1.4f), 7f * Time.deltaTime);
            //Debug.Log(transform.position - new Vector3(0, 1.4f, 0) + " "+nextPos+" "+ Vector3.Distance(transform.position - new Vector3(0, 1.5f, 0), nextPos));

            if (Vector3.Distance(transform.position- new Vector3(0,1.4f,0), nextPos) <= 0.1f)
            {
                node++;
            }
        }
     
    }
}
