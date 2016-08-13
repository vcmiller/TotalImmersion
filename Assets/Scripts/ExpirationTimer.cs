
using UnityEngine;

public class ExpirationTimer {
	public float Expiration { get; set; }
	public float LastSet { get; set; }

	public bool Expired {
		get {
			return Time.time - LastSet > Expiration;
		}
	}

    public float Remaining {
        get {
            return Mathf.Max(0, Expiration - (Time.time - LastSet));
        }
    }

	public ExpirationTimer (float expiration) {
		Expiration = expiration;
		Clear ();
	}

	public void Set() {
		LastSet = Time.time;
	}

	public void Clear() {
		LastSet = Time.time - Expiration;
	}
}

