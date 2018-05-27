using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAffordance : MonoBehaviour {
    CameraRaycaster cameraRayCaster;
    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Texture2D disabledCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(1, 1);

	// Use this for initialization
	void Start () {
        cameraRayCaster = GetComponent<CameraRaycaster>();
        Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto); //Initialize with standard cursor. 
        cameraRayCaster.layerChangeObservers += OnLayerChange; //Observe layerchanges
    }

    void OnLayerChange(Layer newLayer) {
        switch (newLayer) {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(disabledCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Debug.LogError("No layer found. Error setting cursor");
                return;
        }
    }
}
