using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mgl;

public class I18nManager : MonoBehaviour {

    private I18n i18n = I18n.Instance;

    [SerializeField] private TMP_Dropdown LangInputField;

    [SerializeField] private TextMeshProUGUI pressStart;
    [SerializeField] private TextMeshProUGUI idioma;

    private void Start()
    {
        //Messenger.AddListener<string>("Language:Change", SetLanguage); // event when langauge change
        InitLanguage();
    }

    public void ChangeLanguage()
    {
        var locale = "";
        if(LangInputField.value == 0)
        {
            locale = "en-US";
        } else if (LangInputField.value == 1)
        {
            locale = "es-ES";
        }
        PlayerPrefs.SetString("Language", locale);
        I18n.SetLocale(locale);
        DoTranslations();
    }

    private void SetLanguage(string locale)
    {
        PlayerPrefs.SetString("Language", locale);
        I18n.SetLocale(locale);
        DoTranslations();
    }

    private void DoTranslations()
    {
        if (pressStart != null) pressStart.text = i18n.__("pressStart");
        if (idioma != null) idioma.text = i18n.__("idioma");
    }

    private void InitLanguage()
    {

        if (PlayerPrefs.HasKey("Language"))
        {
            SetLanguage(PlayerPrefs.GetString("Language"));

            if (PlayerPrefs.GetString("Language") == "en-US")
            {
                LangInputField.value = 0;
            }
            else if (PlayerPrefs.GetString("Language") == "es-ES")
            {
                LangInputField.value = 1;
            }

            return;
        }

        // Sanity check:
        if (!PlayerPrefs.HasKey("Language"))
        {
            SetLanguage("en-US");
        }

    }
}