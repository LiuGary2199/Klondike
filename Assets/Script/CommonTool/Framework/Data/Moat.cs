using LitJson;

namespace zeta_framework
{
    public class Moat : SkinDB
    {
        public class SkinData
        {
            public bool Speaker;    // 是否正在使用
        }

        public SkinData Mark;

        public Afar Seal        {
            get
            {
                return OverheadPeak.Instance.FarAfarSoAt(Seal_To);
            }

        }

        public bool Plethora        {
            get
            {
                return Seal.MortiseDodge > 0;
            }
        }

        public bool Speaker        {
            get
            {
                return Mark.Speaker;
            }
            private set
            {
                Mark.Speaker = value;
            }
        }

        /// <summary>
        /// 读取存档，初始化data
        /// </summary>
        /// <param name="_data"></param>
        public void CudLine(JsonData _data)
        {
            if (_data != null)
            {
                Mark = JsonMapper.ToObject<SkinData>(_data.ToJson());
            }
            else
            {
                Mark = new SkinData();
            }
        }

        /// <summary>
        /// 使用本皮肤
        /// </summary>
        public void CudRefute(bool active)
        {
            Speaker = active;
        }
    }
}