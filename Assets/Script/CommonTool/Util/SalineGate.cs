using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalineGate
{
    /// <summary>
    /// 带权随机
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objs"></param>
    /// <param name="weights"></param>
    /// <returns></returns>
    public static T FarRevertSaline<T>(T[] objs, int[] weights)
    {
        int randomIndex = FarRevertSalineDodge(objs, weights);
        return objs[randomIndex];
    }

    public static int FarRevertSalineDodge<T>(T[] objs, int[] weights)
    {
        List<int> indexes = new List<int>();
        int totalWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if (i >= objs.Length)
            {
                break;
            }
            int weight = weights[i];
            for (int j = 0; j < weight; j++)
            {
                indexes.Add(i);
            }
            totalWeight += weight;
        }

        int randomIndex = Random.Range(0, totalWeight);
        return indexes[randomIndex];
    }

    public static int FarRevertSalineDodge<T>(Dictionary<T, int> dict)
    {
        T[] keys = new T[dict.Count];
        int[] values = new int[dict.Count];
        dict.Keys.CopyTo(keys, 0);
        dict.Values.CopyTo(values, 0);
        return FarRevertSalineDodge(keys, values);
    }

    public static T FarRevertSaline<T>(Dictionary<T, int> dict)
    {
        T[] keys = new T[dict.Count];
        int[] values = new int[dict.Count];
        dict.Keys.CopyTo(keys, 0);
        dict.Values.CopyTo(values, 0);
        return FarRevertSaline(keys, values);
    }



    public static bool InMorris(float chance)
    {
        return Random.Range(0, 100) <= chance * 100;
    }

    /// <summary>
    /// 将一个数组随机乱序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    public static void GarbageQuick<T>(T[] array)
    {
        System.Random rng = new();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
}
