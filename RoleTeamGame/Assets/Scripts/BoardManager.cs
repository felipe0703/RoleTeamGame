using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager sharedInstance;              //  singleton    

    public int xSize, ySize;                                //  tamaño del tablero

    public GameObject currenteEdifice;                      //  prefabs del edificio    
    public List<GameObject> prefabs = new List<GameObject>();       //  lista de edificios   
    private GameObject[,] edifices;                         // arreglo de edificios    

    public GameObject currentStreet;
    public List<Sprite> streetList = new List<Sprite>();
    private GameObject[,] streets;

    public GameObject currentRiver;
    public List<Sprite> riverList = new List<Sprite>();
    private GameObject[,] rivers;

    void Start()
    {
        //      SINGLETON
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Vector2 offset = currenteEdifice.GetComponent<BoxCollider2D>().size; // obtengo el tamaño del edificio
        CreateInitialBoard(offset); //  inicio el tablero
    }


    //  GENERADOR DEL TABLERO
    private void CreateInitialBoard(Vector2 offset)
    {
        xSize *= 2;
        ySize *= 2;

        edifices = new GameObject[xSize, ySize];
        streets = new GameObject[xSize, ySize];
        rivers = new GameObject[xSize, ySize];

        // posicionamiento inicial del tablero
        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        Sprite sprite = null;
        GameObject edifice;

       // int idX = -1;

        //  BUCLE QUE POSICIONA LOS EDIFICIOS
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if(x % 2 == 0 && y % 2 == 0)
                {
                    //TODO:  COLOCAR LOS SPRITES EN LA LISTA, CREAR PREFABS MEJOR
                    edifice = prefabs[Random.Range(0, prefabs.Count)];
                    currenteEdifice = edifice;

                    //  GENERACIÓN EDIFICIOS
                    GameObject newEdifice = Instantiate(currenteEdifice,
                        new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0),
                        currenteEdifice.transform.rotation
                        );

                    // Formato al nombre de los objetos
                    newEdifice.name = string.Format("Edifice[{0}][{1}]", x, y);

                    

                    newEdifice.transform.parent = GameObject.Find("Edifices").transform;
                    edifices[x, y] = newEdifice;
                }
                else if(x != xSize - 1)
                {
                    //  GENERACIÓN CALLES
                    GameObject newStreet = Instantiate(currentStreet,
                        new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0),
                        currentStreet.transform.rotation
                        );

                    // Formato al nombre de los objetos
                    newStreet.name = string.Format("Street[{0}][{1}]", x, y);                    

                    newStreet.transform.parent = GameObject.Find("Streets").transform;
                    streets[x, y] = newStreet;

                   
                    if (x % 2 == 0 && y % 2 != 0)           //  HORIZONTALES
                    {
                        sprite = streetList[Random.Range(0,2)];
                        newStreet.GetComponent<SpriteRenderer>().sprite = sprite;
                    }
                    else if(x % 2 != 0 && y % 2 == 0)       //  CALLES VERTICALES
                    {
                        sprite = streetList[Random.Range(2, 4)];
                        newStreet.GetComponent<SpriteRenderer>().sprite = sprite;
                    }
                    else if (x % 2 != 0 && y % 2 != 0)      //  INTERCEPCIÓN CALLES CALLES 
                    {
                        sprite = streetList[Random.Range(4,6)];
                        newStreet.GetComponent<SpriteRenderer>().sprite = sprite;
                    }  
                }else if(x == xSize - 1)
                {
                    //  GENERACIÓN RIO
                    GameObject newRiver = Instantiate(currentRiver,
                        new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0),
                        currentRiver.transform.rotation
                        );

                    // Formato al nombre de los objetos
                    newRiver.name = string.Format("River[{0}][{1}]", x, y);

                    sprite = riverList[0];
                    newRiver.GetComponent<SpriteRenderer>().sprite = sprite;

                    newRiver.transform.parent = GameObject.Find("Rivers").transform;
                    rivers[x, y] = newRiver;
                }        
            }
        }
    }
}
