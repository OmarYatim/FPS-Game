using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingScript : MonoBehaviour
{
    [SerializeField] private Vector3 ForceVector = new Vector3(0.25f, 1.0f, 0.0f);
    [SerializeField] private float ForceSpeed = 25.0f;
    [SerializeField] private float RotationSpeed = 250.0f;
    [SerializeField] private float DestroyTime = 2.0f;

    AudioSource CasingSource;

    ISoundManager sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = SoundManager.Instance;
        CasingSource = GetComponent<AudioSource>();
        GetComponent<Rigidbody>().AddRelativeForce(ForceVector * ForceSpeed);
        transform.rotation = Random.rotation;
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, RotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.down, RotationSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        CasingSource.clip = sound.CasingSound;
        CasingSource.Play();
    }
}
