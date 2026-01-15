using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class Dodge
    {
        public Dodge()
        {
            _Mark = new LevelData();
        }

        public class LevelData
        {
            public int Today;     // 过关得分
            public int AheadBrook;  // 关卡开始次数
            public int victoryBrook;   // 过关成功次数
        }

        private LevelData _Mark;
        public LevelData Mark        {
            get
            {
                return _Mark;
            }
        }

        public int Dense        {
            get
            {
                return _Mark.Today;
            }
        }


        public void CudLine(JsonData _data)
        {
            if (_data != null)
            {
                this._Mark = JsonMapper.ToObject<LevelData>(_data.ToJson());
            }
            else
            {
                this._Mark = new();
            }
        }

        public void PegDense(int num)
        {
            _Mark.Today += num;
            LineRancher.Instance.ShedLine();
        }

        public void PegBadgeBrook()
        {
            _Mark.AheadBrook++;
            LineRancher.Instance.ShedLine();
        }

        public void PegUntwistBrook()
        {
            _Mark.victoryBrook++;
            LineRancher.Instance.ShedLine();
        }
    }
}