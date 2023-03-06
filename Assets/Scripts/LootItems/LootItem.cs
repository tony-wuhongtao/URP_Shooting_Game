using System;
using System.Collections;
using TonyLearning.ShootingGame.Audio;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace TonyLearning.ShootingGame.LootItems
{
    public class LootItem : MonoBehaviour
    {
        [SerializeField] float minSpeed = 5f;
        [SerializeField] float maxSpeed = 15f;
        [SerializeField] protected AudioData defaultPickUpSFX;
        
        int pickUpStateID = Animator.StringToHash("PickUp");
        protected Player.Player player;
        protected AudioData pickUpSFX;
        
        Animator animator;
        protected Text lootMessage;
        
        void Awake()
        {
            animator = GetComponent<Animator>();

            player = FindObjectOfType<Player.Player>();

            lootMessage = GetComponentInChildren<Text>(true);
            //
            pickUpSFX = defaultPickUpSFX;
        }
        void OnEnable()
        {
            StartCoroutine(MoveCoroutine());
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            PickUp();
        }
        protected virtual void PickUp()
        {
            StopAllCoroutines();
            animator.Play(pickUpStateID);
            AudioManager.Instance.PlayRandomSFX(pickUpSFX);
        }
        
        IEnumerator MoveCoroutine()
        {
            float speed = Random.Range(minSpeed, maxSpeed);

            Vector3 direction = Vector3.left;

            while (true)
            {
                if (player.isActiveAndEnabled)
                {
                    direction = (player.transform.position - transform.position).normalized;
                }

                transform.Translate(direction * speed * Time.deltaTime);

                yield return null;
            }
        }
    }
}
