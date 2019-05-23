using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuItems : MonoBehaviour
{
   public GameObject spwnr;
    
   [MenuItem("GameObject/Spawner Object", false, 10)]
	static void CreateObject(MenuCommand menuCommand)
	{
		GameObject spwnr = new GameObject("Spawner Object");
		spwnr.AddComponent<SuperSpawner>();
	}
}
