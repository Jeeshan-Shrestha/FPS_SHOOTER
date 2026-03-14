using UnityEngine;

public class AssaultRifle : BaseGun
{
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        gunShotSound = audioSources[0];
        reloadSound = audioSources[1];
    }
}
