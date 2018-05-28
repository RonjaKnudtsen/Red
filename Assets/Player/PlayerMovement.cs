using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;
    bool isInDirectMode = false; //Gamepad
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;
    NavMeshAgent agent;
    AICharacterControl aICharacterControl;

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform

    private void Start() {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        m_Cam = Camera.main.transform;
        agent = GetComponent<NavMeshAgent>();
        aICharacterControl = GetComponent<AICharacterControl>();

        cameraRaycaster.notifyMouseClickObservers += onMouseClick;
    }


    private void ProcessGamePadMovement() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * m_Cam.right;
        thirdPersonCharacter.Move(movement, false, false);
    }

    private void onMouseClick(RaycastHit raycastHit, int layerHit) {

        switch (layerHit) {
            case enemyLayerNumber:
                GameObject enemy = raycastHit.collider.gameObject;
                aICharacterControl.target = raycastHit.collider.gameObject.transform;
                break;
            case walkableLayerNumber:
                agent.SetDestination(raycastHit.point);
                aICharacterControl.target = null;
                break;
            default:
                Debug.LogWarning("Cant walk here?");
                aICharacterControl.SetTarget(null);
            return;
        }

    }
}

