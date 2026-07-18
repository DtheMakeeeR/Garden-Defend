using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GardenDefense
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        GameObject _enemyPrefab;

        [SerializeField]
        float _spawnInterval = 1f;

        [SerializeField]
        int _spawnCount = 0;

        [SerializeField]
        float _spawnRadius = 1f;
        [SerializeField]
        Transform _target;

        void Start()
        {
           Timing.RunCoroutine(_SpawnCoroutine().CancelWith(gameObject));
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private IEnumerator<float> _SpawnCoroutine()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                SpawnEnemy(i);
                yield return Timing.WaitForSeconds(_spawnInterval);
            }   
        }

        private void SpawnEnemy(int index = 0)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(offset.x, 0, offset.y);
            GameObject enemyGameobject = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity, transform);
            enemyGameobject.name = $"Enemy_{index}";
            enemyGameobject.GetComponent<EnemyMovement>().Target = _target;
        }
    }
}
