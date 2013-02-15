using UnityEngine;
using System.Collections;


public class Grapher : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// parameter
	public int history_length;
	public float min_x, max_x, width;
	public UnityEngine.Color colour;
	
	// local variables
	private float[] history;
  private LineRenderer line;
	private float span_x;
	private Vector3 point; 
	
	// sub-objects
	private GameObject plot, border;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	void Start ()
	{
		// create the plot object for plotting data
		plot = new GameObject("plot");
		line = (LineRenderer)plot.AddComponent("LineRenderer");	
		
		// set up the line 
		line.SetVertexCount(history_length);
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.SetWidth(width, width);
		line.SetColors(colour, colour);
		
		// calculate width of graph
		span_x = max_x - min_x;
		
		// create history buffer
		history = new float[history_length];

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
			point.Set(x, history[i], 0);
			line.SetPosition(i, point);
		}

		// get a new data point
		history[history_length - 1] = new_point;
		point.Set(max_x, new_point, 0);
		line.SetPosition(history_length - 1, point);
	}
}
