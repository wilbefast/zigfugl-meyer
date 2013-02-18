using UnityEngine;
using System.Collections;

public class FlexionMonitor : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameters
	public FailGrapher graph_elbow, graph_elevation, graph_forwards;
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
		
		//! Progress --
		// -- How high has the arm been raised ?
		float elevation = 180 - Vector3.Angle(skeleton.Torso.up, upperarm);
		
		//! Contraints --
		// -- How straight is the arm ?
		float elbow_bend = Vector3.Angle(upperarm, forearm);
		graph_elbow.setFail(elbow_bend > max_elbow_bend);
		// -- How much is the arm been turn towards the front ?
		float forwards = 90 - Vector3.Angle(skeleton.Torso.forward, upperarm);
		graph_forwards.setFail(forwards > max_forward_turn);
		
		// plot the data
		if(graph_elevation != null)
			graph_elevation.newDataPoint(elevation);
		if(graph_elbow != null)
			graph_elbow.newDataPoint(elbow_bend);
		if(graph_forwards != null)
			graph_forwards.newDataPoint(forwards);
	}
}
