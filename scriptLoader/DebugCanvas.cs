using System;
using UnityEngine;

namespace scriptLoader
{
    public class DebugCanvas : MonoBehaviour
    {
        public Action PostRender = null;
        public Action PostRenderScreen = null;
        public Color Shade;
        public Material DebugMaterial;

        public bool RenderEnabled = true;
        public KeyCode KeyCode_Shade = KeyCode.RightShift;
        public KeyCode KeyCode_RenderToggle = KeyCode.RightControl;

        private Camera renderCamera = null;

        public void Awake()
        {
            var shader = Shader.Find("Hidden/Internal-Colored");

            DebugMaterial = new Material(shader);

            DebugMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            DebugMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            DebugMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            DebugMaterial.SetInt("_ZWrite", 0);
            DebugMaterial.SetInt("_ZTest", 0);

            Shade = Color.black;
            Shade.a = 0.99f;

            renderCamera = this.GetComponent<Camera>();
        }

        public void OnRenderObject()
        {
            if (Input.GetKeyDown(KeyCode_RenderToggle)) RenderEnabled = !RenderEnabled;
            if (!RenderEnabled) return;
            if (Camera.current != renderCamera) return;
            DebugMaterial.SetPass(0);
            GL.PushMatrix();

            //Screen space
            GL.LoadPixelMatrix();
            if (Input.GetKey(KeyCode_Shade))
            {
                GL.Begin(GL.QUADS);
                Draw.color = Shade;
                Draw.Rect(-4000, -3000, 8000, 6000);
                GL.End();
            }
            GL.Begin(GL.LINES);
            PostRenderScreen?.Invoke();
            GL.End();
            GL.PopMatrix();

            //World space
            GL.LoadProjectionMatrix(renderCamera.projectionMatrix);
            GL.Begin(GL.LINES);
            PostRender?.Invoke();
            GL.End();
            GL.PopMatrix();
        }
    }
}
