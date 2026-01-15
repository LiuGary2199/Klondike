using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class AfarPerry : ItemGroupDB
    {
        public AfarPerry()
        {

        }
        public AfarPerry(string item_id, int item_num)
        {
            this.Seal_To = item_id;
            this.Seal_Toy = item_num;
        }

        public Afar Afar=> (Afar)OverheadPeak.Instance.GetType().GetProperty(Seal_To).GetValue(OverheadPeak.Instance);

        public string AfarLadSow        {
            get
            {
                if (Seal_To.Equals("unlimit_health"))
                {
                    // 无限体力需要转为分钟
                    return (Seal_Toy / 60) + "m";
                }
                return Seal_Toy.ToString();
            }
        }
    }
}

