using DG.Tweening;
using Omni;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.game.Omni.InGameUi
{
    public class PasswordBehaviour : MonoBehaviour
    {
        public TMP_InputField inputField_password;
        public TMP_InputField inputField_username;

        public Image toggleHiddenImg;
        public Sprite toHide;
        public Sprite toShow;

        private bool isHidden = true;

        public string correctUsername;
        public string correctPassword;

        public Image profileImage;
        public GameObject passwordArea;
        public Image submitLoginButtonImage;
        public CanvasGroup cg_welcome;

        void Start()
        {
            TurnOffPasswordScreen();
        }

        public void Boot()
        {
            passwordArea.SetActive(true);
            submitLoginButtonImage.gameObject.SetActive(false);
        }

        public void TurnOffPasswordScreen()
        {
            // 初始化输入框为隐藏状态
            inputField_password.contentType = TMP_InputField.ContentType.Password;
            isHidden = true;
            passwordArea.SetActive(false);
            submitLoginButtonImage.gameObject.SetActive(false);
            cg_welcome.alpha = 0;
            cg_welcome.interactable = false;
            cg_welcome.blocksRaycasts = false;
        }

        public Sprite spUser;
        public Sprite spDefault;

        public void OnUsernameChanged(string v)
        {
            if (v == correctUsername)
            {
                profileImage.sprite = spUser;
            }
            else
            {
                profileImage.sprite = spDefault;
            }
        }

        public Sprite spLoginOk;
        public Sprite spLoginNotOk;

        public void OnPasswordChanged(string v)
        {
            if (inputField_username.text == correctUsername)
            {
                submitLoginButtonImage.gameObject.SetActive(true);
                if (v.Length > 5)
                {
                    submitLoginButtonImage.sprite = spLoginOk;
                }
                else
                {
                    submitLoginButtonImage.sprite = spLoginNotOk;
                }
            }
            else
            {
                submitLoginButtonImage.gameObject.SetActive(false);
            }

        }

        public void ToggleHidden()
        {
            if (isHidden)
            {
                // 显示输入内容
                inputField_password.contentType = TMP_InputField.ContentType.Standard;
                toggleHiddenImg.sprite = toHide;
            }
            else
            {
                // 隐藏输入内容
                inputField_password.contentType = TMP_InputField.ContentType.Password;
                toggleHiddenImg.sprite = toShow;
            }

            // 刷新输入框，确保显示状态更新
            inputField_password.ForceLabelUpdate();
            isHidden = !isHidden;
        }

        public void Submit()
        {
            if (inputField_username.text == correctUsername)
            {
                if (inputField_password.text == correctPassword)
                {
                    Debug.Log("用户名密码正确");
                    Omni2DSystem.instance.OnLoginSuc();
                }
                else
                {
                    Debug.Log("密码错误");
                }
            }
            else
            {
                Debug.Log("用户不存在");
            }
        }
    }
}