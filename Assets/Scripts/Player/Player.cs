using System.Collections;
using TonyLearning.ShootingGame.Audio;
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
        [SerializeField] private AudioData AD_projectileSFX;
        
        [SerializeField, Range(0,2)] private int weaponPower = 0;
        [SerializeField] private float fireInterval = 0.2f;

        [Header("--- DODGE ---")] 
        [SerializeField] AudioData AD_dodgeSFX;
        [SerializeField,Range(0,100)] private int dodgeEnergyCost = 25;

        private bool isDodging = false;

        [SerializeField] private float maxRoll = 720f;
        [SerializeField] private float rollSpeed = 360f;
        [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        private float currentRoll;

        private float dodgeDurration;

        float t;
        Vector2 previousVelocity;
        Quaternion previousRotation;

        WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
        
        
        private Collider2D _collider2D;
        
        WaitForSeconds waitForFireInterval;
        private WaitForSeconds waitHealthRegenerateTime;

        private Coroutine moveCoroutine;
        private Coroutine healthRegenerateCoroutine;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            dodgeDurration = maxRoll / rollSpeed;
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
            _input.onDodge += Dodge;
        }

       private void OnDisable()
        {
            _input.onMove -= Move;
            _input.onStopMove -= StopMove;
            _input.onFire -= Fire;
            _input.onStopFire -= StopFire;
            _input.onDodge -= Dodge;
        }

       #region DODGE

       private void Dodge()
       {
            if(isDodging || !PlayerEnergy.Instance.IsEnoughEnergy(dodgeEnergyCost)) return;
            StartCoroutine(nameof(DodgeCoroutine));
           

           //Change player's scale
       }

       IEnumerator DodgeCoroutine()
       {
           isDodging = true;
           AudioManager.Instance.PlayRandomSFX(AD_dodgeSFX);
           // Cost energy
           PlayerEnergy.Instance.UseEnergy(dodgeEnergyCost);
           //Make player invincible
           _collider2D.isTrigger = true;

           //Make player rotate along x axis x
           currentRoll = 0f;

           var scale = transform.localScale;
           
           while (currentRoll < maxRoll)
           {
               currentRoll += rollSpeed * Time.deltaTime;
               transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

               if (currentRoll < maxRoll / 2f)
               {
                   // scale -= (Time.deltaTime / dodgeDurration) * Vector3.one;
                   scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDurration, dodgeScale.x, 1f);
                   scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDurration, dodgeScale.y, 1f);
                   scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDurration, dodgeScale.z, 1f);
               }
               
               else
               {
                   // scale += (Time.deltaTime / dodgeDurration) * Vector3.one;
                   scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDurration, dodgeScale.x, 1f);
                   scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDurration, dodgeScale.y, 1f);
                   scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDurration, dodgeScale.z, 1f);
               }

               transform.localScale = scale;
               
               yield return null;
           }
           _collider2D.isTrigger = false;
           isDodging = false;
       }

       #endregion
             


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
            StartCoroutine(nameof(MovePositionLimitCoroutine));
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
            StopCoroutine(nameof(MovePositionLimitCoroutine));
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
            t = 0;
            previousVelocity = _rigidbody2D.velocity;
            previousRotation = transform.rotation;

            while (t < 1f)
            {
                t += Time.fixedDeltaTime / time;
                _rigidbody2D.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t);
                transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t);

                yield return _waitForFixedUpdate;
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
                AudioManager.Instance.PlayRandomSFX(AD_projectileSFX);
                yield return waitForFireInterval;;
            }
        }

        #endregion
    }
}
