using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scriptLoader
{
    internal class Edge
    {
        public int i1;
        public int i2;

        public Edge(int i1, int i2)
        {
            this.i1 = i1;
            this.i2 = i2;
        }

        public bool Match(Edge e)
        {
            return (e.i1 == i1 && e.i2 == i2) || (e.i1 == i2 && e.i2 == i1);
        }
    }

    public static class Draw
    {
        public static Color color = Color.white;

        #region Line

        /// <summary>
        /// Draws line between 2 screen points. Use Draw.PixelToScreen to convert from pixels to screen points
        /// </summary>
        public static void Line(Vector2 p1, Vector2 p2)
        {
            Prepare();

            // Vertices
            GL.Vertex(p1);
            GL.Vertex(p2);
        }

        /// <summary>
        /// Draws line between 2 world points
        /// </summary>
        public static void Line3D(Vector3 p1, Vector3 p2)
        {
            Prepare3D();

            GL.Vertex(p1);
            GL.Vertex(p2);
        }

        #endregion


        #region Circles and Ellipses

        /// <summary>
        /// Draws circle using a screen point center, and pixel radius. Use Draw.ScreenToPixel to convert screen points to pixels
        /// </summary>
        public static void Circle(Vector2 center, float pixelRadius)
        {
            Vector2 size = new Vector2(pixelRadius / Screen.width, pixelRadius / Screen.height);

            Ellipse(center, size);
        }

        /// <summary>
        /// Draws dashed circle using a screen point center, and pixel radius. Use Draw.ScreenToPixel to convert screen points to pixels
        /// The dashes are not very nice, just skipping every other point..
        /// </summary>
        public static void CircleDashed(Vector2 center, float radius)
        {
            Prepare();

            float radX = radius / Screen.width;
            float radY = radius / Screen.height;

            // Vertices
            for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.01f)
            {
                Vector2 ci = new Vector2(center.x + (Mathf.Cos(theta) * radX), center.y + (Mathf.Sin(theta) * radY));
                GL.Vertex(ci);
            }
        }

        /// <summary>
        /// Draws an ellipse using a center, and size (in screen fractions). Use Draw.ScreenToPixel to convert screen points to pixels
        /// </summary>
        public static void Ellipse(Vector2 center, Vector2 size)
        {
            Prepare();

            float radX = size.x;
            float radY = size.y;

            // Vertices
            for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.01f)
            {
                Vector2 ci = new Vector2(center.x + (Mathf.Cos(theta) * radX), center.y + (Mathf.Sin(theta) * radY));
                GL.Vertex(ci);

                if (theta != 0)
                    GL.Vertex(ci);
            }
        }

        /// <summary>
        /// Draws a circle in world space
        /// </summary>
        public static void Circle3D(Vector3 center, float radius, Vector3 normal)
        {
            Prepare3D();

            normal = normal.normalized;
            Vector3 forward = normal == Vector3.up ?
                Vector3.ProjectOnPlane(Vector3.forward, normal).normalized :
                Vector3.ProjectOnPlane(Vector3.up, normal);
            Vector3 right = Vector3.Cross(normal, forward);

            for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.2f)
            {
                Vector3 ci = center + forward * Mathf.Cos(theta) * radius + right * Mathf.Sin(theta) * radius;
                GL.Vertex(ci);

                if (theta != 0)
                    GL.Vertex(ci);
            }
            GL.Vertex(center + forward * Mathf.Cos(0) * radius + right * Mathf.Sin(0) * radius);
        }


        /// <summary>
        /// Draws an elliptic orbit using eccentricity and semi-major axis in pixels
        /// </summary>
        public static void Orbit(Vector2 center, float eccentricity, float semiMajorAxis, float dir = 0)
        {
            Prepare();

            eccentricity = Mathf.Clamp01(eccentricity);

            Vector2 up = new Vector2(Mathf.Cos(dir), Mathf.Sin(dir));
            Vector2 right = Vector3.Cross(up, Vector3.forward);

            for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.01f)
            {
                float r = (semiMajorAxis * (1 - eccentricity * eccentricity)) / (1 + eccentricity * Mathf.Cos(theta));

                Vector2 point = PixelToScreen((right * Mathf.Cos(theta) * r) + (up * Mathf.Sin(theta) * r));
                Vector2 ci = center + point;

                GL.Vertex(ci);

                if (theta != 0)
                    GL.Vertex(ci);
            }
        }

        /// <summary>
        /// Draws an elliptic orbit using periapsis and apoapsis in pixels
        /// </summary>
        public static void OrbitApses(Vector2 center, float periapsis, float apoapsis, float dir = 0)
        {
            float a = (periapsis + apoapsis) / 2;
            float e = (apoapsis - periapsis) / (apoapsis + periapsis);

            Orbit(center, e, a, dir);
        }

        /// <summary>
        /// Draws an elliptic orbit in world space using eccentricity and semi-major axis
        /// </summary>
        public static void Orbit3D(Vector3 center, float eccentricity, float semiMajorAxis, Vector3 normal, Vector3 forward)
        {
            Prepare3D();

            eccentricity = Mathf.Clamp01(eccentricity);

            forward = Vector3.ProjectOnPlane(forward, normal).normalized;
            Vector3 right = Vector3.Cross(forward, normal).normalized;

            for (float theta = 0.0f; theta < (2 * Mathf.PI); theta += 0.01f)
            {
                float r = (semiMajorAxis * (1 - eccentricity * eccentricity)) / (1 + eccentricity * Mathf.Cos(theta));

                Vector3 point = (right * Mathf.Cos(theta) * r) + (forward * Mathf.Sin(theta) * r);
                Vector3 ci = center + point;

                GL.Vertex(ci);

                if (theta != 0)
                    GL.Vertex(ci);
            }
        }

        /// <summary>
        /// Draws an elliptic orbit in world space using periapsis and apoapsis
        /// </summary>
        public static void Orbit3DApses(Vector3 center, float periapsis, float apoapsis, Vector3 normal, Vector3 forward)
        {
            float a = (periapsis + apoapsis) / 2;
            float e = (apoapsis - periapsis) / (apoapsis + periapsis);

            Orbit3D(center, e, a, normal, forward);
        }

        #endregion


        #region Rectangle

        /// <summary>
        /// Draws rectangle on screen using a Rect (in pixels)
        /// </summary>
        public static void Rect(Rect rect)
        {
            Rect(rect.x, rect.y, rect.width, rect.height);
        }

        /// <summary>
        /// Draws rectangle on screen (in screen 0-1 fractions)
        /// </summary>
        public static void RectScreen(float x, float y, float width, float height)
        {
            Prepare();

            GL.Vertex(new Vector3(x, y));
            GL.Vertex(new Vector3(x + width, y));

            GL.Vertex(new Vector3(x + width, y));
            GL.Vertex(new Vector3(x + width, y + height));

            GL.Vertex(new Vector3(x + width, y + height));
            GL.Vertex(new Vector3(x, y + height));

            GL.Vertex(new Vector3(x, y + height));
            GL.Vertex(new Vector3(x, y));
        }

        /// <summary>
        /// Draws rectangle on screen (in pixels)
        /// </summary>
        public static void Rect(float x, float y, float width, float height)
        {
            Prepare();

            GL.Vertex(new Vector3(x, y));
            GL.Vertex(new Vector3(x + width, y));

            GL.Vertex(new Vector3(x + width, y));
            GL.Vertex(new Vector3(x + width, y + height));

            GL.Vertex(new Vector3(x + width, y + height));
            GL.Vertex(new Vector3(x, y + height));

            GL.Vertex(new Vector3(x, y + height));
            GL.Vertex(new Vector3(x, y));
        }

        #endregion


        #region Grid
        /// <summary>
        /// Draws a horizontal grid
        /// </summary>
        public static void Grid(Vector3 center, float edgeLength, int lines = 10)
        {
            Grid(center, edgeLength, lines, Vector3.forward, Vector3.up);
        }

        /// <summary>
        /// Draws a gird with custom orientantion
        /// </summary>
        public static void Grid(Vector3 center, float edgeLength, int lines, Vector3 forward, Vector3 normal)
        {
            if (lines <= 1) return;

            Prepare3D();

            forward = forward.normalized;
            normal = Vector3.ProjectOnPlane(normal, forward).normalized;
            Vector3 right = Vector3.Cross(forward, normal);

            // forward lines
            for (int i = 0; i < lines; i++)
            {
                Vector3 fDir = forward * edgeLength * 0.5f;
                Vector3 rDir = right * (-(edgeLength * 0.5f) + (i * edgeLength / (lines - 1)));

                GL.Vertex(center - fDir + rDir);
                GL.Vertex(center + fDir + rDir);
            }

            // sideways lines
            for (int i = 0; i < lines; i++)
            {
                Vector3 rDir = right * edgeLength * 0.5f;
                Vector3 fDir = forward * (-(edgeLength * 0.5f) + (i * edgeLength / (lines - 1)));

                GL.Vertex(center - rDir + fDir);
                GL.Vertex(center + rDir + fDir);
            }
        }

        #endregion


        #region 3D Primitives

        /// <summary>
        /// Draws a sphere in world space. Similar to Gizmos.DrawWireSphere, but with the ability to add radial and vertical segments
        /// </summary>
        public static void Sphere(Vector3 center, float radius, int verticalSegments = 1, int radialSegments = 2)
        {
            if (radialSegments > 2)
            {
                for (int i = 0; i < radialSegments; i++)
                {
                    Vector3 normal = new Vector3(Mathf.Sin((i * Mathf.PI) / radialSegments), 0, Mathf.Cos((i * Mathf.PI) / radialSegments));

                    Circle3D(center, radius, normal);
                }
            }
            else
            {
                Circle3D(center, radius, Vector3.forward);
                Circle3D(center, radius, Vector3.right);
            }

            if (verticalSegments > 1)
            {
                for (int i = 1; i < verticalSegments; i++)
                {
                    Vector3 c = center + Vector3.up * (-radius + (i * 2 * (radius / (verticalSegments))));

                    // Radius of base circle is a=sqrt(h(2R-h)), 
                    float height = ((float)i / verticalSegments) * radius * 2;
                    float ra = Mathf.Sqrt(height * (2 * radius - height));

                    Circle3D(c, ra, Vector3.up);
                }
            }
            else
                Circle3D(center, radius, Vector3.up);
        }

        public static void Cube(Transform t, float size, Color c)
        {
            color = c;
            Cube(t.position, Vector3.one * size, t.forward, t.up);
        }

        public static void Cube(Vector3 center, Quaternion rotation, float size)
        {
            var pl = (rotation * Vector3.left) * size;
            var pu = (rotation * Vector3.up) * size;
            var pf = (rotation * Vector3.forward) * size;
            

            Cube(center, Vector3.one * size, rotation * Vector3.forward, rotation * Vector3.up);
        }

        public static void Cube(Vector3 center, Vector3 size, Vector3 forward, Vector3 up)
        {
            Prepare3D();

            forward = forward.normalized;
            up = Vector3.ProjectOnPlane(up, forward).normalized;
            Vector3 right = Vector3.Cross(forward, up);

            Vector3 frw = forward * size.z * 0.5f;
            Vector3 rgt = right * size.x * 0.5f;
            Vector3 upw = up * size.y * 0.5f;

            // vertical lines
            GL.Vertex(center - frw - rgt - upw);
            GL.Vertex(center - frw - rgt + upw);

            GL.Vertex(center - frw + rgt - upw);
            GL.Vertex(center - frw + rgt + upw);

            GL.Vertex(center + frw - rgt - upw);
            GL.Vertex(center + frw - rgt + upw);

            GL.Vertex(center + frw + rgt - upw);
            GL.Vertex(center + frw + rgt + upw);

            // horizontal lines
            GL.Vertex(center - frw - rgt - upw);
            GL.Vertex(center - frw + rgt - upw);

            GL.Vertex(center - frw - rgt + upw);
            GL.Vertex(center - frw + rgt + upw);

            GL.Vertex(center + frw - rgt - upw);
            GL.Vertex(center + frw + rgt - upw);

            GL.Vertex(center + frw - rgt + upw);
            GL.Vertex(center + frw + rgt + upw);

            GL.Vertex(center + frw - rgt + upw);
            GL.Vertex(center + frw + rgt - upw);

            GL.Vertex(center + frw + rgt + upw);
            GL.Vertex(center + frw - rgt - upw);

            // forward lines
            GL.Vertex(center - frw - rgt - upw);
            GL.Vertex(center + frw - rgt - upw);

            GL.Vertex(center - frw + rgt - upw);
            GL.Vertex(center + frw + rgt - upw);

            GL.Vertex(center - frw - rgt + upw);
            GL.Vertex(center + frw - rgt + upw);

            GL.Vertex(center - frw + rgt + upw);
            GL.Vertex(center + frw + rgt + upw);
        }

        #endregion


        #region Wireframe


        /// <summary>
        /// Draws mesh wireframe. Use Draw.GetEdgePointsFromMesh() to get edgePoints, preferably only once
        /// </summary>
        public static void Wireframe(Transform t, Vector3[] edgePoints)
        {
            if (edgePoints == null) return;
            if (edgePoints.Length < 2) return;

            Prepare3D();

            for (int i = 0; i < edgePoints.Length; i++)
                GL.Vertex(t.TransformPoint(edgePoints[i]));
        }

        /// <summary>
        /// Gets edge points from a mesh.
        /// Call this once, and then use Wireframe() to draw the lines
        /// </summary>
        public static Vector3[] GetEdgePointsFromMesh(Mesh mesh)
        {
            Edge[] edges = GetEdges(mesh);
            return EdgesToVertices(edges, mesh);
        }

        /// <summary>
        /// Gets edge points from a mesh.
        /// In case you want both the shaded model and wireframe to show, you can use normalPush to offset edges from the mesh so it doesn't intersect with it.
        /// Call this once, and then use Wireframe() to draw the lines
        /// </summary>
        public static Vector3[] GetEdgePointsFromMesh(Mesh mesh, float normalPush)
        {
            Edge[] edges = GetEdges(mesh);
            return EdgesToVertices(edges, mesh, normalPush);
        }


        static Edge[] GetEdges(Mesh mesh)
        {
            int[] tris = mesh.triangles;

            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < tris.Length; i += 3)
            {

                Edge e1 = new Edge(tris[i], tris[i + 1]);
                Edge e2 = new Edge(tris[i + 1], tris[i + 2]);
                Edge e3 = new Edge(tris[i + 2], tris[i]);

                // if line already exists, skip

                foreach (var edge in edges)
                    if (edge.Match(e1)) goto NoE1;

                edges.Add(e1);

                NoE1:

                foreach (var edge in edges)
                    if (edge.Match(e2)) goto NoE2;

                edges.Add(e2);

                NoE2:

                foreach (var edge in edges)
                    if (edge.Match(e3)) goto NoE3;

                edges.Add(e3);

                NoE3:;

            }

            return edges.ToArray();
        }

        static Vector3[] EdgesToVertices(Edge[] edges, Mesh mesh, float normalPush = 0)
        {
            Vector3[] vertices = mesh.vertices;

            Vector3[] edgesV3 = new Vector3[edges.Length * 2];

            Vector3[] normals = null;

            if (normalPush != 0)
                normals = mesh.normals;

            for (int i = 0; i < edges.Length; i++)
            {
                edgesV3[i * 2] = vertices[edges[i].i1];
                edgesV3[i * 2 + 1] = vertices[edges[i].i2];

                if (normalPush != 0)
                {
                    edgesV3[i * 2] += normals[edges[i].i1] * normalPush;
                    edgesV3[i * 2 + 1] += normals[edges[i].i2] * normalPush;
                }
            }

            return edgesV3;
        }

        #endregion Wireframe


        #region Utils

        /// <summary>
        /// Converts a coordinate in pixels to screen 0-1 fraction point.
        /// Example: 400, 300, on a 800x600 screen will output 0.5, 0.5 (middle of the screen)
        /// </summary>
        public static Vector2 PixelToScreen(Vector2 pos)
        {
            return new Vector2(pos.x / Screen.width, pos.y / Screen.height);
        }

        /// <summary>
        /// Converts a coordinate in pixels to screen 0-1 fraction point.
        /// Example: 400, 300, on a 800x600 screen will output 0.5, 0.5 (middle of the screen)
        /// </summary>
        public static Vector2 PixelToScreen(float x, float y)
        {
            return new Vector2(x / Screen.width, y / Screen.height);
        }

        static void Prepare()
        {
            GL.Color(color);
        }
        
        static void Prepare3D()
        {
            GL.Color(color);
        }

        #endregion

        #region Custom Draw Methods

        public static void DynamicBoneCollider(DynamicBoneCollider dbc, Color c)
        {
            Draw.color = c;
            float radius = dbc.m_Radius * Mathf.Abs(dbc.transform.lossyScale.x);
            float num = dbc.m_Height * 0.5f - dbc.m_Radius;
            if (num <= 0f)
            {
                Draw.Sphere(dbc.transform.TransformPoint(dbc.m_Center), radius, 8, 8);
            }
            else
            {
                Vector3 center = dbc.m_Center;
                Vector3 center2 = dbc.m_Center;
                switch (dbc.m_Direction)
                {
                    case global::DynamicBoneCollider.Direction.X:
                        center.x -= num;
                        center2.x += num;
                        break;
                    case global::DynamicBoneCollider.Direction.Y:
                        center.y -= num;
                        center2.y += num;
                        break;
                    case global::DynamicBoneCollider.Direction.Z:
                        center.z -= num;
                        center2.z += num;
                        break;
                }
                Draw.Sphere(dbc.transform.TransformPoint(center), radius, 8, 8);
                Draw.Sphere(dbc.transform.TransformPoint(center2), radius, 8, 8);
            }
        }


        public static void DynamicBoneV1(DynamicBone b, Color c)
        {
            var lstParticles = Reflect.FetchValue<IList>(b, "m_Particles");
            var os = Reflect.FetchValue<float>(b, "m_ObjectScale");
            Transform prev = null;
            var t = lstParticles[0].GetType();
            Draw.color = c;
            foreach (var part in lstParticles)
            {
                var pf = (Transform)t.GetField("m_Transform").GetValue(part);
                var pr = (float)t.GetField("m_Radius").GetValue(part);
                if (prev != null)
                    Draw.Line3D(prev.position, pf.position);
                prev = pf;
                Draw.Circle3D(pf.position, 0.004f, pf.rotation * Vector3.up);
            }
        }

        public static void DynamicBoneV2(DynamicBone_Ver02 b, Color c)
        {
            Draw.color = c;
            for (var i = 0; i < b.Bones.Count - 1; i++)
                Draw.Line3D(b.Bones[i].position, b.Bones[i + 1].position);
            Draw.Line3D(b.Bones[3].position, b.Bones[3].position + b.Force * 10f);
        }

        public static void Diamond(Vector3 position, Quaternion angle, float size, Color c)
        {
            var up = (angle * Vector3.up) * size;
            var left = (angle * Vector3.left) * size;
            Draw.color = c;
            var p1 = position + up;
            var p2 = position + left;
            var p3 = position - up;
            var p4 = position - left;
            Draw.Line3D(p1, p2);
            Draw.Line3D(p2, p3);
            Draw.Line3D(p3, p4);
            Draw.Line3D(p4, p1);
        }

        public static void Diamond(Transform t, float size, Color c)
        {
            Draw.Diamond(t.position, t.rotation, size, c);
        }

        public static void Star(Vector3 position, Quaternion angle, float size, Color c)
        {
            Draw.color = c;
            var up = (angle * Vector3.up) * size;
            var left = (angle * Vector3.left) * size;
            var forward = (angle * Vector3.forward) * size;

            Draw.Line3D(position - up, position + up);
            Draw.Line3D(position - left, position + left);
            Draw.Line3D(position - forward, position + forward);
        }

        public static void Star(Transform t, float size, Color c)
        {
            Draw.Star(t.position, t.rotation, size, c);
        }
        #endregion
    }
}