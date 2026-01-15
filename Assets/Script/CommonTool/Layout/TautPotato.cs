using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TargetType
{
    Scene,
    UGUI
}
public enum LayoutType
{
    Sprite_First_Weight,
    Sprite_First_Height,
    Screen_First_Weight,
    Screen_First_Height,
    Bottom,
    Top,
    Left,
    Right
}
public enum RunTime
{
    Awake,
    Start,
    None
}
public class TautPotato : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("Target_Type")]    public TargetType London_Joke;
[UnityEngine.Serialization.FormerlySerializedAs("Layout_Type")]    public LayoutType Layout_Joke;
[UnityEngine.Serialization.FormerlySerializedAs("Run_Time")]    public RunTime Old_Mold;
[UnityEngine.Serialization.FormerlySerializedAs("Layout_Number")]    public float Potato_Formal;
    private void Awake()
    {
        if (Old_Mold == RunTime.Awake)
        {
            AbroadGossip();
        }
    }
    private void Start()
    {
        if (Old_Mold == RunTime.Start)
        {
            AbroadGossip();
        }
    }

    public void AbroadGossip()
    {
        if (Layout_Joke == LayoutType.Sprite_First_Weight)
        {
            if (London_Joke == TargetType.UGUI)
            {

                float scale = Screen.width / Potato_Formal;
                //GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.width / w * h);
                transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        if (Layout_Joke == LayoutType.Screen_First_Weight)
        {
            if (London_Joke == TargetType.Scene)
            {
                float scale = FarFarmerLine.FarBefriend().HueDecadeAgree() / Potato_Formal;
                transform.localScale = transform.localScale * scale;
            }
        }
        
        if (Layout_Joke == LayoutType.Bottom)
        {
            if (London_Joke == TargetType.Scene)
            {
                float screen_bottom_y = FarFarmerLine.FarBefriend().HueDecadeMilieu() / -2;
                screen_bottom_y += (Potato_Formal + (FarFarmerLine.FarBefriend().HueReviewCity(gameObject).y / 2f));
                transform.position = new Vector3(transform.position.x, screen_bottom_y, transform.position.y);
            }
        }
    }
        
}
