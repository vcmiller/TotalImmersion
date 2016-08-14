using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    private PlayerControl player;

    public Text ammoCount;
    public Slider healthbar;
    public Slider battery;
    public Animator gameOver;
    public Animator gameOverBG;
    public CanvasGroup hud;

	private int heldRestart = 0;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void Update () {
        ammoCount.text = "x " + FindObjectOfType<Pistol>().ammo;
        healthbar.value = player.health / 100;
        battery.value = player.battery / 100;

        hud.alpha = player.health > 0 ? 1 : 0;

        if (player.health <= 0) {
            gameOver.SetTrigger("GameOver");
            gameOverBG.SetTrigger("GameOver");
        }


		if (Input.GetKey (KeyCode.R)) {
			heldRestart++;
			print (heldRestart);
			if (heldRestart > 60) {
				Application.LoadLevel (Application.loadedLevel);
			}
		} else {
			heldRestart = 0;
		}
	}
}
