using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zeta_framework;

public class ImpendDynamicAfarUI : DynamicAfarUI
{
    public override void DynamicSteamshipCb()
    {
        base.DynamicSteamshipCb();

        gameObject.GetComponent<AfarManUI>()?.PegSteamship();
    }
}
