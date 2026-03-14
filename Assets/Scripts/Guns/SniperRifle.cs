using UnityEngine;

public class SniperRifle: BaseGun
{
    void Start()
    {
        scopedFOV = 15;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        gunShotSound = audioSources[0];
        reloadSound = audioSources[1];
    }
}