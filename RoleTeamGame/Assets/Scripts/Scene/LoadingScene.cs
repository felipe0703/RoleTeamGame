using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScene : MonoBehaviour
{

    private void Start()
    {
        //iniciar coroutine
        //Iniciamos una corrutina, que es un método que se ejecuta 
        //en una línea de tiempo diferente al flujo principal del programa
        // LoadLevel(sceneToLoad);
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
}
