using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mgl;


namespace Com.BrumaGames.Llamaradas
{
    public class I18MngRoomListEntry : I18nMng
    {

        [SerializeField] private TextMeshProUGUI joinRandomButton;

        protected override void DoTranslations()
        {
            if (joinRandomButton != null) joinRandomButton.text = i18n.__("joinRandomButton");
        }
    }
}