using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class BuildingTerminationsCreator
    {
        public static List<Vector3> CreateRoofTop(Building.BuildingTerminations buildingTerminations,
            List<Vector3> roofBase)
        {
            switch (buildingTerminations)
            {
                case Building.BuildingTerminations.Flat:
                    return CreateFlatRooftop(roofBase);
                case Building.BuildingTerminations.Rounded:
                    return CreateRoundedRooftop(roofBase);
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

        private static List<Vector3> CreateRoundedRooftop(List<Vector3> baseRoofPoints)
        {
            List<Vector3> roofPoints = new List<Vector3>();
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
            return new List<Vector3>();
        }

        public static List<int> GetRoofTriangles(List<Vector3> vertices, Building.BuildingTerminations termination)
        {
            switch (termination)
            {
                case Building.BuildingTerminations.Flat:
                    return GetFlatRooftopTriangles(vertices);
                case Building.BuildingTerminations.Rounded:
                    break;
                case Building.BuildingTerminations.StairsOut:
                    break;
                case Building.BuildingTerminations.StairsIn:
                    break;
                case Building.BuildingTerminations.ToMiddle:
                    break;
                case Building.BuildingTerminations.Floor:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("termination", termination, null);
            }

            return new List<int>();
        }

        private static List<int> GetFlatRooftopTriangles(List<Vector3> vertices)
        {
            List<int> triangles = new List<int>();

            for (int i = 0; i < vertices.Count - 1; i++)
            {
                int index1 = i;
                int index2 = (i + 1) >= vertices.Count - 1 ? 0 : (i + 1);
                triangles.Add(index2);
                triangles.Add(vertices.Count - 1);
                triangles.Add(index1);
            }

            return triangles;
        }
    }
}