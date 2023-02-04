using System;

[Serializable]
public struct Position
{
  public float x;
  public float z;
}

[Serializable]
public struct Furniture
{
  public string id;
  public string model;
  public float x;
  public float z;
  public float rotation;
  public bool marked;
}

[Serializable]
public struct JustId
{
  public string id;
}