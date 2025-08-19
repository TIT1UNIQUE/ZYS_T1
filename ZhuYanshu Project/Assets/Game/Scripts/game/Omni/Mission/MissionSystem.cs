using Assets.Game.Scripts.game.Omni.Mission.TextInfo;
using System.Collections;
using System.Collections.Generic;
using Assets.Game.Scripts.game.Omni.Mission.VideoRecord;
using UnityEngine;

namespace Assets.Game.Scripts.game.Omni.Mission
{
    public class MissionSystem : MonoBehaviour
    {
        public static MissionSystem instance;

        public MissionPackagePrototype[] tests;

        public List<MissionData> missions = new List<MissionData>();

        public TextInfoMissionSystem textInfoMissionSystem;
        public VoiceRecordMissionSystem voiceRecordMissionSystem;

        private void Awake()
        {
            instance = this;
        }

        void Add(MissionPackagePrototype mpp)
        {
            MissionListPanel.instance.notification.Show(mpp);
        }

        public void AddMissionByPackage(MissionPackagePrototype mpp)
        {
            foreach (var p in mpp.protos)
            {
                var newMission = new MissionData();
                newMission.proto = p;
                newMission.state = MissionData.State.Unread;
                newMission.missionPackageId = mpp.id;
                missions.Add(newMission);
                MissionListPanel.instance.AddMission(newMission);
            }
        }

        public int GetCompletedMissionsCount()
        {
            int result = 0;
            foreach (var m in missions)
            {
                if (m.state == MissionData.State.Done)
                {
                    result += 1;
                }
            }
            return result;
        }

        public void Complete(MissionData missionToComplete)
        {
            foreach (var m in missions)
            {
                if (m == missionToComplete)
                {
                    m.state = MissionData.State.Done;
                    MissionListPanel.instance.RefreshAllMissionItems();
                }
            }
            MissionListPanel.instance.RefreshCompletedMissionsCount();
        }

        void Update()
        {
            if (Input.GetKeyDown("1"))
            {
                Add(tests[0]);
            }

            if (Input.GetKeyDown("2"))
            {
                Add(tests[1]);
            }

            if (Input.GetKeyDown("0"))
            {
                Complete(missions[0]);
            }
        }

        public void ShowMission(MissionData md)
        {
            if (md.proto.type == MissionPrototype.Type.TextInfo)
            {
                textInfoMissionSystem.ResetMission();
                voiceRecordMissionSystem.Hide();
            }
            else if (md.proto.type == MissionPrototype.Type.VoiceRecord)
            {
                voiceRecordMissionSystem.ResetMission();
                textInfoMissionSystem.Hide();
            }
            else
            {
                textInfoMissionSystem.Hide();
                voiceRecordMissionSystem.Hide();
            }
        }
    }
}