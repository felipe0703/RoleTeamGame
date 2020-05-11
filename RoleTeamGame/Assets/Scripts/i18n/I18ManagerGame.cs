using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mgl;

namespace Com.BrumaGames.Llamaradas
{
    public class I18ManagerGame : I18nManager
    {
        [SerializeField] private TextMeshProUGUI buttonWind_M;
        [SerializeField] private TextMeshProUGUI countdownTimerText_M;
        [SerializeField] private TextMeshProUGUI countdownTimerText_C;
        [SerializeField] private TextMeshProUGUI changeTurn_M;
        [SerializeField] private TextMeshProUGUI changeTurn_C;
        [SerializeField] private TextMeshProUGUI advanceFire_M;
        [SerializeField] private TextMeshProUGUI advanceFire_C;
        [SerializeField] private TextMeshProUGUI changeWind_M;
        [SerializeField] private TextMeshProUGUI changeWind_C;
        [SerializeField] private TextMeshProUGUI countEnergiesText_M;
        [SerializeField] private TextMeshProUGUI countEnergiesText_C;



        protected override void DoTranslations()
        {
            if (buttonWind_M != null) buttonWind_M.text = i18n.__("buttonWind_M");
            if (countdownTimerText_M != null) countdownTimerText_M.text = i18n.__("countdownTimerText_M");
            if (changeTurn_M != null) changeTurn_M.text = i18n.__("changeTurn_M");
            if (advanceFire_M != null) advanceFire_M.text = i18n.__("advanceFire_M");
            if (changeWind_M != null) changeWind_M.text = i18n.__("changeWind_M");
            if (countEnergiesText_M != null) countEnergiesText_M.text = i18n.__("countEnergiesText_M");


            if (countdownTimerText_C != null) countdownTimerText_C.text = i18n.__("countdownTimerText_C");
            if (changeTurn_C != null) changeTurn_C.text = i18n.__("changeTurn_C");
            if (advanceFire_C != null) advanceFire_C.text = i18n.__("advanceFire_C");
            if (changeWind_C != null) changeWind_C.text = i18n.__("changeWind_C");
            if (countEnergiesText_C != null) countEnergiesText_C.text = i18n.__("countEnergiesText_C");


        }


    }
}

