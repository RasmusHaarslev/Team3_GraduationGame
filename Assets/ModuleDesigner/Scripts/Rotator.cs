﻿using System;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.ModuleDesigner.Scripts
{
    public class Rotator : TriggerReceiver
    {
        [Header("Objects")]
        [Tooltip("Should contain objects the trigger should affect")]
        public List<GameObject> ObjectsToAffect;

        [Header("Rotation settings")]
        public float RotateDegrees;
        public bool RotateRandomly;
        public bool RotateIndividual;

        public override void TriggerEnter()
        {
            Vector3 globalRotation = new Vector3(UnityEngine.Random.Range(0.0f, RotateDegrees), UnityEngine.Random.Range(0.0f, RotateDegrees), UnityEngine.Random.Range(0.0f, RotateDegrees));

            foreach (var obj in ObjectsToAffect)
            {
                if (RotateRandomly)
                    obj.transform.Rotate(globalRotation);
                else if (RotateIndividual)
                {
                    Vector3 individualRotation = new Vector3(UnityEngine.Random.Range(0.0f, RotateDegrees), UnityEngine.Random.Range(0.0f, RotateDegrees), UnityEngine.Random.Range(0.0f, RotateDegrees));
                    obj.transform.Rotate(individualRotation);
                }
                else
                {
                    obj.transform.Rotate(Vector3.up, RotateDegrees);
                }
            }
        }

        public override void TriggerExit()
        {
            Vector3 globalRotation = new Vector3(UnityEngine.Random.Range(0.0f, RotateDegrees), UnityEngine.Random.Range(0.0f, RotateDegrees), UnityEngine.Random.Range(0.0f, RotateDegrees));

            foreach (var obj in ObjectsToAffect)
            {
                if (RotateRandomly)
                    obj.transform.Rotate(globalRotation);
                else if (RotateIndividual)
                {
                    Vector3 individualRotation = new Vector3(UnityEngine.Random.Range(0.0f, RotateDegrees), UnityEngine.Random.Range(0.0f, RotateDegrees), UnityEngine.Random.Range(0.0f, RotateDegrees));
                    obj.transform.Rotate(individualRotation);
                }
                else
                    obj.transform.Rotate(Vector3.up, -RotateDegrees);
            }
        }

        public override void Expose(GameObject go)
        {

        }

        public override void ShowGizmos()
        {
            OnDrawGizmosSelected();
        }

        void OnDrawGizmos()
        {
            if (KeepGizmo)
                OnDrawGizmosSelected();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawMesh(gizmoMesh, transform.position, transform.rotation, Vector3.one);

            #region checkingStuff
            List<GameObject> Removes = new List<GameObject>();
            foreach (var obj in ObjectsToAffect)
            {
                if (obj == null)
                {
                    Removes.Add(obj);
                }
            }
            foreach (var obj in Removes)
            {
                if (obj == null)
                {
                    ObjectsToAffect.Remove(obj);

                }
            }
            #endregion

            foreach (var obj in ObjectsToAffect)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position + new Vector3(0,0.5f,0), obj.transform.position - new Vector3(0, 0.5f, 0));
            }
        }

        void OnValidate()
        {
            foreach(var obj in ObjectsToAffect)
            {
                obj.isStatic = false;
            }
        }
    }
}
