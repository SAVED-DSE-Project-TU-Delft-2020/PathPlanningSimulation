using System.Collections;
using UnityEngine;

public class Node :  IHeapItem<Node> {
    public int[] coordinates;
    public Vector3 position;
    // public GameObject obj;
    public float fScore;
    public float gScore;
    public Node parent;
    // public NodeBehaviour script;

    // public LineRenderer edge;
    // public GameObject node;

    int heapIndex;


    public Node(int[] coor, Vector3 pos) 
        { 
            coordinates = coor;
            position = pos;
            fScore = int.MaxValue;
            gScore = int.MaxValue;

        }
    // public Vector3 coordinate_to_world(int[] coor){
        
    //     Vector3 diagonal =  Singleton.instance.landing_base.transform.position - Singleton.instance.drone.transform.position;
    //     return new Vector3(((float)coor[0]/100)*(diagonal.x) + Singleton.instance.drone.transform.position.x, 0.0f, ((float)coor[1]/100)*(diagonal.z) + Singleton.instance.drone.transform.position.z);
    // }

    public void setParent(Node par){
        parent = par;
        // edge.SetPosition(0, par.position );
        // edge.SetPosition(1,position);
        // edge.startWidth = .03f;
        // edge.endWidth = 0.03f;
    }
    public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

    	public float fCost {
		get {
			return fScore;
		}
	}

    public float hCost{
        get{
            return fScore-gScore;
        }
    }
    public float cost(){
        if (Physics.CheckSphere(position,0.5f)){
            return 10000;
        }else{
            Vector3 storm_pos = GameObject.Find("Weather storm").transform.position;
            Vector3 populated_pos = GameObject.Find("Highly populated area").transform.position;
            float population_density_cost = 1/(Mathf.Pow (Vector3.Distance(storm_pos, position)/70f,2)+1)*5;
            float weather_forecast_cost = 1/(Mathf.Pow( Vector3.Distance(populated_pos, position)/70f,5)+1)*4;
            float total_cost = population_density_cost + weather_forecast_cost + 1;
            float c = total_cost;
            return c;
        }
    }

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
    

}
