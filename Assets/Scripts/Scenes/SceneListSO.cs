using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SceneList", menuName = "ScriptableObjects/SceneList", order = 1)]
public class SceneListSO : ScriptableObject
{
    public List<string> sceneNames = new List<string>();
}