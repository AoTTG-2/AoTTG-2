using Assets.Scripts.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class AccountPanel : UiContainer
    {
        public TextMeshProUGUI DatabaseStatus;
        public Button LoginButton;
        public Button LogoutButton;
        public TextMeshProUGUI Username;

        private AuthenticationService AuthenticationService => Service.Authentication;

        private async void Awake()
        {
            var status = await AuthenticationService.GetHealthCheckResponse();
            if (status.IsSuccessStatusCode)
            {
                DatabaseStatus.text = "Database: <color=#00ff00>Online</color>";
            }
            else
            {
                DatabaseStatus.text = "Database: <color=#ff0000>Offline</color>";
            }
        }

        // ReSharper disable once UnusedMember.Global
        public async void Login()
        {
            if (await AuthenticationService.LoginAsync())
            {
                LoginButton.gameObject.SetActive(false);
                Username.transform.parent.gameObject.SetActive(true);
                LogoutButton.gameObject.SetActive(true);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public async void Logout()
        {
            if (await AuthenticationService.LogoutAsync())
            {
                LoginButton.gameObject.SetActive(true);
                Username.transform.parent.gameObject.SetActive(false);
                LogoutButton.gameObject.SetActive(false);
            }
        }
    }
}
