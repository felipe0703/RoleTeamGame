using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapInteractive : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform mainCam;
    public Transform pointer;

    public void OnPointerClick(PointerEventData EventData)
    {
        float panelWidth = gameObject.GetComponent<RectTransform>().rect.width;
        float panelHeight = gameObject.GetComponent<RectTransform>().rect.height;
        float minimapPosX = EventData.pressPosition.x - (Screen.width - panelWidth) / 2;
        float minimapPosY = EventData.pressPosition.y - (Screen.height - panelHeight) / 2;

        Vector3 newPos = Vector3.zero;

        float scale = panelWidth / 120f;
        float diffMinimapX = minimapPosX - (panelWidth / 2);
        float diffMinimapY = minimapPosY - (panelHeight / 2);
        float minimapToWorldX = diffMinimapX / scale;
        float minimapToWorldY = diffMinimapY / scale;
        
        newPos = new Vector3(mainCam.position.x + minimapToWorldX, 
                            mainCam.position.y + minimapToWorldY, 
                            0f);     

        pointer.position = newPos;

       // pointer.GetComponent<PointerMinimap>().ChangeCam();
    }


}