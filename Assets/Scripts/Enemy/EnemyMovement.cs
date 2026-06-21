using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace GardenDefense
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField]
        NavMeshAgent _agent;
        [SerializeField]
        public float Speed = 3.5f;
        [SerializeField]
        public Transform Target;


        private void Awake()
        {
            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
                if (_agent == null)
                {
                    Debug.LogError("NavMeshAgent component not found on EnemyMovement. Please add a NavMeshAgent component to the GameObject.");
                }
            }
            _agent.speed = Speed;
            if (Target != null)
            {
                _agent.SetDestination(Target.position);
            }
        }

        private void Update()
        {
            
        }

    }
}
