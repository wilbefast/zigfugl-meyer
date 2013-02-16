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
  private LineRenderer line, borderLine;
	private float span_x;
	private Vector3 point; 
	
	// sub-objects
	private GameObject plot, border, legend;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	void Start ()
	{
		// create graph borders
		create_border();
		
		// create graph plot line
		create_plot();
		
		// calculate width of graph
		span_x = max_x - min_x;
		
		// create history buffer
		history = new float[history_length];
	}
	
	//! --------------------------------------------------------------------------
	//! PRIVATE SUBROUTINES
	//! --------------------------------------------------------------------------
	
	private void create_border()
	{
		// create a border around the plot (X and Y axes)
		border = new GameObject("border");
		borderLine = (LineRenderer)border.AddComponent("LineRenderer"); 
		
		// set up the border line
		borderLine.SetVertexCount(5); // (0,1), (0,0), (1,0), (1,1), (0,1)
		borderLine.material = new Material(Shader.Find("Particles/Additive"));
		borderLine.SetWidth(width, width);
		borderLine.SetColors(UnityEngine.Color.white, UnityEngine.Color.white);
		
		// create borders once and for all
		point.Set(min_x, 1, 0); borderLine.SetPosition(0, point);
		point.Set(min_x, 0, 0); borderLine.SetPosition(1, point);
		point.Set(max_x, 0, 0); borderLine.SetPosition(2, point);
		point.Set(max_x, 1, 0); borderLine.SetPosition(3, point);
		point.Set(min_x, 1, 0); borderLine.SetPosition(4, point);
	}
	
	private void create_plot()
	{
		// create the plot object for plotting data
		plot = new GameObject("plot");
		line = (LineRenderer)plot.AddComponent("LineRenderer");	
		
		// set up the plot line 
		line.SetVertexCount(history_length);
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.SetWidth(width, width);
		line.SetColors(colour, colour);
	}
	
	
	//! --------------------------------------------------------------------------
	//! PUBLIC METHODS
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
