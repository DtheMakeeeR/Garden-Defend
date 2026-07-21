using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace GardenDefense
{
    [CreateAssetMenu(fileName = "WaveObject", menuName = "Scriptable Objects/WaveObject")]
    public class WaveObject : ScriptableObject
    {
        [System.Serializable]
        public struct EnemyEntry
        {
            public GameObject EnemyPrefab;
            public int Amount;
        }

        [SerializeField]
        public string WaveName;
        [SerializeField]
        public string Description;
        [SerializeField]
        public List<EnemyEntry> Enemies;
        [SerializeField]
        public float SpawnInterval;
    }
}
