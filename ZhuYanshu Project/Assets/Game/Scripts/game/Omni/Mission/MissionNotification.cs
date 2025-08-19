using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.game.Omni.Mission
{
    public class MissionNotification : MonoBehaviour
    {
        public MissionPackagePrototype crtMissionPackage;
        public GameObject view;

        public TextMeshProUGUI title;
        public TextMeshProUGUI agentNameTxt;

        private bool _isShowing;

        private void Awake()
        {
            _isShowing = true;
            Hide();
        }

        public void OnClickChecked()
        {
            Debug.Log("OnClickChecked");
            //点击勾 添加这个新任务
            MissionSystem.instance.AddMissionByPackage(crtMissionPackage);
            crtMissionPackage = null;
            Hide();
        }

        public void Show(MissionPackagePrototype mpp)
        {
            if (_isShowing)
            {
                return;
            }

            crtMissionPackage = mpp;
            title.text = "\"" + mpp.title + "\"";
            agentNameTxt.text = mpp.senderName;
            _isShowing = true;
            view.SetActive(true);
        }

        public void Hide()
        {
            if (!_isShowing)
            {
                return;
            }

            _isShowing = false;
            view.SetActive(false);
        }
    }
}