using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class Building
    {
        public readonly List<int> Triangles;
        public readonly List<Vector2> Uvs;
        public readonly List<Vector3> Vertices;
        protected Vector3 PlaneDirection;

        protected BuildingPart BuildingBase;
        protected StreetPlanes StreetPlanes;
        protected BuildingRoof RoofTermination;
        
        private readonly int _enumLenght = (int)BuildingTerminations.COUNT; //Enum.GetNames(typeof(BuildingTermination)).Length;
        private const float StreetWidthFactor = .1f;
        private const float ModuleHeight = 4f;
        
        //ReSharper disable once InconsistentNaming
        protected enum BuildingTerminations
        {
            Flat,
            Rounded,
            StairsOut,
            StairsIn,
            ToMiddle,
            Floor,
            COUNT
        }
        
        protected BuildingTerminations BuildingTermination;

        protected Building()
        {
            Uvs = new List<Vector2>();
            Triangles = new List<int>();
            Vertices = new List<Vector3>();
            StreetPlanes = new StreetPlanes();
            RoofTermination = new BuildingRoof();
        }

        public abstract void Create(List<Vector3> square, int frequency);
        public abstract void RefreshFloors(int frequency);

        protected List<Vector3> GetStreetPoints(out List<Vector3> streetPlanes, List<Vector3> square)
        {
            Vector3 segmentAb = square[1] - square[0];
            Vector3 segmentBc = square[2] - square[1];
            Vector3 segmentCd = square[3] - square[2];
            Vector3 segmentDa = square[0] - square[3];

            Vector3 subpoint1Ab = square[0] + segmentAb * StreetWidthFactor;
            Vector3 subpoint2Ab = square[1] - segmentAb * StreetWidthFactor;

            Vector3 subpoint1Bc = square[1] + segmentBc * StreetWidthFactor;
            Vector3 subpoint2Bc = square[2] - segmentBc * StreetWidthFactor;

            Vector3 subpoint1Cd = square[2] + segmentCd * StreetWidthFactor;
            Vector3 subpoint2Cd = square[3] - segmentCd * StreetWidthFactor;

            Vector3 subpoint1Da = square[3] + segmentDa * StreetWidthFactor;
            Vector3 subpoint2Da = square[0] - segmentDa * StreetWidthFactor;

            List<Vector3> internalSquare = new List<Vector3>
            {
                SegmentIntersection(subpoint1Ab, subpoint2Cd, subpoint1Bc, subpoint2Da),
                SegmentIntersection(subpoint1Cd, subpoint2Ab, subpoint1Bc, subpoint2Da),
                SegmentIntersection(subpoint2Ab, subpoint1Cd, subpoint2Bc, subpoint1Da),
                SegmentIntersection(subpoint1Ab, subpoint2Cd, subpoint1Da, subpoint2Bc)
            };

            streetPlanes = new List<Vector3>
            {
                //Long planeA
                subpoint2Ab,
                internalSquare[1],
                internalSquare[0],
                subpoint1Ab,
                //corner PlaneA
                square[0],
                subpoint2Da,

                //Long planeB
                subpoint2Bc,
                internalSquare[2],
                internalSquare[1],
                subpoint1Bc,
                //corner PlaneB
                square[1],
                subpoint2Ab,

                //Long planeC
                subpoint2Cd,
                internalSquare[3],
                internalSquare[2],
                subpoint1Cd,
                //corner PlaneC
                square[2],
                subpoint2Bc,
                //Long planeD
                subpoint2Da,
                internalSquare[0],
                internalSquare[3],
                subpoint1Da,
                //corner PlaneD
                square[3],
                subpoint2Cd
            };

            return internalSquare;
        }

        private Vector3 SegmentIntersection(Vector3 subpoint1Ab, Vector3 subpoint2Cd, Vector3 subpoint1Bc,
            Vector3 subpoint2Da)
        {
            Vector3 intesection;
            Math3D.LineLineIntersection(out intesection, subpoint1Ab, subpoint1Ab - subpoint2Cd, subpoint1Bc,
                subpoint1Bc - subpoint2Da);
            return intesection;
        }

        protected List<Vector3> ExtendFloorBase(List<Vector3> buildingBase, int floorNumber, float scaledFactor,
            Vector3 eulerRotation,
            Vector3 up)
        {
            List<Vector3> result = new List<Vector3>();

            Vector3 centerPivot =
                Math3D.GetCenterFromPointList(buildingBase) + up.normalized * ModuleHeight * floorNumber;

            for (int i = 0; i < buildingBase.Count; i++)
            {
                Vector3 vector = buildingBase[i] + up.normalized * ModuleHeight * floorNumber;
                Vector3 dir = vector - centerPivot;
                vector = centerPivot + dir.normalized * (scaledFactor * dir.magnitude);
                vector = RotatePointAroundPivot(eulerRotation, centerPivot, vector);
                result.Add(vector);
            }

            return result;
        }

        protected List<Vector3> ConstructTermination(BuildingTerminations buildingTerminations, List<Vector3> roofBase)
        {
            switch (buildingTerminations)
            {
                case BuildingTerminations.Flat:
                    return BuildingTerminationsCreator.CreateFlatRooftop(roofBase);
                case BuildingTerminations.Rounded:
                    return BuildingTerminationsCreator.CreateRoundedRooftop(roofBase);
                case BuildingTerminations.StairsOut:
                    return BuildingTerminationsCreator.CreateStairsOutRooftop(roofBase);
                case BuildingTerminations.StairsIn:
                    return BuildingTerminationsCreator.CreateStairsInRooftop(roofBase);
                case BuildingTerminations.ToMiddle:
                    return BuildingTerminationsCreator.CreateToMiddleUpRooftop(roofBase);
                case BuildingTerminations.Floor:
                    return BuildingTerminationsCreator.CreateFloor(roofBase);
                default:
                    throw new ArgumentOutOfRangeException("buildingTerminations", buildingTerminations, null);
            }
        }

        private Vector3 RotatePointAroundPivot(Vector3 eulerRotation, Vector3 pivot, Vector3 vector)
        {
            Vector3 dir = vector - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(eulerRotation) * dir; // rotate it
            return dir + pivot;
        }

        protected BuildingTerminations GetTermination()
        {
            return BuildingTerminations.Flat;
            // return (BuildingTermination) Random.Range(0, _enumLenght);
        }
    }
}