using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<Vector3> velocities = new List<Vector3>();
    public List<Vector3> ballPositions = new List<Vector3>();
    public List<Vector3> objectPositions = new List<Vector3>();
    public List<long> velocitiesCT = new List<long>();
    public List<long> ballPositionsCT = new List<long>();
    public List<long> objectPositionsCT = new List<long>();
    public List<string> foundObjectsTags = new List<string>();
    public long lastStepNum = 0;

}