using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagEndCharacter : BagEndInteractibleBase
{

    private Vector3 _StartingScale;
    private AvatarController CurAvatarController;

    private void Awake()
    {
        _StartingScale = this.gameObject.transform.localScale;
        CurAvatarController = this.gameObject.GetComponent<AvatarController>();
        this.OnHide(0,0);
        if (CurAvatarController == null)
            throw new System.Exception("A BagEndCharacter must have an AvatarController attached");
    }


    public override void OnHide(long userId, int userIndex)
    {
        this.gameObject.transform.localScale = Vector3.zero;
        base.OnHide(userId, userIndex);
    }

    public override void OnShow(long userId, int userIndex)
    {
        base.OnShow(userId, userIndex);
        SetUserIndex(userIndex);
        this.gameObject.transform.localScale = _StartingScale;
    }

    public override void SetUserIndex(int userIndex)
    {
        CurAvatarController.playerIndex = userIndex;
        base.SetUserIndex(userIndex);
    }


}
