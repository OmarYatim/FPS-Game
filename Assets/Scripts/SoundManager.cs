using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface ISoundManager
{
    AudioClip AimSound { get; }
    AudioClip TakeOutWeaponSound { get; }
    AudioClip ShootSound { get; }
    AudioClip ReloadSound(bool isHandGun);
    AudioClip CasingSound { get; }
    AudioClip ImpactSound { get; }
}
public class SoundManager : MonoBehaviour, ISoundManager
{
    [SerializeField] private AudioClip aimSound;
    [SerializeField] private AudioClip takeOutWeaponSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip assaultRifleReloadSound;
    [SerializeField] private AudioClip handGunReloadSound;
    [SerializeField] private AudioClip casingSound;
    [SerializeField] private AudioClip impactSound;

    [HideInInspector] public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioClip AimSound
    {
        get { return aimSound; }
    }

    public AudioClip TakeOutWeaponSound
    {
        get { return takeOutWeaponSound; }
    }

    public AudioClip ShootSound
    {
        get { return shootSound; }
    }

    public AudioClip ReloadSound(bool isHandGun)
    {
        if (isHandGun)
            return handGunReloadSound;
        else
            return assaultRifleReloadSound;
    }

    public AudioClip CasingSound
    {
        get { return casingSound; }
    }

    public AudioClip ImpactSound
    {
        get { return impactSound; }
    }
}
