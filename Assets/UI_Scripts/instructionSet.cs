using UnityEngine;
using UnityEngine.UI;

public class instructionSet : MonoBehaviour {

	public Toggle key;
	public Toggle passPin;

	void Update () {
		if (key.isOn)
			gameObject.GetComponent<Text> ().text = "For secure encryption, key accepted by the system must be of exact length i.e. 128BIT key should be of 16 characters, 192BIT key should be of 24 characters, 256BIT key should be of 32 characters";
		else if (passPin.isOn)
			gameObject.GetComponent<Text> ().text = "Password/PIN based encryption uses a 256BIT key, password must be between 8 to 32 characters long and PIN must be 6 to 32 digits long";
	}
}
