﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mgl;

namespace Com.BrumaGames.Llamaradas
{
    public abstract class I18nMng : MonoBehaviour
    {

        protected static I18n i18n = I18n.Instance;
        //public static I18nMng sharedInstance;


        [SerializeField] private TMP_Dropdown LangInputField;

        /*private void Awake()
        {
            //      SINGLETON
            if (sharedInstance == null)
            {
                sharedInstance = this;
                DontDestroyOnLoad(sharedInstance);
            }
            else
            {
                Destroy(gameObject);
            }
        }*/

        public static string GetText(string text)
        {
            return i18n.__(text);
        }


        private void Start()
        {
            //Messenger.AddListener<string>("Language:Change", SetLanguage); // event when langauge change
            InitLanguage();
            if(LangInputField != null)
            {
                LangInputField.onValueChanged.AddListener(delegate {
                    ChangeLanguage();
                });
            }
        }

        public void ChangeLanguage()
        {
            var locale = "";
            if (LangInputField.value == 0)
            {
                locale = "en-US";
            }
            else if (LangInputField.value == 1)
            {
                locale = "es-ES";
            }
            PlayerPrefs.SetString("Language", locale);
            I18n.SetLocale(locale);
            DoTranslations();
        }

        protected void SetLanguage(string locale)
        {
            PlayerPrefs.SetString("Language", locale);
            I18n.SetLocale(locale);
            DoTranslations();
        }

        protected abstract void DoTranslations();

        protected void InitLanguage()
        {

            if (PlayerPrefs.HasKey("Language"))
            {
                SetLanguage(PlayerPrefs.GetString("Language"));

                if (LangInputField != null)
                {
                    if (PlayerPrefs.GetString("Language") == "en-US")
                    {
                        LangInputField.value = 0;
                    }
                    else if (PlayerPrefs.GetString("Language") == "es-ES")
                    {
                        LangInputField.value = 1;
                    }
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


}