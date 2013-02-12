using UnityEngine;
using System.Collections;

public class TextUpdate : MonoBehaviour 
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// parameter
	public ZigSkeleton skeleton;	
	
	//! --------------------------------------------------------------------------
	//! UNITY CALLBACKS
	//! --------------------------------------------------------------------------
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{	
		gameObject.guiText.text = "Hello World!";
		
		// Drilling-like rotation around the arm axis
		//gameObject.guiText = skeleton.RightShoulder.localRotation.eulerAngles.x.ToString();
		
		// Rotation around the vertical axis
		//gameObject.guiText = skeleton.RightShoulder.localRotation.eulerAngles.y.ToString();
		
		// Rotation around the shoulder-span axis
		
		// Rotation around the body's frontal axis 
		//gameObject.guiText = skeleton.RightShoulder.localRotation.eulerAngles.z.ToString ();
	}
	
	//! --------------------------------------------------------------------------
	//! OTHER METHODS
	//! --------------------------------------------------------------------------
	
	
}
