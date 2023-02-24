using System;

[Serializable]
public struct Location
{
  public float x;
  public float z;
}

[Serializable]
public struct Furniture
{
  public int id;
  public string model;
  public float x;
  public float z;
  public float rotation;
  public bool marked;
}

[Serializable]
public struct JustId
{
  public int id;
}