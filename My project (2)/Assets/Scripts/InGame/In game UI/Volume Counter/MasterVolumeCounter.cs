using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeCounter : baseCounter
{
    // Update is called once per frame
    void Update()
    {
        updateCounter(AudioMixer.getMasterVolume());
    }
    public override void updateCounter(int setNewCounter)
    {
        base.updateCounter(setNewCounter);
        AudioMixer.setMasterVolume(setNewCounter);
    }
}
