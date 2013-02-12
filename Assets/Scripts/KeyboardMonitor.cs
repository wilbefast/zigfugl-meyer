using UnityEngine;
using System.Collections;

public class KeyboardMonitor : MonoBehaviour 
{
	public Grapher graphA, graphB;
	
	private float a, b;
	
	// Use this for initialization
	void Start () 
	{
		a = 0;
		b = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//! ------------------------------------------------------------------------
		// Get keyboard input
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			a -= 0.01f;
			if(a < 0)
				a = 0;
		}
		if(Input.GetKey(KeyCode.RightArrow))
		{
			a += 0.01f;
			if(a > 1)
				a = 1;
		}
			
		if(Input.GetKey(KeyCode.DownArrow))
		{
			b -= 0.01f;
			if(b < 0)
				b = 0;
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			b += 0.01f;
			if(b > 1)
				b = 1;
		}
		
		
		//! ------------------------------------------------------------------------
		// Graph the input
		graphA.newDataPoint(a);
		graphB.newDataPoint(b);
		
		//! ------------------------------------------------------------------------
		// Write the input
		gameObject.guiText.text = "a = " + a + "\nb = " + b;
	}
}
