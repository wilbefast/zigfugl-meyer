using UnityEngine;
using System.Collections;


public class Grapher : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// parameter
	public int history_length;
	public float min_value, max_value, min_x, max_x, width;
	public UnityEngine.Color colour;
	public string caption_text;
	
	// local variables
	private float[] history;
  private LineRenderer line;
	private float span_value, span_x;
	private Vector3 point; 
	
	// sub-objects
	private GameObject curve, border, caption, amount_indicator;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	void Start ()
	{
		// create the legend
		create_caption();
		create_amount_indicator();
		
		// create graph borders
		create_border();
		
		// create graph plot line
		create_curve();
		
		// calculate height and width of graph
		span_value = max_value - min_value;
		span_x = max_x - min_x;
		
		// create history buffer
		history = new float[history_length];
	}
	
	//! --------------------------------------------------------------------------
	//! START SUBROUTINES
	//! --------------------------------------------------------------------------
	
	private void create_caption()
	{
		caption = new GameObject("caption");
		GUIText caption_gui = (GUIText)caption.AddComponent("GUIText");
		caption_gui.text = caption_text;
		caption_gui.anchor = TextAnchor.UpperCenter;
	}
	
	private void create_amount_indicator()
	{
		amount_indicator = new GameObject("amount_indicator");
		GUIText amount_gui = (GUIText)amount_indicator.AddComponent("GUIText");
		amount_gui.anchor = TextAnchor.MiddleLeft;
	}
	
	private void create_border()
	{
		// create a border around the plot (X and Y axes)
		border = new GameObject("border");
		LineRenderer borderLine = (LineRenderer)border.AddComponent("LineRenderer"); 
		
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
	
	private void create_curve()
	{
		// create the plot object for plotting data
		curve = new GameObject("plot");
		line = (LineRenderer)curve.AddComponent("LineRenderer");	
		
		// set up the plot line 
		line.SetVertexCount(history_length);
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.SetWidth(width, width);
		line.SetColors(colour, colour);
	}
	
	//! --------------------------------------------------------------------------
	//! PUBLIC METHODS
	//! --------------------------------------------------------------------------
	public void newDataPoint(float new_value)
	{
		//! ------ FIRST OFF ALL ------
		// clamp value between min and max
		if(new_value > max_value)
			new_value = max_value;
		else if(new_value < min_value)
			new_value = min_value;
		float new_point = valueToPoint(new_value);
		
		// move GUI to a position relative to camera so it is always in view --
		// -- caption
		caption.guiText.transform.position = 
			screenPoint(min_x + span_x/2, -width, 0.0f);
		// -- current level
		amount_indicator.guiText.transform.position =
			screenPoint(max_x + width, new_point, 0.0f);
		amount_indicator.guiText.text = Mathf.Round(new_value).ToString();
		
		// shift history to the left
		for(int i = 0; i < history_length - 1; i++)
		{
			float x = min_x + (float)i / history_length * span_x;
			history[i] = history[i+1];
			point.Set(x, valueToPoint(history[i]), 0);
			line.SetPosition(i, point);
		}

		// get a new data point
		history[history_length - 1] = new_value;
		point.Set(max_x, new_point, 0);
		line.SetPosition(history_length - 1, point);
	}
	
	//! --------------------------------------------------------------------------
	//! GUI SUBROUTINES
	//! --------------------------------------------------------------------------
	
	private Vector3 screenPoint(float x, float y, float z)
	{
		point.Set(x, y, z);
		point = Camera.main.WorldToScreenPoint(point);
		point.x /= Screen.width;
		point.y /= Screen.height;
		
		return point;
	}
	
	private float valueToPoint(float data)
	{
		return ((data - min_value) / span_value);
	}
}
