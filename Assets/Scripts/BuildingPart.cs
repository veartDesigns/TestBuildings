using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class BuildingPart
    {
        public readonly List<int> Triangles;
        public readonly List<Vector2> Uvs;
        public List<Vector3> Vertices;

        protected BuildingPart()
        {
            Triangles = new List<int>();
            Uvs = new List<Vector2>();
        }

        public void SetVertex(List<Vector3> verts)
        {
            if (Vertices != null) Vertices.Clear();
            Vertices = verts;
        }

        public abstract void SetTriangles();
        public abstract void SetUVs();
    }

    public class BuildingBase : BuildingPart
    {
        public override void SetTriangles()
        {
        }

        public override void SetUVs()
        {
        }
    }
    
    public class RoundedBuildingBase : BuildingPart
    {
        public override void SetTriangles()
        {
            
        }

        public override void SetUVs()
        {
        }
    }
    public class BuildingRoof : BuildingPart
    {
        public override void SetTriangles()
        {
        }

        public override void SetUVs()
        {
        }
    }

    public class BuildingFloor : BuildingPart
    {
        public override void SetTriangles()
        {
        }

        public override void SetUVs()
        {
        }
    }

    public class StreetPlanes : BuildingPart
    {
        public override void SetTriangles()
        {
            for (int i = 0; i < Vertices.Count - 6; i += 6)
            {
                Triangles.Add(i);
                Triangles.Add(i + 1);
                Triangles.Add(i + 2);

                Triangles.Add(i + 2);
                Triangles.Add(i + 3);
                Triangles.Add(i);

                Triangles.Add(i + 4);
                Triangles.Add(i + 3);
                Triangles.Add(i + 2);

                Triangles.Add(i + 2);
                Triangles.Add(i + 5);
                Triangles.Add(i + 4);

                Math3D.CreateSphereInPos(Vertices[i], Color.magenta, "StreetPlanes_" + i);
            }

            GameObject go = new GameObject();
            MeshRenderer goad = go.AddComponent<MeshRenderer>();
            MeshFilter gomf = go.AddComponent<MeshFilter>();

            Mesh goadMesh = new Mesh
            {
                vertices = Vertices.ToArray(),
                triangles = Triangles.ToArray()
            };
            gomf.mesh = goadMesh;
        }

        public override void SetUVs()
        {
        }
    }
}