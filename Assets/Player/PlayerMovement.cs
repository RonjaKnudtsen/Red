using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 5.0f;

    bool isInDirectMode = false; //Gamepad
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        m_Cam = Camera.main.transform;
    }

    // Fixed update is called in sync with physics
    ////private void FixedUpdate()
    ////{
    ////    if (Input.GetKeyDown(KeyCode.G)) { //g for gamepad //TODO: allow player to remap keys
    ////        isInDirectMode = !isInDirectMode;
    ////        currentDestination = transform.position;
    ////    }

    ////    if (isInDirectMode) {
    ////        this.ProcessGamePadMovement();
    ////    } else {
    ////        this.ProcessMouseMovement();
    ////    }
    ////}

    private void ProcessGamePadMovement() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * m_Cam.right;
        thirdPersonCharacter.Move(movement, false, false);
    }


    //private void ProcessMouseMovement() {
    //    if (Input.GetMouseButton(0)) {
    //        Layer layerHit = cameraRaycaster.currentLayerHit;
    //        clickPoint = cameraRaycaster.hit.point;

    //        switch (cameraRaycaster.currentLayerHit) {
    //            case Layer.Walkable:
    //                currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
    //                break;
    //            case Layer.Enemy:
    //            currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
    //            break;
    //            default:
    //                print("No layer found");
    //                return;
    //        }

    //    }
    //    WalkToDestination();
    //}

    private void WalkToDestination() {
        var playerToClickpoint = currentDestination - transform.position;
        if (playerToClickpoint.magnitude >= walkMoveStopRadius) {
            thirdPersonCharacter.Move(playerToClickpoint, false, false);
        } else {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening) {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);

        Gizmos.color = new Color(255f, 0f, 0f, .3f);
        Gizmos.DrawWireSphere(currentDestination, attackMoveStopRadius);
    }
}

