using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class emptyInputField : MonoBehaviour {

	Toggle selection;
	public InputField entryField;
	public InputField reEntryField;

	void Start()
	{
		selection = gameObject.GetComponent<Toggle> ();

		selection.onValueChanged.AddListener (delegate {
			onValueChanged (selection);
		});
	}

	void onValueChanged (Toggle arg)
	{
		if (gameObject.GetComponent<Toggle> ().isOn == true)
		{
			entryField.text = "";
			reEntryField.text = "";
		}
	}
		
}
