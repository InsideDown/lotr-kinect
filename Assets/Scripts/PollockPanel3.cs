using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;

public class PollockPanel3 : PollockPanel {

    public VideoPlayer CurVideoPlayer;

    private float _XDist = 80.0f;
    private Vector3 _EndPos;
    private float _AnimInSpeed = 0.3f;

    void Awake()
    {
        base.Awake();
        _EndPos = CurVideoPlayer.gameObject.transform.localPosition;
    }

    public override void Init(string swipeDirection = "")
    {
        base.Init();
        
        float startXPos;
        //x anim
        if (swipeDirection == "right")
        {
            startXPos = _EndPos.x - _XDist;
        }
        else
        {
            startXPos = _EndPos.x + _XDist;
        }

        CurVideoPlayer.gameObject.transform.localPosition = new Vector3(
                 startXPos,
                 _EndPos.y,
                 _EndPos.z
        );

        CanvasGroup curCanvasGroup = CurVideoPlayer.GetComponent<CanvasGroup>();
        if (curCanvasGroup != null)
        {
            curCanvasGroup.alpha = 0;
            curCanvasGroup.DOFade(1.0f, _AnimInSpeed).SetEase(Ease.OutSine);
        }
        CurVideoPlayer.gameObject.transform.DOLocalMoveX(_EndPos.x, _AnimInSpeed, false).SetEase(Ease.OutSine);
    }

    public override void Destroy(string swipeDirection)
    {
        /*if (CurVideoPlayer != null)
            CurVideoPlayer.Stop();*/
        base.Destroy(swipeDirection);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
