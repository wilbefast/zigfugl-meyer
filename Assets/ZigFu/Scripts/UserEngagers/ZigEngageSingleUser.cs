using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZigEngageSingleUser : MonoBehaviour
{
	//! --------------------------------------------------------------------------
	//! ATTRIBUTES
	//! --------------------------------------------------------------------------
	
	public bool SkeletonTracked = true;
	public bool RaiseHand;
	public List<GameObject> engagedUsers;
	Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();

	public ZigTrackedUser engagedTrackedUser { get; private set; }

	
	//! --------------------------------------------------------------------------
	//! INITIALISE / REINITIALISE
	//! --------------------------------------------------------------------------
	
	void Start()
	{
		// make sure we get zig events
		ZigInput.Instance.AddListener(gameObject); // wtf is 'gameObject' ?
	}
	
	public void Reset()
	{
		if(engagedTrackedUser != null)
			DisengageUser(engagedTrackedUser);
	}
	
	//! --------------------------------------------------------------------------
	//! MANAGE USERS
	//! --------------------------------------------------------------------------

	void EngageUser(ZigTrackedUser user)
	{
		// only engage a user if there currently isn't one engaged
		if(engagedTrackedUser == null)
		{
			engagedTrackedUser = user;
			// other users should now receive events from this user
			foreach(GameObject other_user in engagedUsers)
				user.AddListener(other_user);
			// tell other users that this user has been engaged
			SendMessage("UserEngaged", this, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void DisengageUser(ZigTrackedUser user)
	{
		// only disengage a user if it is currently engaged
		if(engagedTrackedUser == user)
		{
			// other users should now no-longer receive events from this user
			foreach(GameObject other_user in engagedUsers)
				user.RemoveListener(other_user);
			engagedTrackedUser = null;
			// tell other users that the current user has been disengaged
			SendMessage("UserDisengaged", this, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	//! --------------------------------------------------------------------------
	//! MANAGE EVENTS
	//! --------------------------------------------------------------------------

	void Zig_UserFound(ZigTrackedUser user)
	{
		// create game object to listen for events for this user
		GameObject go = new GameObject("WaitForEngagement" + user.Id);
		go.transform.parent = transform;
		objects[user.Id] = go;

		// add various detectors & events
		if(RaiseHand)
		{
			ZigHandRaiseDetector detector = go.AddComponent<ZigHandRaiseDetector>();
			detector.HandRaise += delegate { EngageUser(user); };
		}

		// attach the new object to the new user
		user.AddListener(go);
	}
	
	void Zig_UserLost(ZigTrackedUser user)
	{
		// disengage, destroy and remove the user object
		DisengageUser(user);
		Destroy(objects[user.Id]);
		objects.Remove(user.Id);
	}

	void Zig_Update(ZigInput zig)
	{
		if(SkeletonTracked && engagedTrackedUser == null)
			// engage each user
			foreach(ZigTrackedUser trackedUser in zig.TrackedUsers.Values)
				if(trackedUser.SkeletonTracked)
					EngageUser(trackedUser);
	}

}
