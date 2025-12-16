using Assets.Game.Scripts.game.VIC.ui.Message;
using Assets.Game.Scripts.game.VIC.ui.Misc;
using Assets.Game.Scripts.game.VIC.ui.Product;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC
{
    public class VicDebugger : MonoBehaviour
    {
        public void OnClkSmallBtn1()
        {

        }

        public void OnClkSmallBtn2()
        {

        }
        public void OnClkSmallBtn3()
        {

        }
        public void OnClkBigBtn1()
        {

        }
        public void OnClkBigBtn2()
        {

        }

        public void OnPressKey_1()
        {
            Debug.LogWarning("OnPressKey_1");
            Debug.Log("!-> show job done popup add income");
            //TODO
            ProductSystem.instance.ShowJobDonePopup();
        }


        public void OnPressKey_2()
        {
            Debug.LogWarning("OnPressKey_2");
            Debug.Log("!-> plug in/out arduino");
            ProductSystem.instance.ToggleArduinoDevice();
        }

        public void OnPressKey_3()
        {
            Debug.LogWarning("OnPressKey_3");
            Debug.Log("!-> add some boss's message runtime");

            MessageSystem.instance.InsertBossRandomMessage();
        }

        public void OnPressKey_4()
        {
            Debug.LogWarning("OnPressKey_4");
            Debug.Log("!-> showpopup upgrade vip & refill token");
            ProductSystem.instance.ShowVipUpPopup();
        }

        public void OnPressKey_5()
        {
            Debug.LogWarning("OnPressKey_5");
            Debug.Log("!-> reset vip to lv 1 & refill token");
            ProductSystem.instance.SetVipLevel(1);
        }

        public void OnPressKey_6()
        {
            Debug.LogWarning("OnPressKey_6");
            Debug.Log("!-> consume some token");
            // 
            ProductSystem.instance.PayTokens();
        }

        public void OnPressKey_7()
        {
            Debug.LogWarning("OnPressKey_7");
            Debug.Log("!-> re-allocate income & spendings");
            ProductSystem.instance.personalInfoFrame.InitIncomeAndSpending();
            //
        }

        public void OnPressKey_8()
        {
            Debug.LogWarning("OnPressKey_8");
            Debug.Log("!-> add spending");
            //
            ProductSystem.instance.AddSomeSpending();
        }

        public void OnPressKey_9()
        {
            Debug.LogWarning("OnPressKey_9");
            Debug.Log("!-> pop AD");
            //
            InterlinkAdSystem.instance.PopupAd();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnPressKey_1();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnPressKey_2();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                OnPressKey_3();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                OnPressKey_4();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                OnPressKey_5();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                OnPressKey_6();
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                OnPressKey_7();
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                OnPressKey_8();
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                OnPressKey_9();
            }
        }
    }
}