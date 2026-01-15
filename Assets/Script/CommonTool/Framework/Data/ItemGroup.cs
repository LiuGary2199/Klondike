using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class ItemGroup : ItemGroupDB
    {
        public ItemGroup()
        {

        }
        public ItemGroup(string item_id, int item_num)
        {
            this.item_id = item_id;
            this.item_num = item_num;
        }

        public Item Item => (Item)ResourceCtrl.Instance.GetType().GetProperty(item_id).GetValue(ResourceCtrl.Instance);

        public string ItemNumStr
        {
            get
            {
                if (item_id.Equals("unlimit_health"))
                {
                    // 无限体力需要转为分钟
                    return (item_num / 60) + "m";
                }
                return item_num.ToString();
            }
        }
    }
}

