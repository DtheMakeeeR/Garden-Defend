using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace GardenDefense
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        public List<WaveObject> Waves;

        [SerializeField]
        float _intervalBetweenWaves = 10f;

        [SerializeField]
        float _spawnRadius = 1f;
        [SerializeField]
        Transform _target;
        

        private int _enemiesAlive;
        private bool _isSpawning = false;
        private bool _canSpawnNextWave => _enemiesAlive <= 0 && !_isSpawning;
        private int _waveIndex = 0;

        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            if(_canSpawnNextWave && _waveIndex <Waves.Count)
            {
                Debug.Log($"SPWANER: Previous wave is clear. {Waves[_waveIndex].WaveName} is ready to spawn");
                Timing.RunCoroutine(_SpawnWaveCoroutine(Waves[_waveIndex++]).CancelWith(gameObject));
            }
        }

        private IEnumerator<float> _SpawnWaveCoroutine(WaveObject wave)
        {
            _isSpawning = true;
            Debug.Log($"SPWANER: Spawning wave: {wave.WaveName}, in {_intervalBetweenWaves} seconds");
            yield return Timing.WaitForSeconds(_intervalBetweenWaves);
            foreach (var enemy in wave.Enemies)
            {
                for(int i = 0; i < enemy.Amount; i++)
                {
                    SpawnEnemy(enemy.EnemyPrefab, i);
                }
            }
            _isSpawning = false;
        }
        private void SpawnWave(WaveObject wave)
        {
            foreach(var enemy in wave.Enemies)
            {
                for(int i = 0; i < enemy.Amount; i++)
                {
                    SpawnEnemy(enemy.EnemyPrefab, i);
                }
            }
        }

        private void SpawnEnemy(GameObject enemyPrefab, int index)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(offset.x, 0, offset.y);
            GameObject enemyGameobject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
            enemyGameobject.name = $"Enemy_{index}";
            EnemyMovement enemyMovement = enemyGameobject.GetComponent<EnemyMovement>();
            enemyMovement.Target = _target; 
            enemyMovement.Speed = enemyMovement.Speed * Random.Range(0.9f, 1.1f); // Randomize speed slightly
            _enemiesAlive++;
            enemyGameobject.GetComponent<Enemy>().OnDeath += () =>
            {
                Debug.Log($"SPAWNER: Enemies left: {_enemiesAlive - 1}");
                _enemiesAlive--;
            };
        }
    }
}
