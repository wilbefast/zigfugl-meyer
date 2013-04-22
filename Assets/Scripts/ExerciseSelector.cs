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
								top_left = new Rect(275, 250, 200, 200),
								top_right = new Rect(525, 250, 200, 200),
								bottom_left = new Rect(275, 400, 200, 200),
								bottom_right = new Rect(525, 400, 200, 200);
	
	private GUIContent abduction_button, flexion_button, 
											dorsiflexion1_button, dorsiflexion2_button;
		
	
	//! --------------------------------------------------------------------------
	//! PARAMETERS
	//! --------------------------------------------------------------------------
	
	public Texture2D abduction, flexion, dorsiflexion1, dorsiflexion2;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	
	void Start()
	{
		abduction_button = new GUIContent ("Abduction", abduction, 
			"Lêver le bras de côté.");
		
		flexion_button = new GUIContent ("Flexion", flexion, 
			"Lêver le bras vers le devant.");
		
		dorsiflexion1_button = new GUIContent ("Dorsiflexion I", dorsiflexion1, 
			"Lêver le poignet, coude plié.");
		
		dorsiflexion2_button = new GUIContent ("Dorsiflexion II", dorsiflexion2, 
			"Lêver le poignet, coude droit.");
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
		/*if(GUI.Button(bottom_left, dorsiflexion1_button))
			Application.LoadLevel("test_dorsiflexion"); /// FIXME
		
		// BOTTOM RIGHT BUTTON
		if(GUI.Button(bottom_right, dorsiflexion2_button))
			Application.LoadLevel("test_dorsiflexion"); /// FIXME*/
		
		// EXIT
    if (Event.current.Equals(Event.KeyboardEvent("escape"))) 
      Application.LoadLevel("choose_hand");
	}
}
