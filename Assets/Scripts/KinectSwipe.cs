using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KinectSwipe : MonoBehaviour, KinectGestures.GestureListenerInterface {

    public int PlayerIndex = 0;
    public Text GestureInfo;
    public Text UserDebugTxt;
    public GameObject PanelContainer;
    public SwipePanel SwipePanelPrefab;

    public List<PollockPanel> PollockPrefabs;

    public GameObject KinectDebug;

    private bool _SwipeLeft;
    private bool _SwipeRight;
    private int _UserCount = 0;
    private int _CurPanelInt = 0;
    private int _TotalPanels;
    private PollockPanel _CurPanel;

    // internal variables to track if progress message has been displayed
    private bool ProgressDisplayed;
    private float ProgressGestureTime;

    public delegate void SwipeAction(string swipeDirection);
    public static SwipeAction OnSwipeAction;

    public delegate void TapAction();
    public static TapAction OnTapAction;

    void Awake()
    {
        _TotalPanels = PollockPrefabs.Count;
        UserDebugTxt.text = "No users detected";
    }


    public void OnButtonClick()
    {
        bool isDebug = KinectDebug.activeSelf;
        if(KinectDebug != null)
        {
            KinectDebug.SetActive(!isDebug);
        }

        if(KinectManager.Instance != null)
        {
            KinectManager.Instance.displayUserMap = !isDebug;
        }
    }

    public bool IsSwipeLeft()
    {
        if (_SwipeLeft)
        {
            _SwipeLeft = false;
            return true;
        }
        return false;
    }

    public bool IsSwipeRight()
    {
        if (_SwipeRight)
        {
            _SwipeRight = false;
            return true;
        }
        return false;
    }
    
    /// <summary>
	/// Invoked when a new user is detected. Here you can start gesture tracking by invoking KinectManager.DetectGesture()-function.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	public void UserDetected(long userId, int userIndex)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userIndex != PlayerIndex))
            return;

        _UserCount++;

        //if this is our first user, init the swipe panel
        if (_UserCount == 1)
            FirstUserDetected();

        if(UserDebugTxt != null)
        {
            string userStr = " user detected";
            if (_UserCount > 1)
                userStr = "users detected";
            UserDebugTxt.text = string.Format("{0} {1}", _UserCount, userStr);
        }
            

        // detect these user specific gestures
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);

        if (GestureInfo != null)
        {
            GestureInfo.text = "Swipe left or right to change the slides.";
        }
    }

    void FirstUserDetected()
    {
        ClearPanels();
        //InitSwipePanel();
        InitPollockPanel(0);
    }

    void InitPollockPanel(int newPanel = 0)
    {
        string swipeDirection = "left";
        //if we have a current panel, have to move out and destroy
        if(_CurPanel != null)
        {
            if(newPanel > _CurPanelInt)
            {
                _CurPanel.Destroy("left");
            }
            else
            {
                _CurPanel.Destroy("right");
                swipeDirection = "right";
            }
        }

        _CurPanelInt = newPanel;
        PollockPanel curPanel = PollockPrefabs[_CurPanelInt];

        StartCoroutine(AnimInNewPanel(curPanel, swipeDirection));

        //_CurPanel = Instantiate(curPanel, PanelContainer.transform) as PollockPanel;
        //_CurPanel.Init();

        //PollockPanel1 pollockPanel = Instantiate(Panel1Prefab, PanelContainer.transform) as PollockPanel1;
        //pollockPanel.Init();
    }

    IEnumerator AnimInNewPanel(PollockPanel curPanel, string swipeDirection)
    {
        yield return new WaitForSeconds(0.4f);
        _CurPanel = Instantiate(curPanel, PanelContainer.transform) as PollockPanel;
        _CurPanel.Init(swipeDirection);
    }

 /*
    void InitSwipePanel(string swipeDirection = "")
    {
        SwipePanel swipePanel = Instantiate(SwipePanelPrefab, PanelContainer.transform) as SwipePanel;
        swipePanel.Init(swipeDirection);
    }
*/

    void LastUserLeft()
    {
        ClearPanels();
    }

    void ClearPanels()
    {
        foreach (Transform child in PanelContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        _CurPanel = null;
    }

    /// <summary>
	/// Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	public void UserLost(long userId, int userIndex)
    {
        // the gestures are allowed for the primary user only
        if (userIndex != PlayerIndex)
            return;

        _UserCount--;

        if (_UserCount == 0)
            LastUserLeft();

        if (UserDebugTxt != null)
        {
            string userStr = " user detected";
            if (_UserCount > 1)
            {
                userStr = "users detected";
            }else if(_UserCount == 0)
            {
                userStr = "No users detected";
            }
            UserDebugTxt.text = string.Format("{0} {1}", _UserCount, userStr);
        }

        if (GestureInfo != null)
        {
            GestureInfo.text = string.Empty;
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
        // the gestures are allowed for the primary user only
        if (userIndex != PlayerIndex)
            return;

        string gestureText = "";

        if ((gesture == KinectGestures.Gestures.ZoomOut || gesture == KinectGestures.Gestures.ZoomIn) && progress > 0.5f)
        {
            gestureText = string.Format("{0} - {1:F0}%", gesture, screenPos.z * 100f);
        }
        else if ((gesture == KinectGestures.Gestures.Wheel || gesture == KinectGestures.Gestures.LeanLeft ||
                 gesture == KinectGestures.Gestures.LeanRight) && progress > 0.5f)
        {
            gestureText = string.Format("{0} - {1:F0} degrees", gesture, screenPos.z);
        }
        else if (gesture == KinectGestures.Gestures.Run && progress > 0.5f)
        {
            gestureText = string.Format("{0} - progress: {1:F0}%", gesture, progress * 100);
        }

        if(!string.IsNullOrEmpty(gestureText))
        {
            if (GestureInfo != null)
            {
                ProgressDisplayed = true;
                ProgressGestureTime = Time.realtimeSinceStartup;
            }
        }
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
        // the gestures are allowed for the primary user only
        if (userIndex != PlayerIndex)
            return false;

        if (GestureInfo != null)
        {
            string sGestureText = gesture + " detected";
            GestureInfo.text = sGestureText;
        }
        if (gesture == KinectGestures.Gestures.SwipeLeft)
        {
            _SwipeLeft = true;
            ProcessSwipe("left");
            if (OnSwipeAction != null)
                OnSwipeAction("left");

            //** InitSwipePanel("left");
        }
        else if (gesture == KinectGestures.Gestures.SwipeRight)
        {
            _SwipeRight = true;
            ProcessSwipe("right");
            if (OnSwipeAction != null)
                OnSwipeAction("right");

            //** InitSwipePanel("right");
        }

        return true;
    }

    void DestroyCurPanel(string swipeDirection)
    {
        if(_CurPanel != null)
        {
            _CurPanel.OnAnimSwipeAway(swipeDirection);
        }
    }

    void ProcessSwipe(string swipeDirection)
    {
        int newPanel;
        if(swipeDirection == "left")
        {
            if(_CurPanelInt < _TotalPanels-1)
            {
                newPanel = _CurPanelInt + 1;
                DestroyCurPanel(swipeDirection);
                InitPollockPanel(newPanel);
            } 
        }else if(swipeDirection == "right")
        {
            if(_CurPanelInt > 0)
            {
                newPanel = _CurPanelInt - 1;
                DestroyCurPanel(swipeDirection);
                InitPollockPanel(newPanel);
            }
        }
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
        // the gestures are allowed for the primary user only
        if (userIndex != PlayerIndex)
            return false;

        if (ProgressDisplayed)
        {
            ProgressDisplayed = false;

            if (GestureInfo != null)
                GestureInfo.text = String.Empty;
            
        }

        return true;
    }

    void Update()
    {
        if (ProgressDisplayed && ((Time.realtimeSinceStartup - ProgressGestureTime) > 2f))
        {
            ProgressDisplayed = false;
            GestureInfo.text = String.Empty;

            Debug.Log("Forced progress to end.");
        }
    }
}
