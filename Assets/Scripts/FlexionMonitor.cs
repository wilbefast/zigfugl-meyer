using UnityEngine;
using System.Collections;

public class FlexionMonitor : ExerciseMonitor
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameters
	public FailGrapher graph_elbow, graph_backwards;
	public float max_back_turn, max_elbow_bend, max_start_elevation;
	
	// Local variables
	private Vector3 shoulder, elbow, wrist, upperarm, forearm, spine_up;
	private bool cheating = false;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	
	// Update is called once per frame
	void Update ()
	{
		if(state != State.NO_USER)
		{
			// get new joint positions
			reset_joint_position();
				
			// constraints
			cheating = false;
			// these two can reset 'cheating' to 'true' if applicable
			monitor_bend();
			monitor_backwards();
			
			// progress 
			monitor_elevation(cheating);
		}
	}
	
	
	//! --------------------------------------------------------------------------
	//! SUBROUTINES -- ROTATION MEASUREMENTS
	//! --------------------------------------------------------------------------
	
	private void reset_joint_position()
	{
		// reset local variables -- NB: '=' operator performs a deep copy
		shoulder = skeleton.getTransform(ZigJointId.RightShoulder, !right_hand).position;
		elbow = skeleton.getTransform(ZigJointId.RightElbow, !right_hand).position;
		wrist = skeleton.getTransform(ZigJointId.RightWrist, !right_hand).position;
		upperarm = elbow - shoulder;
		forearm = wrist - elbow;
	}
	
	private void monitor_elevation(bool cheating)
	{
		// how high has the arm been raised ?
		float elevation = 180 - Vector3.Angle(skeleton.Torso.up, upperarm);
		if(state == State.RESTARTING_EXERCISE 
			&& !cheating && elevation < max_start_elevation)
		{
			// try again !
			assumedStartPosition();
			graph_elbow.setFail(false);
			graph_backwards.setFail(false);
		}
			
		// plot progress data
		if(primary_graph != null)
			primary_graph.newDataPoint(elevation);
	}
	
	private void monitor_bend()
	{
		// how straight is the arm ?
		float elbow_bend = Vector3.Angle(upperarm, forearm);
		if(elbow_bend > max_elbow_bend)
		{
			graph_elbow.setFail(true);
			failedExercise();
			cheating = true;
		}
		else
			graph_elbow.setFail(false);
		// plot constraint data
		if(graph_elbow != null)
			graph_elbow.newDataPoint(elbow_bend);
	}
	
	private void monitor_backwards()
	{
		// how much is the arm been turn towards the front ?
		float backwards = Vector3.Angle(skeleton.Torso.forward, upperarm);
		Debug.Log(backwards.ToString ());
		if(backwards > max_back_turn)
		{
			graph_backwards.setFail(true);
			failedExercise();
			cheating = true;
		}
		else
			graph_backwards.setFail(false);
		// plot constraint data
		if(graph_backwards != null)
			graph_backwards.newDataPoint(backwards);
	}
}
