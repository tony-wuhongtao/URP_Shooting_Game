using System;
using System.Collections;
using System.Collections.Generic;
using TonyLearning.ShootingGame.Pool_System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TonyLearning.ShootingGame.System_Modules
{
    public class EnermyManager : Singleton<EnermyManager>
    {
        public int WaveNumber => waveNumber;
        public float TimeBetweenWaves => timeBetweenWaves;
        
        [SerializeField] bool spawnEnemy = true;
        [SerializeField] private GameObject waveUI;
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] float timeBetweenSpawns = 1f;
        [SerializeField] private float timeBetweenWaves = 1f;
        [SerializeField] private int minEnemyAmount = 4;
        [SerializeField] private int maxEnemyAmount = 10;

        int waveNumber = 1;
        int enemyAmount;

        private List<GameObject> enemyList;
        
        WaitForSeconds waitTimeBetweenSpawns;
        private WaitForSeconds waitTimeBetweenWaves;
        WaitUntil waitUntilNoEnemy;

        protected override void Awake()
        {
            base.Awake();
            enemyList = new List<GameObject>();
            waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
            waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
            waitUntilNoEnemy = new WaitUntil(NoEnemy);
        }

        bool NoEnemy()
        {
            return enemyList.Count == 0;
        }

        IEnumerator Start()
        {
            while (spawnEnemy)
            {
                yield return waitUntilNoEnemy;
                
                waveUI.SetActive(true);
                yield return waitTimeBetweenWaves;
                waveUI.SetActive(false);
                yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
            }

        }

        IEnumerator RandomlySpawnCoroutine()
        {
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);

            for (int i = 0; i < enemyAmount; i++)
            {
                // var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                // PoolManager.Release(enemy);
                enemyList.Add( PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));

                yield return waitTimeBetweenSpawns;

            }

            waveNumber++;
        }

        public void RemoveFromList(GameObject enemy)
        {
            enemyList.Remove(enemy);
        }
    }
}
