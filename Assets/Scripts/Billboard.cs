using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
    public bool roll = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (roll) {
            transform.rotation = Camera.main.transform.rotation;
        } else {
            Vector3 v = transform.position - Camera.main.transform.position;
            v.y = 0;
            transform.forward = v;
        }
	}
}
