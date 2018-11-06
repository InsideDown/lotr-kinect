using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PollockPanel: MonoBehaviour
{

    private float _XTweenDistance = 800f;
    private float _XStartPos;
    private float _SwipeTweenDuration = 0.4f;

    public virtual void Awake()
    {
        _XStartPos = this.gameObject.transform.localPosition.x;
        Debug.Log("pollock parent awake");
    }

    public virtual void Init(string swipeDirection = "")
    {
        Debug.Log("pollock parent init");
    }

    public virtual void Destroy(string swipeDirection)
    {
        Debug.Log("pollock parent destroy");

    }

    public void OnAnimSwipeAway(string swipeDirection)
    {
        float tweenDistance = _XTweenDistance;
        if (swipeDirection == "left")
        {
            tweenDistance = _XStartPos - _XTweenDistance;
        }
        else if (swipeDirection == "right")
        {
            tweenDistance = _XStartPos + _XTweenDistance;
        }

        this.gameObject.transform.DOLocalMoveX(tweenDistance, _SwipeTweenDuration).SetEase(Ease.InOutSine).OnComplete(SwipeComplete);
    }

    void SwipeComplete()
    {
        Destroy(this.gameObject);
    }

    public virtual void OnDestroy()
    {
        DOTween.Kill(this.gameObject.transform);
    }
}
