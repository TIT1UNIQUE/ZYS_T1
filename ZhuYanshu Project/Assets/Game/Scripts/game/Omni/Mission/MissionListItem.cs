using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.Omni.Mission
{
    public class MissionListItem : MonoBehaviour
    {
        public MissionData data;

        public TextMeshProUGUI title;
        public Image photoImg;
        public TextMeshProUGUI stateTxt;
        public GameObject toggleOnView;
        public GameObject toggleOffView;

        public void SyncView()
        {
            title.text = data.proto.title;
            photoImg.sprite = data.proto.agentPhoto;
            switch (data.state)
            {
                case MissionData.State.Waiting:
                    stateTxt.text = "";
                    break;
                case MissionData.State.PendingInNotification:
                    stateTxt.text = "";
                    break;
                case MissionData.State.Unread:
                    stateTxt.text = "! New";
                    break;
                case MissionData.State.Read:
                    stateTxt.text = "";
                    break;
                case MissionData.State.Done:
                    stateTxt.text = "√ Completed";
                    break;
            }
        }

        public void ToggleOn()
        {
            toggleOnView.SetActive(true);
            toggleOffView.SetActive(false);
        }

        public void ToggleOff()
        {
            toggleOnView.SetActive(false);
            toggleOffView.SetActive(true);
        }

        public void OnClick()
        {
            if (data.state == MissionData.State.Unread)
                data.state = MissionData.State.Read;

            SyncView();
            MissionListPanel.instance.ToggleOffMissions();
            MissionSystem.instance.ShowMission(data);
            ToggleOn();

        }
    }
}