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
		// gui.text = skeleton.RightWrist.rotation.ToString ();
		gui.text = skeleton.RightShoulder.rotation.eulerAngles.ToString ();
	}
	
	//! --------------------------------------------------------------------------
	//! OTHER METHODS
	//! --------------------------------------------------------------------------
	
	
}
