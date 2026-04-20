using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    public int column = 9;
    public int row = 20;
}