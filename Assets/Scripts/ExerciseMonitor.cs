using UnityEngine;
using System.Collections;

public class ExerciseMonitor : MonoBehaviour 
{
	//! --------------------------------------------------------------------------
	//! NESTING
	//! --------------------------------------------------------------------------
	
	public enum State
	{
		NO_USER,
		RESTARTING_EXERCISE, 		// waiting for user to assume starting position
		PERFORMING_EXERCISE
	};
	
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameters
	public ZigSkeleton skeleton;
	public FailGrapher primary_graph;
	
	// local variables
	protected State state = State.NO_USER;
	protected GameObject hand_choice;
	private GameObject state_indicator;
	
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	
	// Use this for initialization
	void Start ()
	{
		hand_choice = GameObject.Find("Hand");
		state_indicator = new GameObject("state_indicator");
		GUIText caption_gui = (GUIText)state_indicator.AddComponent("GUIText");
		caption_gui.text = "Hello World";
		caption_gui.anchor = TextAnchor.UpperCenter;
		caption_gui.fontSize = 36;
		caption_gui.transform.position = new Vector3(0.5f, 0.0f, 0.0f);
	}
	
	void OnGUI()
	{
		// EXIT
    if (Event.current.Equals(Event.KeyboardEvent("escape"))) 
      Application.LoadLevel("menu");
	}
	
	//! --------------------------------------------------------------------------
	//! METHODS
	//! --------------------------------------------------------------------------
	
	public void foundUser()
	{
		if(state == State.NO_USER)
		{
			state = State.RESTARTING_EXERCISE;
			primary_graph.setFail(true);
		}
	}
	
	public void lostUser()
	{
		state = State.NO_USER;
	}
	
	public void failedExercise()
	{
		if(state != State.NO_USER)
		{
			state = State.RESTARTING_EXERCISE;
			primary_graph.setFail(true);
		}
	}
	
	public void assumedStartPosition()
	{
		if(state == State.RESTARTING_EXERCISE)
		{
			state = State.PERFORMING_EXERCISE;
			primary_graph.setFail(false);
		}
	}
}
