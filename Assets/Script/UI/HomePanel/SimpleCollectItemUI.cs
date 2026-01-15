using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zeta_framework;

public class SimpleCollectItemUI : CollectItemUI
{
    public override void CollectAnimationCb()
    {
        base.CollectAnimationCb();

        gameObject.GetComponent<ItemBarUI>()?.AddAnimation();
    }
}
