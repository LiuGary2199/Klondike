using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskUsSwear : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("Stars")]    public Button[] Taint;
[UnityEngine.Serialization.FormerlySerializedAs("star1Sprite")]    public Sprite Germ1Review;
[UnityEngine.Serialization.FormerlySerializedAs("star2Sprite")]    public Sprite Germ2Review;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Button star in Taint)
        {
            star.onClick.AddListener(() =>
            {
                string indexStr = System.Text.RegularExpressions.Regex.Replace(star.gameObject.name, @"[^0-9]+", "");
                int index = indexStr == "" ? 0 : int.Parse(indexStr);
                MessyBadge(index);
            });
        }
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        for (int i = 0; i < 5; i++)
        {
            Taint[i].gameObject.GetComponent<Image>().sprite = Germ2Review;
        }
    }


    private void MessyBadge(int index)
    {
        for (int i = 0; i < 5; i++)
        {
            Taint[i].gameObject.GetComponent<Image>().sprite = i <= index ? Germ1Review : Germ2Review;
        }
        WhimInuitRemove.FarBefriend().LeafInuit("1301", (index + 1).ToString());
        if (index < 3)
        {
            StartCoroutine(StuffSwear());
        } else
        {
            // 跳转到应用商店
            MaskBeRancher.instance.PlusAPBatWholly();
            StartCoroutine(StuffSwear());
        }
        
        // 打点
        //WhimInuitRemove.GetInstance().SendEvent("1210", (index + 1).ToString());
    }

    IEnumerator StuffSwear(float waitTime = 0.5f)
    {
        yield return new WaitForSeconds(waitTime);
        FirstUIMode(GetType().Name);
    }
}
