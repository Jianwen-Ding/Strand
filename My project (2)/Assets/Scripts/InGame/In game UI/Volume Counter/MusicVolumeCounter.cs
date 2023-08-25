using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumeCounter : baseCounter
{
    // Update is called once per frame
    void Update()
    {
        setCurrentCount(AudioMixer.getMusicVolume());
    }
}
