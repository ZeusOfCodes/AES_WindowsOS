using System.IO;
using UnityEngine;
using UnityEngine.UI;
using verification;

public class decryptionSpecs : MonoBehaviour {

	public InputField cipherFile;
	public InputField destFolder;
	public InputField keyFile;
	public InputField passPin;
	public Text systemComplaints;

	public Button Encryption;
	public Button Decryption;
	public Button About;

	public GameObject decipherDespPanel;
	public GameObject decryptionPanel;
	public GameObject invalidKeyPanel;
	public GameObject progressBarDisplay;

	public Text visibleFilePath;

	string cipherFilePath;
	string destDirectory;
	string keyFilePath;
	string passPinString;

	bool validDestFolder = false;
	bool validCipherFile = false;
	bool validKeyFile = false;
	bool validPassPin = false;
	bool headsUp = false;

	int threshold;
	int keyLength;

	void Start () {
		gameObject.GetComponent<Button> ().onClick.AddListener (OnClick);
		systemComplaints.text = "";
	}

	void OnClick(){

		validDestFolder = false;
		validCipherFile = false;
		validKeyFile = false;
		validPassPin = false;
		headsUp = false;

		systemComplaints.text = "";

		cipherFilePath = cipherFile.text;
		if (string.IsNullOrEmpty (cipherFilePath))
			systemComplaints.text += "> Enter path of the file to be decypted.\n";
		else if (!File.Exists (cipherFilePath))
			systemComplaints.text += "> File you specified for decryption doesn't exists, enter a valid file path.\n";
		else
			validCipherFile = true;

		destDirectory = destFolder.text;
		if (string.IsNullOrEmpty (destDirectory))
			systemComplaints.text += "> Specify a destination folder for the decrypted file.\n";
		else if (!Directory.Exists (destDirectory))
			systemComplaints.text += "> Directory you specified for decrypted file doesn't exists, specify a valid directory.\n";
		else
			validDestFolder = true;

		keyFilePath = keyFile.text;
		passPinString = passPin.text;

		if (string.IsNullOrEmpty (keyFilePath) && string.IsNullOrEmpty (passPinString))
			systemComplaints.text += "> Either specify a key file or password/PIN to decrypt the file.\n";
		else if (!string.IsNullOrEmpty (keyFilePath) && !string.IsNullOrEmpty (passPinString))
			systemComplaints.text += "> Either specify a key file or password/PIN you have filled both fields.\n";
		else {
			if (string.IsNullOrEmpty (keyFilePath))
				validPassPin = true;
			else
				validKeyFile = true;
		}

		if (string.IsNullOrEmpty (keyFilePath)) {
			threshold = 8;
			keyLength = 256;
		} else if (string.IsNullOrEmpty (passPinString)) {
			verifyKeyPasswords.verifyKey (keyFilePath,ref keyLength);
		}

		if(validDestFolder && validCipherFile && (validKeyFile || validPassPin) && !(validKeyFile && validPassPin))
		{
			bool validPass = false;
			bool validKey = false;
			if (string.IsNullOrEmpty (keyFilePath)) {
				threshold = 6;
				keyLength = 256;
				if (verifyKeyPasswords.VerifyPassPin (passPinString, threshold) == 0)
					validPass = true;
				else if (verifyKeyPasswords.VerifyPassPin (passPinString, threshold) < 0)
					systemComplaints.text = "> Invalid Password/PIN, password should be atleast 8 characters long and PIN should be atleast 6 characters long.";
				else if (verifyKeyPasswords.VerifyPassPin (passPinString, threshold) > 0)
					systemComplaints.text = "> Invalid Password/PIN, password/PIN can be atmost 32 characters long.";
			} else if (string.IsNullOrEmpty (passPinString)) {
				if (!verifyKeyPasswords.verifyKey (keyFilePath,ref keyLength))
					systemComplaints.text = "> Invalid key, key size should discretely be 16, 24 or 32 characters long.";
				else
					validKey = true;
				}

			if(validKey || validPass)
				headsUp = true;
		}
	
			if (headsUp == true) {
				
				visibleFilePath.text = cipherFilePath;

				Encryption.interactable = false;
				Decryption.interactable = false;
				About.interactable = false;

				decipherDespPanel.SetActive (false);
				decryptionPanel.SetActive (true);
			if (decryptionPanel.activeInHierarchy == true) {
				if (string.IsNullOrEmpty (passPinString)) {
					cipherCallBacks.__INV_CIPHER (cipherFilePath, destDirectory, "_EMPTY", keyFilePath, keyLength);
					if (cryptServCS.AES_CxDecipher.incorrectKey) {
						Encryption.interactable = true;
						Decryption.interactable = true;
						invalidKeyPanel.SetActive (true);
						invalidKeyPanel.GetComponentInChildren<Text> ().text = " Either the key is incorrect or " + Path.GetFileName (cipherFilePath) + " file is corrupted.";
					} else
						progressBarDisplay.SetActive (true);
				} else if (string.IsNullOrEmpty (keyFilePath)) {
					cipherCallBacks.__INV_CIPHER (cipherFilePath, destDirectory, passPinString, "_EMPTY", 256);
					if (cryptServCS.AES_CxDecipher.incorrectKey) {
						Encryption.interactable = true;
						Decryption.interactable = true;
						invalidKeyPanel.SetActive (true);
						invalidKeyPanel.GetComponentInChildren<Text> ().text = " Either the password/PIN is incorrect or " + Path.GetFileName (cipherFilePath) + " file is corrupted.";
					} else
						progressBarDisplay.SetActive (true);
				}
			}
			}
		}
		
}