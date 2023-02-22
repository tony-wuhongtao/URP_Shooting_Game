using System.Collections;
using TonyLearning.ShootingGame.Characters;
using TonyLearning.ShootingGame.Input;
using TonyLearning.ShootingGame.Miscs;
using TonyLearning.ShootingGame.Pool_System;
using TonyLearning.ShootingGame.UI;
using UnityEngine;

namespace TonyLearning.ShootingGame.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Character
    {
        [SerializeField] private StatusBar_HUD statusBar_HUD;
        [SerializeField] private bool regenerateHealth = true;
        [SerializeField] float healthRegenerateTime;
        [SerializeField, Range(0f,1f)] float healthRegeneratePercent;
        
        [Header("--- INPUT ---")]
        [SerializeField]private PlayerInput _input;

        private Rigidbody2D _rigidbody2D;

        [Header("--- MOVE ---")]
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float accelerationTime = 3f;
        [SerializeField] private float decelerationTime = 3f;

        [SerializeField] private float moveRotationAngle = 50f;
        
        [SerializeField] private float paddingX = 0.2f;
        [SerializeField] private float paddingY = 0.2f;

        [Header("--- FIRE ---")]
        [SerializeField] private GameObject projectile1;
        [SerializeField] private GameObject projectile2;
        [SerializeField] private GameObject projectile3;
        
        [SerializeField] private Transform muzzleMiddle;
        [SerializeField] private Transform muzzleTop;
        [SerializeField] private Transform muzzleBottom;
        [SerializeField, Range(0,2)] private int weaponPower = 0;
        [SerializeField] private float fireInterval = 0.2f;
        
        WaitForSeconds waitForFireInterval;
        private WaitForSeconds waitHealthRegenerateTime;

        private Coroutine moveCoroutine;
        private Coroutine healthRegenerateCoroutine;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _rigidbody2D.gravityScale = 0f;
            
            waitForFireInterval = new WaitForSeconds(fireInterval);
            waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
            
            statusBar_HUD.Initialize(health, maxHealth);
            
            _input.EnableGameplayInput();
            
        }



        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            statusBar_HUD.UpdateStatus(health, maxHealth);
            
            if (gameObject.activeSelf)
            {
                if (regenerateHealth)
                {
                    if(healthRegenerateCoroutine != null)
                    {
                        StopCoroutine(healthRegenerateCoroutine);
                    }
                    healthRegenerateCoroutine =
                        StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
                }
            }
        }

        public override void RestoreHealth(float value)
        {
            base.RestoreHealth(value);
            statusBar_HUD.UpdateStatus(health, maxHealth);
        }

        public override void Die()
        {
            statusBar_HUD.UpdateStatus(0f, maxHealth);
            base.Die();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _input.onMove += Move;
            _input.onStopMove += StopMove;
            _input.onFire += Fire;
            _input.onStopFire += StopFire;
        }

        private void OnDisable()
        {
            _input.onMove -= Move;
            _input.onStopMove -= StopMove;
            _input.onFire -= Fire;
            _input.onStopFire -= StopFire;
        }
        


        #region Move

        private void Move(Vector2 moveInput)
        {
            // Vector2 moveAmount = moveInput * moveSpeed;
            //
            // _rigidbody2D.velocity = moveAmount;
            // StartCoroutine(StartMoveCoroutine((moveInput.normalized * moveSpeed)));

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
            
            moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveInput.normalized * moveSpeed, moveRotation));
            StartCoroutine(MovePositionLimitCoroutine());
        }
        
        private void StopMove()
        {
            // _rigidbody2D.velocity = Vector2.zero;
            // StartCoroutine(StartStopCoroutine(Vector2.zero));
            
            
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
            StopCoroutine(MovePositionLimitCoroutine());
        }
        

        

        // IEnumerator StartMoveCoroutine(Vector2 moveVelocity)
        // {
        //     float t = 0f;
        //     while (t < accelerationTime)
        //     {
        //         t += Time.fixedDeltaTime / accelerationTime;
        //         _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, moveVelocity, t / accelerationTime);
        //         yield return null;
        //     }
        //     
        // }
        //
        // IEnumerator StartStopCoroutine(Vector2 moveVelocity)
        // {
        //     float t = 0f;
        //     while (t < decelerationTime)
        //     {
        //         t += Time.fixedDeltaTime / decelerationTime;
        //         _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, moveVelocity, t / decelerationTime);
        //         yield return null;
        //     }
        // }

        IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
        {
            float t = 0;

            while (t < 1f)
            {
                t += Time.fixedDeltaTime / time;
                _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, moveVelocity, t);
                transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t);

                yield return null;
            }
            // while (t < time)
            // {
            //     t += Time.fixedDeltaTime;
            //     _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, moveVelocity, t / time);
            //     transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t / time);
            //
            //     yield return null;
            // }
        }
        IEnumerator MovePositionLimitCoroutine()
        {
            while (true)
            {
                transform.position = ViewPort.Instance.PlayerMoveablePosition(transform.position, paddingX,paddingY);
                yield return null;
            }
        }
        
        #endregion

        #region Fire
        
        private void Fire()
        {
            StartCoroutine(nameof(FireCoroutine));

        }
        
        private void StopFire()
        {
            StopCoroutine(nameof(FireCoroutine));
        }

        IEnumerator FireCoroutine()
        {
            while (true)
            {
                switch (weaponPower)
                {
                    case 0:
                        PoolManager.Release(projectile1, muzzleMiddle.position);
                        break;
                    case 1:
                        PoolManager.Release(projectile1, muzzleTop.position);
                        PoolManager.Release(projectile1, muzzleBottom.position);
                        break;
                    case 2:
                        PoolManager.Release(projectile1, muzzleMiddle.position);
                        PoolManager.Release(projectile2, muzzleTop.position);
                        PoolManager.Release(projectile3, muzzleBottom.position);
                        break;
                    default:
                        break;
                
                }
                
                yield return waitForFireInterval;;
            }
        }

        #endregion
    }
}
