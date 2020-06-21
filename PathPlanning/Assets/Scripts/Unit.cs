using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour

{
    const float minPathUpdateTime = .2f;
	const float pathUpdateMoveThreshold = .5f;

	public Transform target;
	public float speed = 20;
	public float turnSpeed = 3;
	public float turnDst = 5;
	public float stoppingDst = 10;    // Start is called before the first frame update
    void Start()
    {
        		StartCoroutine (UpdatePath ());

    }


	public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
		if (pathSuccessful) {
			// path = new Path(waypoints, transform.position, turnDst, stoppingDst);

			// StopCoroutine("FollowPath");
			// StartCoroutine("FollowPath");
            print("Path Found!!!!");
		}
	}
    IEnumerator UpdatePath() {

		if (Time.timeSinceLevelLoad < .3f) {
			yield return new WaitForSeconds (.3f);
		}
		PathRequestManager.RequestPath (new PathRequest(transform.position, target.position, OnPathFound));
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target.position;
        Vector3 dronePosOld = transform.position;

		while (true) {
			yield return new WaitForSeconds (minPathUpdateTime);
			// print (((target.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
			if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
		        PathRequestManager.RequestPath (new PathRequest(transform.position, target.position, OnPathFound));
				targetPosOld = target.position;
            }else if(transform.position != dronePosOld){
                dronePosOld = transform.position ;
		        PathRequestManager.RequestPath (new PathRequest(transform.position, target.position, OnPathFound));

            }else{
                		        PathRequestManager.RequestPath (new PathRequest(transform.position, target.position, OnPathFound));

            }
		}
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
