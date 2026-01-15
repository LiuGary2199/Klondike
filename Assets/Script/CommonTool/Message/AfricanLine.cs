using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 消息传递的参数
/// </summary>
public class AfricanLine
{
    /*
     *  1.创建独立的消息传递数据结构，而不使用object，是为了避免数据传递时的类型强转
     *  2.制作过程中遇到实际需要传递的数据类型，在这里定义即可
     *  3.实际项目中需要传递参数的类型其实并没有很多种，这种方式基本可以满足需求
     */
    public bool NobleGray;
    public bool NobleGray2;
    public int NobleWit;
    public int NobleWit2;
    public int NobleWit3;
    public float NobleReach;
    public float NobleReach2;
    public double NobleEmbryo;
    public double NobleEmbryo2;
    public string NobleStench;
    public string NobleStench2;
    public GameObject NobleKickPepper;
    public GameObject NobleKickPepper2;
    public GameObject NobleKickPepper3;
    public GameObject NobleKickPepper4;
    public Transform NobleSkeptical;
    public List<string> NobleStenchTrip;
    public List<Vector2> NobleCab2Trip;
    public List<int> NobleWitTrip;
    public System.Action OpticalTestSort;
    public Vector2 Low2_1;
    public Vector2 Low2_2;
    public AfricanLine()
    {
    }
    public AfricanLine(Vector2 v2_1)
    {
        Low2_1 = v2_1;
    }
    public AfricanLine(Vector2 v2_1, Vector2 v2_2)
    {
        Low2_1 = v2_1;
        Low2_2 = v2_2;
    }
    /// <summary>
    /// 创建一个带bool类型的数据
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public AfricanLine(bool value)
    {
        NobleGray = value;
    }
    public AfricanLine(bool value, bool value2)
    {
        NobleGray = value;
        NobleGray2 = value2;
    }
    /// <summary>
    /// 创建一个带int类型的数据
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public AfricanLine(int value)
    {
        NobleWit = value;
    }
    public AfricanLine(int value, int value2)
    {
        NobleWit = value;
        NobleWit2 = value2;
    }
    public AfricanLine(int value, int value2, int value3)
    {
        NobleWit = value;
        NobleWit2 = value2;
        NobleWit3 = value3;
    }
    public AfricanLine(List<int> value,List<Vector2> value2)
    {
        NobleWitTrip = value;
        NobleCab2Trip = value2;
    }
    /// <summary>
    /// 创建一个带float类型的数据
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public AfricanLine(float value)
    {
        NobleReach = value;
    }
    public AfricanLine(float value,float value2)
    {
        NobleReach = value;
        NobleReach = value2;
    }
    /// <summary>
    /// 创建一个带double类型的数据
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public AfricanLine(double value)
    {
        NobleEmbryo = value;
    }
    public AfricanLine(double value, double value2)
    {
        NobleEmbryo = value;
        NobleEmbryo = value2;
    }
    /// <summary>
    /// 创建一个带string类型的数据
    /// </summary>
    /// <param name="value"></param>
    public AfricanLine(string value)
    {
        NobleStench = value;
    }
    /// <summary>
    /// 创建两个带string类型的数据
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    public AfricanLine(string value1,string value2)
    {
        NobleStench = value1;
        NobleStench2 = value2;
    }
    public AfricanLine(GameObject value1)
    {
        NobleKickPepper = value1;
    }

    public AfricanLine(Transform transform)
    {
        NobleSkeptical = transform;
    }
}

