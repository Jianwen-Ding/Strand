using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXVolumeCounter : baseCounter
{
    // Update is called once per frame
    void Update()
    {
        updateCounter(AudioMixer.getSFXVolume());
    }
    public override void updateCounter(int setNewCounter)
    {
        base.updateCounter(setNewCounter);
        AudioMixer.setSFXVolume(setNewCounter);
    }
}
