using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class RoundedBuilding : Building
    {
        private readonly float _angleCorrectionLimit = 135;
        private readonly float _angleCorrectionFactor = 0.2f;
        private List<Vector3> _buildingShape;

        public RoundedBuilding()
        {
            BuildingBase = new RoundedBuildingBase();
        }

        public override void Create(List<Vector3> square, int frequency)
        {
            DebugDrawPointList(square, Color.green);
            PlaneDirection = Vector3.Cross(square[0] - square[1], square[0] - square[3]);
            List<Vector3> streetPlanes = new List<Vector3>();
            List<Vector3> internalSquare = GetStreetPoints(out streetPlanes, square);
            StreetPlanes.SetVertex(streetPlanes);
            StreetPlanes.SetTriangles();

            DebugDrawPointList(internalSquare, Color.blue);
            _buildingShape = GetBuildingBase(internalSquare);
            List<Vector3> allBasePoints = new List<Vector3>(internalSquare);
            allBasePoints.AddRange(_buildingShape);

            BuildingBase.SetVertex(allBasePoints);
            BuildingBase.SetTriangles();

            DebugDrawPointList(_buildingShape, Color.red);
            BuildingTermination = GetTermination();
            BuildingParts.Add(StreetPlanes);
            BuildingParts.Add(BuildingBase);
            RefreshFloors(frequency);
        }

        public override void RefreshFloors(int frequency)
        {
            List<Vector3> allStructureVerts = new List<Vector3>(_buildingShape);
            List<Vector3> newFloorVerts = null;
            
            for (int i = 1; i <= frequency; i++)
            {
                newFloorVerts = ExtendFloorBase(_buildingShape, floorNumber: i,
                    scaledFactor: Random.Range(.7f, 1.2f),
                    eulerRotation: new Vector3(Random.Range(-3, 3), Random.Range(-5, 5), Random.Range(-3, 3)),
                    up: PlaneDirection);
                allStructureVerts.AddRange(newFloorVerts);
            }

            List<Vector3> roofTopPoints = new List<Vector3>();

            BuildingTerminations buildingTermination;
            
            if (frequency != 0)
            {
                BuildingStructure.SetVertex(allStructureVerts);
                BuildingStructure.FloorVerts = _buildingShape.Count;
                BuildingStructure.SetTriangles();
                
                buildingTermination = BuildingTermination;
                roofTopPoints = newFloorVerts;
            }
            else
            {
                buildingTermination = BuildingTerminations.Floor;
                roofTopPoints.AddRange(_buildingShape);
            }
            
            roofTopPoints = ConstructTermination(buildingTermination, roofTopPoints);

            for (int i =0; i<roofTopPoints.Count;i++)
            {
                Math3D.CreateSphereInPos(roofTopPoints[i], Color.yellow, "roofftop_"+i);
            }
            RoofTermination.SetVertex(roofTopPoints);
            RoofTermination.RoofType = buildingTermination;
            RoofTermination.SetTriangles();
        }

        private List<Vector3> GetBuildingBase(List<Vector3> internalSquare)
        {
            Vector3 puntA = internalSquare[0];
            Vector3 puntB = internalSquare[1];
            Vector3 puntC = internalSquare[2];
            Vector3 puntD = internalSquare[3];
            Vector3 segmentAb = puntB - puntA;
            Vector3 halfSegmentAb = GetMiddlePoint(puntA, puntB);
            Vector3 segmentBc = puntC - puntB;
            Vector3 halfSegmentBc = GetMiddlePoint(puntC, puntB);
            Vector3 segmentCd = puntD - puntC;
            Vector3 halfSegmentCd = GetMiddlePoint(puntD, puntC);
            Vector3 segmentDa = puntA - puntD;
            Vector3 halfSegmentDa = GetMiddlePoint(puntD, puntA);
            Vector3 intesection1 = GetIntesectionsBetweenHalfsWithBisector(halfSegmentDa, halfSegmentAb, puntA, puntC);
            Vector3 intesection2 = GetIntesectionsBetweenHalfsWithBisector(halfSegmentCd, halfSegmentBc, puntA, puntC);
            Vector3 intesection3 = GetIntesectionsBetweenHalfsWithBisector(halfSegmentAb, halfSegmentBc, puntB, puntD);
            Vector3 intesection4 = GetIntesectionsBetweenHalfsWithBisector(halfSegmentDa, halfSegmentCd, puntB, puntD);
            Vector3 roundedA = GetMiddlePoint(intesection1, puntA);
            Vector3 roundedB = GetMiddlePoint(intesection3, puntB);
            Vector3 roundedC = GetMiddlePoint(intesection2, puntC);
            Vector3 roundedD = GetMiddlePoint(intesection4, puntD);
            Vector3 helperPointAb1 = GetHelperPoint(roundedA, halfSegmentAb, puntA, segmentAb);
            Vector3 helperPointAb2 = GetHelperPoint(roundedB, halfSegmentAb, puntA, segmentAb);
            Vector3 helperPointBc1 = GetHelperPoint(roundedB, halfSegmentBc, puntB, segmentBc);
            Vector3 helperPointBc2 = GetHelperPoint(roundedC, halfSegmentBc, puntB, segmentBc);
            Vector3 helperPointCd1 = GetHelperPoint(roundedC, halfSegmentCd, puntC, segmentCd);
            Vector3 helperPointCd2 = GetHelperPoint(roundedD, halfSegmentCd, puntC, segmentCd);
            Vector3 helperPointDa1 = GetHelperPoint(roundedD, halfSegmentDa, puntD, segmentDa);
            Vector3 helperPointDa2 = GetHelperPoint(roundedA, halfSegmentDa, puntD, segmentDa);
            roundedA = AngleCorrectionRoundedPoint(helperPointAb1, roundedA, helperPointDa2, puntA);
            roundedB = AngleCorrectionRoundedPoint(helperPointAb2, roundedB, helperPointBc1, puntB);
            roundedC = AngleCorrectionRoundedPoint(helperPointBc2, roundedC, helperPointCd1, puntC);
            roundedD = AngleCorrectionRoundedPoint(helperPointCd2, roundedD, helperPointDa1, puntD);

            //BISECTORS
            Color yellowAlpha = Color.yellow;
            yellowAlpha.a = .2f;
            Debug.DrawLine(puntA, puntC, yellowAlpha, 5f);
            Debug.DrawLine(puntB, puntD, yellowAlpha, 5f);

            List<Vector3> roundedPoints = new List<Vector3>
            {
                //CornerA
                halfSegmentDa,
                helperPointDa2,
                roundedA,
                helperPointAb1,
                //CornerB
                halfSegmentAb,
                helperPointAb2,
                roundedB,
                helperPointBc1,
                //CornerC
                halfSegmentBc,
                helperPointBc2,
                roundedC,
                helperPointCd1,
                //CornerD
                halfSegmentCd,
                helperPointCd2,
                roundedD,
                helperPointDa1
            };
            return roundedPoints;
        }

        private static Vector3 GetMiddlePoint(Vector3 puntA, Vector3 puntB)
        {
            return Vector3.Lerp(puntA, puntB, 0.5f);
        }

        private Vector3 AngleCorrectionRoundedPoint(Vector3 helperPoint1, Vector3 rounded, Vector3 helperPoint2,
            Vector3 punt)
        {
            float angleHelpersSegmentsA = Vector3.Angle(helperPoint1 - rounded, helperPoint2 - rounded);
            if (angleHelpersSegmentsA < _angleCorrectionLimit)
            {
                Vector3 dir = punt - rounded;
                rounded = rounded - dir * _angleCorrectionFactor;
            }

            return rounded;
        }

        private Vector3 GetIntesectionsBetweenHalfsWithBisector(Vector3 halfSegment1, Vector3 halfSegment2,
            Vector3 punt1, Vector3 punt2)
        {
            Vector3 intesection;
            Math3D.LineLineIntersection(out intesection, halfSegment1, halfSegment1 - halfSegment2, punt1,
                punt1 - punt2);
            return intesection;
        }

        private Vector3 GetHelperPoint(Vector3 roundedPoint, Vector3 halfSegment, Vector3 puntVert, Vector3 segment)
        {
            Vector3 middleHalfSegment =
                GetMiddlePoint(roundedPoint, halfSegment);
            Vector3 projectedMiddleHalfToSegment =
                Math3D.ProjectPointOnLine(puntVert, segment.normalized, middleHalfSegment);

            Vector3 helperPoint =
                GetMiddlePoint(middleHalfSegment,
                    projectedMiddleHalfToSegment);
            return helperPoint;
        }

        private void DebugDrawPointList(List<Vector3> vectorslist, Color c)
        {
            for (int i = 0; i <= vectorslist.Count; i++)
            {
                int index1 = i % vectorslist.Count;
                int index2 = (i + 1) % vectorslist.Count;
                Debug.DrawLine(vectorslist[index1], vectorslist[index2], c, 5f);
            }
        }
    }
}