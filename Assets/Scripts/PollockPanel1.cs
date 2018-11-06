using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PollockPanel1 : PollockPanel
{

    public List<Text> TextFields;

    private List<float> _EndXPos = new List<float>();
    private float _XDist = 80.0f;
    private float _AnimInSpeed = 0.3f;

    void Awake()
    {
        base.Awake();
    
        foreach(Text txtField in TextFields)
        {

            float curXPos = txtField.gameObject.transform.localPosition.x;
            _EndXPos.Add(curXPos);

            CanvasGroup curCanvasGroup = txtField.GetComponent<CanvasGroup>();
            if (curCanvasGroup != null)
                curCanvasGroup.alpha = 0.0f;   
        }
    }

    public override void Init(string swipeDirection = "")
    {
        base.Init();

        int counter = 0;
        float delay = 0.3f;
        
        foreach (Text txtField in TextFields)
        {
            float curDelay = counter * delay;
            float endXPos = _EndXPos[counter];
            float startXPos = txtField.gameObject.transform.localPosition.x;
            counter++;
            //x anim
            if (swipeDirection == "right")
            {
                startXPos = startXPos - _XDist;
            }
            else
            {
                startXPos = startXPos + _XDist;
            }

            txtField.gameObject.transform.localPosition = new Vector3(
                     startXPos,
                     txtField.gameObject.transform.localPosition.y,
                     txtField.gameObject.transform.localPosition.z
            );

            CanvasGroup curCanvasGroup = txtField.GetComponent<CanvasGroup>();
            if (curCanvasGroup != null)
                curCanvasGroup.alpha = 0;
                    
            StartCoroutine(AnimIn(endXPos, curDelay, txtField));
        }
    }

    IEnumerator AnimIn(float endXPos, float curDelay, Text txtField)
    {
        yield return new WaitForSeconds(curDelay);
        CanvasGroup curCanvasGroup = txtField.GetComponent<CanvasGroup>();
        if (curCanvasGroup != null)
            curCanvasGroup.DOFade(1.0f, _AnimInSpeed).SetEase(Ease.OutSine);
        
        txtField.gameObject.transform.DOLocalMoveX(endXPos, _AnimInSpeed, false).SetEase(Ease.OutSine);
    }
	
    public override void Destroy(string swipeDirection)
    {
        base.Destroy(swipeDirection);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
