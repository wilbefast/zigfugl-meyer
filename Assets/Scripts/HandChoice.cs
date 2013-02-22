using UnityEngine;
using System.Collections;

public class HandChoice : MonoBehaviour 
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	public bool right_hand;
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS
	//! --------------------------------------------------------------------------
	public void Awake () 
	{
		// persistent object
    DontDestroyOnLoad (transform.gameObject);
	}
}
