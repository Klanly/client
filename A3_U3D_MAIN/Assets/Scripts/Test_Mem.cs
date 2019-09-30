using UnityEngine;
using MuGame;
using System.Collections;

public class Test_Mem : MonoBehaviour
{
    int m_nAudio_Index = 0;
    string[] m_strAudio_Names = { "audio_monster_10014_atk", "audio_common_collect_coin", "audio_monster_10036_dead", "audio_monster_10018_atk", "audio_monster_10034_atk", "audio_monster_10016_atk", "audio_skill_2010", "audio_monster_10008_dead", "audio_monster_10002_atk", "audio_monster_10007_enter1", "audio_monster_10018_dead", "audio_common_trance", "audio_skill_5008", "audio_monster_10014_dead", "audio_monster_10001_dead", "audio_monster_10025_dead", "audio_monster_10015_dead", "audio_monster_10012_sk1", "audio_monster_10023_dead", "audio_monster_10055_dead", "audio_monster_10047_skill_2", "audio_skill_5005", "audio_monster_10041_atk", "audio_monster_10003_fly", "audio_skill_3009", "audio_map_3334", "audio_skill_10000", "audio_monster_10009_atk", "audio_monster_10001_atk", "audio_monster_10040_dead3", "audio_monster_10038_atk", "audio_monster_10003_fire", "audio_skill_2002", "audio_monster_10042_dead", "audio_skill_5001_3", "audio_monster_10012_atk", "audio_monster_10058_dead", "audio_map_3335", "audio_monster_10007_skill01", "audio_monster_10034_dead", "audio_monster_10016_dead", "audio_monster_10007_skill2", "audio_monster_10047_dead", "audio_monster_10040_skill_1_1", "audio_monster_10007_enter2", "audio_monster_10022_dead", "audio_skill_3007", "audio_skill_5002", "audio_monster_10003_atk", "audio_monster_10011_dead", "audio_monster_10045_atk1", "audio_monster_10007_loop", "audio_monster_10058_skill", "audio_monster_10029_dead", "audio_map_2", "audio_monster_10040_skill_1_2", "audio_monster_10056_atk", "audio_monster_10045_dead", "audio_skill_2001_3", "audio_monster_10020_dead", "audio_monster_10011_atk", "audio_monster_10003_dead", "audio_monster_10039_atk1", "audio_monster_10008_atk", "audio_monster_10026_dead", "audio_monster_10004_dead", "audio_monster_10023_atk", "audio_skill_3001_3", "audio_monster_10005_atk", "audio_monster_10024_dead", "audio_monster_10057_atk", "audio_monster_10032_dead", "audio_skill_2008", "audio_common_use_hp", "audio_monster_10010_dead", "audio_monster_10047_enter2", "audio_skill_3004_1", "audio_monster_10056_dead", "audio_skill_3006", "audio_skill_3002", "audio_monster_10039_atk2", "audio_map_4", "audio_common_zhanshi", "audio_common_equip_aex", "audio_monster_10012_dead", "audio_map_11", "audio_monster_10058_atk", "audio_monster_10038_dead", "audio_skill_walk", "audio_monster_10037_skill_1", "audio_monster_10007_atk", "audio_monster_10044_dead", "audio_common_sold_coin", "audio_monster_10026_atk", "audio_monster_10022_atk", "audio_skill_3010", "audio_skill_3004_4", "audio_map_3336", "audio_monster_10004_atk", "audio_map_3333", "audio_monster_10047_enter3", "audio_monster_10024_atk", "audio_skill_2006", "audio_monster_10006_dead", "audio_map_15", "audio_skill_5010", "audio_monster_10032_atk", "audio_common_fashi", "audio_map_10", "audio_common_equip_armour", "audio_skill_3004_3", "audio_skill_3004_2", "audio_skill_3001_2", "audio_monster_10021_atk", "audio_monster_gbn_thw", "audio_skill_5004", "audio_common_assassin_dead", "audio_monster_10029_atk", "audio_monster_10007_dead1", "audio_monster_10054_dead", "audio_monster_10028_atk3", "audio_common_levelup", "audio_monster_10000", "audio_skill_3004_6", "audio_common_equip_staff", "audio_monster_10006_atk", "audio_monster_10043_atk", "audio_common_mage_dead", "audio_monster_10057_dead", "audio_monster_10047_atk", "audio_skill_3001_4", "audio_monster_10003_enter", "audio_skill_3008", "audio_monster_10060_enter", "audio_monster_10017_dead", "audio_monster_10021_dead", "audio_map_3339", "audio_common_star_pic_unlock", "audio_monster_10040_dead2", "audio_monster_10005_dead", "audio_monster_10033_dead", "audio_monster_10044_atk", "audio_skill_3001", "audio_monster_10041_dead", "audio_monster_10039_dead", "audio_monster_10007_dead2", "audio_monster_10010_atk", "audio_monster_10037_atk", "audio_monster_10033_atk", "audio_map_3340", "audio_monster_10040_skill_2_1", "audio_monster_10040_skill_2_2", "audio_map_3", "audio_skill_5006", "audio_monster_10007_skill1", "audio_monster_10017_atk", "audio_monster_10043_dead", "audio_skill_3001_1", "audio_common_cike", "audio_common_click_button", "audio_map_7", "audio_eviryment_amb_circus_gate", "audio_monster_10028_atk2", "audio_skill_2005", "audio_monster_10036_atk", "audio_map_music_0", "audio_skill_2003", "audio_skill_2001_2", "audio_monster_10040_atk", "audio_monster_10052_atk", "audio_monster_10040_dead1", "audio_skill_3003", "audio_monster_10052_dead", "audio_monster_10015_atk", "audio_common_equip_dagger", "audio_skill_2001_1", "audio_monster_10045_atk3", "audio_skill_5009", "audio_common_warrior_dead", "audio_monster_10028_dead", "audio_map_14", "audio_monster_10034_skill1", "audio_monster_10002_dead", "audio_monster_10055_atk", "audio_monster_10045_atk2", "audio_skill_wing", "audio_skill_5001_1", "audio_monster_10053_atk", "audio_monster_10019_atk", "audio_monster_10007_skill02", "audio_monster_10042_atk", "audio_skill_5007", "audio_monster_10007_dead3", "audio_monster_10040_skill_2_3", "audio_map_1", "audio_skill_2007", "audio_monster_10025_atk", "audio_skill_5003", "audio_monster_10028_atk1", "audio_monster_10047_skill_1", "audio_eviryment_yzdl", "audio_skill_3004_5", "audio_map_8", "audio_monster_10054_atk", "audio_common_open_interface", "audio_skill_5001_2", "audio_monster_10019_dead", "audio_monster_10047_enter1", "audio_monster_10001_born1", "audio_monster_10053_dead", "audio_skill_2004", "audio_monster_10060_dead", "audio_skill_3005", "audio_monster_10037_dead", "audio_skill_2009" };

    int m_nUI_Index = 0;
    string[] m_strUI_Names;

    int m_nClearNextFrame = 0;

    void Start()
    {
        InterfaceMgr.getInstance();
        SimpleFramework.Manager.ResourceManager._inst.Initialize();

        InterfaceMgr.LINK_RUN_CS = false;
        m_strUI_Names = InterfaceMgr.getInstance().m_strUI_winNames;
    }

    void Update()
    {
        float fdt = Time.deltaTime;
        InterfaceMgr.getInstance().FrameMove(fdt);

        ++m_nClearNextFrame;
        if (m_nClearNextFrame > 60*60)
        {
            m_nClearNextFrame = 0;
            Clear_GC();
        }
    }

    public void PlaySound()
    {
        if (m_nAudio_Index >= m_strAudio_Names.Length) m_nAudio_Index = 0;
        MediaClient.instance.PlaySoundUrl(m_strAudio_Names[m_nAudio_Index], false, null);
        ++m_nAudio_Index;
    }

    public void OpenUI()
    {
        if (m_nUI_Index >= m_strUI_Names.Length) m_nUI_Index = 0;
        InterfaceMgr.getInstance().ui_async_open(m_strUI_Names[m_nUI_Index]);
        ++m_nUI_Index;

        Debug.Log("m_nUI_Index=" + m_nUI_Index);
    }

    public void ClearMem()
    {
        m_nClearNextFrame = 60 * 60;
        //一定要在释放了，后点UI再释放，才能做到完全的释放
        //测试了UI的Mesh重新构建一个延时的过程，现在是点2次UI才能清理干净，所以延迟了180帧测试
        //如果马上GC ， UI的变化常常会带来资源释放的不干净，所以放在别处更能看出问题
    }

    private void Clear_GC()
    {
        for (int i = 0; i < m_strUI_Names.Length; ++i)
        {
            InterfaceMgr.getInstance().DisposeUI(m_strUI_Names[i]);
        }

        MediaClient.instance.StopSounds(); //清理所有的音效缓存

        //因为资源有2次的缓存策略，所以要销毁2次
        GAMEAPI.ClearAllOneAsset();
        GAMEAPI.ClearAllOneAsset();

        Resources.UnloadUnusedAssets();
        System.GC.Collect(0, System.GCCollectionMode.Forced);
    }
}
