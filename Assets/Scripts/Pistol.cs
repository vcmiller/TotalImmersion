using UnityEngine;
using System.Collections;

public class Pistol : Weapon {
    public int ammo = 5;
    public AudioClip shotSound;

    public override void SetFiring(bool firing) {
        if (ammo > 0 || !firing) {
            base.SetFiring(firing);

            if (firing && ammo > 0) {
                ammo--;

                foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
                    enemy.Alert(40.0f / Vector3.Distance(transform.position, enemy.transform.position));
                }


                AudioSource.PlayClipAtPoint(shotSound, transform.position);
                RaycastHit hit;
                if (Physics.Raycast(player.head.position, player.head.forward, out hit)) {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null) {
                        enemy.Damage(100);
                    }
                }
            }
        }
    }
}
