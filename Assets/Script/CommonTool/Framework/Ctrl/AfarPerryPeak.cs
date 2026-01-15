using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class AfarPerryPeak
    {
        public static AfarPerryPeak Instance;

        public Dictionary<string, List<AfarPerry>> SealModern;

        public AfarPerryPeak(JsonData setting)
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Dictionary<string, List<AfarPerry>> SealModern= new Dictionary<string, List<AfarPerry>>();
            List<AfarPerry> itemGroupList = JsonMapper.ToObject<List<AfarPerry>>(setting.ToJson());
            foreach (AfarPerry itemGroup in itemGroupList)
            {
                if (!SealModern.ContainsKey(itemGroup.id))
                {
                    SealModern.Add(itemGroup.id, new List<AfarPerry>());
                }
                SealModern[itemGroup.id].Add(itemGroup);
            }
            this.SealModern = SealModern;
        }

    }
}
