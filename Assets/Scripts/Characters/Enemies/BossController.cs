using System;
using System.Collections;
using System.Collections.Generic;
using TonyLearning.ShootingGame.Audio;
using TonyLearning.ShootingGame.Miscs;
using TonyLearning.ShootingGame.Pool_System;
using TonyLearning.ShootingGame.Projectile;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TonyLearning.ShootingGame.Characters.Enemies
{
    public class BossController : EnemyController
    {
        [SerializeField] private float continuousFireDuration = 1.5f;

        [Header("---- Player Detection ----")]
        [SerializeField] Transform playerDetectionTransform;
        [SerializeField] Vector3 playerDetectionSize;
        [SerializeField] LayerMask playerLayer;
        
        [Header("---- Beam ----")]
        [SerializeField] float beamCooldownTime = 12f;
        [SerializeField] AudioData beamChargingSFX;
        [SerializeField] AudioData beamLaunchSFX;
        
        bool isBeamReady;
        
        int launchBeamID = Animator.StringToHash("launchBeam");
        
        private WaitForSeconds waitForContinuousFireInterval;
        private WaitForSeconds waitForFireInterval;

        WaitForSeconds waitBeamCooldownTime;
        
        private List<GameObject> magazine;
        private AudioData launchSFX;
        
        Animator animator;
        private Transform playerTransform;
        
        
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);
            waitForFireInterval = new WaitForSeconds(maxFireInterval);
            magazine = new List<GameObject>(projectiles.Length);
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        protected override void OnEnable()
        {
            isBeamReady = false;
            muzzleVFX.Stop();
            StartCoroutine(nameof(BeamCooldownCoroutine));
            base.OnEnable();
            
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(playerDetectionTransform.position, playerDetectionSize);
        }
        
        void ActivateBeamWeapon()
        {
            isBeamReady = false;
            animator.SetTrigger(launchBeamID);
            AudioManager.Instance.PlayRandomSFX(beamChargingSFX);
        }
        
        void AnimationEventLaunchBeam()
        {
            AudioManager.Instance.PlayRandomSFX(beamLaunchSFX);
        }

        void AnimationEventStopBeam()
        {
            StopCoroutine(nameof(ChasingPlayerCoroutine));
            StartCoroutine(nameof(BeamCooldownCoroutine));
            StartCoroutine(nameof(RandomlyFireCoroutine));
        }
        
        void LoadProjectiles()
        {
            magazine.Clear();
            
            if (Physics2D.OverlapBox(playerDetectionTransform.position, playerDetectionSize, 0f, playerLayer))
            {
                //launch projectile 1
                magazine.Add(projectiles[0]);
                launchSFX = projectileLuanchSFX[0];
            }
            else
            {
                //lanche projectile 2 or 3
                if (Random.value < 0.5)
                {
                    magazine.Add(projectiles[1]);
                    launchSFX = projectileLuanchSFX[1];
                }
                else
                {
                    for (int i = 2; i < projectiles.Length; i++)
                    {
                        magazine.Add(projectiles[i]);
                        
                    }
                    launchSFX = projectileLuanchSFX[2];
                }
            }
        }
        protected override IEnumerator RandomlyFireCoroutine()
        {
            
            while (isActiveAndEnabled)
            {
                if(GameManager.GameState == GameState.GameOver) yield break;
                
                if (isBeamReady)
                {
                    ActivateBeamWeapon();
                    StartCoroutine(nameof(ChasingPlayerCoroutine));
                    yield break;
                }
                yield return waitForFireInterval;
                yield return StartCoroutine(nameof(ContinuousFireCoroutine));
            }
        }
        
        IEnumerator BeamCooldownCoroutine()
        {
            yield return waitBeamCooldownTime;

            isBeamReady = true;
        }

        IEnumerator ContinuousFireCoroutine()
        {
            LoadProjectiles();
            muzzleVFX.Play();
            float continuousFireTime = 0f;
            while (continuousFireTime < continuousFireDuration)
            {
                foreach (var projectile in magazine)
                {
                    PoolManager.Release(projectile, muzzle.position);
                }

                continuousFireTime += minFireInterval;
                AudioManager.Instance.PlayRandomSFX(launchSFX);
                
                yield return waitForContinuousFireInterval;
            }
            muzzleVFX.Stop();
        }
        
        IEnumerator ChasingPlayerCoroutine()
        {
            while (isActiveAndEnabled)
            {
                targetPosition.x = ViewPort.Instance.MaxX - paddingX;
                targetPosition.y = playerTransform.position.y;

                yield return null;
            }
        }
    }
}
