using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager sharedInstance; // singleton    
    public GameObject currenteEdifice;
    public List<Sprite> prefabs = new List<Sprite>(); // lista de edificios
    public int xSize, ySize;
    private GameObject[,] edifices;

    // Start is called before the first frame update
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

        Vector2 offset = currenteEdifice.GetComponent<BoxCollider2D>().size;
        Debug.Log(offset);
        CreateInitialBoard(offset);
    }



    private void CreateInitialBoard(Vector2 offset)
    {
        edifices = new GameObject[xSize, ySize];

        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        int idX = -1;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newEdifice = Instantiate(currenteEdifice,
                    new Vector3(startX + (offset.x*x*2), startY + (offset.y*y*2), 0),
                    currenteEdifice.transform.rotation
                    );

                // Formato al nombre de los objetos
                newEdifice.name = string.Format("Edifice[{0}][{1}]", x, y);

                Sprite sprite = prefabs[Random.Range(0, prefabs.Count)];
                newEdifice.GetComponent<SpriteRenderer>().sprite = sprite;

                newEdifice.transform.parent = transform;
                edifices[x, y] = newEdifice;
            }
        }
    }
}
