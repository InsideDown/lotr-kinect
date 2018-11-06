using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SwipePanel : MonoBehaviour {

    [SerializeField]
    private Image _BackgroundImage;
    [SerializeField]
    private CanvasGroup _CanvasGroup;

    private float _XTweenDistance = 700f;
    private float _SwipeTweenDuration = 0.4f;
    private float _ScaleTweenDuration = 0.1f;
    private Color32 _StartingColor;
    private Color32 _ReplaceColor = new Color32(150, 255, 255, 255);

    void Awake()
    {
        _StartingColor = _BackgroundImage.color;
    }

    void OnSwipeAction(string swipeDirection)
    {
        float tweenDistance = _XTweenDistance;
        if (swipeDirection == "left")
        {
            tweenDistance = -_XTweenDistance;
        } else if (swipeDirection == "right")
        {
            tweenDistance = _XTweenDistance;
        }

        this.gameObject.transform.DOLocalMoveX(tweenDistance, _SwipeTweenDuration).SetEase(Ease.OutSine).OnComplete(SwipeComplete);
    }

    void SwipeComplete()
    {
        Destroy(this.gameObject);
    }

    public void Init(string swipeDirection)
    {
        if (swipeDirection == "left")
        {
            this.gameObject.transform.localPosition = new Vector3(_XTweenDistance, 0, 0);
        } else if (swipeDirection == "right")
        {
            this.gameObject.transform.localPosition = new Vector3(-_XTweenDistance, 0, 0);
        }else
        {
            //if we did not get either direction, we should fade in
            _CanvasGroup.alpha = 0;
            _CanvasGroup.DOFade(1.0f, 0.5f);
        }
        this.gameObject.transform.DOLocalMoveX(0, _SwipeTweenDuration).SetEase(Ease.OutSine);
    }

    void OnTapAction()
    {
        Debug.Log("Tap action triggered");
        Color32 placeholderColor = _StartingColor;

        if (_BackgroundImage.color == _StartingColor)
            placeholderColor = _ReplaceColor;

        _BackgroundImage.color = placeholderColor;
    }

    void OnEnable()
    {
        KinectSwipe.OnSwipeAction += OnSwipeAction;
        KinectSwipe.OnTapAction += OnTapAction;
    }

    void OnDisable()
    {
        KinectSwipe.OnSwipeAction -= OnSwipeAction;
        KinectSwipe.OnTapAction -= OnTapAction;
    }

    void OnDestroy()
    {
        DOTween.Kill(this.gameObject.transform);
    }


}
