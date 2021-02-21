using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactScript : MonoBehaviour
{
    [SerializeField] private float DespawnTime = 5.0f;
    private AudioSource ImpactSource;
    ISoundManager sound;

    void Start()
    {
        sound = SoundManager.Instance;
        ImpactSource = GetComponent<AudioSource>();
        ImpactSource.clip = sound.ImpactSound;
        ImpactSource.PlayDelayed(0.1f);
        Destroy(gameObject, DespawnTime);
    }

}
