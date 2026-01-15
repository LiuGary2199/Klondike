using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class Level
    {
        public Level()
        {
            _data = new LevelData();
        }

        public class LevelData
        {
            public int score;     // 过关得分
            public int startTimes;  // 关卡开始次数
            public int victoryTimes;   // 过关成功次数
        }

        private LevelData _data;
        public LevelData data
        {
            get
            {
                return _data;
            }
        }

        public int Score
        {
            get
            {
                return _data.score;
            }
        }


        public void SetData(JsonData _data)
        {
            if (_data != null)
            {
                this._data = JsonMapper.ToObject<LevelData>(_data.ToJson());
            }
            else
            {
                this._data = new();
            }
        }

        public void AddScore(int num)
        {
            _data.score += num;
            DataManager.Instance.SaveData();
        }

        public void AddStartTimes()
        {
            _data.startTimes++;
            DataManager.Instance.SaveData();
        }

        public void AddVictoryTimes()
        {
            _data.victoryTimes++;
            DataManager.Instance.SaveData();
        }
    }
}