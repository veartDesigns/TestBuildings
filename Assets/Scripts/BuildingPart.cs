using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class BuildingPart
    {
        public List<int> Triangles;
        public readonly List<Vector2> Uvs;
        public List<Vector3> Vertices;
        public int BuildingShapeVerts;
        public Building.BuildingTerminations RoofType;

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
            for (int i = 0; i < 4; i++)
            {
                int initialPoint = (i + 1) * 4;
                int endPoint = (i + 2) * 4;
                endPoint = endPoint < Vertices.Count ? endPoint : 4;
                Triangles.Add(i);
                Triangles.Add(initialPoint + 1);
                Triangles.Add(initialPoint);

                Triangles.Add(i);
                Triangles.Add(initialPoint + 2);
                Triangles.Add(initialPoint + 1);

                Triangles.Add(i);
                Triangles.Add(initialPoint + 3);
                Triangles.Add(initialPoint + 2);

                Triangles.Add(i);
                Triangles.Add(endPoint);
                Triangles.Add(initialPoint + 3);
            }

            GeneralFunctions.CreateObject(Vertices, Triangles);
        }

        public override void SetUVs()
        {
        }
    }

    public class BuildingRoof : BuildingPart
    {
        public override void SetTriangles()
        {
            Triangles = BuildingTerminationsCreator.GetRoofTriangles(Vertices, RoofType, BuildingShapeVerts);
            GeneralFunctions.CreateObject(Vertices, Triangles);
        }

        public override void SetUVs()
        {
        }
    }

    public class BuildingStructure : BuildingPart
    {
        public override void SetTriangles()
        {
            int floors = Vertices.Count / BuildingShapeVerts;

            for (int i = 0; i < floors - 1; i++)
            {
                for (int j = 0; j < BuildingShapeVerts; j++)
                {
                    int initialPoint = i * BuildingShapeVerts;
                    int index1 = initialPoint + j;
                    int index2 = index1 + BuildingShapeVerts;
                    int index3 = index1 + 1 >= initialPoint + BuildingShapeVerts ? initialPoint : index1 + 1;
                    int index4 = index3 + BuildingShapeVerts;

                    Triangles.Add(index1);
                    Triangles.Add(index3);
                    Triangles.Add(index2);

                    Triangles.Add(index4);
                    Triangles.Add(index2);
                    Triangles.Add(index3);
                }
            }

            GeneralFunctions.CreateObject(Vertices, Triangles);
        }

        public override void SetUVs()
        {
        }
    }

    public class StreetPlanes : BuildingPart
    {
        public override void SetTriangles()
        {
            for (int i = 0; i < Vertices.Count; i += 6)
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
            }

            GeneralFunctions.CreateObject(Vertices, Triangles);
        }

        public override void SetUVs()
        {
        }
    }

    public static class GeneralFunctions
    {
        public static void CreateObject(List<Vector3> verts, List<int> tris)
        {
            GameObject go = new GameObject();
            go.AddComponent<MeshRenderer>();
            MeshFilter gomf = go.AddComponent<MeshFilter>();

            Mesh goadMesh = new Mesh();
            goadMesh.vertices = verts.ToArray();
            goadMesh.triangles = tris.ToArray();
            goadMesh.RecalculateNormals();
            gomf.mesh = goadMesh;
        }
    }
}