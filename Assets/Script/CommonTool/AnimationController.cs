using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class AnimationController : MonoBehaviour
{





    /// <summary>
    /// 弹窗出现效果
    /// </summary>
    /// <param name="PopBarUp"></param>
    public static void PopShow(GameObject PopBarUp, System.Action finish)
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
    public static void PopHide(GameObject PopBarDisapper, System.Action finish)
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
    public static void ChangeNumber(int startNum, int endNum, float delay, Text text, System.Action finish)
    {
        DOTween.To(() => startNum, x => text.text = x.ToString(), endNum, 0.5f).SetDelay(delay).OnComplete(() =>
        {
            finish();
        });
    }

    public static void ChangeNumber(double startNum, double endNum, float delay, Text text, System.Action finish)
    {
        ChangeNumber(startNum, endNum, delay, text, "", finish);
    }
    public static void ChangeNumber(double startNum, double endNum, float delay, Text text, string prefix, System.Action finish)
    {
        DOTween.To(() => startNum, x => text.text = prefix + NumberUtil.DoubleToStr(x), endNum, 0.5f).SetDelay(delay).OnComplete(() =>
        {
            finish();
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
    public static void GoldMoveBest(GameObject GoldImage, int a, Vector2 StartPosition, Vector2 EndPosition, System.Action finish)
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
            GameObject GoldIcon = Instantiate(GoldImage, GoldImage.transform.parent);
            GoldIcon.SetActive(true);
            //初始化
            GoldIcon.transform.position = StartPosition;
            GoldIcon.transform.localScale = new Vector3(0f, 0f, 0f);
            //金币弹出随机位置
            float OffsetX = UnityEngine.Random.Range(-1f, 1f);
            float OffsetY = UnityEngine.Random.Range(-1f, 1f);
            //创建动画队列
            var s = DOTween.Sequence();
            //金币出现
            s.Append(GoldIcon.transform.DOMove(new Vector3(GoldIcon.transform.position.x + OffsetX, GoldIcon.transform.position.y + OffsetY, GoldIcon.transform.position.z), 0.2f).SetDelay(Delaytime).OnComplete(() =>
            {
                //限制音效播放数量
                //if (Mathf.Sin(t) > 0)
                if (t % 5 == 0)
                {
                    MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Sound_GoldCoin);
                }
            }));
            Vector3 midPos = new Vector3(GoldIcon.transform.position.x, EndPosition.y, 0);
            midPos = new Vector3(EndPosition.x, GoldIcon.transform.position.y, 0);
            Vector3[] dotweenpath = BezierUtils.GetBeizerList(GoldIcon.transform.position, midPos, EndPosition, 6);
            s.Join(GoldIcon.transform.DOScale(3, 0.2f));
            //金币移动到最终位置
            s.Append(GoldIcon.transform.DOPath(dotweenpath, 0.8f, PathType.CatmullRom).SetDelay(Random.Range(0.2f, 0.3f)));
            s.Join(GoldIcon.transform.DOScale(1.5f, 0.3f).SetEase(Ease.InBack));
            s.AppendCallback(() =>
            {
                //收尾
                s.Kill();
                Destroy(GoldIcon);
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
    public static void KlondikeGetGold(GameObject GoldImage, int a, Transform Pos, Vector2 StartPosition, Vector2 EndPosition, System.Action finish)
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
            GameObject GoldIcon = Instantiate(GoldImage, Pos);
            GoldIcon.SetActive(true);
            //初始化
            GoldIcon.transform.position = StartPosition;
            GoldIcon.transform.localScale = new Vector3(0f, 0f, 0f);
            //金币弹出随机位置
            float OffsetX = UnityEngine.Random.Range(-1f, 1f);
            float OffsetY = UnityEngine.Random.Range(-1f, 1f);
            //创建动画队列
            var s = DOTween.Sequence();
            //金币出现
            s.Append(GoldIcon.transform.DOMove(new Vector3(GoldIcon.transform.position.x + OffsetX, GoldIcon.transform.position.y + OffsetY, GoldIcon.transform.position.z), 0.2f).SetDelay(Delaytime).OnComplete(() =>
            {
                //限制音效播放数量
                //if (Mathf.Sin(t) > 0)
                if (t % 5 == 0)
                {
                    MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Sound_GoldCoin);
                }
            }));
            Vector3 midPos = new Vector3(GoldIcon.transform.position.x, EndPosition.y, 0);
            midPos = new Vector3(EndPosition.x, GoldIcon.transform.position.y, 0);
            Vector3[] dotweenpath = BezierUtils.GetBeizerList(GoldIcon.transform.position, midPos, EndPosition, 6);
            s.Join(GoldIcon.transform.DOScale(1.5f, 0.2f));
            //金币移动到最终位置
            s.Append(GoldIcon.transform.DOPath(dotweenpath, 0.8f, PathType.CatmullRom).SetDelay(Random.Range(0.2f, 0.3f)));
            s.Join(GoldIcon.transform.DOScale(1f, 0.3f).SetEase(Ease.InBack));
            s.AppendCallback(() =>
            {
                //收尾
                s.Kill();
                Destroy(GoldIcon);
                if (t == a - 1)
                {
                    finish();
                }
            });
        }
    }
    public static void GoldMoveBest(GameObject GoldImage, int a, Transform StartTF, Transform EndTF, System.Action finish)
    {
        GoldMoveBest(GoldImage, a, StartTF.position, EndTF.position, finish);
    }

    /// <summary>
    /// 横向滚动
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="addPosition"></param>
    /// <param name="Finish"></param>
    public static void HorizontalScroll(GameObject obj, float addPosition, System.Action Finish)
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
    public static void WinPanelShow(List<GameObject> WinPanelList)
    {
        float delayTime = 0.5f;
        for (int i = 0; i < WinPanelList.Count; i++)
        {
            GameObject item = WinPanelList[i].gameObject;
            item.transform.localScale = new Vector3(0, 0, 0);
            item.transform.DOScale(1.2f, 0.2f).SetDelay(delayTime).OnComplete(() =>
            {
                item.transform.DOScale(1, 0.2f);
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
    public static void WinStar(int num, List<Transform> StarList, List<Transform> StartTF, List<Transform> EndTF, List<GameObject> StarEffect, Transform WinPanel)
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
            GameObject item = StarList[i].gameObject;
            item.SetActive(true);
            List<Vector3> Endpos = EndTF.ConvertAll(t => t.position);
            item.transform.position = StartTF[i].position;
            GameObject StarFX = StarEffect[i];
            item.transform.eulerAngles = new Vector3(0, 0, 270);
            item.transform.localScale = new Vector3(3, 3, 3);
            item.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
            Vector3 midPos = new Vector3(Endpos[i].x, StartTF[i].position.y, 0);
            Vector3[] PathPos = BezierUtils.GetBeizerList(StartTF[i].position, midPos, Endpos[i], 6);
            //动画开始
            item.GetComponent<UnityEngine.UI.Image>().DOFade(1, 0.3f).SetDelay(delayTime);
            item.transform.DORotate(new Vector3(0, 0, 15 + i * -15), 0.4f, DG.Tweening.RotateMode.FastBeyond360).SetDelay(delayTime);
            item.transform.DOScale(1.3f, 0.5f).SetDelay(delayTime).OnComplete(() =>
            {
                item.transform.DOScale(1, 0.1f).OnComplete(() =>
                {
                    item.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
                    {
                        item.transform.DOScale(1, 0.1f);
                    });
                });
            });
            item.transform.DOPath(PathPos, 0.5f, PathType.CatmullRom).SetDelay(delayTime).OnComplete(() =>
            {
                WinPanel.DOShakePosition(0.3f, 10, 15, 90);
                StarFX.SetActive(true);
            });
            delayTime += 0.3f;
        }
    }
    /// <summary>
    /// 魔法棒特效
    /// </summary>
    /// <param name="Item"></param>
    /// <param name="StartPos"></param>
    /// <param name="GetPos"></param>
    public static void MagicWand(GameObject Item, Vector3 StartPos, Vector3 GetPos)
    {
        //Init动画准备
        Item.transform.position = StartPos;
        GameObject ItemImage = Item.transform.GetChild(1).gameObject;
        ItemImage.transform.localScale = new Vector3(0, 0, 0);
        Item.transform.eulerAngles = new Vector3(0, 0, 0);
        ItemImage.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
        Item.SetActive(true);
        float A = 1;
        if (GetPos.x <= 0)
        {
            A *= -1;
        }
        Vector3 midPos = new Vector3(GetPos.x, StartPos.y, 0);
        Vector3[] PathPos = BezierUtils.GetBeizerList(StartPos, midPos, GetPos, 6);
        //动画开始
        Sequence MagicAni = DOTween.Sequence();
        ItemImage.GetComponent<UnityEngine.UI.Image>().DOFade(1, 0.2f);
        ItemImage.transform.DOScale(1, 0.7f).SetEase(Ease.OutBack);
        Item.transform.localScale = new Vector3(1 * A, 1, 1);
        Item.transform.DORotate(new Vector3(0, 0, 80 * A), 0.5f).SetEase(Ease.InQuad);
        MagicAni.Append(Item.transform.DOPath(PathPos, 0.5f, PathType.CatmullRom));
        MagicAni.Append(Item.transform.DORotate(new Vector3(0, 0, 90 * A), 0.2f));
        MagicAni.Append(Item.transform.DORotate(new Vector3(0, 0, -45 * A), 0.2f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            GameObject ParticleMagic = Instantiate(GamePanel.Instance.StarsEffect, GamePanel.Instance.transform);
            ParticleMagic.transform.position = new Vector3(GetPos.x + 1 * A, GetPos.y, 0);
            ParticleMagic.SetActive(true);
        }));
        MagicAni.Append(ItemImage.GetComponent<UnityEngine.UI.Image>().DOFade(0, 0.4f));
        MagicAni.Append(ItemImage.GetComponent<UnityEngine.UI.Image>().DOFade(0, 0.1f));
        MagicAni.AppendCallback(() =>
        {
            Item.SetActive(false);
        });
    }

    public static void AddScore(Vector3 StartPos)
    {
        GameObject Item = Instantiate(GamePanel.Instance.AddScoreItem, GamePanel.Instance.transform);
        Item.transform.position = StartPos;
        Item.transform.localScale = new Vector3(0, 0, 0);
        Item.GetComponent<CanvasGroup>().alpha = 0;
        Item.SetActive(true);
        Item.transform.DOMoveY(StartPos.y + 0.2f, 0.5f).OnComplete(() =>
        {
            Item.transform.DOMoveY(StartPos.y + 0.4f, 0.3f);
            Item.GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
            {
                Item.SetActive(false);
                Destroy(Item);
            });
        });
        Item.transform.DOScale(1, 0.3f);
        Item.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
    }

}
