using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAffordance : MonoBehaviour {
    CameraRaycaster cameraRayCaster;
    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Texture2D disabledCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(1, 1);
    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;


    // Use this for initialization
    void Start () {
        cameraRayCaster = GetComponent<CameraRaycaster>();
        Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto); //Initialize with standard cursor. 
        cameraRayCaster.notifyLayerChangeObservers += OnLayerChange; //Observe layerchanges
    }

    void OnLayerChange(int newLayer) {
        print("NEW LAYER:"+ newLayer);
        switch (newLayer) {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(disabledCursor, cursorHotspot, CursorMode.Auto);
            return;
        }
    }
}
