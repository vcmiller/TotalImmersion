using UnityEngine;
using System.Collections;

public class AmmoBox : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
        PlayerControl player = col.GetComponent<PlayerControl>();
        if (player != null) {
            FindObjectOfType<Pistol>().ammo += 5;
            Destroy(gameObject);
        }
    }
}
