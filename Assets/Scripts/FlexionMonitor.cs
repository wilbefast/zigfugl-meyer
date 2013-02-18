using UnityEngine;
using System.Collections;

public class FlexionMonitor : ExerciseMonitor
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameters
	public FailGrapher graph_elbow, graph_forwards;
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
		if(elbow_bend > max_elbow_bend)
		{
			graph_elbow.setFail(true);
			failedExercise();
		}
		// -- How much is the arm been turn towards the front ?
		float forwards = 90 - Vector3.Angle(skeleton.Torso.forward, upperarm);
		if(forwards > max_forward_turn)
		{
			graph_elbow.setFail(true);
			failedExercise();
		}
		
		// plot progress data
		if(primary_graph != null)
			primary_graph.newDataPoint(elevation);
		
		// plot constraint data
		if(graph_elbow != null)
			graph_elbow.newDataPoint(elbow_bend);
		if(graph_forwards != null)
			graph_forwards.newDataPoint(forwards);
	}
}
