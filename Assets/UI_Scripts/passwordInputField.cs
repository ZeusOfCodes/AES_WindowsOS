using UnityEngine;
using UnityEngine.UI;

public class passwordInputField : MonoBehaviour {

	public Toggle password;
	public Toggle pin;

	void Start()
	{
		gameObject.GetComponent<InputField> ().contentType = InputField.ContentType.Password;
		if(password && pin) 
		{
			gameObject.GetComponent<InputField>().contentType = InputField.ContentType.Pin;
			password.onValueChanged.AddListener (delegate
				{
					setAsPasswordField();
				});

			pin.onValueChanged.AddListener (delegate
				{
					setAsPinField();
				});
		}
	}

	void setAsPinField()
	{
		if(pin.isOn == true)
			gameObject.GetComponent<InputField>().contentType = InputField.ContentType.Pin;
	}

	void setAsPasswordField()
	{
		if (password.isOn == true)
			gameObject.GetComponent<InputField> ().contentType = InputField.ContentType.Password;
	}
}
