using UnityEngine;

public class AssaultRifle : BaseGun
{
    void Start()
    {
        scopedFOV = 40;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        gunShotSound = audioSources[0];
        reloadSound = audioSources[1];
    }
}
