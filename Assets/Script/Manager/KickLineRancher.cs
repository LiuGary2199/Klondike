using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickLineRancher : TiltDigestion<KickLineRancher>
{
    public void DeafKickLine()
    {
#if SOHOShop
        // 提现商店初始化
        // 提现商店中的金币、现金和amazon卡均为double类型，参数请根据具体项目自行处理
        SOHOShopManager.instance.InitSOHOShopAction(
            getToken,
            getGold, 
            getAmazon,    // amazon
            (subToken) => { addToken(-subToken); }, 
            (subGold) => { addGold(-subGold); }, 
            (subAmazon) => { addAmazon(-subAmazon); });
#endif
    }

    //处理json数据中，枚举和字符串转换问题
    public string CrayonAmidLine(string jsonData)
    {
        jsonData = jsonData.Replace("\"type\": \"gold\"", "\"type\":0"); //gold转换为0
        jsonData = jsonData.Replace("\"type\": \"cash\"", "\"type\":1"); //cash转换为1
        jsonData = jsonData.Replace("\"type\": \"tip\"", "\"type\":2"); //tip转换为2
        jsonData = jsonData.Replace("\"type\": \"undo\"", "\"type\":3"); //undo转换为3
        jsonData = jsonData.Replace("\"type\": \"amazon\"", "\"type\":4"); //amazon转换为4
        return jsonData;
    }

    // 金币
    public double FarCast()
    {
        return (int)ShedLineRancher.FarEmbryo(CAdjoin.Or_CastSlit);
    }

    public void PegCast(double gold)
    {
        PegCast(gold, LimyRancher.instance.transform);
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_Coin);
        LeafyLeg.FarBefriend().HaulBalance(Lofelt.NiceVibrations.HapticPatterns.PresetType.HeavyImpact);
    }

    public void PegCast(double gold, Transform startTransform)
    {
        double oldGold = ShedLineRancher.FarEmbryo(CAdjoin.Or_CastSlit);
        ShedLineRancher.CudEmbryo(CAdjoin.Or_CastSlit, oldGold + gold);
        if (gold > 0)
        {
            ShedLineRancher.CudEmbryo(CAdjoin.Or_GenerationCastSlit, ShedLineRancher.FarEmbryo(CAdjoin.Or_GenerationCastSlit) + gold);
        }
        AfricanLine md = new AfricanLine(oldGold);
        md.NobleSkeptical = startTransform;
        AfricanExtendHorse.FarBefriend().Leaf(CAdjoin.We_Me_Miocene, md);
    }

    // 现金
    public double FarNote()
    {
        return CashOutManager.FarBefriend().Money;
        //return System.Math.Round(ShedLineRancher.GetDouble(CAdjoin.sv_Token), 2);
    }

    public void PegNote(double cash)
    {
        PegNote(cash, LimyRancher.instance.transform);
        CashOutManager.FarBefriend().AddMoney((float)cash);
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_Diamond);
        LeafyLeg.FarBefriend().HaulBalance(Lofelt.NiceVibrations.HapticPatterns.PresetType.HeavyImpact);
    }
    public void PegNote(double cashNum, Transform startTransform)
    {
        double oldToken = PlayerPrefs.HasKey(CAdjoin.Or_Goods) ? double.Parse(ShedLineRancher.FarStench(CAdjoin.Or_Goods)) : 0;
        double newToken = System.Math.Round(oldToken + cashNum, 2);
        ShedLineRancher.CudEmbryo(CAdjoin.Or_Goods, newToken);
        if (cashNum > 0)
        {
            double allToken = ShedLineRancher.FarEmbryo(CAdjoin.Or_GenerationGoods);
            ShedLineRancher.CudEmbryo(CAdjoin.Or_GenerationGoods, allToken + cashNum);
        }
#if SOHOShop
        SOHOShopManager.instance.UpdateCash();
#endif
        AfricanLine md = new AfricanLine(oldToken);
        md.NobleSkeptical = startTransform;
        AfricanExtendHorse.FarBefriend().Leaf(CAdjoin.We_Me_Tapestry, md);
    }

    //Amazon卡
    public double FarEntity()
    {
        return ShedLineRancher.FarEmbryo(CAdjoin.Or_Entity);
    }

    public void PegEntity(double amazon)
    {
        PegEntity(amazon, LimyRancher.instance.transform);
    }
    public void PegEntity(double amazon, Transform startTransform)
    {
        double oldAmazon = PlayerPrefs.HasKey(CAdjoin.Or_Entity) ? double.Parse(ShedLineRancher.FarStench(CAdjoin.Or_Entity)) : 0;
        double newAmazon = oldAmazon + amazon;
        ShedLineRancher.CudEmbryo(CAdjoin.Or_Entity, newAmazon);
        if (amazon > 0)
        {
            double allAmazon = ShedLineRancher.FarEmbryo(CAdjoin.Or_GenerationEntity);
            ShedLineRancher.CudEmbryo(CAdjoin.Or_GenerationEntity, allAmazon + amazon);
        }
        AfricanLine md = new AfricanLine(oldAmazon);
        md.NobleSkeptical = startTransform;
        AfricanExtendHorse.FarBefriend().Leaf(CAdjoin.We_Me_Partition, md);
    }

    //积分
    public int FarDense()
    {
        return ShedLineRancher.FarWit("Score");
    }

    public void PegDense(int score)
    {
        ShedLineRancher.CudWit("Score", FarDense() + score);
    }

    public void CudDense(int score)
    {
        ShedLineRancher.CudWit("Score", score);
    }

    //道具
    public int FarGenu_May()
    {
        return ShedLineRancher.FarWit("Prop_Tip");
    }
    public void PegGenu_May(int prop_tip)
    {
        ShedLineRancher.CudWit("Prop_Tip", FarGenu_May() + prop_tip);
    }
    public int FarGenu_Note()
    {
        return ShedLineRancher.FarWit("Prop_Undo");
    }
    public void PegGenu_Note(int prop_undo)
    {
        ShedLineRancher.CudWit("Prop_Undo", FarGenu_Note() + prop_undo);
    }

    //根据权重获取奖励数据
    public RewardData FarStarveLineSoRevert(RewardData[] rewardDatas)
    {
        int totalWeight = 0;
        foreach (RewardData item in rewardDatas)
        {
            totalWeight += item.weight;
        }
        float randomWeight = Random.Range(0f, totalWeight);
        float currentWeight = 0;
        foreach (RewardData item in rewardDatas)
        {
            currentWeight += item.weight;
            if (randomWeight <= currentWeight)
            {
                if (UnfoldGate.UpMound() && item.type == RewardType.Cash)
                    item.type = RewardType.Gold;

                if (item.type == RewardType.Gold)
                    item.num *= (float)GameUtil.GetGoldMulti();
                else if (item.type == RewardType.Cash)
                    item.num *= (float)GameUtil.GetCashMulti();
                return item;
            }
        }
        return null;
    }

    //根据权重和范围获取奖励数据
    public RewardData FarStarveLineSoRevertBusDelay(List<RewardData> rewardDatas)
    {
        int totalWeight = 0;
        foreach (RewardData item in rewardDatas)
            totalWeight += item.weight;

        float randomWeight = Random.Range(0f, totalWeight);
        float currentWeight = 0;
        foreach (RewardData item in rewardDatas)
        {
            currentWeight += item.weight;
            if (randomWeight <= currentWeight)
            {
                if (UnfoldGate.UpMound() && item.type == RewardType.Cash)
                    item.type = RewardType.Gold;

                if (item.type == RewardType.Gold)
                {
                    item.num = Random.Range(item.numMin, item.numMax);
                    item.num *= (float)GameUtil.GetGoldMulti();
                }
                else if (item.type == RewardType.Cash)
                {
                    item.num = (float)System.Math.Round(Random.Range((float)item.numMin, (float)item.numMax), 2);
                    item.num *= (float)GameUtil.GetCashMulti();
                    item.num = (float)System.Math.Round(item.num, 2);
                }
                else if (item.type == RewardType.Tip)
                    item.num = Random.Range(item.numMin, item.numMax);
                else if (item.type == RewardType.Undo)
                    item.num = Random.Range(item.numMin, item.numMax);
                return item;
            }
        }
        return null;
    }

    //根据权重获取奖励索引(转盘用)
    public int FarDodgeSoRevert(List<SlotItem> Items)
    {
        int totalWeight = 0;
        foreach (SlotItem item in Items)
            totalWeight += item.weight;
        float randomWeight = Random.Range(0f, totalWeight);
        float currentWeight = 0;
        int index = 0;
        foreach (SlotItem item in Items)
        {
            currentWeight += item.weight;
            if (randomWeight <= currentWeight)
                return index;
            index++;
        }
        return -1;
    }

    //根据权重获取触发的小游戏类型
    public string FarBrieflyKickJokeSoRevert(List<SpecialGameMulti> specialGameMultis)
    {
        int totalWeight = 0;
        foreach (SpecialGameMulti item in specialGameMultis)
            totalWeight += item.weight;
        float randomWeight = Random.Range(0f, totalWeight);
        float currentWeight = 0;
        foreach (SpecialGameMulti item in specialGameMultis)
        {
            currentWeight += item.weight;
            if (randomWeight <= currentWeight)
                return item.name;
        }
        return "Null";
    }
}
