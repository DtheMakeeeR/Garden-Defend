using System.Collections.Generic;
using UnityEngine;
using MEC;
namespace GardenDefense
{
    public class BasicGenerator : MonoBehaviour, IStation
    {
        [SerializeField]
        string _recourseName;
        [SerializeField]
        float _generateTime;
        [SerializeField]
        int _generateAmount;

        [SerializeField]
        bool _isGenerating = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if(!ItemsManager.Instance.HasItem(_recourseName))
            {
                Debug.LogError($"Item '{_recourseName}' not found in ItemsManager. Please add it to the ItemsManager before using this generator.");
            }
            Timing.RunCoroutine(_GenerateRoutine().CancelWith(gameObject));
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public IEnumerator<float> _GenerateRoutine()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(_generateTime);
                if (_isGenerating)
                {
                    Debug.Log("ADD");
                    ItemsManager.Instance.AddItem(_recourseName, _generateAmount);
                }
            }            
        }

        public void Activate()
        {
            _isGenerating = true;
        }
    }
}
