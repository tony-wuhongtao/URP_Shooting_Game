using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TonyLearning.ShootingGame
{
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] private Vector2 scrollVelocity;
        private Material _material;

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            _material.mainTextureOffset += scrollVelocity * Time.deltaTime;
        }
    }
}
