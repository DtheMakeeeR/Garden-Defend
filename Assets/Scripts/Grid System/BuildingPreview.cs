using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GardenDefense
{
    public class BuildingPreview : MonoBehaviour
    {
        public enum BuildingPreviewState
        {
            POSITIVE,
            NEGATIVE,
        }
        [SerializeField]
        Material _positiveMaterial;
        [SerializeField]
        Material _negativeMaterial;
        public BuildingPreviewState State { get; private set; } = BuildingPreviewState.NEGATIVE;
        public BuildingData Data { get; private set; }
        public BuildingModel BuildingModel { get; private set; }
        List<Renderer> _renderers = new List<Renderer>();
        List<Collider> _colliders = new List<Collider>();

        public void Setup(BuildingData data)
        {
            Data = data;
            BuildingModel = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
            _renderers.AddRange(BuildingModel.GetComponentsInChildren<Renderer>());
            _colliders.AddRange(BuildingModel.GetComponentsInChildren<Collider>());
            foreach (var col in _colliders)
            {
                col.enabled = false;
            }
            SetPreviewMaterial(State);
        }
        public void ChangeState(BuildingPreviewState newState)
        {
            if (newState == State) return;
            State = newState;
            SetPreviewMaterial(State);
        }
        public void Rotate(int rotationStep = 90)
        {
            BuildingModel.Rotate(rotationStep);
        }
        private void SetPreviewMaterial(BuildingPreviewState newState)
        {
            Material previewMat = newState == BuildingPreviewState.POSITIVE ? _positiveMaterial : _negativeMaterial;
            foreach (var renderer in _renderers)
            {
                Material[] mats = new Material[renderer.sharedMaterials.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = previewMat;
                }
                renderer.materials = mats;
            }
        }
    }
}
