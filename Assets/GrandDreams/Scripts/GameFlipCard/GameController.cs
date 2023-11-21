using GrandDreams.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GrandDreams.Common.Unity.Utilities;
using GrandDreams.Core.Kinect;
using System.Collections;

namespace GrandDreams.Game.FlipCard
{
    public class GameController : MonoBehaviour
    {
        #region Declare Variables
        [SerializeField] private BaseKinectCursorController kinectCursor;
        [SerializeField] private PanelLogin panelLogin;
        [SerializeField] private PanelPlaying panelPlaying;
        [SerializeField] private PanelEndGame panelEndGame;


        private KinectManager kinectManager;
        #endregion Declare Variables

        #region Private Function
        private void Awake()
        {
            ConfigData.Initialize();
            DOTween.SetTweensCapacity(1000, 200);
            kinectCursor.OnKinectCursor_Interacted += HandleKinectCursor_Interacted;

            panelEndGame.OnPanelEndGame_RequestRestart += HandlePanelEndGame_RequestRestart;
        }

        private IEnumerator Start()
        {
            InitializeGameState();

            if (SoundManager.SharedInstance != null)
            {
                SoundManager.SharedInstance.PlaySong("BGMusic");
            }
            //panelLogin.OnPanelStart_RequestNextStep += HandelPanelLoginNextStep;
            panelPlaying.ResultPanelPlaying += HandlePanelPlaying_IsWin;

            BlackImage.Instance.Hide(0, null, () =>
            {
                panelLogin.Show(null, 0, null, null, null);
            });
            Application.targetFrameRate = 60;

            yield return new WaitUntil(() => KinectManager.Instance != null);
            kinectManager = KinectManager.Instance;
            kinectManager.maxUserDistance = ConfigData.Data.MaxUserDistance;

        }

        private void OnDestroy()
        {
            kinectCursor.OnKinectCursor_Interacted -= HandleKinectCursor_Interacted;
            panelLogin.OnPanelStart_RequestNextStep -= HandelPanelLoginNextStep;
            panelPlaying.ResultPanelPlaying -= HandlePanelPlaying_IsWin;
            panelEndGame.OnPanelEndGame_RequestRestart -= HandlePanelEndGame_RequestRestart;

        }
        private void InitializeGameState()
        {
            BaseState.Instance.AddState("Initialize", "Login","HideLogin" , "ShowPlaying", "HidePlaying", "ShowEnding", "HideEnding", "GameEnd");
            BaseState.Instance.AddActionToAllState(() => { kinectCursor.SetGameState(BaseState.Instance.IDState); });
            //BaseState.Instance.AddActionState(1, panelGuideLine.ShowButtonContinue);
            //BaseState.Instance.AddActionState(10, panelEndGame.ShowButtonContinue);
            BaseState.Instance.CurrentState = "Initialize";
        }
        private void HandelPanelLoginNextStep()
        {
            panelLogin.Hide(null, 0, null, null, () => { panelPlaying.Show(null, 0, null, null, null); });
        }

        // xử lý trạng thái win và thua của game và gửi sang bước tiếp theo nếu có 
        private void HandlePanelPlaying_IsWin(bool iswin)
        {
            panelPlaying.Hide(null, 0, null, null,
                () => { panelEndGame.Show(new object[] { iswin }, 0, null, null, null); });
        }

        private void Update()
        {
            Debug.Log(BaseState.Instance.CurrentState);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                HandleKinectCursor_Interacted(null, 1, "StartGame");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                HandleKinectCursor_Interacted(null, 4, "Continue");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                HandleKinectCursor_Interacted(null, 6, "Ready");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                HandleKinectCursor_Interacted(null, 10, "Back");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                BaseState.Instance.CurrentState = "GameEnd";
                BlackImage.Instance.Show(0, null, () =>
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
                });
            }
        }
        private void HandlePanelEndGame_RequestRestart()
        {
            if (BlackImage.Instance != null)
            {
                panelEndGame.Hide(null, 0, null, null, () =>
                {
                    BlackImage.Instance.Show(0, null, () =>
                    {
                        SceneManager.LoadScene("MainScenes");
                    });
                });

            }
            else
            {
                SceneManager.LoadScene("MainScenes");
            }
        }

        #endregion Private Function

        #region Event

        private void HandleKinectCursor_Interacted(KinectCursor cursor, int idState, string interactivePointName)
        {
            Debug.Log("a:"+ idState);
            Debug.Log("b:"+ interactivePointName);
            switch (BaseState.Instance.GetStateByID(idState))
            {
                case "Login":
                    if (interactivePointName == "StartGame")
                    {
                        if (Common.Unity.Utilities.SoundManager.SharedInstance != null)
                        {
                            Common.Unity.Utilities.SoundManager.SharedInstance.PlaySfx("Click");
                        }

                        BaseState.Instance.CurrentState = "Starting";
                        HandelPanelLoginNextStep();
                    }
                    break;

                case "ShowGuideLine":
                    if (interactivePointName == "Continue")
                    {
                        if (Common.Unity.Utilities.SoundManager.SharedInstance != null)
                        {
                            Common.Unity.Utilities.SoundManager.SharedInstance.PlaySfx("Click");
                        }

                        BaseState.Instance.CurrentState = "HideGuideLine";
                        //panelGuideLine.Hide(null, 0, null, null, () =>
                        //{
                        //    panelReady.Show(null, 0, null, null, () => { BaseState.Instance.CurrentState = "ShowReady"; });
                        //});
                    }
                    break;

                case "ShowReady":
                    if (interactivePointName == "Ready")
                    {
                        if (Common.Unity.Utilities.SoundManager.SharedInstance != null)
                        {
                            Common.Unity.Utilities.SoundManager.SharedInstance.PlaySfx("Start");
                        }

                        BaseState.Instance.CurrentState = "HideReady";
                        //panelReady.Hide(null, 0, null, null, () =>
                        //{
                        //    panelPlayGame.Show(null, 0, null, null, () => { BaseState.Instance.CurrentState = "Playing"; });
                        //});
                    }
                    break;

                case "Ending":
                    if (interactivePointName == "Back")
                    {
                        if (Common.Unity.Utilities.SoundManager.SharedInstance != null)
                        {
                            Common.Unity.Utilities.SoundManager.SharedInstance.PlaySfx("Start");
                        }

                        BaseState.Instance.CurrentState = "GameEnd";
                        BlackImage.Instance.Show(0, null, () =>
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
                        });
                    }
                    break;
            }
        }


        #endregion Event

    }
}