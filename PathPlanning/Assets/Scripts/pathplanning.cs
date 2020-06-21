using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class pathplanning : MonoBehaviour
{
    public bool drawPath;
    static int N = 100;
    static int W = 10;
    public int t = 0;

    int p = 0;
    public GameObject drone;
    public GameObject takeoff_base;
    bool finished;

    public GameObject landing_base;
    public LineRenderer line;
    public GameObject nodeObj;

    public GameObject target;
    Node current;
    Node start;
    Node end;

    Grid grid;

    List<Vector3> path;


    void Awake(){
        grid = GetComponent<Grid>();
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void FindPath(PathRequest request, Action<PathResult> callback){
        

	Stopwatch sw = new Stopwatch();
        grid.Reset();
		sw.Start();
        Node startNode = grid.NodeFromWorldPoint(request.pathStart);
		Node targetNode = grid.NodeFromWorldPoint(request.pathEnd);
		// startNode.parent = startNode;

        path = new List<Vector3>();
        Heap<Node>  openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        startNode.fScore = 0;
        startNode.gScore = 0;
        openSet.Add(startNode);

        while (openSet.Count > 0){
            current = openSet.RemoveFirst();
        
            closedSet.Add(current);
            // print(closedSet.Count);
            // current.edge.enabled = true;

            if (current == targetNode && path.Count == 0){
                sw.Stop();
                print ("Path found: " + sw.ElapsedMilliseconds + " ms");
                while (current != startNode && current != null){
                    path.Insert(0, current.position);
                    current = current.parent;
                }
                Gizmos.color = Color.black;
    
                break;

            }
                foreach (Node neighbour in grid.GetNeighbours(current)) {

            
                        if (closedSet.Contains(neighbour)){
                            continue;
                        }
                        
                        // The distance from start to a neighbor
                        float tentative_gScore = current.gScore +  neighbour.cost()*Vector3.Distance(current.position, neighbour.position);

                        if (tentative_gScore < neighbour.gScore){
                            neighbour.setParent(current);
                            neighbour.gScore = tentative_gScore;
                            neighbour.fScore = neighbour.gScore +  1.0f*Vector3.Distance(neighbour.position, targetNode.position);
                        }

                        if (!openSet.Contains(neighbour)){
                            openSet.Add(neighbour);
                        }else{
                            openSet.UpdateItem(neighbour);
                        }
            }
        }
        
        callback (new PathResult (path.ToArray(),true, request.callback));
    }
    // Update is called once per frame

    // public void reconstruct(Vector3 unitPos, Vector3 targetPos){
    //     path = new List<Vector3>();
    //     Node startNode = grid.NodeFromWorldPoint(targetPos);
    //     Node node = grid.NodeFromWorldPoint(unitPos);
    //      while (node != startNode){
    //         path.Insert(0, node.position);
    //         node = node.parent;
    //     }
    // }
    IEnumerator SpawnLoop (List<Vector3> pathfound) {
    while (true){
        yield return new WaitForSeconds (0.6f);
            if (p<pathfound.Count){
                target.transform.position = pathfound[p] + new Vector3(0f, 5f, 0f);
                p+=1;
            }
        }
     }

     public void OnDrawGizmos() {
        if (drawPath){

            foreach (Vector3 p in path) {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(p + Vector3.up, 6);
            }
        }

		// Gizmos.color = Color.white;
		// foreach (Line l in turnBoundaries) {
		// 	l.DrawWithGizmos (10);
		// }

	}
     
}
