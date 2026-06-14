using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace GardenDefense
{
    public class BuildingGrid : MonoBehaviour
    {
        [SerializeField]
        int _width;
        [SerializeField]
        int _height;

        BuildingGridCell[,] _grid;
        private void Start()
        {
            _grid = new BuildingGridCell[_width, _height];
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _grid[x, y] = new BuildingGridCell();
                }
            }
        }

        public void SetBuilding(Building building, List<Vector3> allBuildingPositions)
        {
            foreach (var p in allBuildingPositions)
            {
                (int x, int y) = WorldToGridPosition(p);
                _grid[x, y].SetBuilding(building);
            }
        }

        private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt(worldPosition.x / BuildingSystem.CellSize);
            int y = Mathf.FloorToInt(worldPosition.z / BuildingSystem.CellSize);
            return (x, y);
        }

        public bool CanBuild(List<Vector3> allBuildingPositions)
        {
            foreach (var p in allBuildingPositions)
            {
                (int x, int y) = WorldToGridPosition(p);
                if (x < 0 || x >= _width || y < 0 || y >= _height || !_grid[x, y].IsEmpty)
                {
                    return false;
                }
            }
            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (BuildingSystem.CellSize <= 0 || _width <= 0 || _height <= 0)
                return;
            Vector3 origin = transform.position;
            for (int y = 0; y < _height; y++)
            {
                Vector3 start = origin + new Vector3(0, 0.01f, y * BuildingSystem.CellSize);
                Vector3 end = origin + new Vector3(_width * BuildingSystem.CellSize, 0.01f, y * BuildingSystem.CellSize);
                Gizmos.DrawLine(start, end);
            }
            for (int x = 0; x < _width; x++)
            {
                Vector3 start = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, 0);
                Vector3 end = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, _height * BuildingSystem.CellSize);
                Gizmos.DrawLine(start, end);
            }
        }
        public class BuildingGridCell
        {
            Building _building;
            public bool IsEmpty => _building == null;
            public void SetBuilding(Building building)
            {
                _building = building;
            }
        }
    }
}
