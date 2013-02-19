using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EngagePatient : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	// Parameters -- from sample
	public bool SkeletonTracked = true;
	public bool RaiseHand;
	public List<GameObject> EngagedUsers;
	public ZigTrackedUser engagedTrackedUser { get; private set; }
	
	// -- our injections
	public ExerciseMonitor exercise;
	
	// Local variables
	Dictionary<int, GameObject> objects = new Dictionary<int, GameObject> ();
	
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS -- UNITY
	//! --------------------------------------------------------------------------
	
	void Start ()
	{
		// make sure we get zig events
		ZigInput.Instance.AddListener (gameObject);
	}
	
	public void Reset ()
	{
		if (null != engagedTrackedUser) 
			DisengageUser (engagedTrackedUser);
	}
	
	//! --------------------------------------------------------------------------
	//! CALLBACKS -- ZIGFU
	//! --------------------------------------------------------------------------
	
	void Zig_UserFound (ZigTrackedUser user)
	{
		// create gameobject to listen for events for this user
		GameObject go = new GameObject ("WaitForEngagement" + user.Id);
		go.transform.parent = transform;
		objects [user.Id] = go;

		// add various detectors & events

		if (RaiseHand) 
		{
			ZigHandRaiseDetector hrd = go.AddComponent<ZigHandRaiseDetector> ();
			hrd.HandRaise += delegate { EngageUser (user); };
		}

		// attach the new object to the new user
		user.AddListener (go);
	}
	
	void Zig_UserLost (ZigTrackedUser user)
	{
		DisengageUser (user);
		Destroy (objects [user.Id]);
		objects.Remove (user.Id);
	}

	void Zig_Update (ZigInput zig)
	{
		if (SkeletonTracked && null == engagedTrackedUser) 
			foreach (ZigTrackedUser trackedUser in zig.TrackedUsers.Values) 
				if (trackedUser.SkeletonTracked) 
					EngageUser (trackedUser);
	}

	//! --------------------------------------------------------------------------
	//! METHODS
	//! --------------------------------------------------------------------------
	
	void EngageUser (ZigTrackedUser arriving_user)
	{
		// if not currently tracking
		if (engagedTrackedUser == null) 
		{
			// -- addition!
			exercise.foundUser();
			// --
			
			engagedTrackedUser = arriving_user;
			foreach (GameObject go in EngagedUsers)
				arriving_user.AddListener (go);
			SendMessage ("UserEngaged", this, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void DisengageUser (ZigTrackedUser departing_user)
	{
		// if the disengaged user was the currently tracked users
		if (departing_user == engagedTrackedUser) 
		{
			// -- addition!
			exercise.lostUser();
			// --
			
			foreach (GameObject go in EngagedUsers)
				departing_user.RemoveListener (go);
			engagedTrackedUser = null;
			SendMessage ("UserDisengaged", this, SendMessageOptions.DontRequireReceiver);
		}
	}
}
