using UnityEngine;
using System.Collections;

public class EndTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
        Application.LoadLevel("End-tro");
    }
}
