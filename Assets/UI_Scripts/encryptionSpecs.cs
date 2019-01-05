using System.IO;
using UnityEngine;
using UnityEngine.UI;
using verification; 

public class encryptionSpecs : MonoBehaviour {

	public Button Encryption;
	public Button Decryption;
	public Button About;

	public GameObject ciphDescPanel;
	public GameObject encryptionPanel;
	public Text systemComplaints;
	public Text messageLabel;

	public InputField filePath;		    private string filePathString;
	public InputField cipherFileDir;    private string cipherFileDirString;

	public InputField keyFilePath;      private string keyFilePathString;
	public InputField paPiEntry;        private string paPiEntryString;
	public InputField paPiReEntry;      private string paPiReEntryString;

	public Toggle passwordPin; /*0*/    private int pass_or_key;      
	public Toggle key;  /*1*/
	public Toggle password;				private int threshold;
	public Toggle pin;
	public Toggle AES128;  /*128*/      private int keyLength;
	public Toggle AES192;  /*192*/
	public Toggle AES256;  /*256*/

	public Text visibleFilePath;

	private bool validFilePath = false;
	private bool validDirectory = false;
	private bool validPasswords = false;
	private bool validKeyFile = false;
	private int percentageCompletion;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button> ().onClick.AddListener (OnClick);
		systemComplaints.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnClick(){

		validFilePath = false;
		validDirectory = false;
		validPasswords = false;
		validKeyFile = false;

		if (key.isOn) {
			if (AES128.isOn)
				keyLength = 128;
			else if (AES192.isOn)
				keyLength = 192;
			else if (AES256.isOn)
				keyLength = 256;
		} else if (password.isOn) {
			threshold = 8;
			keyLength = 256;
		} else if (pin.isOn) {
			threshold = 6;
			keyLength = 256;
		}

		systemComplaints.text = "";

		filePathString = filePath.text;
		if (string.IsNullOrEmpty (filePathString))
			systemComplaints.text += "> Provide the location of the file you want to encrypt.\n";
		else if(!File.Exists(filePathString))
			systemComplaints.text += "> File you specified to be encrypted doesn't exists, provide a valid file path.\n";
		else
			validFilePath = true;
		
		cipherFileDirString = cipherFileDir.text;
		if (string.IsNullOrEmpty (cipherFileDirString))
			systemComplaints.text += "> Provide location of the directory in which you want to save the encrypted file.\n";
		else if (!Directory.Exists (cipherFileDirString))
			systemComplaints.text += "> Directory you specified to place the .cipher file doesn't exists, provide a valid directory.\n";
		else
			validDirectory = true;

		if (passwordPin.isOn) {
			paPiEntryString = paPiEntry.text;
			paPiReEntryString = paPiReEntry.text;

			if (!paPiEntryString.Equals (paPiReEntryString) || string.IsNullOrEmpty (paPiEntryString) || string.IsNullOrEmpty (paPiReEntryString)) {
				if (!paPiEntryString.Equals (paPiReEntryString))
					systemComplaints.text += "> Password/PIN mismatch.\n";
				else if (string.IsNullOrEmpty (paPiEntryString) && string.IsNullOrEmpty (paPiReEntryString))
					systemComplaints.text += "> Enter a password/PIN.\n";
			}

			if (verifyKeyPasswords.VerifyPassPin (paPiEntryString,threshold) != 0) {
				if(verifyKeyPasswords.VerifyPassPin (paPiEntryString, threshold) > 0)
					systemComplaints.text += "> Too long password/PIN.\n";
				else if(verifyKeyPasswords.VerifyPassPin (paPiEntryString, threshold) < 0)
					systemComplaints.text+= "> Too short password/PIN.\n";
			}
			else
				validPasswords = true;
		}

		else if (key.isOn) {
			keyFilePathString = keyFilePath.text;
			if (string.IsNullOrEmpty (keyFilePathString))
				systemComplaints.text += "> Enter the location of the key file (must be a .txt file).\n";
			else if (!File.Exists (keyFilePathString))
				systemComplaints.text += "> Key file you specified doesn't exists, provide a valid key file path.\n";
			else if (!verification.verifyKeyPasswords.verifyKey (keyLength, keyFilePathString))
				systemComplaints.text += "> Invalid key, key length should exactly match the key length of the selected encryption mode.\n";
			else
				validKeyFile = true;
		}

		if (validFilePath && validDirectory && (validPasswords || validKeyFile))
		{
			visibleFilePath.text = filePathString;
			Encryption.interactable = false;
			Decryption.interactable = false;
			About.interactable = false;

			ciphDescPanel.SetActive(false);
			encryptionPanel.SetActive(true);
			string empty = "_EMPTY";

			if (encryptionPanel.activeInHierarchy == true) {
				if (passwordPin.isOn)
					cipherCallBacks.__CIPHER (filePathString, cipherFileDirString, paPiEntryString, empty, keyLength);
				else if (key.isOn)
					cipherCallBacks.__CIPHER (filePathString, cipherFileDirString, empty, keyFilePathString, keyLength);
			}
		}
	}
		
}