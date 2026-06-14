using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GardenDefense
{
    public class BuildingSystem : MonoBehaviour
    {
        public const float CellSize = 2f;

        [SerializeField]
        List<BuildingData> _buildingDataList;

        [SerializeField]
        BuildingPreview _previewPrefab;
        [SerializeField]
        Building _buildingPrefab;
        [SerializeField]
        BuildingGrid _grid;

        BuildingPreview _preview;

        private void Update()
        {
            Vector3 mousePos = GetMouseWorldPosition();

            if(_preview != null)
            {

            }
            else
            {
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _preview = CreatePreview(_buildingDataList[0], mousePos);
                }
                else if(Input.GetKeyDown(KeyCode.Alpha2))
                {
                    _preview = CreatePreview(_buildingDataList[1], mousePos);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    _preview = CreatePreview(_buildingDataList[2], mousePos);
                }
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane  = new(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
            return Vector3.zero;
        }
        private BuildingPreview CreatePreview(BuildingData data, Vector3 position)
        {
            BuildingPreview preview = Instantiate(_previewPrefab, position, Quaternion.identity);
            preview.Setup(data);
            return preview;
        }
    }
}
