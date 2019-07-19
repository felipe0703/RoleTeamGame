using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController sharedInstance;
    public int maxNumbersActions = 2;

    //  Variables Públicas
    [Tooltip("Tiempo inicial en segundo")] public int tiempoInicial;
    [Tooltip("Escala del tiempo del Reloj")][Range(-10f, 10f)]  public float escalaDeTiempo = 1;
    [HideInInspector] public int numbersActions = 0;

    //  Variables Privadas   
    private float tiempoDelFrameConTimeScale = 0f;
    private float tiempoAMostrarEnSegundos = 0f;
    private float escalaDeTiempoAlPausar, escalaDeTiempoInicial;
    private bool estaPausado = false;


    //  COMPONENTES
    public TextMeshProUGUI textTimer;
    public TextMeshProUGUI textNumbersActions;
    public GameObject panelEndTurn;
    public GameObject buttonShowActions;
    public Canvas canvas;
    

    void Start()
    {
        //      SINGLETON
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //  ACTIONS
        textNumbersActions.text = numbersActions.ToString() + " / " + maxNumbersActions.ToString();
        
        //  TIMER
        escalaDeTiempoInicial = escalaDeTiempo;         //  Establecer la escala de tiempo original      
        tiempoAMostrarEnSegundos = tiempoInicial;       //  Inicializamos la variables que acumular
        ActualizarReloj(tiempoInicial);
    }


    void Update()
    {
        if (!estaPausado)
        {
            //  La siguiente variable representa el tiempo de cada frame considerando la escala de tiempo
            tiempoDelFrameConTimeScale = Time.deltaTime * escalaDeTiempo;

            //  La siguiente variable va acumulando el tiempo transcurrido para luego mostrarlo en el reloj
            tiempoAMostrarEnSegundos += tiempoDelFrameConTimeScale;
            ActualizarReloj(tiempoAMostrarEnSegundos);
        }
    }

    //   AGREGAR ACCION USADA
    public void AddActions()
    {
        if(numbersActions >= maxNumbersActions)
        {
            numbersActions = maxNumbersActions;
        }
        else
        {
            numbersActions++;
        }        
        textNumbersActions.text = numbersActions.ToString() + " / " + maxNumbersActions.ToString();
    }

    //  RESTAR ACCION
    public void SubtractActions()
    {
        numbersActions--;
        textNumbersActions.text = numbersActions.ToString() + " / " + maxNumbersActions.ToString();
    }

    //  MOSTRAR PANEL AL TERMINAR EL TURNO
    public void ShowPanelEndTurn()
    {
        canvas.sortingOrder = -10;
        panelEndTurn.SetActive(true);   
    }

    //  TIMER
    public void ActualizarReloj(float tiempoEnSegundos)
    {
        int minutos = 0;
        int segundos = 0;
        string textoDelReloj;

        //  Asegurar que el tiempo no sea negativo
        if (tiempoEnSegundos < 0) tiempoEnSegundos = 0;

        //  Calcular minutos y segundos
        minutos = (int)tiempoEnSegundos / 60;
        segundos = (int)tiempoEnSegundos % 60;


        // Formato con el que se ve el tiempo
        //  Crear la cadena de caracteres con 2 dígitos para los minutos y segundos, separados por  ":"
        if (minutos >= 10)
        {
            textoDelReloj = minutos.ToString("00") + ":" + segundos.ToString("00");
        }
        else if (minutos >= 1)
        {
            textoDelReloj = minutos.ToString("0") + ":" + segundos.ToString("00");
        }
        else if (segundos < 10)
        {
            textoDelReloj = segundos.ToString("0");
        }
        else
        {
            textoDelReloj = segundos.ToString("00");
        }


        //  Actualizar el elemento de text de UI con la cadena de caracteres
        textTimer.text = textoDelReloj;
    }

    public void Pausar()
    {
        if (!estaPausado)
        {
            estaPausado = true;
            escalaDeTiempoAlPausar = escalaDeTiempo;
            escalaDeTiempo = 0;
        }
    }

    public void Continuar()
    {
        if (estaPausado)
        {
            estaPausado = false;
            escalaDeTiempo = escalaDeTiempoAlPausar;
        }
    }

    public void Reiniciar()
    {
        estaPausado = false;
        escalaDeTiempo = escalaDeTiempoInicial;
        tiempoAMostrarEnSegundos = tiempoInicial;
        ActualizarReloj(tiempoAMostrarEnSegundos);
    }

}
