using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class ItemGroupCtrl
    {
        public static ItemGroupCtrl Instance;

        public Dictionary<string, List<ItemGroup>> itemGroups;

        public ItemGroupCtrl(JsonData setting)
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Dictionary<string, List<ItemGroup>> itemGroups = new Dictionary<string, List<ItemGroup>>();
            List<ItemGroup> itemGroupList = JsonMapper.ToObject<List<ItemGroup>>(setting.ToJson());
            foreach (ItemGroup itemGroup in itemGroupList)
            {
                if (!itemGroups.ContainsKey(itemGroup.id))
                {
                    itemGroups.Add(itemGroup.id, new List<ItemGroup>());
                }
                itemGroups[itemGroup.id].Add(itemGroup);
            }
            this.itemGroups = itemGroups;
        }

    }
}
