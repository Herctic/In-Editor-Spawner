using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SuperSpawner : MonoBehaviour
{
   /* 
	settings: spawn count, spawn area, randomness/scatter

	collection of spawned items: keeping an array and using a for 
	loop to populate the instantiated objects

	Use an if statement after already randomizing position () to
	check for a collision (to use that surface) and use one 
	transform axis (the horizontal one) to place the object
	
	Instantiate the items and store instances in an array

	scatter/random.seed

	jitter = adding hard values to the items

	Hints: 
	[ExecuteInEditmode] - force a method to excute in edit mode
	OnValidate(); - Executes when a change is made to a public field on a script
	OnDrawGizmos(); - Executes when an object is selected

	[MenuItem("MyMenu/Do Something/New Menu/ Even New Menu")]
	static void DoSomething()
	{
		Debug.Log("Doing Something...");
	}
	*/
	#region Public

	[Header("Settings")]

	[Tooltip("Insert prefab of the object you wish to spawn here")]
	public GameObject prefab;

	[Range(0, 10)]
	[Tooltip("Number of objects to spawn")]
	public int count;

	[Range(1f, 10f)]
	[Tooltip("Spawn distance from this object in scene units")]
	public float spawnRadius;
	
	[Range(0, 100)]
	[Tooltip("Change the seed")]
	public int seed;

	[Tooltip("Randomizes rotation of spawned objects")]
	public bool randomizeRotation;

	[Range(0f, 10f)]
	[Tooltip("Scatters the objects away from the origin on the spawner object")]
	// public float scatter;

	[SerializeField]
	List<GameObject> instances = new List<GameObject>();
	int groundLayer = 1 << 9;
	int objectLayer = 1 << 10;	

	#endregion

	#region Private



	#endregion

	// void Start()
	// {
	// 	Vector3 superPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	// }
	void Spawn()
	{	
		int i = 0;

		if (randomizeRotation)	
		{
			while (i < count)
			{
				Vector3 pos = new Vector3 (Random.Range(transform.position.x + -spawnRadius, transform.position.x + spawnRadius), 
					transform.position.y + spawnRadius, Random.Range(transform.position.z + -spawnRadius, transform.position.z + spawnRadius));
				RaycastHit hit;
				RaycastHit boxHit;
				bool groundHit = (Physics.BoxCast(pos, prefab.GetComponent<MeshRenderer>().bounds.extents*2, Vector3.down, out hit, Quaternion.identity, 
					50, groundLayer, QueryTriggerInteraction.UseGlobal));
				bool objectHit = (Physics.BoxCast(pos, prefab.GetComponent<MeshRenderer>().bounds.extents*2, Vector3.down, out boxHit, Quaternion.identity, 
					50, objectLayer, QueryTriggerInteraction.UseGlobal));

				if (groundHit && !objectHit)
				{
					instances.Add(Instantiate(prefab, hit.point, Quaternion.Euler(0, Random.Range(0, 360), 0), transform));
					instances[i].transform.SetParent(transform);
					i++;
				}
			}
		}

		else
		{
			while (i < count)
			{
				Vector3 pos = new Vector3 (Random.Range(transform.position.x + -spawnRadius, transform.position.x + spawnRadius), 
					transform.position.y + spawnRadius, Random.Range(transform.position.z + -spawnRadius, transform.position.z + spawnRadius));
				RaycastHit hit;
				RaycastHit boxHit;
				bool groundHit = (Physics.BoxCast(pos, prefab.GetComponent<MeshRenderer>().bounds.extents*2, Vector3.down, out hit, Quaternion.identity, 
					50, groundLayer, QueryTriggerInteraction.UseGlobal));
				bool objectHit = (Physics.BoxCast(pos, prefab.GetComponent<MeshRenderer>().bounds.extents*2, Vector3.down, out boxHit, Quaternion.identity, 
					50, objectLayer, QueryTriggerInteraction.UseGlobal));

				if (groundHit && !objectHit)
				{
					instances.Add(Instantiate(prefab, hit.point, transform.rotation, transform));
					instances[i].transform.SetParent(transform);
					i++;
				}
			}
		}
	}

#if UNITY_EDITOR

	// this runs when settings in the inspector are changed
	void OnValidate()
	{
		// Trick for making Destroy work
		EditorApplication.delayCall += () =>
		{
			foreach (GameObject g in instances) 
			{ 
				DestroyImmediate(g); 
			}
			instances = new List<GameObject>();

			Spawn();
		};
		
		Random.InitState(seed);
	}

	// this runs when the object is selected?
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, spawnRadius);

			foreach (GameObject g in instances)
			{
				Vector3 newPos = new Vector3(g.transform.position.x, g.transform.position.y + spawnRadius, g.transform.position.z);

				RaycastHit newHit;
				if(Physics.Raycast(newPos, Vector3.down, out newHit, spawnRadius * 2, groundLayer))
				{
					g.transform.position = newHit.point;
					// Gizmos.DrawRay(newPos, Vector3.down);
				}
			}

		// Gizmos.color = new Color(1, 1, 0, 0.5f);
		// Gizmos.DrawSphere(transform.position, spawnRadius / 2);

		
				
		// if (transform.hasChanged)
		// {
		// 	EditorApplication.delayCall += () =>
		// 	{
		// 		foreach (GameObject x in instances) { DestroyImmediate(x); }
		// 		instances = new List<GameObject>();
		// 	};
		// 	for (int g = 0; g < instances.Count; g++)
		// 	{
		// 		Debug.Log(transform.hasChanged);
		// 		Vector3 newPos = instances[g].transform.position;
		// 		instances.RemoveAt(g);
				
				
		// 		RaycastHit gHit;
		// 		if (Physics.Raycast(newPos, transform.TransformDirection(Vector3.down), out gHit, 100))
		// 		{
		// 			instances.Insert(g, Instantiate(prefab, gHit.point, transform.rotation, transform));

					
		// 		}
		// 	}
			
		// 	transform.hasChanged = false;
		// }

	}
#endif
}
