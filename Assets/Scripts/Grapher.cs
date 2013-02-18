using UnityEngine;
using System.Collections;


public class Grapher : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	
	// parameters
	public int history_length;
	public float min_value, max_value, line_width;
	public Rect gui_area;
	public UnityEngine.Color colour;
	public string caption_text;
	public Camera camContext;
	public Texture illegal_tex; //! FIXME -- script should find this by itself
	
	// local variables
	private float[] history;
  private LineRenderer line, borderLine;
	private float value_span;
	private Vector3 point; 
	
	// sub-objects
	private GameObject curve, border, caption, 
											amount_indicator, min_indicator, max_indicator,
											illegal_indicator;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	void Start ()
	{
		
		// if no camera is specified use the main camera instead
		if(camContext == null)
			camContext = Camera.main;
		
		// create the legend
		create_caption();
		create_indicators();
		
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
		// borders follow camera 
		borderLine.SetPosition(0, graphPos(0,0));
		borderLine.SetPosition(1, graphPos(1,0));
		borderLine.SetPosition(2, graphPos(1,1));
		borderLine.SetPosition(3, graphPos(0,1));
		borderLine.SetPosition(4, graphPos(0,0));
		
		// caption follows camera
		point.Set(gui_area.center.x, gui_area.yMin, 0.0f);
		caption.guiText.transform.position = point;
		
		// indicators follows camera --
		// -- current amount
		float h = valueToPoint(history[history_length - 1]);
		point.Set(gui_area.xMax + 0.01f, gui_area.yMin + h*gui_area.height, 0.0f);
		amount_indicator.guiText.transform.position = point;
		// -- min
		point.Set(gui_area.xMin - 0.01f, gui_area.yMin, 0.0f);
		min_indicator.guiText.transform.position = point;
		// -- max
		point.Set(gui_area.xMin - 0.01f, gui_area.yMax, 0.0f);
		max_indicator.guiText.transform.position = point;
		
		// curve follows camera
		for(int i = 0; i < history_length; i++)
			line.SetPosition(i, graphPos((float)i / history_length, 
																		valueToPoint(history[i])));
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
	
	private void create_indicators()
	{
		// current amount (right-hand side)
		amount_indicator = new GameObject("amount_indicator");
		GUIText gtext = (GUIText)amount_indicator.AddComponent("GUIText");
		gtext.anchor = TextAnchor.MiddleLeft;
		
		// min amount (left-hand side)
		min_indicator = new GameObject("min_indicator");
		gtext = (GUIText)min_indicator.AddComponent("GUIText");
		gtext.anchor = TextAnchor.MiddleRight;
		gtext.text = min_value.ToString();
		
		// max amount (right-hand side)
		max_indicator = new GameObject("max_indicator");
		gtext = (GUIText)max_indicator.AddComponent("GUIText");
		gtext.anchor = TextAnchor.MiddleRight;
		gtext.text = max_value.ToString();
		
		// illegal move (only when needed)
		illegal_indicator = new GameObject("illegal_move");
		GUITexture gimg = (GUITexture)illegal_indicator.AddComponent("GUITexture");
		point.Set(gui_area.center.x, gui_area.center.y, 0.0f);
		illegal_indicator.transform.position = point;
		gimg.pixelInset = new Rect(0, 0, 0.01f, 0.01f);
		/*gimg.pixelInset = new Rect(gui_area.x * Screen.width / 2, 
															gui_area.y * Screen.height / 2,
															gui_area.width * Screen.width / 4 , 
															gui_area.height * Screen.height / 4);*/
		Debug.Log (gimg.pixelInset);
		gimg.texture = illegal_tex;
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
		
		// shift history to the left
		for(int i = 0; i < history_length - 1; i++)
			history[i] = history[i+1];
		
		// add the new data point
		history[history_length - 1] = new_value;
		amount_indicator.guiText.text = Mathf.Round(new_value).ToString();
	}
	
	//! --------------------------------------------------------------------------
	//! GUI UTILITY SUBROUTINES
	//! --------------------------------------------------------------------------
	
	private Vector3 graphPos(float x, float y)
	{
		return viewToWorld(gui_area.xMin + x*gui_area.width, 
													gui_area.yMin + y*gui_area.height);
	}
	
	private Vector3 viewToWorld(float x, float y)
	{
		point.Set(x, y, 1.0f);
		point = camContext.ScreenToWorldPoint(camContext.ViewportToScreenPoint(point)); 
		return point;
	}
	
	private Vector3 worldToView(float x, float y, float z)
	{
		point.Set(x, y, z);
		point = camContext.WorldToViewportPoint(point);
		return point;
	}
	
	private float valueToPoint(float data)
	{
		return ((data - min_value) / value_span);
	}
	
	//! --------------------------------------------------------------------------
	//! ACCESSORS
	//! --------------------------------------------------------------------------
	
	public void setIllegalMove(bool b)
	{
		
		//illegal_indicator.renderer.enabled = true;//b;
	}
	
}
