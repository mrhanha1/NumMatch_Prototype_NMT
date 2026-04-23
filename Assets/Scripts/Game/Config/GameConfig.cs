using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    public int column = 9;
    public int row = 100;
    public List<Sprite> numberSprites;
    public List<Sprite> gemSprites;
}