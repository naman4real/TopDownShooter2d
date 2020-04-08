﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    
    //[SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField] private GameObject player;
    //[SerializeField] private CharacterPathfindingMovementHandler characterPathfinding;
    private Pathfinding pathfinding;
    private float cellSize;
    public static bool move = false;
    public static List<PathNode> p;
    

    private void Start() {
        pathfinding = new Pathfinding(20, 10);
        //pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        cellSize = Pathfinding.cellSize;
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
        player.transform.position = Vector2.zero + new Vector2(Pathfinding.cellSize / 2, Pathfinding.cellSize / 2 + 1.5f);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) 
        {
            Testing.move = true;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
            Testing.p = path;
            if (path != null) {
                for (int i=0; i<path.Count - 1; i++) 
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * cellSize + Vector3.one * cellSize/2, new Vector3(path[i+1].x, path[i+1].y) * cellSize + Vector3.one * cellSize/2, Color.green, 5f);
                }
             

            }

            //characterPathfinding.SetTargetPosition(mouseWorldPosition);
            
        }

        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }

}
