using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FollowAnimation : MonoBehaviour
{

    public GameObject ObjectToFollow;

    private float _FollowSpeed = 0.15f;


    void Awake()
    {

    }

    void Update()
    {
        DOTween.Kill(this.gameObject.transform);
        this.gameObject.transform.DOMove(ObjectToFollow.transform.position, _FollowSpeed);
        this.gameObject.transform.DORotate(ObjectToFollow.transform.eulerAngles, _FollowSpeed);
        //this.gameObject.transform.DOMove(ObjectToFollow.transform, 0.3, false);
        //this.gameObject.transform.DOLocalMoveX(tweenDistance, _SwipeTweenDuration).SetEase(Ease.InOutSine).OnComplete(SwipeComplete);
    }

}
