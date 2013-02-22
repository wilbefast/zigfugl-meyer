using UnityEngine;
using System.Collections;

public class HandSelector : MonoBehaviour 
{
	//! --------------------------------------------------------------------------
	//! CONSTANTS
	//! --------------------------------------------------------------------------
	
	// the "normal" screen size
	private static Vector2 virtual_screen = new Vector2(1000.0f, 750.0f); 
	
	private static Rect centre = new Rect(200, 100, 600, 550),
								left = new Rect(275, 250, 200, 200),
								right = new Rect(525, 250, 200, 200);
	
	private GUIContent left_button, right_button;
		
	
	//! --------------------------------------------------------------------------
	//! PARAMETERS
	//! --------------------------------------------------------------------------
	
	public HandChoice hand_choice;
	public Texture2D tex_left, tex_right;
		
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	
	void Start()
	{
		left_button = new GUIContent ("Gauche", tex_left, 
			"Côté gauche.");
		
		right_button = new GUIContent ("Droite", tex_right, 
			"Côté droite.");
		
	}
	
	void OnGUI () 
	{
		// GUI size should be based on screen size
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity,
																new Vector3(Screen.width / virtual_screen.x, 
																						Screen.height / virtual_screen.y, 
																						1));
		
		// Make a background box
		GUI.Box(centre, "Choisir");

		// LEFT
		if(GUI.Button(left, left_button))
		{
			hand_choice.right_hand = false;
			Application.LoadLevel("menu");
		}
		
		// RIGHT
		if(GUI.Button(right, right_button))
		{
			hand_choice.right_hand = false;
			Application.LoadLevel("menu");
		}
	}
}
