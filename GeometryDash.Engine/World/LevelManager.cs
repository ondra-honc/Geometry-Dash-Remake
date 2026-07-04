using System;
using System.Collections.Generic;
using System.IO;

namespace GeometryDash.Engine.World
{
  public struct LevelData
  {
    public int TypeId;
    public float X;
    public float Y;
  }

  public class LevelManager
  {
    private readonly List<LevelData> levelData = new List<LevelData>();

    public void LoadLevel(string filePath)
    {
      if (!File.Exists(filePath)) return;

      levelData.Clear();

      using (StreamReader reader = new StreamReader(filePath))
      {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
          if (string.IsNullOrWhiteSpace(line)) continue;

          string[] token = line.Split(',');
          if (!(token.Length >= 3)) continue;
          if (int.TryParse(token[0], out int type) && float.TryParse(token[1], out float x) && float.TryParse(token[2], out float y))
          {
            levelData.Add(new LevelData { TypeId = type, X = x, Y = y  });
          }
        }
      }
    }
  }
}
