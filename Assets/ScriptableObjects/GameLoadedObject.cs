using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameLoadedObject", order = 1)]
public class GameLoadedObject : ScriptableObject
{
    public bool newGame;
}
