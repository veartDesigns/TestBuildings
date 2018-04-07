using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public static class BuildingTerminationsCreator
    {
        public static List<Vector3> CreateRoofTop(Building.BuildingTerminations buildingTerminations,
            List<Vector3> roofBase, Vector3 planeDirection, float moduleHeight)
        {
            switch (buildingTerminations)
            {
                case Building.BuildingTerminations.Flat:
                    return CreateFlatRooftop(roofBase);
                case Building.BuildingTerminations.Rounded:
                    return CreateRoundedRooftop(roofBase, planeDirection, moduleHeight);
                case Building.BuildingTerminations.StairsOut:
                    return CreateStairsOutRooftop(roofBase);
                case Building.BuildingTerminations.StairsIn:
                    return CreateStairsInRooftop(roofBase);
                case Building.BuildingTerminations.ToMiddle:
                    return CreateToMiddleUpRooftop(roofBase);
                case Building.BuildingTerminations.Floor:
                    return CreateFloor(roofBase);
                default:
                    throw new ArgumentOutOfRangeException("buildingTerminations", buildingTerminations, null);
            }
        }

        private static List<Vector3> CreateFlatRooftop(List<Vector3> baseRoofPoints)
        {
            if (baseRoofPoints.Count <= 4) return null;

            List<Vector3> roofPoints =
                new List<Vector3>(baseRoofPoints) {Math3D.GetCenterFromPointList(baseRoofPoints)};

            return roofPoints;
        }

        private static List<Vector3> CreateRoundedRooftop(List<Vector3> baseRoofPoints, Vector3 planeDirection,
            float moduleHeight)
        {
            List<Vector3> roofPoints = new List<Vector3>();
            roofPoints.AddRange(baseRoofPoints);

            Vector3 center = Math3D.GetCenterFromPointList(baseRoofPoints);
            int floorsTermination = 5;
            float totalHeight = 0;
            float currentHeight = moduleHeight*Random.Range(0.5f,.7f);
            float centerFactor = Random.Range(0.6f, 0.9f);
            float centerExpFactor = Random.Range(1.1f, 1.8f);
            float heightExpFactor = Random.Range(1.6f, 2.6f);
            float offsetTopCenter = Random.Range(0, 0.3f);

            for (int i = 0; i < floorsTermination; i++)
            {
                Vector3 addedHeight = (totalHeight + currentHeight) * planeDirection;

                for (int j = 0; j < baseRoofPoints.Count; j++)
                {
                    Vector3 point = baseRoofPoints[j];
                    Vector3 newCenter = center + addedHeight;
                    point = point + addedHeight;
                    point = Vector3.Lerp(newCenter, point, centerFactor);

                    roofPoints.Add(point);
                }
                if (i < floorsTermination - 1) totalHeight += currentHeight;

                centerFactor /= centerExpFactor;
                currentHeight /= heightExpFactor;
            }

            roofPoints.Add(center + ((totalHeight + offsetTopCenter) * planeDirection));

            return roofPoints;
        }

        private static List<Vector3> CreateStairsOutRooftop(List<Vector3> baseRoofPoints)
        {
            List<Vector3> roofPoints = new List<Vector3>();
            return roofPoints;
        }

        private static List<Vector3> CreateStairsInRooftop(List<Vector3> baseRoofPoints)
        {
            List<Vector3> roofPoints = new List<Vector3>();
            return roofPoints;
        }

        private static List<Vector3> CreateToMiddleUpRooftop(List<Vector3> baseRoofPoints)
        {
            List<Vector3> roofPoints = new List<Vector3>();
            return roofPoints;
        }

        private static List<Vector3> CreateFloor(List<Vector3> baseRoofPoints)
        {
            return CreateFlatRooftop(baseRoofPoints);
        }

        public static List<int> GetRoofTriangles(List<Vector3> vertices, Building.BuildingTerminations termination,
            int buildingShapeVerts)
        {
            switch (termination)
            {
                case Building.BuildingTerminations.Flat:
                    return GetFlatRooftopTriangles(vertices, 0);
                case Building.BuildingTerminations.Rounded:
                    return GetRoundedRooftopTriangles(vertices, buildingShapeVerts);
                case Building.BuildingTerminations.StairsOut:
                    break;
                case Building.BuildingTerminations.StairsIn:
                    break;
                case Building.BuildingTerminations.ToMiddle:
                    break;
                case Building.BuildingTerminations.Floor:
                    return GetFloorTriangles(vertices);
                default:
                    throw new ArgumentOutOfRangeException("termination", termination, null);
            }

            return new List<int>();
        }

        private static List<int> GetFloorTriangles(List<Vector3> vertices)
        {
            return GetFlatRooftopTriangles(vertices, 0);
        }

        private static List<int> GetRoundedRooftopTriangles(List<Vector3> vertices, int buildingShapeVerts)
        {
            List<int> triangles = new List<int>();
            int floors = vertices.Count / buildingShapeVerts;

            for (int i = 0; i < floors - 1; i++)
            {
                for (int j = 0; j < buildingShapeVerts; j++)
                {
                    int initialPoint = i * buildingShapeVerts;
                    int index1 = initialPoint + j;
                    int index2 = index1 + buildingShapeVerts;
                    int index3 = index1 + 1 >= initialPoint + buildingShapeVerts ? initialPoint : index1 + 1;
                    int index4 = index3 + buildingShapeVerts;

                    triangles.Add(index1);
                    triangles.Add(index3);
                    triangles.Add(index2);

                    triangles.Add(index4);
                    triangles.Add(index2);
                    triangles.Add(index3);
                }
            }

            int ceilPartVerts = buildingShapeVerts + 1;
            int initialCeilPoint = vertices.Count - ceilPartVerts;

            List<Vector3> ceilVerts = vertices.GetRange(initialCeilPoint, ceilPartVerts);

            List<int> ceilPart = GetFlatRooftopTriangles(ceilVerts, initialCeilPoint);
            triangles.AddRange(ceilPart);
            return triangles;
        }

        private static List<int> GetFlatRooftopTriangles(List<Vector3> vertices, int initialCount)
        {
            List<int> triangles = new List<int>();

            for (int i = 0; i < vertices.Count - 1; i++)
            {
                int index1 = initialCount + i;
                int index2 = (i + 1) >= vertices.Count - 1 ? initialCount : (initialCount + i + 1);
                triangles.Add(index2);
                triangles.Add(initialCount + vertices.Count - 1);
                triangles.Add(index1);
            }


            return triangles;
        }
    }
}