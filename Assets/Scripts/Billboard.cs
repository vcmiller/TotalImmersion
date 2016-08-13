using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 v = transform.position - Camera.main.transform.position;
        v.y = 0;
        transform.forward = v;
	}
}
