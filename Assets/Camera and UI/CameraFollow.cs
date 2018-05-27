using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    GameObject player;
    public float speed = 2.0F;

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        this.transform.position = player.transform.position;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        //this.transform.position = player.transform.position;
        this.transform.position = Vector3.Lerp(this.transform.position, player.transform.position, (Time.deltaTime * speed));
    }
}
