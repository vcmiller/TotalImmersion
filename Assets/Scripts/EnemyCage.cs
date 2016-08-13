using UnityEngine;
using System.Collections;

public class EnemyCage : MonoBehaviour {
    private MeshRenderer[] bounds;
    private Transform enemy;

	// Use this for initialization
	void Start () {
        bounds = GetComponentsInChildren<MeshRenderer>();
        enemy = GetComponentInChildren<Enemy>().transform;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 offset = enemy.position - transform.position;
        float d = Mathf.Min(Mathf.Abs(offset.x - 3), Mathf.Abs(offset.x + 3), Mathf.Abs(offset.z - 3), Mathf.Abs(offset.z + 3));
        float f;
        if (d > 1f) {
            f = 1.0f;
        } else {
            f = Mathf.Abs(d - .1f);
        }
        foreach(MeshRenderer bound in bounds) {
            bound.material.SetFloat("_Cutoff", f);
        }
	}

    public bool Inside(Vector3 v) {
        Vector3 offset = v - transform.position;
        if (Mathf.Abs(offset.x) > 3.0f || Mathf.Abs(offset.z) > 3.0f) {
            return false;
        } else {
            return true;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            enemy.GetComponent<Enemy>().alerted = true;
        }
    }
}
