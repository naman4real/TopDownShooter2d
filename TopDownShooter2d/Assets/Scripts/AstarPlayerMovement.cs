
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
        transform.position=Vector3.zero + new Vector3(Pathfinding.cellSize/2, Pathfinding.cellSize/2 + 1.5f);
        //transform.position = Vector3.zero + new Vector3(Testing.p[0].x, Testing.p[0].y) * cellSize + Vector3.one * cellSize / 2;

        cellSize = Pathfinding.cellSize;
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
            transform.position += moveDir * 10f * Time.deltaTime;
            Debug.Log(transform.position - new Vector3(0, 1.5f, 0) + " "+nextPos+" "+ Vector3.Distance(transform.position - new Vector3(0, 1.5f, 0), nextPos));

            if (Vector3.Distance(transform.position- new Vector3(0,1.5f,0), nextPos) <= 0.05f)
            {
                node++;
            }
        }
        //if (canMove)
        //{
        //    int i=-1;
        //    while (i < Testing.p.Count-1)
        //    {


        //        if(Vector3.Distance(transform.position, nextPos) <= 0.0001f)
        //        {
        //            i++;
        //            currentPos = new Vector3(Testing.p[i].x, Testing.p[i].y) * cellSize + Vector3.one * cellSize / 2;
        //            nextPos = new Vector3(Testing.p[i + 1].x, Testing.p[i + 1].y) * cellSize + Vector3.one * cellSize / 2;
        //            moveDir = (nextPos - currentPos).normalized;

        //        }
        //        transform.position += moveDir * 10f * Time.deltaTime;
        //    }
        //}
    }
}
