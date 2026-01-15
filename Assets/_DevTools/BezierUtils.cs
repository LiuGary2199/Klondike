using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BezierUtils : MonoBehaviour
{
    //»ñÈ¡±´Èû¶ûµã
    private static Vector3 GetBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    private static Vector3 GetThird_Order_BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 temp;
        Vector3 p0p1 = (1 - t) * p0 + t * p1;
        Vector3 p1p2 = (1 - t) * p1 + t * p2;
        Vector3 p2p3 = (1 - t) * p2 + t * p3;
        Vector3 p0p1p2 = (1 - t) * p0p1 + t * p1p2;
        Vector3 p1p2p3 = (1 - t) * p1p2 + t * p2p3;
        temp = (1 - t) * p0p1p2 + t * p1p2p3;
        return temp;
    }


    public static Vector3[] GetBeizerList(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint,int segmentNum)
     {
         Vector3[] path = new Vector3[segmentNum];
         for (int i = 1; i <= segmentNum; i++)
         {
             float t = i / (float)segmentNum;
             Vector3 pixel = GetBezierPoint(t, startPoint,controlPoint, endPoint);
             path[i - 1] = pixel;
         }
         return path;

    }

    public static Vector3[] GetThird_Order_BeizerList(Vector3 startPoint, Vector3 controlPoint0, Vector3 controlPoint1, Vector3 endPoint, int segmentNum)
    {
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;
            Vector3 pixel = GetThird_Order_BezierPoint(t, startPoint, controlPoint0, controlPoint1, endPoint);
            path[i - 1] = pixel;
        }
        return path;

    }
}
