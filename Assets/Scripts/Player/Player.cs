using System;
using System.Collections;
using TonyLearning.ShootingGame.Audio;
using TonyLearning.ShootingGame.Characters;
using TonyLearning.ShootingGame.Input;
using TonyLearning.ShootingGame.Miscs;
using TonyLearning.ShootingGame.Pool_System;
using TonyLearning.ShootingGame.System_Modules;
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
        
        [Header("--- FIRE ---")]
        [SerializeField] private GameObject projectile1;
        [SerializeField] private GameObject projectile2;
        [SerializeField] private GameObject projectile3;
        [SerializeField] private GameObject projectileOverdrive;

        [SerializeField] private ParticleSystem muzzleVFX;

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


        #region PROPERTIES

        public bool IsFullHealth => health == maxHealth;
        public bool IsFullPower => weaponPower == 2;

        #endregion

        [Header("--- OVERDRIVE ---")] 
        [SerializeField]private float slowMotionDuration = 2f;
        [SerializeField] private float keepMotionDuration = 3f;
        [SerializeField]private int overdriveDodgeFactor = 2;
        [SerializeField] private float overdriveSpeedFactor = 1.2f;
        [SerializeField] private float overdriveFireFactor = 1.2f;

        [SerializeField] readonly float InvincibleTime = 1f;
        private float paddingX;
        private float paddingY;
        
        private bool isOverdriving = false;

        float t;

        private Vector2 moveDirection;
        Vector2 previousVelocity;
        Quaternion previousRotation;

        WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
        
        
        private Collider2D _collider2D;
        
        WaitForSeconds waitForFireInterval;
        private WaitForSeconds waitForOverdriveFireInterval;
        private WaitForSeconds waitHealthRegenerateTime;
        
        WaitForSeconds waitDecelerationTime;
        
        WaitForSeconds waitInvincibleTime;

        private Coroutine moveCoroutine;
        private Coroutine healthRegenerateCoroutine;


        private MissileSystem missile;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            missile = GetComponent<MissileSystem>();

            var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
            paddingX = size.x / 2f;
            paddingY = size.y / 2f;
            
            dodgeDurration = maxRoll / rollSpeed;
            _rigidbody2D.gravityScale = 0f;
      
            waitForFireInterval = new WaitForSeconds(fireInterval);
            waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
            waitForOverdriveFireInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
            waitDecelerationTime = new WaitForSeconds(decelerationTime);
            waitInvincibleTime = new WaitForSeconds(InvincibleTime);



        }

        private void Start()
        {
            
            statusBar_HUD.Initialize(health, maxHealth);
            
            _input.EnableGameplayInput();
            
        }

        private void Update()
        {
            transform.position = ViewPort.Instance.PlayerMoveablePosition(transform.position, paddingX,paddingY);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            PowerDown();
            statusBar_HUD.UpdateStatus(health, maxHealth);
            
            TimeController.Instance.BulletTime(slowMotionDuration);
            
            if (gameObject.activeSelf)
            {
                Move(moveDirection);
                StartCoroutine(InvincibleCoroutine());
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
            GameManager.onGameOver?.Invoke();
            GameManager.GameState = GameState.GameOver;
            statusBar_HUD.UpdateStatus(0f, maxHealth);
            base.Die();
        }

        IEnumerator InvincibleCoroutine()
        {
            _collider2D.isTrigger = true;

            yield return waitInvincibleTime;

            _collider2D.isTrigger = false;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _input.onMove += Move;
            _input.onStopMove += StopMove;
            _input.onFire += Fire;
            _input.onStopFire += StopFire;
            _input.onDodge += Dodge;
            _input.onOverDrive += Overdirive;
            _input.onLaunchMissile += LaunchMissile;

            PlayerOverdrive.on += OverdriveOn;
            PlayerOverdrive.off += OverdriveOff;
        }

        private void OnDisable()
        {
            _input.onMove -= Move;
            _input.onStopMove -= StopMove;
            _input.onFire -= Fire;
            _input.onStopFire -= StopFire;
            _input.onDodge -= Dodge;
            _input.onOverDrive -= Overdirive;
            _input.onLaunchMissile -= LaunchMissile;
            
            PlayerOverdrive.on -= OverdriveOn;
            PlayerOverdrive.off -= OverdriveOff;
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
           
           TimeController.Instance.BulletTime(slowMotionDuration,slowMotionDuration);

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

            moveDirection = moveInput.normalized;
            Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right);
            
            moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime, moveDirection * moveSpeed, moveRotation));
            StopCoroutine(nameof(DecelerationCoroutine));
            // StartCoroutine(nameof(MovePositionLimitCoroutine));
        }
        
        private void StopMove()
        {
            // _rigidbody2D.velocity = Vector2.zero;
            // StartCoroutine(StartStopCoroutine(Vector2.zero));
            
            
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveDirection = Vector2.zero;
            moveCoroutine = StartCoroutine(MoveCoroutine(decelerationTime, Vector2.zero, Quaternion.identity));
            // StopCoroutine(nameof(MovePositionLimitCoroutine));
            StartCoroutine(nameof(DecelerationCoroutine));
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
        IEnumerator MoveRangeLimatationCoroutine()
        {
            while (true)
            {
                transform.position = ViewPort.Instance.PlayerMoveablePosition(transform.position, paddingX,paddingY);
                yield return null;
            }
        }
        
        IEnumerator DecelerationCoroutine()
        {
            yield return waitDecelerationTime;
        
            StopCoroutine(nameof(MoveRangeLimatationCoroutine));
        }
        #endregion

        #region Fire
        
        private void Fire()
        {
            muzzleVFX.Play();
            StartCoroutine(nameof(FireCoroutine));

        }
        
        private void StopFire()
        {
            muzzleVFX.Stop();
            StopCoroutine(nameof(FireCoroutine));
        }

        IEnumerator FireCoroutine()
        {
            while (true)
            {
                switch (weaponPower)
                {
                    case 0:
                        PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                        break;
                    case 1:
                        PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position);
                        PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleBottom.position);
                        break;
                    case 2:
                        PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position);
                        PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position);
                        PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position);
                        break;
                    default:
                        break;
                
                }
                AudioManager.Instance.PlayRandomSFX(AD_projectileSFX);
                // yield return waitForFireInterval;
                if (isOverdriving)
                {
                    yield return waitForOverdriveFireInterval;
                }
                else
                {
                    yield return waitForFireInterval;
                }
            }
        }
        #endregion
        
        #region MISSILE
        
        private void LaunchMissile()
        {
            missile.Launch(muzzleMiddle);
        }
        
        public void PickUpMissile()
        {
            missile.PickUp();
        }

        // GameObject Projectile()
        // {
        //     return isOverdriving ? projectileOverdrive : projectile1;
        // }

        #endregion

        #region OVERDRIVE
        private void Overdirive()
        {
            if (!PlayerEnergy.Instance.IsEnoughEnergy(PlayerEnergy.MAX)) return;
            
            PlayerOverdrive.on.Invoke();
        }
        
        private void OverdriveOn()
        {
            isOverdriving = true;
            dodgeEnergyCost *= overdriveDodgeFactor;
            moveSpeed *= overdriveSpeedFactor;
            TimeController.Instance.BulletTime(slowMotionDuration, keepMotionDuration, slowMotionDuration);
        }

        private void OverdriveOff()
        {
            isOverdriving = false;
            dodgeEnergyCost /= overdriveDodgeFactor;
            moveSpeed /= overdriveSpeedFactor;
        }

        

        #endregion
        
        #region WEAPON POWER

        public void PowerUp()
        {
            weaponPower = Mathf.Min(++weaponPower, 2);
        }

        void PowerDown()
        {
            //* ??????1
            // weaponPower--;
            // weaponPower = Mathf.Clamp(weaponPower, 0, 2);
            //* ??????2
            // weaponPower = Mathf.Max(weaponPower - 1, 0);
            //* ??????3
            // weaponPower = Mathf.Clamp(weaponPower, --weaponPower, 0);
            //* ??????4
            weaponPower = Mathf.Max(--weaponPower, 0);
        }

        #endregion
        
    }
}
