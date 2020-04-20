using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.BrumaGames.Llamaradas
{
    public class I18n : Mgl.I18n
    {
        protected new static readonly I18n instance = new I18n();

        // Customize your languages here
        protected new static string[] locales = new string[] {
          "en-US",
          "es-ES",
        };

        public new static I18n Instance
        {
            get
            {
                return instance;
            }
        }
    }
}