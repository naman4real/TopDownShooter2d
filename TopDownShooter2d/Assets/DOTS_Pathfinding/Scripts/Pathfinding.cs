
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using CodeMonkey.Utils;

public class Pathfinding : MonoBehaviour 
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private Grid<PathNode> grid;
    public NativeArray<float3> enemyPos;
    private NativeArray<int2> enemyPosCoord;
    public float cellSize;
    public int cellWidth;
    public int cellHeight;
    private NativeArray<JobHandle> jobHandleArray;

    [SerializeField] private Transform playerTransform;
    public List<Transform> enemyTransform;
    int j=0;





    private void Start()
    {
        grid = new Grid<PathNode>(cellWidth, cellHeight, cellSize, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        //enemyTransform.position = Vector3.zero + new Vector3(cellSize / 2, cellSize / 2 + 1.4f);
        

    }
    private void Update() 
    {

        grid.GetXY(playerTransform.position, out int x, out int y);
                      
        enemyPos = new NativeArray<float3>(enemyTransform.Count, Allocator.TempJob);
        enemyPosCoord = new NativeArray<int2>(enemyTransform.Count, Allocator.TempJob);

    

        for (int i = 0; i < enemyTransform.Count; i++)
        {

            enemyPos[i] = enemyTransform[i].position;
            grid.GetXY(enemyTransform[i].position, out int u, out int v);
            enemyPosCoord[i] = new int2(u, v);
        }

        FindPathJob findPathJob = new FindPathJob
        {
            startPosition = enemyPosCoord,
            endPosition = new int2(x, y),
            width = cellWidth,
            height = cellHeight,
            enemyPosition = enemyPos,
            deltaTime = Time.deltaTime,
            cellSize = cellSize

        };
        JobHandle jobHandle = findPathJob.Schedule(enemyTransform.Count,1);
        jobHandle.Complete();

        for(int i = 0; i < enemyTransform.Count; i++)
        {
            enemyTransform[i].position = enemyPos[i];
        }
        

        enemyPos.Dispose();
        enemyPosCoord.Dispose();





        //}



        //Debug.Log("Time: " + ((Time.realtimeSinceStartup - startTime) * 1000f));
        //}, 1f);
    }

    [BurstCompile]
    private struct FindPathJob : IJobParallelFor {

        public NativeArray<int2> startPosition;
        public int2 endPosition;
        public int width;
        public int height;
        public NativeArray<float3> enemyPosition;
        public float deltaTime;
        public float cellSize;

        private int node;
        private float3 nextPos;


        public void Execute(int index) {
            int2 gridSize = new int2(width, height);

            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

            for (int x = 0; x < gridSize.x; x++) {
                for (int y = 0; y < gridSize.y; y++) {
                    PathNode pathNode = new PathNode();
                    pathNode.x = x;
                    pathNode.y = y;
                    pathNode.index = CalculateIndex(x, y, gridSize.x);

                    pathNode.gCost = int.MaxValue;
                    pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
                    pathNode.CalculateFCost();

                    pathNode.isWalkable = true;
                    pathNode.cameFromNodeIndex = -1;

                    pathNodeArray[pathNode.index] = pathNode;
                }
            }

            /*
            // Place Testing Walls
            {
                PathNode walkablePathNode = pathNodeArray[CalculateIndex(1, 0, gridSize.x)];
                walkablePathNode.SetIsWalkable(false);
                pathNodeArray[CalculateIndex(1, 0, gridSize.x)] = walkablePathNode;

                walkablePathNode = pathNodeArray[CalculateIndex(1, 1, gridSize.x)];
                walkablePathNode.SetIsWalkable(false);
                pathNodeArray[CalculateIndex(1, 1, gridSize.x)] = walkablePathNode;

                walkablePathNode = pathNodeArray[CalculateIndex(1, 2, gridSize.x)];
                walkablePathNode.SetIsWalkable(false);
                pathNodeArray[CalculateIndex(1, 2, gridSize.x)] = walkablePathNode;
            }
            */

            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
            neighbourOffsetArray[0] = new int2(-1, 0); // Left
            neighbourOffsetArray[1] = new int2(+1, 0); // Right
            neighbourOffsetArray[2] = new int2(0, +1); // Up
            neighbourOffsetArray[3] = new int2(0, -1); // Down
            neighbourOffsetArray[4] = new int2(-1, -1); // Left Down
            neighbourOffsetArray[5] = new int2(-1, +1); // Left Up
            neighbourOffsetArray[6] = new int2(+1, -1); // Right Down
            neighbourOffsetArray[7] = new int2(+1, +1); // Right Up

            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

            PathNode startNode = pathNodeArray[CalculateIndex(startPosition[index].x, startPosition[index].y, gridSize.x)];
            startNode.gCost = 0;
            startNode.CalculateFCost();
            pathNodeArray[startNode.index] = startNode;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startNode.index);

            while (openList.Length > 0) {
                int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                if (currentNodeIndex == endNodeIndex) {
                    // Reached our destination!
                    break;
                }

                // Remove current node from Open List
                for (int i = 0; i < openList.Length; i++) {
                    if (openList[i] == currentNodeIndex) {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++) {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                    if (!IsPositionInsideGrid(neighbourPosition, gridSize)) {
                        // Neighbour not valid position
                        continue;
                    }

                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);

                    if (closedList.Contains(neighbourNodeIndex)) {
                        // Already searched this node
                        continue;
                    }

                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                    if (!neighbourNode.isWalkable) {
                        // Not walkable
                        continue;
                    }

                    int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

	                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
	                if (tentativeGCost < neighbourNode.gCost) {
		                neighbourNode.cameFromNodeIndex = currentNodeIndex;
		                neighbourNode.gCost = tentativeGCost;
		                neighbourNode.CalculateFCost();
		                pathNodeArray[neighbourNodeIndex] = neighbourNode;

		                if (!openList.Contains(neighbourNode.index)) {
			                openList.Add(neighbourNode.index);
		                }
	                }

                }
            }
            node = 0;
            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.cameFromNodeIndex == -1) {
                // Didn't find a path!
                Debug.Log("Didn't find a path!");
            } else {
                // Found a path
                NativeList<int2> path = CalculatePath(pathNodeArray, endNode);

                //foreach (int2 pathPosition in path)
                //{
                //    Debug.Log(pathPosition + " path yo");
                //}
                if (node < path.Length - 1)
                {
                    //currentPos = new float3(path[node].x, path[node].y, 0f) * cellSize + new float3(cellSize / 2, cellSize / 2, 0f);
                    nextPos = new float3(path[node + 1][0], path[node + 1][1],0f) * cellSize + new float3(cellSize / 2, cellSize / 2,0f);
                    enemyPosition[index] = Vector3.MoveTowards(enemyPosition[index], nextPos, 7f * deltaTime);

                    //if (Vector3.Distance(enemyPosition - new float3(0, 1.4f, 0), nextPos) <= 0.1f)
                    //{
                    //    node++;
                    //}
                }






                path.Dispose();
            }

            pathNodeArray.Dispose();
            
            neighbourOffsetArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
        }
        
        private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode) {
            if (endNode.cameFromNodeIndex == -1) {
                // Couldn't find a path!
                return new NativeList<int2>(Allocator.Temp);
            } else {
                // Found a path
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(endNode.x, endNode.y));

                PathNode currentNode = endNode;
                while (currentNode.cameFromNodeIndex != -1) {
                    PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                    path.Add(new int2(cameFromNode.x, cameFromNode.y));
                    currentNode = cameFromNode;
                }

                return path;
            }
        }

        private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize) {
            return
                gridPosition.x >= 0 && 
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y;
        }

        private int CalculateIndex(int x, int y, int gridWidth) {
            return x + y * gridWidth;
        }

        private int CalculateDistanceCost(int2 aPosition, int2 bPosition) {
            int xDistance = math.abs(aPosition.x - bPosition.x);
            int yDistance = math.abs(aPosition.y - bPosition.y);
            int remaining = math.abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

    
        private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray) {
            PathNode lowestCostPathNode = pathNodeArray[openList[0]];
            for (int i = 1; i < openList.Length; i++) {
                PathNode testPathNode = pathNodeArray[openList[i]];
                if (testPathNode.fCost < lowestCostPathNode.fCost) {
                    lowestCostPathNode = testPathNode;
                }
            }
            return lowestCostPathNode.index;
        }


        public struct PathNode {
            public int x;
            public int y;

            public int index;

            public int gCost;
            public int hCost;
            public int fCost;

            public bool isWalkable;

            public int cameFromNodeIndex;

            public void CalculateFCost() {
                fCost = gCost + hCost;
            }

            public void SetIsWalkable(bool isWalkable) {
                this.isWalkable = isWalkable;
            }
        }

    }

}
