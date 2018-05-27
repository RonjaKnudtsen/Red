using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Walkable
    };

    [SerializeField] float distanceToBackground = 100f; //Max raycast background
    Camera viewCamera;
    RaycastHit raycastHit;

    public RaycastHit hit //getter
    {
        get { return raycastHit; }
    }

    public delegate void OnLayerChange(Layer newLayer); //Declare new delegate type 
                                          //A delegate references methods of particular list and return type.
                                          //In our case functions. 
                                          //Everything in the delegate list gets executed when the delegate is executed. 
                                          //Calling the delegate can also send pass variables to the observers

    public event OnLayerChange layerChangeObservers; // Instantiate an observer set.
    //Also: Set this as event so we cant overwrite the layerchangeobservers. They need to be appended. += instead of =

    Layer layerhit;
    public Layer currentLayerHit
    {
        get { return layerhit; }
    }

    void Start() // TODO Awake?
    {
        viewCamera = Camera.main;
        
    }

    void Update()
    {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                raycastHit = hit.Value;
                if(layerhit != layer) {
                    layerhit = layer;
                    layerChangeObservers(layer);
                }
                
                return;
            }
        }

        // Otherwise return background hit
        raycastHit.distance = distanceToBackground;
        layerhit = Layer.RaycastEndStop;
    }

    RaycastHit? RaycastForLayer(Layer layer) //questionmark makes it nullable. (possible to return null)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation Make bit layer
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}
