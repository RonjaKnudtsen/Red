using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class CameraRaycaster : MonoBehaviour
{
	// INSPECTOR PROPERTIES RENDERED BY CUSTOM EDITOR SCRIPT
	[SerializeField] int[] layerPriorities;

    float maxRaycastDepth = 100f; // Hard coded value
	int topPriorityLayerLastFrame = -1; // So get ? from start with Default layer terrain

	// Setup delegates for broadcasting layer changes to other classes
    public delegate void OnCursorLayerChange(int newLayer); // declare new delegate type
    public event OnCursorLayerChange notifyLayerChangeObservers; // instantiate an observer set

	public delegate void OnClickPriorityLayer(RaycastHit raycastHit, int layerHit); // declare new delegate type
	public event OnClickPriorityLayer notifyMouseClickObservers; // instantiate an observer set

    //Now we have two delegates, one for OnCursorLayerChange and one for OnClickPriorityLayer

    void Update()
	{
		// First thing to check::
        // Check if pointer is over an interactable UI element !
		if (EventSystem.current.IsPointerOverGameObject ())
		{
			NotifyObserersIfLayerChanged (5);
			return; // Stop looking for other objects
		}

        //we will check everything under the mouse pointer until max depth. It will return an array of everything it hits.
		// Raycast to max depth, every frame as things can move under mouse
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit[] raycastHits = Physics.RaycastAll (ray, maxRaycastDepth);

        //If we got a hit, find the top priority hits. 
        RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);

        // if hit NO priority object
        if (!priorityHit.HasValue){
			NotifyObserersIfLayerChanged (0); // broadcast default layer
			return;
		}

		// Otherwise: Notify delegates of layer change
		var layerHit = priorityHit.Value.collider.gameObject.layer;
		NotifyObserersIfLayerChanged(layerHit);

        // If mouse also is CLICKED: 
        // Notify delegates of highest priority game object under mouse when clicked
		if (Input.GetMouseButton (0)){
			notifyMouseClickObservers (priorityHit.Value, layerHit);
		}
	}

	void NotifyObserersIfLayerChanged(int newLayer)
	{
		if (newLayer != topPriorityLayerLastFrame)
		{
			topPriorityLayerLastFrame = newLayer;
			notifyLayerChangeObservers (newLayer);
		}
	}

	RaycastHit? FindTopPriorityHit (RaycastHit[] raycastHits)
	{
		// Form list of layer numbers hit
		List<int> layersOfHitColliders = new List<int> ();
		foreach (RaycastHit hit in raycastHits)
		{
			layersOfHitColliders.Add (hit.collider.gameObject.layer);
		}

		// Step through layers in order of priority looking for a gameobject with that layer
		foreach (int layer in layerPriorities)
		{
			foreach (RaycastHit hit in raycastHits)
			{
				if (hit.collider.gameObject.layer == layer)
				{
					return hit; // stop looking
				}
			}
		}
		return null; // because cannot use GameObject? nullable
	}
}