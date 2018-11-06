using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagEndParticleController : BagEndInteractibleBase
{

    public JointOverlayer RightHandJoint;
    public JointOverlayer LeftHandJoint;

    private Vector3 _StartingScale;

    private void Awake()
    {
        _StartingScale = this.gameObject.transform.localScale;
        this.OnHide(0, 0);
        if (RightHandJoint == null)
            throw new System.Exception("A BagEndParticleController must have a RightHandJoint attached");
        if (LeftHandJoint == null)
            throw new System.Exception("A BagEndParticleController must have a LeftHandJoint attached");
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
        RightHandJoint.playerIndex = userIndex;
        LeftHandJoint.playerIndex = userIndex;
        base.SetUserIndex(userIndex);
    }


}
