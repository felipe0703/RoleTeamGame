using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScene : MonoBehaviour
{

    public GUIAnimFREE panel,titlePartida,inputTxt,buttonOk;

    private void Start()
    {
        //iniciar coroutine
        //Iniciamos una corrutina, que es un método que se ejecuta 
        //en una línea de tiempo diferente al flujo principal del programa
        // LoadLevel(sceneToLoad);
        /*panel.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        titlePartida.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        inputTxt.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        buttonOk.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);*/
    }

    public void LoadLevel(int sceneIndex)
    {

        // Usa una coroutine para cargar la escena en el fondo
        StartCoroutine(LoadScene(sceneIndex));
    }

    //Corrutina
    IEnumerator LoadScene(int sceneIndex)
    {
        // La aplicación carga la escena en segundo plano mientras se ejecuta la escena actual .
        // Esto es particularmente bueno para crear pantallas de carga.
        //Iniciamos la carga asíncrona de la escena y guardamos el proceso en 'operation'
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        // Espera hasta que la escena asíncrona se cargue completamente.
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            //Actualizamos la barra de carga y texto
            //Debug.Log( progress);
            yield return null;
        }
    }


    public void CambiarEscena(string escena)
    {
        Debug.Log("probando script");
       // audioSource.PlayOneShot(next);

        //SceneManager.LoadScene(escena);

        // Disable all buttons
        GUIAnimSystemFREE.Instance.EnableAllButtons(false);

        // Waits 1.5 secs for Moving Out animation then load next level
        GUIAnimSystemFREE.Instance.LoadLevel(escena, 1.5f);

        GameObject.Find("UIManager").gameObject.SendMessage("HideAllGUIs");
    }

}
