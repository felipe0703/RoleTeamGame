using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mgl;

namespace Com.BrumaGames.Llamaradas
{
    public class I18nMngStartWindow : I18nMng
    {

        [SerializeField] private TextMeshProUGUI pressStart;
        [SerializeField] private TextMeshProUGUI idioma;


        public void hacerAlgo()
        {

        }

        protected override void DoTranslations()
        {
            if (pressStart != null) pressStart.text = i18n.__("pressStart");
            if (idioma != null) idioma.text = i18n.__("idioma");
        }
    }
}