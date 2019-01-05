using UnityEngine;
using UnityEngine.UI;

public class buttonTweak : MonoBehaviour {

	public RectTransform other;
	Vector3 otherPos;

	Vector3 originalPosition;
	Vector3 alteredPos;

	float offset = 50f;

	void Start () {
		gameObject.GetComponent<Button> ().onClick.AddListener (OnClick);
		originalPosition = gameObject.GetComponent<RectTransform> ().position;
		otherPos = other.position;
	}
	
	void OnClick()
	{
		alteredPos = gameObject.GetComponent<RectTransform> ().position;
		if (originalPosition == alteredPos) {
			alteredPos.x += offset;
			gameObject.GetComponent<RectTransform> ().position = alteredPos;
		}

		if (otherPos != originalPosition) {
			otherPos.x = originalPosition.x;
			other.position = otherPos;
		}
	}

}
