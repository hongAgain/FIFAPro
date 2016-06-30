using UnityEngine;
using System.Collections;

public class UIMaskBlink : MonoBehaviour {

	private UISprite uiSprite;

	private float blinkSpeed = 1 * Mathf.PI;

	void Start () {
		uiSprite = GetComponent<UISprite>();
	}

	void Update () {
		// between 30% ~ 90%
		uiSprite.alpha = (float)((Mathf.Sin(Time.time * blinkSpeed) + 2) * 0.3);
	}
}
