using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.game.Omni.Mission
{
    public class MissionListPanel : MonoBehaviour
    {
        public static MissionListPanel instance;

        public List<MissionListItem> missionItems = new List<MissionListItem>();

        public MissionListItem prefab;//预制体

        public MissionNotification notification;

        public TextMeshProUGUI txt_completedMissionsCount;

        private void Awake()
        {
            instance = this;
            txt_completedMissionsCount.text = "0";
        }

        public void ToggleOnMission(int index)
        {
            var m = GetCurrentMission(index);
            if (m != null)
            {
                m.ToggleOn();
            }
        }

        public void ToggleOffMissions()
        {
            foreach (var m in missionItems)
            {
                m.ToggleOff();
            }
        }

        public void ToggleOffMission(int index)
        {
            var m = GetCurrentMission(index);
            if (m != null)
            {
                m.ToggleOff();
            }
        }

        public void RemoveMission(int index)
        {
            var m = GetCurrentMission(index);
            if (m != null)
            {
                missionItems.Remove(m);
                Destroy(m.gameObject);
            }
        }

        public void AddMission(MissionData data)
        {
            var newMissionItem = Instantiate(prefab, prefab.transform.parent);
            newMissionItem.data = data;
            newMissionItem.gameObject.SetActive(true);
            newMissionItem.SyncView();
            newMissionItem.ToggleOff();
            missionItems.Add(newMissionItem);
        }

        public void RefreshAllMissionItems()
        {
            foreach (var mi in missionItems)
            {
                mi.SyncView();
            }
        }

        public void RefreshCompletedMissionsCount()
        {
            txt_completedMissionsCount.text = "" + MissionSystem.instance.GetCompletedMissionsCount();
        }

        MissionListItem GetCurrentMission(int index)
        {
            if (index < 0 || index >= missionItems.Count)
            {
                return null;
            }
            return missionItems[index];
        }
    }
}