using UnityEngine;
using System.Collections;


public class Grapher : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// parameter
	public int history_length;
	public float min_value, max_value, line_width;
	public Rect gui_area;
	public UnityEngine.Color colour;
	public string caption_text;
	
	// local variables
	private float[] history;
  private LineRenderer line, borderLine;
	private float value_span;
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
		
		// calculate height of graph
		value_span = max_value - min_value;
		
		// create history buffer
		history = new float[history_length];
	}
	
	void OnGUI()
	{
		borders_follow_camera();
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
		borderLine = (LineRenderer)border.AddComponent("LineRenderer"); 
		
		// set up the border line
		borderLine.SetVertexCount(5); // (0,1), (0,0), (1,0), (1,1), (0,1)
		borderLine.material = new Material(Shader.Find("Particles/Additive"));
		borderLine.SetWidth(line_width, line_width);
		borderLine.SetColors(UnityEngine.Color.white, UnityEngine.Color.white);
	}
	
	private void create_curve()
	{
		// create the plot object for plotting data
		curve = new GameObject("plot");
		line = (LineRenderer)curve.AddComponent("LineRenderer");	
		
		// set up the plot line 
		line.SetVertexCount(history_length);
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.SetWidth(line_width, line_width);
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
		/*caption.guiText.transform.position = 
			worldToScreen(gui_area.center.x, gui_area.y, 0.0f);
		// -- current level
		amount_indicator.guiText.transform.position =
			worldToScreen(max_x + line_width, new_point, 0.0f);
		amount_indicator.guiText.text = Mathf.Round(new_value).ToString();*/
		
		// shift history to the left
		for(int i = 0; i < history_length - 1; i++)
		{
			float x = (float)i / history_length;
			history[i] = history[i+1];
			line.SetPosition(i, graphPos(x, valueToPoint(history[i])));
		}

		// get a new data point
		history[history_length - 1] = new_value;
		line.SetPosition(history_length - 1, graphPos(1, new_point));
	}
	
	//! --------------------------------------------------------------------------
	//! GUI RECALCULATION SUBROUTINES
	//! --------------------------------------------------------------------------
	
	private void borders_follow_camera()
	{
		borderLine.SetPosition(0, graphPos(0,0));
		borderLine.SetPosition(1, graphPos(1,0));
		borderLine.SetPosition(2, graphPos(1,1));
		borderLine.SetPosition(3, graphPos(0,1));
		borderLine.SetPosition(4, graphPos(0,0));
	}
	
	//! --------------------------------------------------------------------------
	//! GUI UTILITY SUBROUTINES
	//! --------------------------------------------------------------------------
	
	private Vector3 graphPos(float x, float y)
	{
		if(x < 0) x = 0; else if (x > 1) x = 1;
		if(y < 0) y = 0; else if (y > 1) y = 1;
		return screenToWorld(gui_area.xMin + x*gui_area.width, 
													gui_area.yMin + y*gui_area.height);
	}
	
	private Vector3 screenToWorld(float x, float y)
	{
		point.Set(x, y, 1.0f);
		point = 
			Camera.main.ScreenToWorldPoint(Camera.main.ViewportToScreenPoint(point)); 
		return point;
	}
	
	private Vector3 worldToScreen(float x, float y, float z)
	{
		point.Set(x, y, z);
		point = Camera.main.WorldToViewportPoint(point);
		return point;
	}
	
	private float valueToPoint(float data)
	{
		return ((data - min_value) / value_span);
	}
}
