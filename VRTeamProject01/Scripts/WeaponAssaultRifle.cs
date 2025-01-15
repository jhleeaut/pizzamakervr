using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTeamProject01
{
    [System.Serializable]
    public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int>
    {
    }
    public class WeaponAssaultRifle : MonoBehaviour
    {
        #region
        [SerializeField] private Animator animator;
        private AudioSource audioSource;

        public Animator Animator
        {
            get
            {
                return animator;
            }
        }

        [Header("Fire Effects")]
        [SerializeField] private GameObject muzzleFlashEffect;

        [SerializeField] private GameObject casingPrefab;
        [SerializeField] private GameObject impactPrefab;

        [Header("SpawnPoints")]
        [SerializeField] private Transform casingSpawnPoint;

        [SerializeField] private Transform bulletSpawnPoint;

        [Header("Sound")]
        [SerializeField] private AudioClip fireSound;

        [SerializeField] private AudioClip reloadSound;

        [Space]
        [SerializeField] private WeaponSetting weaponSetting;

        [HideInInspector] public AmmoEvent onAmmoEvent = new AmmoEvent();

        private bool isAttack = false;
        private bool isAttackStop = false;
        private bool isReload = false;
        private int currentAmmo;
        #endregion

        private void OnEnable()
        {
            onAmmoEvent.Invoke(currentAmmo, weaponSetting.maxAmmo);
        }

        public void Init()
        {
            muzzleFlashEffect.SetActive(false);

            animator = this.GetComponent<Animator>();
            audioSource = this.GetComponent<AudioSource>();
            currentAmmo = weaponSetting.maxAmmo;
        }

        public void StartAttack()
        {
            if (isReload == false && isAttack == false)
            {
                StartCoroutine(TryAttack());
            }
        }

        public void StopAttack()
        {
            isAttackStop = true;
        }

        public void StartReload()
        {
            if (isReload == false)
            {
                StopAttack();
                StartCoroutine(TryReload());
            }
        }

        private IEnumerator TryAttack()
        {
            isAttack = true;

            while (!isAttackStop)
            {
                if (animator.GetFloat("movementSpeed") > 0.5f)
                {
                    break;
                }
                if (currentAmmo <= 0)
                {
                    StartReload();
                    break;
                }

                currentAmmo--;

                onAmmoEvent.Invoke(currentAmmo, weaponSetting.maxAmmo);

                animator.ResetTrigger("onJump");

                animator.Play("Fire");
                StartCoroutine(OnFireEffects());
                PlaySound(fireSound);
                SpawnCasing();

                RaycastCalculate();
                yield return new WaitForSeconds(weaponSetting.fireRate);
            }

            isAttack = false;
            isAttackStop = false;
        }

        private IEnumerator OnFireEffects()
        {
            muzzleFlashEffect.SetActive(true);
            yield return new WaitForSeconds(weaponSetting.fireRate * 0.3f);
            muzzleFlashEffect.SetActive(false);
        }

        private void PlaySound(AudioClip clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }

        private void SpawnCasing()
        {
            Instantiate(casingPrefab, casingSpawnPoint.position, Random.rotation);
        }

        private IEnumerator TryReload()
        {
            isReload = true;

            animator.Play("Reload");
            PlaySound(reloadSound);

            while (true)
            {
                if (audioSource.isPlaying == false && animator.GetCurrentAnimatorStateInfo(0).IsName("Movement"))
                {
                    break;
                }
                yield return null;
            }
            isReload = false;
            currentAmmo = weaponSetting.maxAmmo;

            onAmmoEvent.Invoke(currentAmmo, weaponSetting.maxAmmo);
        }

        private void RaycastCalculate()
        {
            Ray ray;
            RaycastHit hit;

            Vector3 targetPoint = Vector3.zero;

            ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);
            if (Physics.Raycast(ray, out hit, weaponSetting.fireDistance))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.origin + ray.direction * weaponSetting.fireDistance;
            }
            Debug.DrawRay(ray.origin, ray.direction * weaponSetting.fireDistance, Color.red);

            Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
            if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.fireDistance))
            {
                if (hit.transform.name.Contains("Enemy"))
                {
                    Destroy(hit.transform.gameObject);
                    return;
                }

                Instantiate(impactPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
            Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.fireDistance, Color.blue);
        }
    }
}