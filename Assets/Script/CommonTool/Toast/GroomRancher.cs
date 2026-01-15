using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroomRancher : TiltDigestion<GroomRancher>
{

    public void BindGroom(string info)
    {
        UIRancher.FarBefriend().BindUIPlace("Groom", info);
    }
}
