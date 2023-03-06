using System;
using UnityEngine;

namespace TonyLearning.ShootingGame.Miscs
{
    public class AutoRotate : MonoBehaviour
    {
        [SerializeField] private float speed = 360f;
        [SerializeField] private Vector3 angle;

        private void Update()
        {
            transform.Rotate(angle * speed * Time.deltaTime);
        }
    }
}
