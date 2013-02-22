using UnityEngine;
using System.Collections;

public class DorsiflexionMonitor : ExerciseMonitor
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameters
	public FailGrapher graph_wrist;
	//public float max_forward_turn, max_elbow_bend, max_start_elevation;
	// Local variables
	private Vector3 wrist, fingertip;
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
			wrist_bend();
			
			// progress 
			//monitor_elevation(cheating);
		}
	}
	
	
	//! --------------------------------------------------------------------------
	//! SUBROUTINES -- ROTATION MEASUREMENTS
	//! --------------------------------------------------------------------------
	
	private void reset_joint_position()
	{
		// reset local variables -- NB: '=' operator performs a deep copy
		wrist = skeleton.RightWrist.position;
		fingertip = skeleton.RightFingertip.position;
	}
	
	
	private void wrist_bend()
	{
		// how straight is the arm ?
		float wrist_bend = Vector3.Angle(wrist, fingertip);
		/*if(elbow_bend > max_elbow_bend)
		{
			graph_elbow.setFail(true);
			failedExercise();
			cheating = true;
		}
		else
			graph_elbow.setFail(false);
		*/
		// plot constraint data
		if(graph_wrist != null)
			graph_wrist.newDataPoint(wrist_bend);
	}
}
