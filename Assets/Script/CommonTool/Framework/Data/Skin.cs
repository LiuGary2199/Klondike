using LitJson;

namespace zeta_framework
{
    public class Skin : SkinDB
    {
        public class SkinData
        {
            public bool actived;    // 是否正在使用
        }

        public SkinData data;

        public Item item
        {
            get
            {
                return ResourceCtrl.Instance.GetItemById(item_id);
            }

        }

        public bool unlocked
        {
            get
            {
                return item.currentValue > 0;
            }
        }

        public bool actived
        {
            get
            {
                return data.actived;
            }
            private set
            {
                data.actived = value;
            }
        }

        /// <summary>
        /// 读取存档，初始化data
        /// </summary>
        /// <param name="_data"></param>
        public void SetData(JsonData _data)
        {
            if (_data != null)
            {
                data = JsonMapper.ToObject<SkinData>(_data.ToJson());
            }
            else
            {
                data = new SkinData();
            }
        }

        /// <summary>
        /// 使用本皮肤
        /// </summary>
        public void SetActive(bool active)
        {
            actived = active;
        }
    }
}