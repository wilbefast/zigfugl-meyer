using UnityEngine;
using System.Collections;

public class TransformMonitor : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameters
	public Grapher graphRotX;
	public Grapher graphRotY;
	public Grapher graphRotZ;
	public Transform watched_transform;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 gimble = watched_transform.localRotation.eulerAngles;
		
		//! FIXME
		gimble.x = Random.Range(0, 10)/10.0f;
		gimble.y = Random.Range(0, 10)/10.0f;
		gimble.z = Random.Range(0, 10)/10.0f;
		
		// plot the rotations
		if(graphRotX != null)
			graphRotX.newDataPoint(gimble.x);
		if(graphRotY != null)
			graphRotY.newDataPoint(gimble.y);
		if(graphRotZ != null)
			graphRotZ.newDataPoint(gimble.z);
	}
}
