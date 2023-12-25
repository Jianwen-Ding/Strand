using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumeCounter : baseCounter
{
    // Update is called once per frame
    void Update()
    {
        updateCounter(AudioMixer.getMusicVolume());
    }
    public override void updateCounter(int setNewCounter)
    {
        base.updateCounter(setNewCounter);
        AudioMixer.setMusicVolume(setNewCounter);
    }
}
