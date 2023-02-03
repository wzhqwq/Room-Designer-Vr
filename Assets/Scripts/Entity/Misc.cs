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
  public string x;
  public string z;
  public string angle;
}

[Serializable]
public struct JustId
{
  public string id;
}