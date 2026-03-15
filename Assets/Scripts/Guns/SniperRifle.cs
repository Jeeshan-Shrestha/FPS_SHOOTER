using UnityEngine;

public class SniperRifle: BaseGun
{
    void Start()
    {
        scopedFOV = 10;
        bulletVelocity = 500;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        gunShotSound = audioSources[0];
        reloadSound = audioSources[1];
    }
}