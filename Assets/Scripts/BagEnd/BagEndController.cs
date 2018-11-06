using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BagEndController : MonoBehaviour, KinectGestures.GestureListenerInterface
{

    public List<BagEndInteractibleBase> BagEndInteractibles = new List<BagEndInteractibleBase>();
    public int MaxUsers = 2;
    
    private int _UserCount = 0;
    public Text UserDebugTxt;


    private void CheckAddNewEffect(long userId, int userIndex)
    {
        //make sure we have enough effects to go around
        if(userIndex <= MaxUsers)
        {
            if(userIndex <= BagEndInteractibles.Count)
            {
                //randomly grab one of our bag end interactibles and apply
                AddUserEffect(userId, userIndex);
            }
        }
    }

    private void AddUserEffect(long userId, int userIndex)
    {
        //loop through current effects and only add numbers that are not being used
        List<BagEndInteractibleBase> nonUsedInteractibles = new List<BagEndInteractibleBase>();
        for(int i = 0; i<BagEndInteractibles.Count;i++)
        {
            BagEndInteractibleBase curInteractible = BagEndInteractibles[i];
            if(curInteractible.UserID == 0)
            {
                nonUsedInteractibles.Add(curInteractible);
            }
        }

        int ranItem = UnityEngine.Random.Range(0, nonUsedInteractibles.Count);
        nonUsedInteractibles[ranItem].OnShow(userId, userIndex);
    }


    /// <summary>
    /// loop through our current effects, find the ID of the user that matches the one that was removed, and remove/resetID
    /// if this user was the first user, we also need to loop through effects and set the index value of the other effect
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userIndex"></param>
    private void RemoveUser(long userId, int userIndex)
    {
        int i = 0;
        BagEndInteractibleBase curInteractible;
        //loop through our current effects, find the ID that matches this items and turn off
        for (i = 0; i<BagEndInteractibles.Count; i++)
        {
            curInteractible = BagEndInteractibles[i];
            if(curInteractible.UserID == userId)
            {
                curInteractible.OnHide(userId, userIndex);
            }
        }
        //if we were the first user, our second user is now going to be swapped. We need to reset its user index
        if(userIndex == 0)
        {
            for (i = 0; i < BagEndInteractibles.Count; i++)
            {
                curInteractible = BagEndInteractibles[i];
                if (curInteractible.UserID != 0)
                {
                    curInteractible.SetUserIndex(0);
                }
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
        Debug.Log("userID: " + userId);
        Debug.Log("userIndex: " + userIndex);
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager)
            return;

        _UserCount++;
        CheckAddNewEffect(userId, userIndex);

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
        RemoveUser(userId, userIndex);

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
