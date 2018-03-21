using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class BuildingTerminationsCreator
    {
        public static List<Vector3> CreateFlatRooftop(List<Vector3> baseRoofPoints)
        {
            if (baseRoofPoints.Count <= 4) return null;

            List<Vector3> roofPoints = new List<Vector3> {Math3D.GetCenterFromPointList(baseRoofPoints)};
            return roofPoints;
        }

        public static List<Vector3> CreateRoundedRooftop(List<Vector3> baseRoofPoints)
        {
            List<Vector3> roofPoints = new List<Vector3>();
            return roofPoints;
        }

        public static List<Vector3> CreateStairsOutRooftop(List<Vector3> baseRoofPoints)
        {
            List<Vector3> roofPoints = new List<Vector3>();
            return roofPoints;
        }

        public static List<Vector3> CreateStairsInRooftop(List<Vector3> baseRoofPoints)
        {
            List<Vector3> roofPoints = new List<Vector3>();
            return roofPoints;
        }

        public static List<Vector3> CreateToMiddleUpRooftop(List<Vector3> baseRoofPoints)
        {
            List<Vector3> roofPoints = new List<Vector3>();
            return roofPoints;
        }

        public static List<Vector3> CreateFloor(List<Vector3> baseRoofPoints)
        {
            return new List<Vector3>();
        }
    }
}