using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public bool displayGridGizmos;
	public Vector2 gridSize;
	public float nodeRadius;
    float nodeDiameter;

	Node[,] grid;


	int N, M;

    void Awake(){
        nodeDiameter = nodeRadius*2;
		N = Mathf.RoundToInt(gridSize.x/nodeDiameter);
		M = Mathf.RoundToInt(gridSize.y/nodeDiameter);
        CreateGrid();

    }

	void CreateGrid() {
		grid = new Node[N,M];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x/2 - Vector3.forward * gridSize.y/2;

		for (int x = 0; x < N; x ++) {
			for (int y = 0; y < M; y ++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

				grid[x,y] = new Node(new int[]{x,y}, worldPoint);
			}
		}

	}

    
	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -2; x <= 2; x++) {
			for (int y = -2; y <= 2; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.coordinates[0] + x;
				int checkY = node.coordinates[1] + y;

				if (checkX >= 0 && checkX < N && checkY >= 0 && checkY < M) {
					neighbours.Add(grid[checkX,checkY]);
				}
			}
		}

		return neighbours;
	}


    
	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridSize.x/2) / gridSize.x;
		float percentY = (worldPosition.z + gridSize.y/2) / gridSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((N-1) * percentX);
		int y = Mathf.RoundToInt((M-1) * percentY);
		return grid[x,y];
	}

    public void Reset(){
        foreach(Node node in grid){
            node.fScore = int.MaxValue;
            node.gScore = int.MaxValue;
            node.parent = null;
            // node.total_cost = node.cost()
        }
    }


	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridSize.x,1,gridSize.y));
		if (grid != null && displayGridGizmos) {

			foreach (Node n in grid) {

				Gizmos.color = Color.Lerp (Color.green, Color.red, Mathf.InverseLerp (0, 5, n.cost()));
				// Gizmos.color = (n.walkable)?Gizmos.color:Color.red;
				Gizmos.DrawCube(n.position,Vector3.one * (nodeRadius));
			}
		}
	}

    public int MaxSize {
		get {
			return N * M;
		}
	}

	[System.Serializable]
	public class TerrainType {
		public LayerMask terrainMask;
		public int terrainPenalty;
	}


}