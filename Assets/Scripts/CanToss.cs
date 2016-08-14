using UnityEngine;
using System.Collections;

public class CanToss : Weapon {
    private CooldownTimer punch;
    public AudioClip punchSound;
    public GameObject canPrefab;

    new public void Awake() {
        base.Awake();
        punch = new CooldownTimer(.7f);
    }

    public override void SetFiring(bool firing) {
        if (punch.CanUse || !firing) {
            base.SetFiring(firing);

            if (firing && punch.Use) {
                GameObject can = (GameObject)Instantiate(canPrefab, transform.position + transform.parent.forward, Quaternion.identity);
                can.GetComponent<Rigidbody>().velocity = transform.parent.forward * 10;
                AudioSource.PlayClipAtPoint(punchSound, transform.position);
            }
        }
    }
}
