using System.Collections;
using UnityEngine;
using Assets.Game.Scripts.game.VIC.ui.Product;

namespace Assets.Game.Scripts.game.VIC
{
    public class VicDebugger : MonoBehaviour
    {
        public void OnClkSmallBtn1()
        {
            //add vip lv to 3 & showpopup & refill token
            ProductSystem.instance.ShowVipUpPopup();
        }
        public void OnClkSmallBtn2()
        {
            //reset vip to lv 1 & refill token
            ProductSystem.instance.SetVipLevel(1);
        }
        public void OnClkSmallBtn3()
        {
            // consume some token
            ProductSystem.instance.PayTokens();
        }
        public void OnClkBigBtn1()
        {
            ProductSystem.instance.personalInfoFrame.InitIncomeAndSpending();
            //re-allocate income & spendings
        }
        public void OnClkBigBtn2()
        {
            //job done popup
            //  com.SoundSystem.instance.Play("pay");
        }

        public void OnPressKey_1()
        {
            //plugin arduino
        }

        public void OnPressKey_2()
        {
            //plugout arduino
        }

        public void OnPressKey_3()
        {
            //add some message runtime
        }
    }
}