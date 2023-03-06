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
        public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
        public int WaveNumber => waveNumber;
        public float TimeBetweenWaves => timeBetweenWaves;
        
        [SerializeField] bool spawnEnemy = true;
        [SerializeField] private GameObject waveUI;
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] float timeBetweenSpawns = 1f;
        [SerializeField] private float timeBetweenWaves = 1f;
        [SerializeField] private int minEnemyAmount = 4;
        [SerializeField] private int maxEnemyAmount = 10;
        
        [Header("---- Boss Settings ----")]
        [SerializeField] GameObject bossPrefab;
        [SerializeField] int bossWaveNumber;
        

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
            waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);
        }

        IEnumerator Start()
        {
            while (spawnEnemy && GameManager.GameState != GameState.GameOver)
            {
                waveUI.SetActive(true);
                yield return waitTimeBetweenWaves;
                waveUI.SetActive(false);
                yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
            }

        }

        IEnumerator RandomlySpawnCoroutine()
        {
            if (waveNumber % bossWaveNumber == 0)
            {
                var boss = PoolManager.Release(bossPrefab);
                enemyList.Add(boss);
            }
            else
            {
                enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);

                for (int i = 0; i < enemyAmount; i++)
                {
                    // var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                    // PoolManager.Release(enemy);
                    enemyList.Add( PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));

                    yield return waitTimeBetweenSpawns;

                }
            }
            
            yield return waitUntilNoEnemy;
            waveNumber++;
        }

        public void RemoveFromList(GameObject enemy)
        {
            enemyList.Remove(enemy);
        }
    }
}
