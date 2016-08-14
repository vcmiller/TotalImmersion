using UnityEngine;
using System.Collections;

public class Can : MonoBehaviour {
    public AudioClip hitSound;

	void OnCollisionEnter(Collision col) {
        if (col.collider.GetComponent<Rigidbody>() == null) {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject, 3.0f);

            foreach(Enemy enemy in FindObjectsOfType<Enemy>()) {
                if (Vector3.SqrMagnitude(transform.position - enemy.transform.position) < 15 * 15) {
                    enemy.investigate = true;
                    enemy.investigatePoint = transform.position;
                }
            }
        }
    }
}
