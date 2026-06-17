using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
        [SerializeField]
        InputReader _input;

        BuildingPreview _preview;
        private bool _isBuildingMode = false;
        private int _selectedBuildingIndex = -1;
        private string _indexName;

        private void Start()
        {
            if (_input != null)
            {
                _input.EnablePlayerActions();

                _input.Attack += OnBuildAction;
            }
        }

        private void Update()
        {
            CheckBuildingSelection();

            if (_isBuildingMode && _preview != null)
            {
                HandlePreviewWithMouse();
            }
            else if (_isBuildingMode && _preview == null && _selectedBuildingIndex >= 0)
            {
                Vector3 mousePos = GetMouseWorldPosition();
                _preview = CreatePreview(_buildingDataList[_selectedBuildingIndex], mousePos);
            }
        }

        private void HandlePreviewWithMouse()
        {
            Vector3 mousePos = GetMouseWorldPosition();

            if (_preview != null)
            {
                _preview.transform.position = mousePos;
                List<Vector3> buildPositions = _preview.BuildingModel.GetAllBuildingPositions();
                bool canBuild = _grid.CanBuild(buildPositions) && ItemsManager.Instance.HasItem(_indexName, _preview.Data.Cost);

                _preview.transform.position = GetSnappedCenterrPosition(buildPositions);
                if (canBuild)
                {
                    _preview.ChangeState(BuildingPreview.BuildingPreviewState.POSITIVE);
                }
                else
                {
                    _preview.ChangeState(BuildingPreview.BuildingPreviewState.NEGATIVE);
                }
            }
        }

        private void OnBuildAction(bool isBuilding)
        {
            if (isBuilding && _preview != null)
            {
                TryPlaceBuilding();
            }
        }

        private void TryPlaceBuilding()
        {
            if (_preview == null) return;

            List<Vector3> buildPositions = _preview.BuildingModel.GetAllBuildingPositions();
            bool canBuild = _grid.CanBuild(buildPositions) && ItemsManager.Instance.HasItem(_indexName, _preview.Data.Cost);

            if (canBuild)
            {
                PlaceBuilding(buildPositions);
            }
        }

        private void CheckBuildingSelection()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.digit1Key.wasPressedThisFrame && _buildingDataList.Count > 0)
            {
                SelectBuilding(0);
                _indexName = "Wood";
            }
                
            else if (Keyboard.current.digit2Key.wasPressedThisFrame && _buildingDataList.Count > 1)
            {
                SelectBuilding(1);
                _indexName = "Essence";
            }
            else if (Keyboard.current.digit3Key.wasPressedThisFrame && _buildingDataList.Count > 2)
            {
                SelectBuilding(2);
                _indexName = "Ore";
            }
            else if (Keyboard.current.rKey.wasPressedThisFrame && _preview != null)
                _preview.Rotate(90);
        }
        public void SelectBuilding(int index)
        {
            Debug.Log($"Select {index} building");
            if (index < 0 || index >= _buildingDataList.Count) return;

            _selectedBuildingIndex = index;
            _isBuildingMode = true;

            if (_preview != null)
            {
                Destroy(_preview.gameObject);
                _preview = null;
            }
        }

        public void ExitBuildingMode()
        {
            _isBuildingMode = false;
            _selectedBuildingIndex = -1;

            if (_preview != null)
            {
                Destroy(_preview.gameObject);
                _preview = null;
            }
        }

        private void PlaceBuilding(List<Vector3> buildPositions)
        {
            ItemsManager.Instance.RemoveItem(_indexName, _preview.Data.Cost);
            Building building = Instantiate(_buildingPrefab, _preview.transform.position, Quaternion.identity);
            building.Setup(_preview.Data, _preview.BuildingModel.Rotation);
            _grid.SetBuilding(building, buildPositions);
            Destroy(_preview.gameObject);
            _preview = null;

            ExitBuildingMode();
        }

        private Vector3 GetSnappedCenterrPosition(List<Vector3> buildPositions)
        {
            if (buildPositions == null || buildPositions.Count == 0)
                return Vector3.zero;

            float minX = buildPositions.Min(p => p.x);
            float maxX = buildPositions.Max(p => p.x);
            float minZ = buildPositions.Min(p => p.z);
            float maxZ = buildPositions.Max(p => p.z);

            float centerX = (minX + maxX) / 2f;
            float centerZ = (minZ + maxZ) / 2f;

            float snappedX = Mathf.Round(centerX / CellSize) * CellSize;
            float snappedZ = Mathf.Round(centerZ / CellSize) * CellSize;

            return new Vector3(snappedX, 0, snappedZ);
        }

        private Vector3 GetMouseWorldPosition()
        {
            if (Mouse.current == null) return Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

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

        private void OnDestroy()
        {
            if (_input != null)
            {
                _input.DisablePlayerActions();
                _input.Attack -= OnBuildAction;
            }
        }
    }
}