using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡管理
/// </summary>
namespace zeta_framework
{
    public class LevelCtrl : ICtrl
    {
        public static LevelCtrl Instance;

        private Dictionary<string, Level> levelDic;

        private int currentLevelIndex;   // 当前关卡序号，从0开始
        public int maxLevelIndex;       // 最大过关数（主线程关卡进度）

        private int healthCost;    // 记录一下开始当前关卡消耗的体力，如果开始时是无限体力状态

        public LevelCtrl()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            levelDic = new();
            currentLevelIndex = 0;
            maxLevelIndex = 0;
        }

        /// <summary>
        /// 初始化存档数据
        /// </summary>
        /// <param name="data"></param>
        public void Init(JsonData data)
        {
            // 当前关卡存档
            if (data != null && data.ContainsKey("maxLevelIndex"))
            {
                maxLevelIndex = int.Parse(data["maxLevelIndex"].ToString());
            }

            if (data != null && data.ContainsKey("levels"))
            {
                JsonData levelData = data["levels"];
                foreach(string key in levelData.Keys)
                {
                    Level level = new();
                    level.SetData(levelData[key]);
                    levelDic.Add(key, level);
                }
            }
        }

        /// <summary>
        /// 序列化需要存档的数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetSerializeData()
        {
            Dictionary<string, object> data = new();
            data.Add("maxLevelIndex", maxLevelIndex);
            Dictionary<string, object> levelData = new();
            foreach(string key in levelDic.Keys)
            {
                levelData.Add(key, levelDic[key].data);
            }
            data.Add("levels", levelData);
            return data;
        }

        /// <summary>
        /// 开始关卡
        /// </summary>
        /// <param name="levelIndex">如果参数传-1，表示为主线关卡</param>
        public ErrorCode StartLevel(int levelIndex = -1)
        {
            if (!HealthCtrl.Instance.IsHealthEnough(GameSettingCtrl.Instance.health_cost))
            {
                return ErrorCode.HealthNotEnough;
            }
            if (levelIndex == -1)
            {
                // 主进程
                currentLevelIndex = maxLevelIndex;
            }
            else
            {
                currentLevelIndex = levelIndex;
            }

            if (maxLevelIndex < levelIndex)
            {
                maxLevelIndex = levelIndex;
            }

            // 扣除体力
            if (HealthCtrl.Instance.IsUnlimitedState())
            {
                // 无限体力状态，不扣除体力
                healthCost = 0;
            }
            else
            {
                HealthCtrl.Instance.UseHealth(GameSettingCtrl.Instance.health_cost);
            }
            
            // 关卡增加一次开始次数
            if (!levelDic.ContainsKey(currentLevelIndex.ToString()))
            {
                levelDic.Add(currentLevelIndex.ToString(), new Level());
            }
            levelDic[currentLevelIndex.ToString()].AddStartTimes();

            return ErrorCode.Success;
        }

        /// <summary>
        /// 过关成功
        /// </summary>
        public virtual void LevelVictory()
        {
            if (currentLevelIndex == maxLevelIndex)
            {
                // 主线进程，自动增加一点经验值
                maxLevelIndex++;
                MessageCenterLogic.GetInstance().Send(CConfig.mg_LevelMaxLevelChange);
                ResourceCtrl.Instance.AddItemValue(ResourceCtrl.Instance.exp, 1);
                // 增加连胜值
                ResourceCtrl.Instance.AddItemValue(ResourceCtrl.Instance.consecutive_wins, 1);
            }
            // 恢复体力
            HealthCtrl.Instance.AddHealth(healthCost);
            // 关卡增加一次过关成功次数
            levelDic[currentLevelIndex.ToString()].AddVictoryTimes();
            // 存档
            DataManager.Instance.SaveData();
        }

        /// <summary>
        /// 过关失败
        /// </summary>
        public virtual void LevelFail()
        {
            // 连胜数值清零
            ResourceCtrl.Instance.SetItemValue(ResourceCtrl.Instance.consecutive_wins, 0);
        }
    }
}
