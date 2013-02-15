using UnityEngine;
using System.Collections;

public class SkeletonMonitor : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameters
	public Grapher straightnessGraph;
	public Grapher angleGraph;
	public ZigSkeleton skeleton;
	
	// Local variables
	private Vector3 shoulder, elbow, wrist, upperarm, forearm, arm;
	
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
		// Reset local variables -- NB: '=' operator performs a deep copy
		shoulder = skeleton.RightShoulder.position;
		elbow = skeleton.RightElbow.position;
		wrist = skeleton.RightWrist.position;
		upperarm = elbow - shoulder;
		forearm = wrist - elbow;
		arm = wrist - shoulder;
		
		// How straight is the arm ?
		float straightness = Vector3.Dot(upperarm.normalized, forearm.normalized);
		
		// What is the angle between shoulder and wrist ?
		float angle = 1 - Vector3.Angle(Vector3.up, arm) / 180;
		
		// Patient should not be allowed to "cheat" by bending their arm
		if(angle > straightness)
			angle = straightness;

		// plot the data
		if(straightnessGraph != null)
			straightnessGraph.newDataPoint(straightness);
		if(angleGraph != null)
			angleGraph.newDataPoint(angle);
	}
}
