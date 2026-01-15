using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SteamshipRevolution : MonoBehaviour
{





    /// <summary>
    /// 弹窗出现效果
    /// </summary>
    /// <param name="PopBarUp"></param>
    public static void EkeBind(GameObject PopBarUp, System.Action finish)
    {
        /*-------------------------------------初始化------------------------------------*/
        PopBarUp.GetComponent<CanvasGroup>().alpha = 0;
        PopBarUp.transform.localScale = new Vector3(0, 0, 0);
        /*-------------------------------------动画效果------------------------------------*/
        PopBarUp.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
        PopBarUp.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            finish();
        });
    }


    /// <summary>
    /// 弹窗消失效果
    /// </summary>
    /// <param name="PopBarDisapper"></param>
    public static void EkeSend(GameObject PopBarDisapper, System.Action finish)
    {
        /*-------------------------------------初始化------------------------------------*/
        PopBarDisapper.GetComponent<CanvasGroup>().alpha = 1;
        PopBarDisapper.transform.localScale = new Vector3(1, 1, 1);
        /*-------------------------------------动画效果------------------------------------*/
        PopBarDisapper.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        PopBarDisapper.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            finish();
        });
    }
    /// <summary>
    /// 数字变化动画
    /// </summary>
    /// <param name="startNum"></param>
    /// <param name="endNum"></param>
    /// <param name="text"></param>
    /// <param name="finish"></param>
    public static void UranusFormal(int startNum, int endNum, float delay, Text text, System.Action finish)
    {
        if (text == null)
        {
            finish?.Invoke();
            return;
        }
        
        DOTween.To(() => startNum, x => 
        {
            if (text != null)
            {
                text.text = x.ToString();
            }
        }, endNum, 0.5f).SetDelay(delay).OnComplete(() =>
        {
            finish?.Invoke();
        });
    }

    public static void UranusFormal(double startNum, double endNum, float delay, Text text, System.Action finish)
    {
        UranusFormal(startNum, endNum, delay, text, "", finish);
    }
    public static void UranusFormal(double startNum, double endNum, float delay, Text text, string prefix, System.Action finish)
    {
        if (text == null)
        {
            finish?.Invoke();
            return;
        }
        
        DOTween.To(() => startNum, x => 
        {
            if (text != null)
            {
                text.text = prefix + FormalGate.EmbryoBeSow(x);
            }
        }, endNum, 0.5f).SetDelay(delay).OnComplete(() =>
        {
            finish?.Invoke();
        });
    }

    /// <summary>
    /// 收金币
    /// </summary>
    /// <param name="GoldImage">金币图标</param>
    /// <param name="a">金币数量</param>
    /// <param name="StartPosition">起始位置</param>
    /// <param name="EndPosition">最终位置</param>
    /// <param name="finish">结束回调</param>
    public static void CastSlumHour(GameObject GoldImage, int a, Vector2 StartPosition, Vector2 EndPosition, System.Action finish)
    {
        //如果没有就算了
        if (a == 0)
        {
            finish();
        }
        //数量不超过15个
        else if (a > 15)
        {
            a = 15;
        }
        //每个金币的间隔
        float Delaytime = 0;
        int MidChance = 0;
        for (int i = 0; i < a; i++)
        {
            int t = i;
            //每次延迟+1
            Delaytime += 0.06f;
            //复制一个图标
            GameObject CastPart= Instantiate(GoldImage, GoldImage.transform.parent);
            CastPart.SetActive(true);
            //初始化
            CastPart.transform.position = StartPosition;
            CastPart.transform.localScale = new Vector3(0f, 0f, 0f);
            //金币弹出随机位置
            float OffsetX = UnityEngine.Random.Range(-1f, 1f);
            float OffsetY = UnityEngine.Random.Range(-1f, 1f);
            //创建动画队列
            var s = DOTween.Sequence();
            //金币出现
            s.Append(CastPart.transform.DOMove(new Vector3(CastPart.transform.position.x + OffsetX, CastPart.transform.position.y + OffsetY, CastPart.transform.position.z), 0.2f).SetDelay(Delaytime).OnComplete(() =>
            {
                //限制音效播放数量
                //if (Mathf.Sin(t) > 0)
                if (t % 5 == 0)
                {
                    LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.Sound_GoldCoin);
                }
            }));
            Vector3 midPos = new Vector3(CastPart.transform.position.x, EndPosition.y, 0);
            midPos = new Vector3(EndPosition.x, CastPart.transform.position.y, 0);
            Vector3[] dotweenpath = BezierUtils.GetBeizerList(CastPart.transform.position, midPos, EndPosition, 6);
            s.Join(CastPart.transform.DOScale(3, 0.2f));
            //金币移动到最终位置
            s.Append(CastPart.transform.DOPath(dotweenpath, 0.8f, PathType.CatmullRom).SetDelay(Random.Range(0.2f, 0.3f)));
            s.Join(CastPart.transform.DOScale(1.5f, 0.3f).SetEase(Ease.InBack));
            s.AppendCallback(() =>
            {
                //收尾
                s.Kill();
                Destroy(CastPart);
                if (t == a - 1)
                {
                    finish();
                }
            });
        }
    }
    /// <summary>
    /// 收金币
    /// </summary>
    /// <param name="GoldImage">金币图标</param>
    /// <param name="a">金币数量</param>
    /// <param name="StartPosition">起始位置</param>
    /// <param name="EndPosition">最终位置</param>
    /// <param name="finish">结束回调</param>
    public static void FreezingFarCast(GameObject GoldImage, int a, Transform Pos, Vector2 StartPosition, Vector2 EndPosition, System.Action finish)
    {
        //如果没有就算了
        if (a == 0)
        {
            finish();
        }
        //数量不超过15个
        else if (a > 15)
        {
            a = 15;
        }
        //每个金币的间隔
        float Delaytime = 0;
        int MidChance = 0;
        for (int i = 0; i < a; i++)
        {
            int t = i;
            //每次延迟+1
            Delaytime += 0.06f;
            //复制一个图标
            GameObject CastPart= Instantiate(GoldImage, Pos);
            CastPart.SetActive(true);
            //初始化
            CastPart.transform.position = StartPosition;
            CastPart.transform.localScale = new Vector3(0f, 0f, 0f);
            //金币弹出随机位置
            float OffsetX = UnityEngine.Random.Range(-1f, 1f);
            float OffsetY = UnityEngine.Random.Range(-1f, 1f);
            //创建动画队列
            var s = DOTween.Sequence();
            //金币出现
            s.Append(CastPart.transform.DOMove(new Vector3(CastPart.transform.position.x + OffsetX, CastPart.transform.position.y + OffsetY, CastPart.transform.position.z), 0.2f).SetDelay(Delaytime).OnComplete(() =>
            {
                //限制音效播放数量
                //if (Mathf.Sin(t) > 0)
                if (t % 5 == 0)
                {
                    LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.Sound_GoldCoin);
                }
            }));
            Vector3 midPos = new Vector3(CastPart.transform.position.x, EndPosition.y, 0);
            midPos = new Vector3(EndPosition.x, CastPart.transform.position.y, 0);
            Vector3[] dotweenpath = BezierUtils.GetBeizerList(CastPart.transform.position, midPos, EndPosition, 6);
            s.Join(CastPart.transform.DOScale(1.5f, 0.2f));
            //金币移动到最终位置
            s.Append(CastPart.transform.DOPath(dotweenpath, 0.8f, PathType.CatmullRom).SetDelay(Random.Range(0.2f, 0.3f)));
            s.Join(CastPart.transform.DOScale(1f, 0.3f).SetEase(Ease.InBack));
            s.AppendCallback(() =>
            {
                //收尾
                s.Kill();
                Destroy(CastPart);
                if (t == a - 1)
                {
                    finish?.Invoke();
                }
            });
        }
    }
    public static void CastSlumHour(GameObject GoldImage, int a, Transform StartTF, Transform EndTF, System.Action finish)
    {
        CastSlumHour(GoldImage, a, StartTF.position, EndTF.position, finish);
    }

    /// <summary>
    /// 横向滚动
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="addPosition"></param>
    /// <param name="Finish"></param>
    public static void SuccessiveScroll(GameObject obj, float addPosition, System.Action Finish)
    {
        float positionX = obj.transform.localPosition.x;
        float endPostion = positionX + addPosition;
        obj.transform.DOLocalMoveX(endPostion, 2f).SetEase(Ease.InOutQuart).OnComplete(() =>
        {
            Finish?.Invoke();
        });
    }

    /// <summary>
    /// 奖励按钮依次展示
    /// </summary>
    /// <param name="WinPanelList"></param>
    public static void UrnSwearBind(List<GameObject> WinPanelList)
    {
        float delayTime = 0.5f;
        for (int i = 0; i < WinPanelList.Count; i++)
        {
            GameObject Seal= WinPanelList[i].gameObject;
            Seal.transform.localScale = new Vector3(0, 0, 0);
            Seal.transform.DOScale(1.2f, 0.2f).SetDelay(delayTime).OnComplete(() =>
            {
                Seal.transform.DOScale(1, 0.2f);
            });
            delayTime += 0.1f;
        }
    }
    /// <summary>
    /// 星星动画(过关)
    /// </summary>
    /// <param name="StarList"></param>
    /// <param name="StartTF"></param>
    /// <param name="EndTF"></param>
    /// <param name="StarEffect"></param>
    /// <param name="WinPanel"></param>
    public static void UrnFord(int num, List<Transform> StarList, List<Transform> StartTF, List<Transform> EndTF, List<GameObject> StarEffect, Transform WinPanel)
    {
        float delayTime = 0.6f;
        //重置星星状态
        for (int i = 0; i < StarList.Count; i++)
        {
            StarList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < num; i++)
        {
            //INIT
            GameObject Seal= StarList[i].gameObject;
            Seal.SetActive(true);
            List<Vector3> Endpos = EndTF.ConvertAll(t => t.position);
            Seal.transform.position = StartTF[i].position;
            GameObject StarFX = StarEffect[i];
            Seal.transform.eulerAngles = new Vector3(0, 0, 270);
            Seal.transform.localScale = new Vector3(3, 3, 3);
            Seal.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
            Vector3 midPos = new Vector3(Endpos[i].x, StartTF[i].position.y, 0);
            Vector3[] PathPos = BezierUtils.GetBeizerList(StartTF[i].position, midPos, Endpos[i], 6);
            //动画开始
            Seal.GetComponent<UnityEngine.UI.Image>().DOFade(1, 0.3f).SetDelay(delayTime);
            Seal.transform.DORotate(new Vector3(0, 0, 15 + i * -15), 0.4f, DG.Tweening.RotateMode.FastBeyond360).SetDelay(delayTime);
            Seal.transform.DOScale(1.3f, 0.5f).SetDelay(delayTime).OnComplete(() =>
            {
                Seal.transform.DOScale(1, 0.1f).OnComplete(() =>
                {
                    Seal.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
                    {
                        Seal.transform.DOScale(1, 0.1f);
                    });
                });
            });
            Seal.transform.DOPath(PathPos, 0.5f, PathType.CatmullRom).SetDelay(delayTime).OnComplete(() =>
            {
                if (WinPanel != null) WinPanel.DOShakePosition(0.3f, 10, 15, 90);
                if (StarFX != null) StarFX.SetActive(true);
            });
            delayTime += 0.3f;
        }
    }
    // /// <summary>
    // /// 魔法棒特效
    // /// </summary>
    // /// <param name="Afar"></param>
    // /// <param name="StartPos"></param>
    // /// <param name="GetPos"></param>
    // public static void MagicWand(GameObject Afar, Vector3 StartPos, Vector3 GetPos)
    // {
    //     //Init动画准备
    //     Afar.transform.position = StartPos;
    //     GameObject ItemImage = Afar.transform.GetChild(1).gameObject;
    //     ItemImage.transform.localScale = new Vector3(0, 0, 0);
    //     Afar.transform.eulerAngles = new Vector3(0, 0, 0);
    //     ItemImage.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
    //     Afar.SetActive(true);
    //     float A = 1;
    //     if (GetPos.x <= 0)
    //     {
    //         A *= -1;
    //     }
    //     Vector3 midPos = new Vector3(GetPos.x, StartPos.y, 0);
    //     Vector3[] PathPos = BezierUtils.GetBeizerList(StartPos, midPos, GetPos, 6);
    //     //动画开始
    //     Sequence MagicAni = DOTween.Sequence();
    //     ItemImage.GetComponent<UnityEngine.UI.Image>().DOFade(1, 0.2f);
    //     ItemImage.transform.DOScale(1, 0.7f).SetEase(Ease.OutBack);
    //     Afar.transform.localScale = new Vector3(1 * A, 1, 1);
    //     Afar.transform.DORotate(new Vector3(0, 0, 80 * A), 0.5f).SetEase(Ease.InQuad);
    //     MagicAni.Append(Afar.transform.DOPath(PathPos, 0.5f, PathType.CatmullRom));
    //     MagicAni.Append(Afar.transform.DORotate(new Vector3(0, 0, 90 * A), 0.2f));
    //     MagicAni.Append(Afar.transform.DORotate(new Vector3(0, 0, -45 * A), 0.2f).SetEase(Ease.InQuad).OnComplete(() =>
    //     {
    //         GameObject ParticleMagic = Instantiate(KickSwear.Instance.StarsEffect, KickSwear.Instance.transform);
    //         ParticleMagic.transform.position = new Vector3(GetPos.x + 1 * A, GetPos.y, 0);
    //         ParticleMagic.SetActive(true);
    //     }));
    //     MagicAni.Append(ItemImage.GetComponent<UnityEngine.UI.Image>().DOFade(0, 0.4f));
    //     MagicAni.Append(ItemImage.GetComponent<UnityEngine.UI.Image>().DOFade(0, 0.1f));
    //     MagicAni.AppendCallback(() =>
    //     {
    //         Afar.SetActive(false);
    //     });
    // }
    /// <summary>
    /// 魔法棒特效
    /// </summary>
    /// <param name="Afar"></param>
    /// <param name="StartPos"></param>
    /// <param name="GetPos"></param>
    public static void CarveNeon(GameObject Afar, Vector3 StartPos, Vector3 GetPos)
    {
        //Init动画准备
        Afar.transform.position = Vector2.zero;
        GameObject ItemImage = Afar.transform.GetChild(0).gameObject;
        ItemImage.transform.localScale = new Vector3(0, 0, 0);
        ItemImage.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
        Afar.SetActive(true);
        float A = 1;
        if (GetPos.x >= 0)
            A *= -1;
        //动画开始
        Sequence MagicAni = DOTween.Sequence();
        ItemImage.GetComponent<UnityEngine.UI.Image>().DOFade(1, 0.2f);
        ItemImage.transform.DOScale(1, 0.7f).SetEase(Ease.OutBack);
        Afar.transform.localScale = new Vector3(1 * A, 1, 1);
        //绕着屏幕转一圈
        Vector3 screenCenterWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        screenCenterWorld.z = 0;
        // 计算圆形路径点（12个点组成圆） 最后两个点为起始点和终点
        Vector3[] circlePoints = new Vector3[14];
        float radius = 1;
        for (int i = 0; i < 12; i++)
        {
            float angle = i * (360f / 12) * Mathf.Deg2Rad;
            circlePoints[i] = screenCenterWorld + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }
        circlePoints[12] = StartPos;
        circlePoints[13] = GetPos;
        MagicAni.Append(Afar.transform.DOPath(circlePoints, 1.5f, PathType.CatmullRom).SetOptions(false).SetEase(Ease.Linear));
        MagicAni.Append(ItemImage.transform.DORotate(new Vector3(0, 0, 30 * A), 0.2f));
        MagicAni.Append(ItemImage.transform.DORotate(new Vector3(0, 0, 80 * A), 0.2f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            GameObject ParticleMagic = Instantiate(KickSwear.Instance.TaintGuinea, KickSwear.Instance.transform);
            ParticleMagic.transform.position = Afar.transform.position;
            ParticleMagic.transform.localScale = Vector3.one * 120;
            ParticleMagic.SetActive(true);
            Destroy(ParticleMagic, 2);
        }));
        MagicAni.Append(ItemImage.GetComponent<UnityEngine.UI.Image>().DOFade(0, 0.4f));
        MagicAni.Append(ItemImage.GetComponent<UnityEngine.UI.Image>().DOFade(0, 0.1f));
        MagicAni.AppendCallback(() =>
        {
            Afar.SetActive(false);
        });
    }

    public static void PegDense(Vector3 StartPos)
    {
        GameObject Afar = Instantiate(KickSwear.Instance.PegDenseAfar, KickSwear.Instance.transform);
        Afar.transform.position = StartPos;
        Afar.transform.localScale = new Vector3(0, 0, 0);
        Afar.GetComponent<CanvasGroup>().alpha = 0;
        Afar.SetActive(true);
        Afar.transform.DOMoveY(StartPos.y + 0.2f, 0.5f).OnComplete(() =>
        {
            Afar.transform.DOMoveY(StartPos.y + 0.4f, 0.3f);
            Afar.GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
            {
                Afar.SetActive(false);
                Destroy(Afar);
            });
        });
        Afar.transform.DOScale(1, 0.3f);
        Afar.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
    }

    public static void PlusSwear(UnityEngine.UI.Image Bg, Transform UI)
    {
        Bg.color = new Color(0, 0, 0, 0);
        Bg.DOFade(.8f, .3f);
        UI.localScale = Vector3.zero;
        UI.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(.2f);
    }

}
