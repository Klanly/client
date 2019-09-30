using UnityEngine;
using System.Collections;
using MuGame;
using GameFramework;
using WellFired;

public class PlotMain : MonoBehaviour {
    public enum ENUM_POLTSTEP
    {
        PLSP_None = 0, //没有可以播放的剧情
        PLSP_Req_Res = 1, //需要播放剧情了，向dll请求资源
        PLSP_Wait_Res = 2, //等待底层加载资源中...
        PLSP_Playing_Plot = 3, //播放剧情中
    };

    public ENUM_POLTSTEP m_ePlotStep;
    static public PlotMain _inst = null;

    public USSequencer m_curSequence = null;

    public bool m_bUntestPlot = true;
   
    private GameObject m_GamePlotSeq = null;

    void Start () {
        gameST._bUntestPlot = m_bUntestPlot;
        _inst = this;
        gameST.REQ_PLOT_RES = Start_Rev_Plot_Res;
        gameST.REQ_PLAY_PLOT = Start_Rev_Play_Plot;
        gameST.REQ_STOP_PLOT = PlotMain._inst.GamePoltPlayOver;

        UtilsOut.init();

        Debug.Log("剧情模块接入成功");
		
		if( false == m_bUntestPlot ){
			Debug.Log("剧情编辑模式");
         
			InterfaceMgr.getInstance().open(InterfaceMgr.PLOT_LINKUI);
            
		}
	}

    //底层通知上层开启副本剧情开启
    void Start_Rev_Plot_Res(int id)
    {
        //new
        Debug.Log("请求准备需要加载的剧情资源，剧情ID为" + id);

        m_curSequence = null;

        Application.LoadLevelAdditive("p" + id.ToString());
        m_ePlotStep = ENUM_POLTSTEP.PLSP_Req_Res;
    }

    //底层准备完成再通知上层正式开启剧情显示
    void Start_Rev_Play_Plot()
    {
        if (m_ePlotStep == ENUM_POLTSTEP.PLSP_Wait_Res)
        {
            m_ePlotStep = ENUM_POLTSTEP.PLSP_Playing_Plot;

            Debug.Log("开启剧情了.................");
            m_curSequence.Play();
            m_curSequence = null;

            m_GamePlotSeq = GameObject.Find("Sequence");
            if (m_GamePlotSeq == null)
            {
                Debug.Log("剧情资源错误！！！之后会无法释放..............");
                Application.Quit();
            }
            else
            {
                Debug.Log("成功连接到了资源");
            }
        }
    }

    public void GamePoltPlayOver()
    {
        Debug.Log("剧情结束删除不用的Object ");
        if (m_GamePlotSeq == null)
        {
            Debug.Log("资源有错误");
        }
        else
        {
            Debug.Log("成功删除了剧情");
        }

        gameST.REV_ZIMU_TEXT(null);

        GameObject.Destroy(m_GamePlotSeq);
        m_GamePlotSeq = null;

        gameST.REV_PLOT_PLAY_OVER();
    }
}
