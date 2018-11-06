using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ParticleController : MonoBehaviour, KinectGestures.GestureListenerInterface
{

    [Serializable]
    public struct ParticleGroup {
        public GameObject ObjectToFollow;
        public ParticleSystem Particles;
        public Vector3 PrevPos;
    }
    
    public List<ParticleGroup> ParticleList;
    public List<ParticleGroupClass> ParticleGroupList;
    public Text UserDebugTxt;
    public float AllowedDistance = 0.5f;
    public float RateOverDistance = 10.0f;

    private int _UserCount = 0;

    void Awake()
    {

        if (UserDebugTxt != null)
            UserDebugTxt.text = "No users detected";

        foreach(ParticleGroupClass particleGroup in ParticleGroupList)
        {
            particleGroup.PrevPos = Vector3.zero;
        }

/*        for(int i = 0; i < ParticleList.Count; i++)
        {
            ParticleList[i].PrevPos = Vector3.zero;
        }
        */
        
    }

	// Use this for initialization
	void Start () {
        _UserCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//if we have any users
        if(_UserCount > 0)
        {
            foreach(ParticleGroupClass particleGroup in ParticleGroupList)
            {
                ParticleSystem curParticles = particleGroup.Particles;
                GameObject curObject = particleGroup.ObjectToFollow;
                Vector3 newParticlePosition = curObject.transform.position;
                
                float distance = Vector3.Distance(newParticlePosition, particleGroup.PrevPos);
               
                particleGroup.PrevPos = newParticlePosition;

                var emission = curParticles.emission;

                if(distance < AllowedDistance)
                {

                    emission.rateOverDistance = RateOverDistance;
                }
                else
                {
                    emission.rateOverDistance = 0;
                }
                curParticles.transform.position = newParticlePosition;
            }
        }
	}

    /// <summary>
    /// Invoked when a new user is detected. Here you can start gesture tracking by invoking KinectManager.DetectGesture()-function.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    public void UserDetected(long userId, int userIndex)
    {

        Debug.Log("a user has been detected");
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager)
            return;

        _UserCount++;

        //if this is our first user, init the swipe panel
 //       if (_UserCount == 1)
 //          FirstUserDetected();

        if (UserDebugTxt != null)
        {
            string userStr = " user detected";
            if (_UserCount > 1)
                userStr = "users detected";
            UserDebugTxt.text = string.Format("{0} {1}", _UserCount, userStr);
        }
    }

    /// <summary>
	/// Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	public void UserLost(long userId, int userIndex)
    {
        _UserCount--;

//        if (_UserCount == 0)
//            LastUserLeft();

        if (UserDebugTxt != null)
        {
            string userStr = " user detected";
            if (_UserCount > 1)
            {
                userStr = "users detected";
            }
            else if (_UserCount == 0)
            {
                userStr = "No users detected";
            }
            UserDebugTxt.text = string.Format("{0} {1}", _UserCount, userStr);
        }
    }


    /// <summary>
    /// Invoked when a gesture is in progress.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="progress">Gesture progress [0..1]</param>
    /// <param name="joint">Joint type</param>
    /// <param name="screenPos">Normalized viewport position</param>
    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        return;
    }

    /// <summary>
    /// Invoked if a gesture is cancelled.
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="joint">Joint type</param>
    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint)
    {
        return true;
    }

    /// <summary>
    /// Invoked if a gesture is completed.
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    /// <param name="userId">User ID</param>
    /// <param name="userIndex">User index</param>
    /// <param name="gesture">Gesture type</param>
    /// <param name="joint">Joint type</param>
    /// <param name="screenPos">Normalized viewport position</param>
    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint, Vector3 screenPos)
    {

        return true;
    }
}
