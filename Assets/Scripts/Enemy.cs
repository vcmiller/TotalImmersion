﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    private Transform target;
    private float alertness = 0;
    private NavMeshAgent agent;
    private Animator sprite;
    private bool dead = false;
    public AudioClip hurtSound;

    public Waypoint patrolTarget;

    public float Health = 100;

    private CooldownTimer usePunch;
    private ExpirationTimer punching;

    private Vector3 Direction() {
        if (agent.velocity.sqrMagnitude < 1) {
            return transform.forward;
        } else {
            return agent.velocity;
        }
    }

    // Use this for initialization
    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        sprite = GetComponentInChildren<Animator>();

        usePunch = new CooldownTimer(1.0f);
        punching = new ExpirationTimer(0.5f);
    }

    private bool reachedDest() {
        return Vector3.SqrMagnitude(transform.position - patrolTarget.transform.position) < .1f;
    }

    public void Damage(float damage) {
        Health -= damage;
        Alert(1.0f);

        if (!dead) {
            AudioSource.PlayClipAtPoint(hurtSound, transform.position);
        }

        if (Health <= 0) {
            Die();
        }
    }

    public void Die() {
        if (!dead) {
            dead = true;
            sprite.SetBool("Dead", true);
            agent.enabled = false;

            foreach (AudioSource src in GetComponents<AudioSource>()) {
                Destroy(src);
            }
        }
    }

    public bool PlayerInView() {
        Vector3 toPlayer = target.position - transform.position;
        Vector3 forward = Direction();

        if (Vector3.SqrMagnitude(toPlayer) < 400 && Vector3.Angle(toPlayer, forward) < 45) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, toPlayer, out hit)) {
                PlayerControl player = hit.collider.GetComponent<PlayerControl>();
                if (player != null) {
                    return true;
                }
            }
        }

        return false;
    }

    public void Alert(float amount) {
        float f = alertness;
        alertness += amount;
        if (alertness >= 1.0f && f < 1.0f) {
            GetComponent<AudioSource>().Play();
        }
    }

    // Update is called once per frame
    void Update() {
        Vector3 toPlayer = transform.position - target.position;
        Vector3 forward = Direction();

        toPlayer.y = 0;
        toPlayer = toPlayer.normalized;
        forward.y = 0;
        forward = forward.normalized;


        sprite.transform.forward = toPlayer;

        if (agent.velocity.sqrMagnitude == 0 && !dead && punching.Expired) {
            sprite.speed = 0;
        } else {
            sprite.speed = 1;
        }

        if (!dead) {

            if (alertness < 1.0f && PlayerInView()) {
                float r = 10.0f;
                if (target.GetComponentInChildren<Light>().enabled) {
                    r = 20.0f;
                }
                Alert(r / Vector3.Magnitude(transform.position - target.position));
            }

            float d = Vector3.Dot(toPlayer, forward);
            Vector3 c = Vector3.Cross(toPlayer, forward);
            if (d > .71) {
                sprite.SetInteger("Dir", 2);
            } else if (d > -0.71) {
                if (c.y > 0) {
                    sprite.SetInteger("Dir", 1);
                } else {
                    sprite.SetInteger("Dir", 3);
                }
            } else {
                sprite.SetInteger("Dir", 0);
            }

            sprite.SetBool("Punching", !punching.Expired);

            


            if (alertness >= 1.0f) {
                Vector3 lastPos = transform.position;
                Vector3 targetPos;
                
                targetPos = target.position;

                if (Vector3.SqrMagnitude(transform.position - target.position) < 4.0f && usePunch.Use) {
                    punching.Set();
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, target.position - transform.position, out hit, 2.0f)) {
                        print(hit.transform.name);
                        PlayerControl player = hit.transform.GetComponent<PlayerControl>();
                        if (player != null) {
                            player.Damage(25);
                        }
                    }
                }
                
                agent.destination = targetPos;
                
            } else if (patrolTarget != null) {
                agent.destination = patrolTarget.transform.position;

                if (reachedDest()) {
                    patrolTarget = patrolTarget.next;
                }
            }
        }
    }
}
