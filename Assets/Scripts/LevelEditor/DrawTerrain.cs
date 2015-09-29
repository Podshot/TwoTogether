using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using System.IO;

// Only use when running through Unity
public class DrawTerrain : MonoBehaviour {

	public GameObject normalTerrainPrefab;
	public InputField inputField;

#if UNITY_EDITOR

	private GameObject terrainParent;
	private Vector3 startMousePos;
	private Vector3 currentMousePos;
	private bool drawing = false;
	private GameObject copy;
	private int wait;
	private Rect oldBox;
	private TextWriter writer;
	
	void Start () {
        /*
		var path = EditorUtility.SaveFilePanel("Save Level...","","level.txt","txt");
		if (path.Length != 0) {
			writer = new StreamWriter(path);
		} else {
			writer = new StreamWriter("level.txt");
		}
		terrainParent = GameObject.FindGameObjectWithTag("Terrain");
        */
        writer = new StreamWriter("level.txt");
    }

	
	void Update () {

		if (Input.GetKeyDown(KeyCode.S)) {
			writer.WriteLine("\"Type\",\"Position X\",\"Position Y\",\"Position Z\",\"Scale X\",\"Scale Y\",\"Scale Z\"");
			foreach (TransformData data in terrainParent.GetComponentsInChildren<TransformData>()) {
				writer.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", 
				                               data.GetType(), 
				                               data.GetPosition().x, 
				                               data.GetPosition().y, 
				                               data.GetPosition().z, 
				                               data.GetScale().x,
				                               data.GetScale().y,
				                               data.GetScale().z
				                               )
				                 );
			}
			writer.Close();
			Debug.Log("Saved level");
		}

		if (drawing && Input.GetKeyDown(KeyCode.Mouse0)) {
			copy = null;
			drawing = false;
			wait = 5;
		}

		if (wait != 0) {
			wait -= 1;
		}

		if (Input.GetKeyDown(KeyCode.Mouse0) && !drawing && wait == 0) {
			startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			drawing = true;
			copy = Instantiate(normalTerrainPrefab);
			copy.transform.parent = terrainParent.transform;
		}

		if (drawing && copy != null) {
			currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			oldBox = new Rect(startMousePos.x, startMousePos.y, (currentMousePos.x - startMousePos.x), (currentMousePos.y - startMousePos.y));
			Vector3 pos = copy.transform.localPosition;
			pos.x = oldBox.center.x;
			pos.y = oldBox.center.y;
			copy.transform.localPosition = pos;
			//Vector3 scale = normalTerrainPrefab.transform.localScale;
			//scale.x = oldBox.width;
			//scale.y = oldBox.height;
			copy.transform.localScale = oldBox.size * 1.85f;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawCube(oldBox.center, oldBox.size);
	}
#endif
}
