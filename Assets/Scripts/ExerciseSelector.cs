using UnityEngine;
using System.Collections;

public class ExerciseSelector : MonoBehaviour 
{
	//! --------------------------------------------------------------------------
	//! CONSTANTS
	//! --------------------------------------------------------------------------
	
	// the "normal" screen size
	private static Vector2 virtual_screen = new Vector2(1000.0f, 750.0f); 
	
	private static Rect centre = new Rect(200, 100, 600, 550),
								top_left = new Rect(275, 150, 200, 200),
								top_right = new Rect(525, 150, 200, 200),
								bottom_left = new Rect(275, 400, 200, 200);
	
	private GUIContent abduction_button, dorsiflexion_button, flexion_button;
		
	
	//! --------------------------------------------------------------------------
	//! PARAMETERS
	//! --------------------------------------------------------------------------
	
	public Texture2D abduction, flexion, dorsiflexion;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	
	void Start()
	{
		abduction_button = new GUIContent ("Abduction", abduction, "This is the tooltip");
		
		dorsiflexion_button = new GUIContent ("Dorsiflexion", dorsiflexion, "This is the tooltip");
		
		flexion_button = new GUIContent ("Flexion", flexion, "This is the tooltip");
	}
	
	void OnGUI () 
	{
		// GUI size should be based on screen size
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity,
																new Vector3(Screen.width / virtual_screen.x, 
																						Screen.height / virtual_screen.y, 
																						1));
		
		// Make a background box
		GUI.Box(centre, "Choisir l'exercice");

		// TOP LEFT BUTTON
		if(GUI.Button(top_left, abduction_button))
			Application.LoadLevel("test_abduction");
		
		// TOP RIGHT BUTTON
		if(GUI.Button(top_right, flexion_button))
			Application.LoadLevel("test_flexion");
		
		// BOTTOM LEFT BUTTON
		if(GUI.Button(bottom_left, dorsiflexion_button))
			Application.LoadLevel("test_dorsiflexion");
	}
}
