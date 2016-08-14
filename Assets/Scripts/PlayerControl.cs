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
    public float battery = 100;

    public AudioClip hurtSound;

    private Weapon[] weapons;
    public int ActiveWeapon = 0;

    public bool grounded { get; private set; }

    private int ladders = 0;
    public bool batteryCharging = false;

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
        } else {
            AudioSource.PlayClipAtPoint(hurtSound, transform.position);
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
        if (ladders > 0) {
            rigidbody.useGravity = false;
            if (Input.GetAxis("Vertical") != 0) {
                transform.position += Vector3.up * Time.deltaTime * climbSpeed;
            } else {
                transform.position += Vector3.down * Time.deltaTime * climbSpeed;
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && !batteryCharging) {
            flashlight.enabled = !flashlight.enabled;
        }

        if (flashlight.enabled) {
            if (batteryCharging) {
                flashlight.enabled = false;
            }
            battery = Mathf.Max(battery - Time.deltaTime * 5, 0);
            if (battery == 0) {
                batteryCharging = true;
            }
        }

        if (!flashlight.enabled) {
            battery = Mathf.Min(battery + Time.deltaTime * 8, 100);
            if (battery == 100) {
                batteryCharging = false;
            }
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
