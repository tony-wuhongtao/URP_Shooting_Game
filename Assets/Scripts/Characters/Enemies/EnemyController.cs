using System;
using System.Collections;
using System.Data.SqlTypes;
using TonyLearning.ShootingGame.Audio;
using TonyLearning.ShootingGame.Miscs;
using TonyLearning.ShootingGame.Pool_System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TonyLearning.ShootingGame.Characters.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [Header("--- MOVE ---")] 
        [SerializeField] private float paddingX; 
        [SerializeField] private float paddingY;
        [SerializeField] float moveSpeed = 2f, moveRotationAngle = 25f;

        [Header("--- FIRE ---")]
        [SerializeField] GameObject[] projectiles;

        [SerializeField] private AudioData[] projectileLuanchSFX;
        [SerializeField] Transform muzzle;

        [SerializeField] private float minFireInterval, maxFireInterval;

        private float maxMoveDistancePerFrame;
        private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

        private Vector3 targetPosition;

        private void Awake()
        {
            maxMoveDistancePerFrame = moveSpeed * Time.fixedDeltaTime;
        }

        private void OnEnable()
        {
            StartCoroutine(nameof(RandomlyMovingCoroutine));
            StartCoroutine(nameof(RandomlyFireCoroutine));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator RandomlyMovingCoroutine()
        {
            transform.position = ViewPort.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

            targetPosition = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);

            while (gameObject.activeSelf)
            {
                //if has not arrived targetPosion
                //keep moving
                //else
                //set a new targetPosition
                if (Vector3.Distance(transform.position, targetPosition) >= maxMoveDistancePerFrame)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxMoveDistancePerFrame);
                    transform.rotation =
                        Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle,
                            Vector3.right);
                }
                else
                {
                    targetPosition = ViewPort.Instance.RandomRightHalfPosition(paddingX, paddingY);
                }
                
                
                yield return _waitForFixedUpdate;
            }
        }

        IEnumerator RandomlyFireCoroutine()
        {
            while (gameObject.activeSelf)
            {
                yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

                foreach (var projectile in projectiles)
                {
                    PoolManager.Release(projectile, muzzle.position);
                }
                
                AudioManager.Instance.PlayRandomSFX(projectileLuanchSFX);
            }
        }
    }
}
