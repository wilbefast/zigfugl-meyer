using UnityEngine;
using System.Collections;

public class Grapher : MonoBehaviour
{
	// parameter
	public int history_length;
	public Transform measured_object;
	
	// local variables
	private float[] history;
  private LineRenderer line;
	
	void Start ()
	{
		// create history buffer
		history = new float[history_length];
		
		// create line segments
		line = (LineRenderer)gameObject.GetComponent("LineRenderer");
		line.SetVertexCount(history_length);
	}
 
	void Update ()
	{
		// shift history to the left
		for(int i = 0; i < history_length - 1; i++)
		{
			float x = (float)i / history_length;
			history[i] = history[i+1];
			line.SetPosition(i, new Vector3(x, history[i], -1)); 
													//! FIXME -- new in Update loop
		}
		
		// get a new data point
		float new_point = measured_object.localRotation.z;
		history[history_length - 1] = new_point;
		line.SetPosition(history_length - 1, new Vector3(1, new_point, -1)); 
																					//! FIXME -- new in Update loop
	}
}
