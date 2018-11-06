using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class RandomColorUser : MonoBehaviour
{
    private RawImage rawImage;


    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }


    void Update()
    {
        if (rawImage && rawImage.texture == null)
        {
            BackgroundRemovalManager backManager = BackgroundRemovalManager.Instance;
            KinectManager kinectManager = KinectManager.Instance;

            if (kinectManager && backManager && backManager.enabled /**&& backManager.IsBackgroundRemovalInitialized()*/)
            {
                rawImage.texture = backManager.GetForegroundTex();  // user's foreground texture
                rawImage.rectTransform.localScale = kinectManager.GetColorImageScale();
                rawImage.color = Color.white;
                //rawImage.color = RandomColor();

                //start us on a random color
                rawImage.color = RandomColor();
            }
            else if (kinectManager /**&& kinectManager.IsInitialized()*/)
            {
                SimpleBackgroundRemoval simpleBR = GameObject.FindObjectOfType<SimpleBackgroundRemoval>();
                bool isSimpleBR = simpleBR && simpleBR.enabled;

                rawImage.texture = kinectManager.GetUsersClrTex();  // color camera texture
                rawImage.rectTransform.localScale = kinectManager.GetColorImageScale();
                rawImage.color = !isSimpleBR ? Color.white : Color.clear;
            }
        }

        if (rawImage)
        {
            
            rawImage.color = HueShift(); //RandomColor();
        }
    }

    private float _Angle = 0;
    private float _MaxAngle = 1;

    Color RandomColor()
    {
        Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        return newColor;
    }

    Color HueShift()
    {
        float h, s, v;
        Color.RGBToHSV(rawImage.color, out h, out s, out v);
        if (_Angle < _MaxAngle)
        {
            _Angle += .01f;
        }
        else
        {
            _Angle = 0;
        }
        h = _Angle;
        return Color.HSVToRGB(h,s,v);

    }
    
    void OnApplicationPause(bool isPaused)
    {
        // fix for app pause & restore (UWP)
        if (isPaused && rawImage && rawImage.texture != null)
        {
            rawImage.texture = null;
            rawImage.color = Color.clear;
        }
    }

}
