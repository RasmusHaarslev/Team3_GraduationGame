using System;
using System.Collections;
using Assets.ModuleDesigner.Scripts.BaseClasses;
using UnityEngine;

namespace Assets.ModuleDesigner.Scripts
{
    public class PaletteApplier : TriggerReceiver
    {
        [Header("Output objects")]
        public TriggerReceiver[] Targets;
        public Material walkableGroundMaterial;
        public Material nonWalkableGroundMaterial;
        public int paletteNumber = 1;

        //Declare colors
        private Color ambientSkyColor;
        private Color ambientEquatorColor;
        private Color ambientGroundColor;

        private Color walkableGroundColor;
        private Color nonWalkableGroundColor;

        public override void TriggerEnter()
        {
            //Parse colors
            switch (paletteNumber)
            {
                default:
                case 1: //Yellow palette
                    {   parseColors("#D7D1B4", "#A5A08A", "#647388", "#5F4B03", "#472A00");  } break;
                case 2: //Red/purple palette
                    {   parseColors("#AE9393", "#867171", "#848AA1", "#434E88", "#3F3D4C");  } break;
            }

            //Apply colors
            RenderSettings.ambientSkyColor = ambientSkyColor;
            RenderSettings.ambientEquatorColor = ambientEquatorColor;
            RenderSettings.ambientGroundColor = ambientGroundColor;

            walkableGroundMaterial.SetColor("_MainColor", walkableGroundColor);
            nonWalkableGroundMaterial.SetColor("_MainColor", nonWalkableGroundColor);
        }

        //Collectively parses all colors in a palette, and sends them to appropriate
        void parseColors(string c1, string c2, string c3, string c4, string c5)
        {
            ColorUtility.TryParseHtmlString(c1, out walkableGroundColor);
            ColorUtility.TryParseHtmlString(c2, out nonWalkableGroundColor);

            ColorUtility.TryParseHtmlString(c3, out ambientSkyColor);
            ColorUtility.TryParseHtmlString(c4, out ambientEquatorColor);
            ColorUtility.TryParseHtmlString(c5, out ambientGroundColor);
        }

        public override void TriggerExit()
        { 
            //
        }

        void OnDrawGizmos()
        {
            if (KeepGizmo)
                OnDrawGizmosSelected();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawMesh(gizmoMesh, transform.position, transform.rotation, Vector3.one);

            foreach (var obj in Targets) 
            {
                obj.ShowGizmos();
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position + new Vector3(0, 0.5f, 0), obj.transform.position - new Vector3(0, 0.5f, 0));
            }
        }

        void OnValidate()
        {
            foreach (var target in Targets)
            {
                target.Expose(this.gameObject);
            }
        }

        public override void Expose(GameObject go)
        {

        }

        public override void ShowGizmos()
        {
            OnDrawGizmos();
        }
    }
}
