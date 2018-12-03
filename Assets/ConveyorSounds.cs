using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSounds : MonoBehaviour {

	AudioSource conveyorScreamer;

	// Use this for initialization
	void Start () 
	{
		conveyorScreamer = GetComponent<AudioSource>();	
		StartCoroutine(loop());
	}
	
	IEnumerator loop ()
	{
		yield return new WaitForSecondsRealtime(5);
		conveyorScreamer.Play();
	}
}
