using UnityEngine;
using System.Collections;

public class TextUpdate : MonoBehaviour 
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameterable
	public GUIText gui;
	
	// Alias
	private ZigSkeleton skeleton;	
	
	//! --------------------------------------------------------------------------
	//! UNITY CALLBACKS
	//! --------------------------------------------------------------------------
	
	// Use this for initialization
	void Start () 
	{
		skeleton = (ZigSkeleton)gameObject.transform.parent.FindChild("Blockman")
																			.gameObject.GetComponent("ZigSkeleton");
	}
	
	// Update is called once per frame
	void Update () 
	{	
		gui.text = "Hello World!";
		
		// Drilling-like rotation around the arm axis
		//gui.text = skeleton.RightShoulder.localRotation.eulerAngles.x.ToString();
		
		// Rotation around the vertical axis
		//gui.text = skeleton.RightShoulder.localRotation.eulerAngles.y.ToString();
		
		// Rotation around the shoulder-span axis
		
		// Rotation around the body's frontal axis 
		//gui.text = skeleton.RightShoulder.localRotation.eulerAngles.z.ToString ();
	}
	
	//! --------------------------------------------------------------------------
	//! OTHER METHODS
	//! --------------------------------------------------------------------------
	
	
}
