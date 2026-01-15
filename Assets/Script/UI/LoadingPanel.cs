using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public Image sliderImage;
    public Text progressText;
    // Start is called before the first frame update
    void Start()
    {
        sliderImage.fillAmount = 0;
        progressText.text = "0%";
    }

    void Update()
    {
        if (sliderImage.fillAmount <= 0.8f || NetInfoMgr.instance.ready)
        {
            sliderImage.fillAmount += Time.deltaTime * .2f;
            progressText.text = (int)(sliderImage.fillAmount * 100) + "%";
            if (sliderImage.fillAmount >= 1)
            {
                CommonUtil.IsApple();

                Destroy(transform.parent.gameObject);
                MainManager.instance.gameInit();
            }
        }
    }
}
