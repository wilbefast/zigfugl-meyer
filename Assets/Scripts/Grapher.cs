using UnityEngine;
using System.Collections;


public class Grapher : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// parameter
	public int history_length;
	public float min_x, max_x;
	public UnityEngine.Color colour;
	
	// local variables
	private float[] history;
  private LineRenderer line;
	private float span_x;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	void Start ()
	{
		// calculate width of graph
		span_x = max_x - min_x;
		
		// create history buffer
		history = new float[history_length];
	
		// create line segments
		line = (LineRenderer)gameObject.GetComponent("LineRenderer");
		line.SetVertexCount(history_length);
	
		// setup material / colour
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.SetColors(colour, colour);
	}
	
	//! --------------------------------------------------------------------------
	//! METHODS
	//! --------------------------------------------------------------------------
	public void newDataPoint(float new_point)
	{
		// clamp value between 0 and 1
		if(new_point > 1)
			new_point = 1;
		else if(new_point < 0)
			new_point = 0;
		
		// shift history to the left
		for(int i = 0; i < history_length - 1; i++)
		{
			float x = min_x + (float)i / history_length * span_x;
			history[i] = history[i+1];
			line.SetPosition(i, new Vector3(x, history[i], 0)); 
													//! FIXME -- new in Update loop
		}

		// get a new data point
		history[history_length - 1] = new_point;
		line.SetPosition(history_length - 1, new Vector3(max_x, new_point, 0)); 
																					//! FIXME -- new in Update loop
	}
}
