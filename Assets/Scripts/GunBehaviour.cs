using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class GunBehaviour : MonoBehaviour
{
    [Header("Aim Settings")]
    [SerializeField] Camera GunCamera;
    [SerializeField] float DefaultFOV = 40f;
    [SerializeField] float AimFOV;

    private AudioSource MainSource;

    IInputManager input;
    ISoundManager sound;

    Animator GunAnim;

    bool SoundHasPlayed = false;

    [Header("Weapons")]
    [SerializeField] GameObject AssaultRifle;
    [SerializeField] GameObject HandGun;

    GameObject ThisWeapon;
    GameObject NextWeapon;

    [Header("Weapon Settings")]
    [SerializeField] private bool isHandGun;
    [SerializeField] private int WeaponTotalAmmo;
    [SerializeField] private string WeaponName;
    [SerializeField] private Sprite WeaponIcon;
    [SerializeField] private float AssaultRifleFireRate = 10;
    private float LastFired;
    private int currentAmmo;
    private bool isAiming = false;

    //Only use property setter to change currentAmmo 
    private int WeaponCurrentAmmo
    {
        get { return currentAmmo; }
        set
        {
            if (value == WeaponTotalAmmo)
            {
                currentAmmo = value;
                WeaponCurrentAmmoText.text = currentAmmo.ToString();
                return;
            }

            if(currentAmmo == 0)
            {
                Reload();
                return;
            }

            GunEffects();
            ObjectHit();
            currentAmmo = value;
            WeaponCurrentAmmoText.text = currentAmmo.ToString();
            MainSource.clip = sound.ShootSound;
            MainSource.Play();

            if (isHandGun)
            {
                if (currentAmmo == 0)
                {
                    GunAnim.SetBool("Out Of Ammo Slider", true);
                }
            }
        }
    }

    [Header("Spawn Points")]
    [SerializeField] private Transform RayPoint;
    [SerializeField] private Transform CasingSpawnPoint;

    [Header("Prefabs")]
    [SerializeField] private GameObject CasingPrefab;
    [SerializeField] private GameObject ImpactPrefab;

    [Header("UI elements")]
    [SerializeField] Text WeaponNameText;
    [SerializeField] Text WeaponTotalAmmoText;
    [SerializeField] Text WeaponCurrentAmmoText;
    [SerializeField] Image WeaponIconImage;

    [Header("Fire Effects")]
    [SerializeField] Light MuzzleLight;
    [SerializeField] private float LightDuration = 0.05f;
    [SerializeField] ParticleSystem MuzzleParticles;
    [SerializeField] ParticleSystem SparksParticles;
    [SerializeField] private int minSparkEmission = 1;
    [SerializeField] private int maxSparkEmission = 5;


    #region MonoBehaviour Methods
    private void Awake()
    {
        GunAnim = GetComponent<Animator>();

        WeaponCurrentAmmo = WeaponTotalAmmo;

        MuzzleLight.enabled = false;
    }
    private void Start()
    {
        input = InputManager.Instance;
        sound = SoundManager.Instance;

        MainSource = GetComponent<AudioSource>();
        MainSource.clip = sound.TakeOutWeaponSound;
        MainSource.Play();
        
        GunCamera.fieldOfView = DefaultFOV;
        
        ThisWeapon = AssaultRifle.activeSelf ? AssaultRifle : HandGun;
        NextWeapon = !AssaultRifle.activeSelf ? AssaultRifle : HandGun;

        LastFired = Time.time;

        WeaponTotalAmmoText.text = WeaponTotalAmmo.ToString();
        WeaponCurrentAmmoText.text = WeaponCurrentAmmo.ToString();
        WeaponIconImage.sprite = WeaponIcon;
        WeaponNameText.text = WeaponName;
    }

    private void OnEnable()
    {
        WeaponNameText.text = WeaponName;
        WeaponIconImage.sprite = WeaponIcon;
        WeaponTotalAmmoText.text = WeaponTotalAmmo.ToString();
        WeaponCurrentAmmoText.text = WeaponCurrentAmmo.ToString();
    }

    private void Update()
    {
        Aim();
        ChangeWeapon();
        if (input.GetReloadButton)
        {
            Reload();
        }
        Shoot();

    }
    #endregion

    #region My Methods
    void Aim()
    {
        if(input.GetRightMouseButton && !isReloading())
        {

            GunAnim.SetBool("Aim", true);

            GunCamera.fieldOfView = AimFOV;

            isAiming = true;

            if (!SoundHasPlayed)
            {
                MainSource.clip = sound.AimSound;
                MainSource.Play();
                SoundHasPlayed = true;
            }
        }
        else
        {
            GunAnim.SetBool("Aim", false);

            GunCamera.fieldOfView = DefaultFOV;

            isAiming = false;

            SoundHasPlayed = false;
        }
    }

    void ChangeWeapon()
    {
        if(input.GetScrollWheel)
        {
            MainSource.clip = sound.TakeOutWeaponSound;
            NextWeapon.SetActive(true);
            ThisWeapon.SetActive(false);
        }
    }

    void Shoot()
    {
        if(input.GetShootButton(isHandGun) && !isReloading() && !isDrawingGun())
        {
            if(isHandGun)
            {
                ShootAnimation();
                WeaponCurrentAmmo -= 1;
            }
            else
            {
                if(Time.time - LastFired > 1 / AssaultRifleFireRate)
                {
                    LastFired = Time.time;
                    ShootAnimation();
                    WeaponCurrentAmmo -= 1;
                }
            }
        }
    }

    void ShootAnimation()
    {
        if (!isAiming && !isDrawingGun())
            GunAnim.Play("Fire",0,0);
        else
            GunAnim.Play("Aim Fire",0,0);
    }

    void Reload()
    {
        WeaponCurrentAmmo = WeaponTotalAmmo;
        MainSource.clip = sound.ReloadSound(isHandGun);
        MainSource.Play();
        if (isHandGun)
        {
            GunAnim.SetBool("Out Of Ammo Slider", false);
        }
        GunAnim.Play("Reload");
    }

    void GunEffects()
    {
        var bullet = Instantiate(CasingPrefab, CasingSpawnPoint.position, CasingSpawnPoint.rotation);
        MuzzleParticles.Emit(1);
        SparksParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
        StartCoroutine(MuzzleFlashLight());
    }

    void ObjectHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(RayPoint.position, RayPoint.forward, out hit))
        {
            if (hit.collider != null)
            {
                var Impact = Instantiate(ImpactPrefab, hit.point + (hit.normal.normalized * 0.03f), Quaternion.LookRotation(hit.normal));
                Impact.transform.parent = hit.transform;
                if(hit.collider.tag == "Target")
                {
                    hit.collider.GetComponent<TargetScript>().TakeDamage();
                }
            }
        }
    }

    bool isReloading()
    {
        return GunAnim.GetCurrentAnimatorStateInfo(0).IsName("Reload");
    }

    bool isDrawingGun()
    {
        return GunAnim.GetCurrentAnimatorStateInfo(0).IsName("Draw");
    }

    IEnumerator MuzzleFlashLight()
    {
        MuzzleLight.enabled = true;
        yield return new WaitForSeconds(LightDuration);
        MuzzleLight.enabled = false;
    }
    #endregion
}
