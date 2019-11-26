#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
#endregion  //Namespace

namespace Com.BrumaGames.Llamaradas
{

    #region DirectionOfTheWind
    public enum DirectionOfTheWind
    {
        north,
        east,
        south,
        west
    }
    #endregion // DirectionOfTheWind

    public class BoardManager : MonoBehaviourPun
    {
        // ########################################
        // Variables
        // ########################################

        #region Variables
        public static BoardManager sharedInstance;              //  singleton    
        public DirectionOfTheWind currentDirectionWind = DirectionOfTheWind.west;

        public static int directionWind;
        public Image arrowDirectionWind;

        public int xSize, ySize;                                //  tamaño del tablero

        public GameObject currenteEdifice;                              //  prefabs del edificio    
        public List<GameObject> prefabs = new List<GameObject>();       //  lista de edificios que puedes instanciar
        public List<GameObject> allEdifices = new List<GameObject>();  // Lista con todos los edificios generados en el tablero
        public GameObject[,] edifices;                                  // arreglo de edificios    
        public List<GameObject> centralEdifices = new List<GameObject>();// Edificios Centrales del tablero

        public int maxHouse = 12;
        public int maxEdifice = 18;
        public int maxPark = 6;


        [Space(10)]
        public GameObject currentStreet;
        public List<GameObject> streetList = new List<GameObject>();
        private GameObject[,] streets;

        [Space(10)]
        public GameObject currentRiver;
        public List<Sprite> riverList = new List<Sprite>();
        private GameObject[,] rivers;


        [Space(10)]
        public GameObject currentBorder;
        public List<Sprite> borderList = new List<Sprite>();
        private GameObject[,] borders;

        //Detectar los edificios adyacentes
        private Vector2[] adjacentDirections = new Vector2[]
        {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
        };

        public List<GameObject> neighborsEdifices = new List<GameObject>(); // listado con los edificios vecinos al fuego


        PhotonView pv;
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
            pv = gameObject.GetComponent<PhotonView>();

            Vector2 offset = currenteEdifice.GetComponent<BoxCollider2D>().size; // obtengo el tamaño del edificio

            CreateInitialBoard(offset); //  inicio el tablero
            

            directionWind = 3;
        }
        private void Update()
        {
            Debug.Log(directionWind);
            UIManagerGame.sharedInstance.textSetTurn.text = directionWind.ToString();
        }
        #endregion //MonoBehaviour


        // ########################################
        // GENERADOR DEL TABLERO
        // ########################################

        #region GeneraTablero
        private void CreateInitialBoard(Vector2 offset)
        {

            //crea el tablero si soy el master
            // si soy el cliente solo carga los objetos que estan instanciado desde master
            if (!PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom != null)
                return;

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
            //GameObject street;

            int idX = -1;
            int contHouse = 0;
            int contEdifice = 0;
            int contPark = 0;



            //  BUCLES QUE POSICIONA LOS EDIFICIOS, RIÓS Y CALLES
            for (int x = 0; x < xSizeBoard; x++)
            {
                for (int y = 0; y < ySizeBoard; y++)
                {
                    if (x > 1 && x < xSizeBoard - 2 && y > 1 && y < ySizeBoard - 2)
                    {
                        if (x > 2 && x % 2 != 0 && y % 2 != 0)
                        {
                            do
                            {
                                edifice = prefabs[Random.Range(0, prefabs.Count)];
                                idX = edifice.GetComponent<Edifice>().id;

                            } while ((idX == 1 && contHouse > maxHouse - 1)
                                    || (idX == 2 && contEdifice > maxEdifice - 1)
                                    || (idX == 3 && contPark > maxPark - 1));

                            if (idX == 1)
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
                            Vector3 edificePosition = new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0);

                            //GameObject newEdifice = Instantiate(currenteEdifice,positionEdifice, currenteEdifice.transform.rotation);

                            GameObject newEdifice = PhotonNetwork.Instantiate("Edifices/" + currenteEdifice.name, edificePosition, Quaternion.identity);

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

                            if (x % 2 != 0 && y % 2 == 0)           //  HORIZONTALES
                            {
                                currentStreet = streetList[Random.Range(0, 1)];
                                // newStreet.GetComponent<SpriteRenderer>().sprite = sprite;
                            }
                            else if (x % 2 == 0 && y % 2 != 0)       //  CALLES VERTICALES
                            {
                                currentStreet = streetList[Random.Range(1, 3)];
                                // newStreet.GetComponent<SpriteRenderer>().sprite = sprite;


                            }
                            else if (x % 2 == 0 && y % 2 == 0)      //  INTERCEPCIÓN CALLES CALLES 
                            {
                                currentStreet = streetList[Random.Range(3, 4)];
                                // newStreet.GetComponent<SpriteRenderer>().sprite = sprite;

                            }

                            Vector3 streetPosition = new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0);

                           // GameObject newStreet = Instantiate(currentStreet, streetPosition,currentStreet.transform.rotation);

                            GameObject newStreet = PhotonNetwork.Instantiate("Streets/" + currentStreet.name, streetPosition, Quaternion.identity);

                            // Formato al nombre de los objetos
                            newStreet.name = string.Format("Street[{0}][{1}]", x, y);

                            newStreet.transform.parent = GameObject.Find("Streets").transform;
                            //streets[x, y] = newStreet;

                            currentStreet.GetComponent<Street>().isBorder = false;
                            currentStreet.GetComponent<Street>().isCorner = false;

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



                    else if (x >= xSizeBoard - 2)
                    {
                        //  GENERACIÓN RIO
                        Vector3 riverPosition = new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0);

                        // GameObject newRiver = Instantiate(currentRiver,riverPosition,currentRiver.transform.rotation);

                        GameObject newRiver = PhotonNetwork.Instantiate("Rivers/" + currentRiver.name, riverPosition, Quaternion.identity);


                        // Formato al nombre de los objetos
                        newRiver.name = string.Format("River[{0}][{1}]", x, y);

                        if (x == xSizeBoard - 1)
                            sprite = riverList[1];
                        else
                            sprite = riverList[0];

                        newRiver.GetComponent<SpriteRenderer>().sprite = sprite;

                        newRiver.transform.parent = GameObject.Find("Rivers").transform;
                        //rivers[x, y] = newRiver;
                    }


                    else if (x <= 1 || y <= 1 || y >= ySizeBoard - 2)
                    {
                        //  GENERACIÓN BORDER
                        Vector3 borderPosition = new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0);
                        //GameObject newBorder = Instantiate(currentBorder, borderPosition,                            currentBorder.transform.rotation);

                        GameObject newBorder = PhotonNetwork.Instantiate("Borders/" + currentBorder.name,borderPosition, Quaternion.identity);

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

        // ####################################################
        // FUNCIONES COMPLEMENTARIAS EN LA CREACION DEL TABLERO
        // ####################################################

        #region FireStart/SaveEdifice
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
            for (int i = xSizeHalf - 1; i < xSizeHalf + 1; i++)
            {
                for (int j = ySizeHalf - 1; j < ySizeHalf + 1; j++)
                {
                    centralEdifices.Add(edifices[i, j]);
                }
            }

            GameObject edifice;

            // Obtener el edificio donde iniciará el fuego
            edifice = centralEdifices[Random.Range(0, centralEdifices.Count)];

            int idEdifice = edifice.GetComponent<Edifice>().id;

            edifice.GetComponent<Edifice>().CallFireStart();
        }

        #endregion // FireStart/SaveEdifice

        // ####################################################
        // FUNCIONES DIRECCIONES DEL VIENTO
        // ####################################################

        #region DirectionWind
        public void CallWindGeneration()
        {
            // Genera funciones que selecciones direcciones del viento en forma aleatoria
            int[] groupDirection = new int[3];
            int[] group1 = { 0, 2 };
            int[] group2 = { 1, 3 };

            if (PhotonNetwork.IsMasterClient)
            {
                if (directionWind == 0 || directionWind == 2)
                {
                    groupDirection[0] = directionWind;
                    groupDirection[1] = group2[0];
                    groupDirection[2] = group2[1];
                }
                else if (directionWind == 1 || directionWind == 3)
                {
                    groupDirection[0] = directionWind;
                    groupDirection[1] = group1[0];
                    groupDirection[2] = group1[1];
                }

                int i = Random.Range(0, 3);

                directionWind = groupDirection[i];
                pv.RPC("WindGeneration", RpcTarget.AllBuffered, directionWind);
            }                
        }
               
        // girar en 90 grados el viento actual, 3 posibilidades: 
        // me mantengo, giro 90 a la derecha o 90 a la izquierda
        [PunRPC]
        void WindGeneration(int direction)
        {
            directionWind = direction;       

            if (directionWind == 0)
            {
                WindNorth();
            }
            else if (directionWind == 1)
            {
                WindEast();
            }
            else if (directionWind == 2)
            {
                WindSouth();
            }
            else if (directionWind == 3)
            {
                WindWest();
            }
        }


        public void WindNorth()
        {
            SetDirectionWind(DirectionOfTheWind.north);
        }
        public void WindSouth()
        {
            SetDirectionWind(DirectionOfTheWind.south);
        }
        public void WindEast()
        {
            SetDirectionWind(DirectionOfTheWind.east);
        }
        public void WindWest()
        {
            SetDirectionWind(DirectionOfTheWind.west);
        }

        void SetDirectionWind(DirectionOfTheWind newDirectionWind)
        {
            if (newDirectionWind == DirectionOfTheWind.north)
            {
                Debug.Log("viento hacia el norte");
                currentDirectionWind = DirectionOfTheWind.north;
                directionWind = 0;
            }

            if (newDirectionWind == DirectionOfTheWind.east)
            {
                Debug.Log("viento hacia el este");
                currentDirectionWind = DirectionOfTheWind.east;
                directionWind = 1;
            }

            if (newDirectionWind == DirectionOfTheWind.south)
            {
                Debug.Log("viento hacia el sur");
                currentDirectionWind = DirectionOfTheWind.south;
                directionWind = 2;
            }            

            if (newDirectionWind == DirectionOfTheWind.west)
            {
                Debug.Log("viento hacia el oeste");
                currentDirectionWind = DirectionOfTheWind.west;
                directionWind = 3;
            }
        }
        #endregion // DirectionWind

        // ####################################################
        // FUNCIONES AVANCE DEL FUEGO
        // ####################################################

        #region FunctionsFire

        #region IncreseFire
        public void CallIncreaseFire()
        {
           if(PhotonNetwork.IsMasterClient)
                pv.RPC("IncreaseFire", RpcTarget.AllBuffered);
        }
        
        [PunRPC]
        void IncreaseFire()
        {
            Debug.Log("en el metodo increase");
            GameObject edifice;
            int contFire;
            int maxFire;

            // Si no soy el master busco a los edificios y la lista esta vacia
           if (!PhotonNetwork.IsMasterClient && allEdifices.Count == 0)
            {
                GameObject[] edifices = GameObject.FindGameObjectsWithTag("Edifice");
                for (int i = 0; i < 36; i++)
                {
                    allEdifices.Add(edifices[i]);
                }
            }
            //a los edificios que tienen fuego se le aumenta aun mas
            for (int i = 0; i < allEdifices.Count; i++)
            {
                edifice = allEdifices[i];
                contFire = edifice.GetComponent<Edifice>().contFire;
                maxFire = edifice.GetComponent<Edifice>().maxFire;

                if (contFire > 0 && contFire < maxFire)
                {
                    edifice.GetComponent<Edifice>().contFire++;
                    edifice.GetComponent<Edifice>().CallFireLevel();
                }
            }                
        }
        #endregion

        #region EdificeNeighborWithFire
        public void CallEdificeNeighborWithFire()
        {
            pv.RPC("EdificeNeighborWithFire", RpcTarget.AllBuffered);
        }

        // obtiene los edificios que son vecinos a los que tienen fuego y estan en direción del viento 
        [PunRPC]
        void EdificeNeighborWithFire()
        {
            GameObject edifice;
            GameObject neighborEdifice;
            int contFire;

            //comprobar si esto es necesario
            neighborsEdifices.Clear();

            // Si no soy el master busco a los edificios
            if (!PhotonNetwork.IsMasterClient && allEdifices.Count == 0)
            {
                GameObject[] edifices = GameObject.FindGameObjectsWithTag("Edifice");
                for (int i = 0; i < 36; i++)
                {
                    allEdifices.Add(edifices[i]);
                }
            }

            for (int i = 0; i < allEdifices.Count; i++)
            {
                edifice = allEdifices[i];
                contFire = edifice.GetComponent<Edifice>().contFire;

                if (contFire > 0)
                {
                    neighborEdifice = edifice.GetComponent<Edifice>().GetNeighborEdifice(adjacentDirections[directionWind]);

                    if (neighborEdifice != null && neighborEdifice.GetComponent<Edifice>().contFire == 0)
                    {
                        neighborsEdifices.Add(neighborEdifice); //obtengo todos los edificios donde debe iniciar fuego
                    }
                }
            }
            StartFireNeighborEdifice();
        }

        //inicia el fuego en los vecinos que estan en la dirección del viento
        public void StartFireNeighborEdifice()
        {
            GameObject edifice;
            //Aumenta el fuego en todos los edificios de la lista neighborEdifices
            for (int i = 0; i < neighborsEdifices.Count; i++)
            {
                edifice = neighborsEdifices[i];
                edifice.GetComponent<Edifice>().contFire = 1;
                
                PhotonView pvEdifice = edifice.GetComponent<PhotonView>();
                int viewID = pvEdifice.ViewID;
                edifice.GetComponent<Edifice>().CallStartFireNeighbor(viewID);
            }
        }
        #endregion

        #endregion // FunctionFire
    }

}