using UnityEngine;
using System.Collections;

public class FlexionMonitor : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameters
	public Grapher bendGraph, angleGraph;
	public ZigSkeleton skeleton;
	public float max_forward_turn, max_elbow_bend;
	
	// Local variables
	private Vector3 shoulder, elbow, wrist, upperarm, forearm, spine_up;
	
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
		
		// How straight is the arm ?
		float bend = Vector3.Angle(upperarm, forearm);
		
		// How high has the arm been raised
		float elevation = 180 - Vector3.Angle(skeleton.Torso.up, upperarm);
		
		// plot the data
		if(bendGraph != null)
			bendGraph.newDataPoint(bend);
		if(angleGraph != null)
			angleGraph.newDataPoint(elevation);
	}
}
