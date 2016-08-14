using UnityEngine;
using System.Collections;

public class Fist : Weapon {
    private CooldownTimer punch;
    public AudioClip punchSound;

    new public void Awake() {
        base.Awake();
        punch = new CooldownTimer(.7f);
    }

    public override void SetFiring(bool firing) {
        if (punch.CanUse || !firing)
        base.SetFiring(firing);

        if (firing && punch.Use) {

            AudioSource.PlayClipAtPoint(punchSound, transform.position);
            RaycastHit hit;
            if (Physics.Raycast(player.head.position, player.head.forward, out hit, 2.0f)) {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null) {
                    enemy.Damage(enemy.alertness > 1 ? 50 : 200);
                }
            }
        }
    }
}
