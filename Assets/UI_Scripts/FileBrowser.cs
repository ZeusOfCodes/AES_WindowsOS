using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class FileBrowser : MonoBehaviour {
    private string _path;

	void Start()
	{
		gameObject.GetComponent<Button> ().onClick.AddListener (OnClick);
	}

	void OnClick()
	{
		if (gameObject.CompareTag ("encryptionInputFile"))
			WriteResult(StandaloneFileBrowser.OpenFilePanel("Select a File to Encrypt", "", "",false));
		else if (gameObject.CompareTag ("cipherFileFolder")) {
			var paths = StandaloneFileBrowser.OpenFolderPanel("", "", false);
			WriteResult (paths);
		} else if (gameObject.CompareTag ("keyFile"))
			WriteResult (StandaloneFileBrowser.OpenFilePanel ("Select a File Containing Key", "", "txt", false));
		else if (gameObject.CompareTag ("decryptionInputFile"))
			WriteResult (StandaloneFileBrowser.OpenFilePanel ("Select a `.cipher` File to Decrypt", "", "cipher", false));

		gameObject.GetComponentInParent<InputField> ().text = _path;
	}

    public void WriteResult(string[] paths) {
        if (paths.Length == 0) {
            return;
        }

        _path = "";
        foreach (var p in paths) {
            _path += p + "\n";
        }
    }

    public void WriteResult(string path) {
        _path = path;
    }
}