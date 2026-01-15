using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoSingleton<GameDataManager>
{
    public void InitGameData()
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
    public string HandleJsonData(string jsonData)
    {
        jsonData = jsonData.Replace("\"type\": \"gold\"", "\"type\":0"); //gold转换为0
        jsonData = jsonData.Replace("\"type\": \"cash\"", "\"type\":1"); //cash转换为1
        jsonData = jsonData.Replace("\"type\": \"tip\"", "\"type\":2"); //tip转换为2
        jsonData = jsonData.Replace("\"type\": \"undo\"", "\"type\":3"); //undo转换为3
        jsonData = jsonData.Replace("\"type\": \"amazon\"", "\"type\":4"); //amazon转换为4
        return jsonData;
    }

    // 金币
    public double GetGold()
    {
        return (int)SaveDataManager.GetDouble(CConfig.sv_GoldCoin);
    }

    public void AddGold(double gold)
    {
        AddGold(gold, MainManager.instance.transform);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_Coin);
        MusicMgr.GetInstance().PlayVibrate(Lofelt.NiceVibrations.HapticPatterns.PresetType.HeavyImpact);
    }

    public void AddGold(double gold, Transform startTransform)
    {
        double oldGold = SaveDataManager.GetDouble(CConfig.sv_GoldCoin);
        SaveDataManager.SetDouble(CConfig.sv_GoldCoin, oldGold + gold);
        if (gold > 0)
        {
            SaveDataManager.SetDouble(CConfig.sv_CumulativeGoldCoin, SaveDataManager.GetDouble(CConfig.sv_CumulativeGoldCoin) + gold);
        }
        MessageData md = new MessageData(oldGold);
        md.valueTransform = startTransform;
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ui_addgold, md);
    }

    // 现金
    public double GetCash()
    {
        return System.Math.Round(SaveDataManager.GetDouble(CConfig.sv_Token), 2);
    }

    public void AddCash(double token)
    {
        AddCash(token, MainManager.instance.transform);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_Diamond);
        MusicMgr.GetInstance().PlayVibrate(Lofelt.NiceVibrations.HapticPatterns.PresetType.HeavyImpact);
    }
    public void AddCash(double token, Transform startTransform)
    {
        double oldToken = PlayerPrefs.HasKey(CConfig.sv_Token) ? double.Parse(SaveDataManager.GetString(CConfig.sv_Token)) : 0;
        double newToken = System.Math.Round(oldToken + token, 2);
        SaveDataManager.SetDouble(CConfig.sv_Token, newToken);
        if (token > 0)
        {
            double allToken = SaveDataManager.GetDouble(CConfig.sv_CumulativeToken);
            SaveDataManager.SetDouble(CConfig.sv_CumulativeToken, allToken + token);
        }
#if SOHOShop
        SOHOShopManager.instance.UpdateCash();
#endif
        MessageData md = new MessageData(oldToken);
        md.valueTransform = startTransform;
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ui_addtoken, md);
    }

    //Amazon卡
    public double GetAmazon()
    {
        return SaveDataManager.GetDouble(CConfig.sv_Amazon);
    }

    public void AddAmazon(double amazon)
    {
        AddAmazon(amazon, MainManager.instance.transform);
    }
    public void AddAmazon(double amazon, Transform startTransform)
    {
        double oldAmazon = PlayerPrefs.HasKey(CConfig.sv_Amazon) ? double.Parse(SaveDataManager.GetString(CConfig.sv_Amazon)) : 0;
        double newAmazon = oldAmazon + amazon;
        SaveDataManager.SetDouble(CConfig.sv_Amazon, newAmazon);
        if (amazon > 0)
        {
            double allAmazon = SaveDataManager.GetDouble(CConfig.sv_CumulativeAmazon);
            SaveDataManager.SetDouble(CConfig.sv_CumulativeAmazon, allAmazon + amazon);
        }
        MessageData md = new MessageData(oldAmazon);
        md.valueTransform = startTransform;
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ui_addamazon, md);
    }

    //积分
    public int GetScore()
    {
        return SaveDataManager.GetInt("Score");
    }

    public void AddScore(int score)
    {
        SaveDataManager.SetInt("Score", GetScore() + score);
    }

    public void SetScore(int score)
    {
        SaveDataManager.SetInt("Score", score);
    }

    //道具
    public int GetProp_Tip()
    {
        return SaveDataManager.GetInt("Prop_Tip");
    }
    public void AddProp_Tip(int prop_tip)
    {
        SaveDataManager.SetInt("Prop_Tip", GetProp_Tip() + prop_tip);
    }
    public int GetProp_Undo()
    {
        return SaveDataManager.GetInt("Prop_Undo");
    }
    public void AddProp_Undo(int prop_undo)
    {
        SaveDataManager.SetInt("Prop_Undo", GetProp_Undo() + prop_undo);
    }

    //根据权重获取奖励数据
    public RewardData GetRewardDataByWeight(RewardData[] rewardDatas)
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
                if (CommonUtil.IsApple() && item.type == RewardType.Cash)
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
    public RewardData GetRewardDataByWeightAndRange(List<RewardData> rewardDatas)
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
                if (CommonUtil.IsApple() && item.type == RewardType.Cash)
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

    //根据权重获取触发的小游戏类型
    public string GetSpecialGameTypeByWeight(List<SpecialGameMulti> specialGameMultis)
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
