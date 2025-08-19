using com;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Omni
{

    public partial class Omni2DSystem : MonoBehaviour
    {
        public static Omni2DSystem instance;


        public enum GamePhase
        {
            Default,

            Start,//游戏开始，进入电脑界面
            Login,//登录 OMNI 系统
            ReceiveDailyWork,//接受第一轮任务，熟悉系统界面

            //插入支线 日常任务
            Work_Daily1,//外卖
            Work_Daily2,//购物
            Work_Daily3,//监控

            ReceiveSpecialWork,//接收特殊任务包

            //插入支线 特殊任务
            Work_TrackingKiller,//追踪杀人犯
            Work_TrackingCoworker,//追踪前同事
            Work_MythGirl,//神秘任务：女孩监控事件

            UnknownEmail,//邮件事件：收到自己被监控的数据
            SystemCrash,//系统崩溃，画面切换，进入 3D 第一人称恐怖办公空间
            ExploreCompany,//探索公司黑幕，AI 克隆暴露
            Ending_Normal,//回到原点
            Ending_Bad,//主角被改造
        }

        public GamePhase gameStartPhase;

        void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {

            }
        }

        void ToggleCg(CanvasGroup cg, bool on)
        {
            cg.alpha = on ? 1 : 0;
            cg.interactable = on;
            cg.blocksRaycasts = on;
        }

        private void Start()
        {
            switch (gameStartPhase)
            {
                case GamePhase.Default:
                case GamePhase.Start:
                    ToggleCg(cg_office_2d_login, true);
                    StartGameSetup();
                    break;
            }
        }

        IEnumerator DelayAction(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        public GameObject[] 测试用的所有对象;

        public void 改变测试用的所有对象的可见性(bool 可见与否)
        {
            foreach (var g in 测试用的所有对象)
            {
                g.SetActive(可见与否);
            }
        }
    }
}