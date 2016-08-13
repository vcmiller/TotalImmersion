using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
    protected bool active;
    protected bool firing;
    protected Animator anim;
    protected SpriteRenderer sprite;

    protected PlayerControl player;

    public void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        player = transform.root.GetComponent<PlayerControl>();
    }

	public virtual void SetFiring(bool firing) {
        this.firing = firing;
        if (firing) {
            anim.SetTrigger("Shoot");
        }
    }

    public virtual void SetActive(bool active) {
        sprite.enabled = active;
        this.active = active;
    }
}
