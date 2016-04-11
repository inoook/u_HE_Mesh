using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeMeshTestPlane : MonoBehaviour
{
	HE_Mesh container;
	
	//size of box
	public float S = 2;
	
	void Start ()
	{
		buildContainer ();
		drawMesh (); // debug
	}
	
	public float roundEdges = 0.0f;
	public float roundCorners = 0.0f;
	
	void Update()
	{
		
	}
	
	void updateContainerMesh()
	{
		container.buildMesh (containerVertices, containerFaces);
		container.roundEdges(roundEdges);
		container.roundCorners(roundCorners);
		drawMesh();
	}
	
	float[][] containerVertices;
	int[][] containerFaces;
	// container = cube, 8 vertices, 6 faces of 4 vertices each
	void buildContainer ()
	{
		containerVertices = new float[][]{
	    new float[]{ S,0,S },
	    new float[]{ -S,0,S },
	    new float[]{ -S,0,-S },
	    new float[]{ S,0,-S }
	  };
		//  vertices need to be in a consistent order (clockwise, or counterclockwise around the face)
		containerFaces = new int[][]{
	    new int[]{ 0,1,2,3 }
	  }; 
		container = new HE_Mesh ();
		container.buildMesh (containerVertices, containerFaces);
	}
	
	private Mesh mesh;
	public MeshFilter meshFilter;
	
	public void drawMesh ()
	{
		Triangle3D tri = container.draw();
		
		List<Vector3> vertices = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		List<Vector2> uvs = new List<Vector2> ();
		
		for(int i = 0; i < tri.vertices.Count; i++){
			vertices.Add(tri.vertices[i].pos);
			triangles.Add(i);
			uvs.Add(Vector2.zero);
		}
		
		// mesh
		if (mesh == null) {
			mesh = new Mesh ();
			mesh.name = "generateMesh";
		}
		mesh.Clear ();
		
		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.uv = uvs.ToArray ();
		//mesh.SetIndices(triangles.ToArray(), MeshTopology.Triangles, 0);
		mesh.RecalculateNormals ();
		
		meshFilter.mesh = mesh;
	}
	
	
	//
	public GUISkin skin;
	public Rect windowRect = new Rect(20, 20, 140, 50);
	void OnGUI()
	{
		GUI.skin = skin;
		windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "param");
		
	}
	void DoMyWindow(int windowID) {
		
		GUILayout.Label("roundEdges: "+roundEdges.ToString("0.00"));
        roundEdges = GUILayout.HorizontalSlider(roundEdges, 0f, 3);
		
		GUILayout.Label("roundCorners: "+roundCorners.ToString("0.00"));
        roundCorners = GUILayout.HorizontalSlider(roundCorners, 0f, 3);
        
		
		if(GUI.changed){
			//Debug.Log("changed");
			updateContainerMesh();
		}
		
		
        GUI.DragWindow();
		
    }
}
