using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchFullscreen : MonoBehaviour {

    public static StretchFullscreen Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize (float width, float height) {
        GetComponent<Camera>().aspect = width / height;
	}
	
	
}
