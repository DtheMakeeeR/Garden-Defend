using UnityEngine;

namespace GardenDefense
{
    public class Building : MonoBehaviour
    {
        BuildingModel _model;
        BuildingData _data;
        public string Description => _data.Description;
        public int Cost => _data.Cost;
        public void Setup(BuildingData data, float rotation)
        {
            _data = data;
            _model = Instantiate(_data.Model, transform.position, Quaternion.identity, transform);
            _model.Rotate(rotation);
        }
    }
}
