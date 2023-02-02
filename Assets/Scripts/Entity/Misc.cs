using System;

[Serializable]
public struct Position
{
  public string x;
  public string z;
}

[Serializable]
public struct Furniture
{
  public string id;
  public string model;
  public string material;
}

[Serializable]
public struct JustId
{
  public string id;
}