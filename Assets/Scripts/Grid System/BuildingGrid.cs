using System.Collections.Generic;
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

        private Vector3 GridOrigin => transform.position;

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
                if (IsValidGridPosition(x, y))
                {
                    _grid[x, y].SetBuilding(building);
                }
            }
        }

        private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
        {
            Vector3 localPos = worldPosition - GridOrigin;
            int x = Mathf.FloorToInt(localPos.x / BuildingSystem.CellSize);
            int y = Mathf.FloorToInt(localPos.z / BuildingSystem.CellSize);
            return (x, y);
        }

        private bool IsValidGridPosition(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }

        public bool CanBuild(List<Vector3> allBuildingPositions)
        {
            foreach (var p in allBuildingPositions)
            {
                (int x, int y) = WorldToGridPosition(p);
                if (!IsValidGridPosition(x, y) || !_grid[x, y].IsEmpty)
                {
                    return false;
                }
            }
            return true;
        }

        private void OnDrawGizmos()
        {
            if (BuildingSystem.CellSize <= 0.01f || _width <= 0 || _height <= 0)
                return;

            Gizmos.color = Color.yellow;

            Vector3 origin = GridOrigin;
            float cellSize = BuildingSystem.CellSize;

            for (int x = 0; x <= _width; x++)
            {
                Vector3 start = origin + new Vector3(x * cellSize, 0.01f, 0);
                Vector3 end = origin + new Vector3(x * cellSize, 0.01f, _height * cellSize);
                Gizmos.DrawLine(start, end);
            }

            for (int z = 0; z <= _height; z++)
            {
                Vector3 start = origin + new Vector3(0, 0.01f, z * cellSize);
                Vector3 end = origin + new Vector3(_width * cellSize, 0.01f, z * cellSize);
                Gizmos.DrawLine(start, end);
            }

            Gizmos.color = Color.green;
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    Vector3 cellCenter = origin + new Vector3(x * cellSize + cellSize / 2f, 0.02f, z * cellSize + cellSize / 2f);
                    Gizmos.DrawSphere(cellCenter, 0.1f);
                }
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