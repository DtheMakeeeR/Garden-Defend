using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace GardenDefense
{
    public class BuildingModel : MonoBehaviour
    {
        [SerializeField]
        Transform _wrapper;
        public float Rotation => _wrapper.rotation.eulerAngles.y;
        BuildingShapeUnit[] _shapeUnits;
        private void Awake()
        {
            _shapeUnits = GetComponentsInChildren<BuildingShapeUnit>();
        }
        public void Rotate(float rotationStep = 90)
        {
            _wrapper.Rotate(new Vector3(0, rotationStep, 0));
        }
        public List<Vector3> GetAllBuildingPositions()
        {
            return _shapeUnits.Select(s => s.transform.position).ToList();
        }
    }
}
