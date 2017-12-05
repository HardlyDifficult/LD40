using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;

    // Use this for initialization
    void Start ()
	{
	    this.volumeSlider = this.GetComponent<Slider>();
	    this.volumeSlider.value = AudioListener.volume;
	}

    // Update is called once per frame
    void Update ()
	{
	    AudioListener.volume = this.volumeSlider.value;
	}
}
