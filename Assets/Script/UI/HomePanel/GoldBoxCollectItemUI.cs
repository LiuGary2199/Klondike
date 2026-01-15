using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

/// <summary>
/// 金箔活动动画脚本
/// </summary>

public class GoldBoxCollectItemUI : CollectItemUI
{
    public ProgressUI ProgressBar;
    public RectTransform RewardIcon;

    private int progress;
    private int level;

    private ExpBox box;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        box = ExpBoxCtrl.Instance.GetBoxDataById("GoldBox");

        progress = box.currentProgress;
        level = box.currentLv;
        InitProgress();
    }

    public override void CollectAnimationCb()
    {
        ExpBoxDB boxLevelData = box.GetBoxLevelDataByLv(level);
        if (boxLevelData == null)
        {
            gameObject.SetActive(false);
            CollectManager.Instance.Next();
            return;
        }

        // 进度条动画
        if (level == box.currentLv)
        {
            // 当前进度没有完成，不需要发放奖励，仅展示进度条滚动动画
            ProgressBar.RefreshProgress(box.currentProgress, boxLevelData.exp_value, true, () => {
                progress = box.currentProgress;
                CollectManager.Instance.Next();
            });
        }
        else
        {
            // 等级不同，说明当前级别进度已经完成
            // 进度条滚动到最后，然后弹出奖励
            // 当前等级奖励发放后，需要再次执行该回调函数继续执行下一级进度条动画
            ProgressBar.RefreshProgress(boxLevelData.exp_value, boxLevelData.exp_value, true, () => {
                // 优于奖励动画插入接口将动画插入list最前面，所以先插入下一次进度条动画，再插入奖励动画
                // 下一次进度条滚动插入到奖励动画队列中
                CollectItemData data1 = new(item_id, 0, StartPoint, EndPoint, CollectAnimationCb);
                List<CollectItemData> list1 = new()
                {
                    data1
                };
                CollectManager.Instance.InsertSequence(list1);

                // 将奖励插入到奖励动画队列中
                List<CollectItemData> list2 = new();
                if (!string.IsNullOrEmpty(boxLevelData.item_id))
                {
                    CollectItemData data2 = new(boxLevelData.item_id, boxLevelData.item_value, RewardIcon, null, null);
                    list2.Add(data2);
                }
                if (!string.IsNullOrEmpty(boxLevelData.itemgroup_id))
                {
                    foreach(ItemGroup itemgroup in ResourceCtrl.Instance.GetItemGroupById(boxLevelData.itemgroup_id))
                    {
                        CollectItemData data = new(itemgroup.item_id, itemgroup.item_num, RewardIcon, null, null);
                        list2.Add(data);
                    }
                }
                CollectManager.Instance.InsertSequence(list2);

                level++;
                progress = 0;
                InitProgress();

                CollectManager.Instance.Next();
            });
        }
    }

    /// <summary>
    /// 初始化进度条
    /// </summary>
    private void InitProgress()
    {
        ExpBoxDB boxLevelData = box.GetBoxLevelDataByLv(level);
        if (boxLevelData != null)
        {
            ProgressBar.RefreshProgress(progress, boxLevelData.exp_value, false);
            if (!string.IsNullOrEmpty(boxLevelData.itemgroup_id))
            {
                RewardIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Tex/UI/Item/itemgroup");
            }
            else if (!string.IsNullOrEmpty(boxLevelData.item_id))
            {
                RewardIcon.GetComponent<Image>().sprite = ResourceCtrl.Instance.GetItemById(boxLevelData.item_id).Icon;
            }
        }
    }
}