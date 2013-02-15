using UnityEngine;
using System.Collections;

public class SkeletonMonitor : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameters
	public Grapher graph;
	public ZigSkeleton skeleton;
	
	// Local variables
	private Vector3 shoulder, elbow, wrist, arm, forearm;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Reset local variables
		shoulder = skeleton.RightShoulder.position;
		elbow = skeleton.RightElbow.position;
		wrist = skeleton.RightWrist.position;
		arm = elbow - shoulder;
		forearm = wrist - elbow;
		
		// How straight is the arm ?
		float straightness = Vector3.Dot(arm.normalized, forearm.normalized);
		
		
		float data = skeleton.RightShoulder.position.y 
							- skeleton.RightWrist.position.y ;

		// plot the delta height
		if(graph != null)
			graph.newDataPoint(straightness);
	}
}
