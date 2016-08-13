using UnityEngine;
using System.Collections;

public class Medkit : MonoBehaviour {
    public float health = 50;

	void OnTriggerEnter(Collider col) {
        PlayerControl player = col.GetComponent<PlayerControl>();
        if (player != null && player.health < 100) {
            player.health += health;
            if (player.health > 100) {
                player.health = 100;
            }
            Destroy(gameObject);
        }
    }
}
