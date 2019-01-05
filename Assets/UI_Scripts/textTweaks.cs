using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class textTweaks : MonoBehaviour {

	public float updateDelay = 1.0f;

	IEnumerator Start(){
		
		Text textUpdation = gameObject.GetComponent<Text> ();
		int i = 0;
		while (i < 2) {
			i += 1;
			textUpdation.text = "";
			yield return new WaitForSeconds (updateDelay);
			textUpdation.text = "_";
			yield return new WaitForSeconds (updateDelay);
		}
		SceneManager.LoadScene (1);
	}
}
