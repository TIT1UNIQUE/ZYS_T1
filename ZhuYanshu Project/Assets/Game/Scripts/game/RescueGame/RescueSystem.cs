using com;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Rescue
{

    public partial class RescueSystem : MonoBehaviour
    {
        public static RescueSystem instance;

        public CanvasGroup sceneCg_girlInBed1;
        public CanvasGroup sceneCg_washFace;
        public CanvasGroup sceneCg_dressing;

        public enum GameStartPhase
        {
            Default,
            SceneWashFace,
            Dressing,
            GoOutside,
            WakeUpBoy,
            WashBoy,
            FoodBoy,
            GoOutsideBoy,

            ClockNarrative,
            Rescue,
            DialogTwoPerson,
            FinalOfRescue,

            StartLogo,
        }

        public GameStartPhase gameStartPhase;

        void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                //Debug.Log("debug fast OnClickExitJisawScene");

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
            ToggleCg(sceneCg_girlInBed1, false);
            ToggleCg(sceneCg_washFace, false);
            ToggleCg(sceneCg_dressing, false);
            ToggleCg(sceneCg_OutsidePuzzle, false);
            ToggleCg(sceneCg_WakeUpBoy, false);
            ToggleCg(sceneCg_WashBoy, false);
            ToggleCg(sceneCg_FoodBoy, false);
            ToggleCg(sceneCg_OutsidePuzzle_boy, false);
            ToggleCg(sceneCg_ClockNarrative, false);
            ToggleCg(sceneCg_Rescue, false);
            ToggleCg(sceneCg_DialogTwoPerson, false);
            ToggleCg(sceneCg_FinalOfRescue, false);
            ToggleCg(logoCg, false);

            switch (gameStartPhase)
            {
                case GameStartPhase.Default:
                    ToggleCg(sceneCg_girlInBed1, true);
                    StartSceneGirlInBed();
                    break;

                case GameStartPhase.SceneWashFace:
                    ToggleCg(sceneCg_washFace, true);
                    stage = GameStage.SceneGirlInBed_AfterMobileInteraction;
                    break;

                case GameStartPhase.Dressing:
                    ToggleCg(sceneCg_dressing, true);
                    stage = GameStage.SceneGirlInBed_AfterMobileInteraction;
                    break;

                case GameStartPhase.GoOutside:
                    ToggleCg(sceneCg_OutsidePuzzle, true);
                    stage = GameStage.SceneGirlInBed_AfterMobileInteraction;
                    StartCoroutine(StartScene_OutsidePuzzle(0));
                    break;
                case GameStartPhase.WakeUpBoy:
                    ToggleCg(sceneCg_WakeUpBoy, true);
                    stage = GameStage.SceneBoyInBed_beforeMobileInteraction;
                    StartCoroutine(StartScene_WakeUpBoy());
                    break;

                case GameStartPhase.WashBoy:
                    ToggleCg(sceneCg_WashBoy, true);
                    stage = GameStage.SceneBoyInBed_AfterMobileInteraction;
                    break;

                case GameStartPhase.FoodBoy:
                    ToggleCg(sceneCg_FoodBoy, true);
                    stage = GameStage.SceneBoyInBed_AfterMobileInteraction;
                    ShowBoyFoodScene();
                    break;

                case GameStartPhase.GoOutsideBoy:
                    ToggleCg(sceneCg_OutsidePuzzle_boy, true);
                    stage = GameStage.SceneGirlInBed_AfterMobileInteraction;
                    StartCoroutine(StartScene_OutsidePuzzle_boy(0));
                    break;
                case GameStartPhase.ClockNarrative:
                    ToggleCg(sceneCg_ClockNarrative, true);
                    stage = GameStage.SceneGirlInBed_AfterMobileInteraction;
                    StartCoroutine(StartScene_ClockNarrative(0));
                    break;
                case GameStartPhase.Rescue:
                    ToggleCg(sceneCg_Rescue, true);
                    stage = GameStage.SceneGirlInBed_AfterMobileInteraction;
                    StartCoroutine(StartScene_Rescue(0));
                    break;
                case GameStartPhase.DialogTwoPerson:
                    ToggleCg(sceneCg_DialogTwoPerson, true);
                    stage = GameStage.SceneGirlInBed_AfterMobileInteraction;
                    StartCoroutine(StartScene_DialogTwoPerson(0));
                    break;
                case GameStartPhase.FinalOfRescue:
                    ToggleCg(sceneCg_FinalOfRescue, true);
                    stage = GameStage.SceneGirlInBed_AfterMobileInteraction;
                    StartCoroutine(StartScene_FinalOfRescue(0));
                    break;

                case GameStartPhase.StartLogo:
                    ToggleCg(logoCg, false);
                    logoCg.interactable = true;
                    logoCg.blocksRaycasts = true;
                    logoCg.DOFade(1, 1);
                    break;
            }

            zoomParentOrginalPos = zoomParent.position;
        }

        public RectTransform mobile;
        public RectTransform zoomParent;

        public GameStage stage;
        public float mobileVibStrength = 1;
        public int mobileVibVibrato = 10;
        public float zoomParentZoomInDuration = 2;
        public RectTransform zoomParentZoomInRectTrans;
        private Vector3 zoomParentOrginalPos;

        public Image girlInbedImg_closeEye;
        public Image girlInbedImg_openEye;

        public AudioSource mobileVibSound;


        public enum GameStage
        {
            None = 0,
            SceneGirlInBed_beforeMobileInteraction = 1,
            SceneGirlInBed_PendingMobileInteraction = 2,
            SceneGirlInBed_AfterMobileInteraction = 3,

            SceneBoyInBed_beforeMobileInteraction = 4,
            SceneBoyInBed_PendingMobileInteraction = 5,
            SceneBoyInBed_AfterMobileInteraction = 6,
        }

        IEnumerator DelayAction(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}