using ProgressBar;
using System.Collections;
using UnityEngine;

public class progressScript : MonoBehaviour 
{ 
	ProgressRadialBehaviour BarBehaviour;
	public static float barValue;

	float UpdateDelay = 0.00f;

	IEnumerator Start ()
	{ 
		BarBehaviour = GetComponent<ProgressRadialBehaviour>();
		while (true)
		{ 
			yield return new WaitForSeconds(UpdateDelay);
			BarBehaviour.Value = cryptServCS.AES_CxDecipher.percentCompleted;
		}
	}
}