using UnityEngine;
using System.Collections;

public class Pistol : Weapon {
    public int ammo = 5;

    public override void SetFiring(bool firing) {
        if (ammo > 0 || !firing) {
            base.SetFiring(firing);

            if (firing && ammo > 0) {
                ammo--;

                RaycastHit hit;
                if (Physics.Raycast(player.head.position, player.head.forward, out hit)) {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null) {
                        enemy.Damage(50);
                    }
                }
            }
        }
    }
}
