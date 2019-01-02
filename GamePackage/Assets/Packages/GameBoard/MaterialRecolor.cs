using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialRecolor : MonoBehaviour {
  public Color ChangeTo = Color.black;
	// Use this for initialization
	void Awake ()
	{
	  this.GetComponent<Renderer>().material.color = this.ChangeTo;
	}
}
