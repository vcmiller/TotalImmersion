using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    private Rigidbody rigidbody;
    public Transform head;
    public float moveSpeed = 5.0f;
    public float climbSpeed = 3.0f;
    public float mouseSensitivity = 2.0f;
    public float jumpSpeed = 2.0f;
    public float health = 100;
    private Light flashlight;

    private Weapon[] weapons;
    public int ActiveWeapon = 0;

    public bool grounded { get; private set; }

    private int ladders = 0;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        head = transform.GetChild(0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        weapons = GetComponentsInChildren<Weapon>();

        int i = 0;
        foreach (Weapon wep in weapons) {
            wep.SetActive(i++ == ActiveWeapon);
        }

        flashlight = GetComponentInChildren<Light>();
	}

    public void Damage(float damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ladder")) {
            ladders++;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Ladder")) {
            ladders--;
        }
    }

    // Update is called once per frame
    void Update () {
        rigidbody.useGravity = true;
        if (Input.GetAxis("Vertical") != 0 && ladders > 0) {
            rigidbody.useGravity = false;
            transform.position += Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * climbSpeed;
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            flashlight.enabled = !flashlight.enabled;
        }

        grounded = IsGrounded();
        Vector3 movement = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        rigidbody.MovePosition(transform.position + movement * Time.deltaTime * moveSpeed);

        transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);
        head.Rotate(-Input.GetAxis("Mouse Y") * mouseSensitivity, 0, 0, Space.Self);

        float p = head.localEulerAngles.x;
        if (p > 80 && p < 180) {
            p = 80;
        } else if (p >= 180 && p < 280) {
            p = 280;
        }
        head.localEulerAngles = new Vector3(p, 0, 0);
        if (grounded && Input.GetButtonDown("Jump")) {
            rigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
        }


        if (Input.GetButtonDown("Fire1")) {
            weapons[ActiveWeapon].SetFiring(true);
        } else if (Input.GetButtonUp("Fire1")) {
            weapons[ActiveWeapon].SetFiring(false);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            weapons[ActiveWeapon].SetFiring(false);
            weapons[ActiveWeapon].SetActive(false);
            ActiveWeapon++;
            if (ActiveWeapon >= weapons.Length) {
                ActiveWeapon = 0;
            }
            weapons[ActiveWeapon].SetActive(true);
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            weapons[ActiveWeapon].SetFiring(false);
            weapons[ActiveWeapon].SetActive(false);
            ActiveWeapon--;
            if (ActiveWeapon < 0) {
                ActiveWeapon = weapons.Length - 1;
            }
            weapons[ActiveWeapon].SetActive(true);
        }

    }

    private bool IsGrounded() {
        return Physics.SphereCast(new Ray(transform.position, Vector3.down), 0.5f, 1.1f);
    }
}
