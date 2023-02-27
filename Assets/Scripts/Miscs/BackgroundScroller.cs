using System;
using System.Collections;
using TonyLearning.ShootingGame.System_Modules;
using UnityEngine;

namespace TonyLearning.ShootingGame.Miscs
{
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] private Vector2 scrollVelocity;
        private Material _material;

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        // private void Update()
        // {
        //     _material.mainTextureOffset += scrollVelocity * Time.deltaTime;
        // }

        IEnumerator Start()
        {
            while (GameManager.GameState != GameState.GameOver)
            {
                _material.mainTextureOffset += scrollVelocity * Time.deltaTime;
                yield return null;
            }
        }
    }
}
