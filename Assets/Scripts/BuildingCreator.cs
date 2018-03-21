using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
    private readonly List<Building> _buildings = new List<Building>();
    private int _buildingTypesIndex = 3;
    private int _rows = 4;
    private int _cols = 4;
    private float _initialTime;
    private void Start()
    {
        _initialTime = Time.realtimeSinceStartup;


        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                List<Vector3> square = new List<Vector3>
                {
                    new Vector3(-Random.Range(3, 10), 0, Random.Range(3, 10)) + new Vector3(i * 20, 0, j * 20),
                    new Vector3(Random.Range(3, 10), 0, Random.Range(3, 10)) + new Vector3(i * 20, 0, j * 20),
                    new Vector3(Random.Range(3, 10), 0, -Random.Range(3, 10)) + new Vector3(i * 20, 0, j * 20),
                    new Vector3(-Random.Range(3, 10), 0, -Random.Range(3, 10)) + new Vector3(i * 20, 0, j * 20)
                };

                Building building = CreateBuilding(square);
                _buildings.Add(building);
            }
        }

        JoinAndCreateMesh();
    }

    private void JoinAndCreateMesh()
    {
        int allVerts = _buildings.Sum(item => item.Vertices.Count);
     
        
        Debug.Log(_rows * _cols + " buildings generating : " + allVerts + " vertices ");
        Debug.Log("time: " + (Time.realtimeSinceStartup - _initialTime));
    }

    public Building CreateBuilding(List<Vector3> square)
    {
        int randomIndex = 0; //Random.Range(0, _buildingTypesIndex);
        Building building = GetBuilding(randomIndex);
        building.Create(square, Random.Range(0, 10));
        return building;
    }

    private Building GetBuilding(int randomIndex)
    {
        switch (randomIndex)
        {
            case 0:
                return new RoundedBuilding();
            case 1:
                return new SquareBuilding();
            case 2:
                return new ModernBuilding();
        }

        return null;
    }
}

public class SquareBuilding : Building
{
    public override void Create(List<Vector3> square, int frequency)
    {
        Debug.Log("SquareBuilding");
    }

    public override void RefreshFloors(int frequency)
    {
        Debug.Log("RefreshFloors SquareBuilding");
    }
}

public class ModernBuilding : Building
{
    public override void Create(List<Vector3> square, int frequency)
    {
        Debug.Log("ModernBuilding");
    }

    public override void RefreshFloors(int frequency)
    {
        Debug.Log("RefreshFloors ModernBuilding");
    }
}