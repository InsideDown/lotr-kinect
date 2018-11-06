using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagEndInteractibleBase : MonoBehaviour
{
    
    [HideInInspector]
    public long UserID = 0;
    [HideInInspector]
    public int UserIndex = -1;

    public virtual void OnHide(long userId, int userIndex)
    {
        UserID = 0;
        UserIndex = -1;
    }

    public virtual void OnShow(long userId, int userIndex)
    {
        UserID = userId;
        UserIndex = userIndex;
    }

    public virtual void SetUserIndex(int userIndex)
    {
        UserIndex = userIndex;
    }
    
}
