using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LothlorienController : MonoBehaviour {

    public KinectManager KinectManager;
    public List<GameObject> HandModels = new List<GameObject>();
    public VideoPlayer VideoPlayer;
    public GameObject BackgroundCanvas;
    public GameObject BackgroundCamera;


	void Start()
    {
        ToggleHandModelsOn(false);
    }

	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("q"))
        {
            ToggleHandModelsOn(true);
        }
        if(Input.GetKeyDown("w"))
        {
            ToggleHandModelsOn(false);
        }

        if(Input.GetKeyDown("a"))
        {
            ToggleKinectSettingsOn(true);
        }
        if(Input.GetKeyDown("s"))
        {
            ToggleKinectSettingsOn(false);
        }

        if(Input.GetKeyDown("z"))
        {
            ToggleVideoPlayerOn(true);
        }

        if(Input.GetKeyDown("x"))
        {
            ToggleVideoPlayerOn(false);
        }
	}

    void ToggleHandModelsOn(bool isOn = true)
    {
        for (int i = 0; i < HandModels.Count; i++)
        {
            GameObject curModel = HandModels[i];
            curModel.SetActive(isOn);
        }
    }

    void ToggleKinectSettingsOn(bool isKinectSettingsOn = true)
    {
        if (KinectManager.Instance != null)
        {
            KinectManager.Instance.displayUserMap = isKinectSettingsOn;
            KinectManager.Instance.displayColorMap = isKinectSettingsOn;
            KinectManager.Instance.displaySkeletonLines = isKinectSettingsOn;
        }
        
    }

    void ToggleVideoPlayerOn(bool isVideoOn = true)
    {
        VideoPlayer.enabled = isVideoOn;
        BackgroundCamera.SetActive(!isVideoOn);
        BackgroundCanvas.SetActive(!isVideoOn);
    }
}
