using UnityEngine;
using System.Collections;

public class TextUpdate : MonoBehaviour 
{
	public GUIText gui;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		gui.text = "Hello World!";
	}
}
