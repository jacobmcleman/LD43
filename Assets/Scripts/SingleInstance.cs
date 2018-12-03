using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingleInstance : MonoBehaviour {

    private void Start()
    {
       SingleInstance[] found = FindObjectsOfType<SingleInstance>();
        foreach (SingleInstance instance in found)
        {
            if (instance.gameObject.name == gameObject.name && instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
