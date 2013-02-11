using UnityEngine;
using System.Collections;

public class Grapher : MonoBehaviour
{
	// parameter
	public int resolution;
	
	// local variables
	private float[] history;
  private LineRenderer line;
	
	void Start ()
	{
		// create data
		history = new float[resolution];
		
		// create line segments
		line = (LineRenderer)gameObject.GetComponent("LineRenderer");
		line.SetVertexCount(resolution);
		
		// place line vertices
		for(int i = 0; i < resolution; i++)
		{
			float interval = i / (float)resolution;
			
			history[i] = interval;
			line.SetPosition(i, new Vector3(interval, history[i], 0));
		}
	}
 
	void Update ()
	{
		
	}
}
