using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mgl;

namespace Com.BrumaGames.Llamaradas
{
    public class i18nManagerSetGame : I18nManager
    {
        [SerializeField] private TextMeshProUGUI connectionStatus;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI loginButton;
        [SerializeField] private TextMeshProUGUI createRoomButton;
        [SerializeField] private TextMeshProUGUI joinRandomRoomButton;
        [SerializeField] private TextMeshProUGUI roomListButton;
        [SerializeField] private TextMeshProUGUI roomNameText;
        [SerializeField] private TextMeshProUGUI roomNameInput;
        [SerializeField] private TextMeshProUGUI maxPlayersText;
        [SerializeField] private TextMeshProUGUI maxPlayersInput;
        [SerializeField] private TextMeshProUGUI showTimeText;
        [SerializeField] private TextMeshProUGUI roundTimeText;
        [SerializeField] private TextMeshProUGUI cancelButton;
        [SerializeField] private TextMeshProUGUI createRoomButton1;
        [SerializeField] private TextMeshProUGUI joinRandomRoomText;
        [SerializeField] private TextMeshProUGUI backButton;
        [SerializeField] private TextMeshProUGUI leaveGameButton;
        [SerializeField] private TextMeshProUGUI startGameButton;




        protected override void DoTranslations()
        {
            if (connectionStatus != null) connectionStatus.text = i18n.__("connectionStatus");
            if (playerName != null) playerName.text = i18n.__("playerName");
            if (loginButton != null) loginButton.text = i18n.__("loginButton");
            if (createRoomButton != null) createRoomButton.text = i18n.__("createRoomButton");
            if (joinRandomRoomButton != null) joinRandomRoomButton.text = i18n.__("joinRandomRoomButton");
            if (roomListButton != null) roomListButton.text = i18n.__("roomListButton");
            if (roomNameText != null) roomNameText.text = i18n.__("roomNameText");
            if (roomNameInput != null) roomNameInput.text = i18n.__("roomNameInput");
            if (maxPlayersText != null) maxPlayersText.text = i18n.__("maxPlayersText");
            if (maxPlayersInput != null) maxPlayersInput.text = i18n.__("maxPlayersInput");
            if (showTimeText != null) showTimeText.text = i18n.__("showTimeText");
            if (roundTimeText != null) roundTimeText.text = i18n.__("roundTimeText");
            if (cancelButton != null) cancelButton.text = i18n.__("cancelButton");
            if (createRoomButton1 != null) createRoomButton1.text = i18n.__("createRoomButton1");
            if (joinRandomRoomText != null) joinRandomRoomText.text = i18n.__("joinRandomRoomText");
            if (backButton != null) backButton.text = i18n.__("backButton");
            if (leaveGameButton != null) leaveGameButton.text = i18n.__("leaveGameButton");
            if (startGameButton != null) startGameButton.text = i18n.__("startGameButton");

                                                       
        }
    }
}
