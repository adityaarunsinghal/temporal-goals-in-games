using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<Vector3> velocities = new List<Vector3>();
    public List<Vector3> ballPositions = new List<Vector3>();
    public List<Vector3> objectPositions = new List<Vector3>();
    public List<double> velocitiesCT = new List<double>();
    public List<double> ballPositionsCT = new List<double>();
    public List<double> objectPositionsCT = new List<double>();
    public List<string> foundObjectsTags = new List<string>();

}