#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion  //Namespace



public class BoardManager : MonoBehaviour
{
    // ########################################
    // Variables
    // ########################################

    #region Variables
    public static BoardManager sharedInstance;              //  singleton    

    public int xSize, ySize;                                //  tamaño del tablero

    public GameObject currenteEdifice;                              //  prefabs del edificio    
    public List<GameObject> prefabs = new List<GameObject>();       //  lista de edificios que puedes instanciar
    private List<GameObject> allEdifices = new List<GameObject>();  // Lista con todos los edificios generados en el tablero
    public GameObject[,] edifices;                                  // arreglo de edificios    
    private List<GameObject> centralEdifices = new List<GameObject>();// Edificios Centrales del tablero

    public int maxHouse = 12;
    public int maxEdifice = 18;
    public int maxPark = 6;

    [Space(10)]
    public GameObject currentStreet;
    public List<Sprite> streetList = new List<Sprite>();
    private GameObject[,] streets;

    [Space(10)]
    public GameObject currentRiver;
    public List<Sprite> riverList = new List<Sprite>();
    private GameObject[,] rivers;

    [Space(10)]
    public GameObject currentBorder;
    public List<Sprite> borderList = new List<Sprite>();
    private GameObject[,] borders;
    #endregion //Variables

    // ########################################
    // Funciones MonoBehaviour 
    // ########################################

    #region MonoBehaviour
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

        Vector2 offset = currenteEdifice.GetComponent<BoxCollider2D>().size; // obtengo el tamaño del edificio
        CreateInitialBoard(offset); //  inicio el tablero
    }
    #endregion //MonoBehaviour

    // ########################################
    // GENERADOR DEL TABLERO
    // ########################################

    #region GeneraTablero
    private void CreateInitialBoard(Vector2 offset)
    {
        int xSizeBoard = xSize * 2 + 5;
        int ySizeBoard = ySize * 2 + 5;

        //  TODO: REVISAR UTILIDAD Y TAMAÑOS DE LOS ARREGLOS
        streets = new GameObject[xSize, ySize];
        rivers = new GameObject[xSize, ySize];
        borders = new GameObject[xSize, ySize];

        // posicionamiento inicial del tablero
        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        Sprite sprite = null;
        GameObject edifice;

        int idX = -1;
        int contHouse = 0;
        int contEdifice = 0;
        int contPark = 0;
    
        //  BUCLES QUE POSICIONA LOS EDIFICIOS, RIÓS Y CALLES
        for (int x = 0; x < xSizeBoard; x++)
        {
            for (int y = 0; y < ySizeBoard; y++)
            {
                if(x > 1 && x < xSizeBoard - 2 && y > 1 && y < ySizeBoard - 2)
                {
                    if (x > 2 && x % 2 != 0 && y % 2 != 0)
                    {            
                        do
                        {
                            edifice = prefabs[Random.Range(0, prefabs.Count)];
                            idX = edifice.GetComponent<Edifice>().id;

                        } while (   (idX == 1 && contHouse > maxHouse - 1) 
                                ||  (idX == 2 && contEdifice > maxEdifice - 1) 
                                ||  (idX == 3 && contPark > maxPark - 1));

                        if(idX == 1)
                        {
                            contHouse++;
                        }
                        if (idX == 2)
                        {
                            contEdifice++;
                        }
                        if (idX == 3)
                        {
                            contPark++;
                        }

                        currenteEdifice = edifice;

                        //  GENERACIÓN EDIFICIOS
                        GameObject newEdifice = Instantiate(currenteEdifice,
                            new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0),
                            currenteEdifice.transform.rotation
                            );

                        // Formato al nombre de los objetos
                        newEdifice.name = string.Format("Edifice[{0}][{1}]", x, y);
                        // Lo agrega como hijo de Edifices para tener un orden
                        newEdifice.transform.parent = GameObject.Find("Edifices").transform;    
                        // agrega todos los edificios, para poder acceder después a ellos
                        allEdifices.Add(newEdifice);
                    }
                    else 
                    {
                        //  GENERACIÓN CALLES
                        GameObject newStreet = Instantiate(currentStreet,
                            new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0),
                            currentStreet.transform.rotation
                            );

                        // Formato al nombre de los objetos
                        newStreet.name = string.Format("Street[{0}][{1}]", x, y);

                        newStreet.transform.parent = GameObject.Find("Streets").transform;
                        //streets[x, y] = newStreet;

                        currentStreet.GetComponent<Street>().isBorder = false;
                        currentStreet.GetComponent<Street>().isCorner = false;                        


                        if (x % 2 != 0 && y % 2 == 0)           //  HORIZONTALES
                        {
                            sprite = streetList[Random.Range(0, 2)];
                            newStreet.GetComponent<SpriteRenderer>().sprite = sprite;
                        }
                        else if (x % 2 == 0 && y % 2 != 0)       //  CALLES VERTICALES
                        {
                            sprite = streetList[Random.Range(2, 4)];
                            newStreet.GetComponent<SpriteRenderer>().sprite = sprite;

                            
                        }
                        else if (x % 2 == 0 && y % 2 == 0)      //  INTERCEPCIÓN CALLES CALLES 
                        {
                            sprite = streetList[Random.Range(4, 6)];
                            newStreet.GetComponent<SpriteRenderer>().sprite = sprite;
                            
                        }
                        
                        // la calle esta en los borde
                        if (x == 2 || y == 2 || x == xSizeBoard - 3 || y == ySizeBoard - 3)
                        {
                            newStreet.GetComponent<Street>().isBorder = true;                           
                        }

                        // la calle es una esquina
                        if ((x == 2 && y == 2) || 
                            (x == xSizeBoard - 3 && y == ySizeBoard - 3) ||
                            (x == 2 && y == ySizeBoard - 3) ||
                            (x == xSizeBoard - 3 && y == 2))
                        {
                            newStreet.GetComponent<Street>().isCorner = true;
                        }


                    }
                }
                


                else if (x >= xSizeBoard - 2 )
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
                    //rivers[x, y] = newRiver;
                }


                else if (x <= 1 || y <= 1 || y >= ySizeBoard - 2 )
                {
                    //  GENERACIÓN BORDER
                    GameObject newBorder = Instantiate(currentBorder,
                        new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0),
                        currentBorder.transform.rotation
                        );

                    // Formato al nombre de los objetos
                    newBorder.name = string.Format("Border[{0}][{1}]", x, y);

                    sprite = borderList[0];
                    newBorder.GetComponent<SpriteRenderer>().sprite = sprite;

                    newBorder.transform.parent = GameObject.Find("Borders").transform;

                    //borders[x, y] = newBorder;
                }
            }

            
        }


        SaveEdificesInMatrix();
        FireStart();
    }
    #endregion

    public void SaveEdificesInMatrix()
    {
        int cont = 0;
        edifices = new GameObject[xSize, ySize];

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                edifices[i, j] = allEdifices[cont];
                cont++;
            }
        }
    }

    // Inicia el fuego de forma aleatorio en uno de los edificios centrales
    public void FireStart()
    {
        int xSizeHalf = xSize / 2;
        int ySizeHalf = ySize / 2;

        // Obtención de edificios centrales
        for (int i = xSizeHalf - 1 ; i < xSizeHalf + 1; i++)
        {
            for (int j = ySizeHalf - 1; j < ySizeHalf + 1; j++)
            {
                centralEdifices.Add(edifices[i, j]);
            }
        }

        GameObject edifice;

        // Obtener el edificio donde iniciará el fuego
        edifice = centralEdifices[Random.Range(0, centralEdifices.Count)];

        //Iniciar fuego
        edifice.GetComponent<Edifice>().FireStart();

    }
}
