using UnityEngine;
using System.Collections;


public class FailGrapher : Grapher
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	
	// local variables
	private GameObject fail_indicator;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	void Start ()
	{
		// enrich base callback
		//base.Start(); //! FIXME 'method without body' error caused here
		init ();
		create_fail_indicator();
	}

	
	//! --------------------------------------------------------------------------
	//! START SUBROUTINES
	//! --------------------------------------------------------------------------
	
	private void create_fail_indicator()
	{
		// illegal move (only when needed)
		fail_indicator = new GameObject("fail_indicator");
		fail_indicator.layer = gameObject.layer;
		point.Set(gui_area.center.x, gui_area.center.y, 0.0f);
		fail_indicator.transform.position = point;
		fail_indicator.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f); 
		fail_indicator.SetActive(false);
		GUITexture tex = (GUITexture)fail_indicator.AddComponent("GUITexture");
		tex.texture = (Texture)Resources.Load("cross");
	}
	
	//! --------------------------------------------------------------------------
	//! ACCESSORS
	//! --------------------------------------------------------------------------
	
	public void setFail(bool b)
	{
		fail_indicator.SetActive(b);
	}
	
}
