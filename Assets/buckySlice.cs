using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class buckySlice : Processing
{
	
	float[][] containerVertices;
	int[][] containerFaces;
	HE_Mesh container = new HE_Mesh ();
	float bufferMouseX, bufferMouseY;
	public List<HE_Mesh> meshes = new List<HE_Mesh> ();
	public List<PVector> centers = new List<PVector> ();
	float S = 1.75f;
	
	public override void setup ()
	{
		//size(800,800,OPENGL);
		//background(Color.white);
		//hint(ENABLE_OPENGL_4X_SMOOTH);
		buildContainer ();
		buildMesh ();
		
		drawMesh ();
	}
	
	public override void draw ()
	{
		//lights();
	}
	
	public override void mousePressed ()
	{
		PVector O = new PVector (random (-S, S), random (-S, S), random (-S, S));
		Plane P = new Plane (O, new PVector (random (-1, 1), random (-1, 1), random (-1, 1)));
		List<HE_Mesh> newMeshes = new List<HE_Mesh> ();
		List<PVector> newCenters = new List<PVector> ();
		for (int i=0; i<meshes.Count; i++) {
			HE_Mesh mesh = (HE_Mesh)meshes [i];
			HE_Mesh mesh2 = mesh.get ();
			mesh.cutMesh (P, new PVector ());   
			mesh2.cutMesh (P, new PVector (2 * O.x, 2 * O.y, 2 * O.z));      
			newMeshes.Add (mesh);
			newMeshes.Add (mesh2);
	   		
			PVector center = new PVector ();
			for (int j=0; j<mesh.vertices.Count; j++) {
				HE_Vertex v = (HE_Vertex)mesh.vertices [j];   
				center.add (v);
			} 
			center.div (mesh.vertices.Count);
			newCenters.Add (center);
			
			center = new PVector ();
			for (int j=0; j<mesh2.vertices.Count; j++) {
				HE_Vertex v = (HE_Vertex)mesh2.vertices [j];   
				center.add (v);
			} 
			center.div (mesh2.vertices.Count);
			newCenters.Add (center);
			
		}
		meshes = newMeshes;
		centers = newCenters;
		
		//
		drawMesh ();
	}
	
	public override void keyPressed ()
	{
		meshes.Clear ();
		buildMesh ();
	}
	
	void buildContainer ()
	{
		float phi = 0.5f * (sqrt (5f) + 1f) * S;
		float[][] tmpv = new float[][]{
		    new float[]{S,phi,0},
		    new float[]{S,-phi,0},
		    new float[]{-S,-phi,0},
		    new float[]{-S,phi,0},
		    new float[]{phi,0,S},
		    new float[]{-phi,0,S},
		    new float[]{-phi,0,-S},
		    new float[]{phi,0,-S},
		    new float[]{0,S,phi},
		    new float[]{0,S,-phi},
		    new float[]{0,-S,-phi},
		    new float[]{0,-S,phi}
		};
		containerVertices = tmpv;
		/*
		int[][] tmpf = new int[][]{
		    new int[]{8,4,0},
		    new int[]{8,0,3},
		    new int[]{8,3,5},
		    new int[]{8,5,11},
		    new int[]{8,11,4},
		    new int[]{4,7,0},
		    new int[]{0,9,3},
		    new int[]{3,6,5},
		    new int[]{5,2,11},
		    new int[]{11,1,4},
		    new int[]{4,1,7},
		    new int[]{0,7,9},
		    new int[]{3,9,6},
		    new int[]{5,6,2},
		    new int[]{11,2,1},
		    new int[]{10,7,1},
		    new int[]{10,9,7},
		    new int[]{10,6,9},
		    new int[]{10,2,6},
		    new int[]{10,1,2}
		};
		*/
		int[][] tmpf = new int[][]{
		    new int[]{8,0,4},
		    new int[]{8,3,0},
		    new int[]{8,5,3},
		    new int[]{8,11,5},
		    new int[]{8,4,11},
		    new int[]{4,0,7},
		    new int[]{0,3,9},
		    new int[]{3,5,6},
		    new int[]{5,11,2},
		    new int[]{11,4,1},
		    new int[]{4,7,1},
		    new int[]{0,9,7},
		    new int[]{3,6,9},
		    new int[]{5,2,6},
		    new int[]{11,1,2},
		    new int[]{10,1,7},
		    new int[]{10,7,9},
		    new int[]{10,9,6},
		    new int[]{10,6,2},
		    new int[]{10,2,1}
		};
		containerFaces = tmpf;
		container.buildMesh (containerVertices, containerFaces);
		container.roundCorners (130);
	}
	
	void buildMesh ()
	{
		HE_Mesh mesh = new HE_Mesh ();
		mesh = container.get ();
	
		meshes.Add (mesh);
		centers.Add (new PVector ());
	
	}
	
	private Mesh generateMesh;
	public MeshFilter meshFilter;
	string lineshader = "Shader \"Unlit/Color\" { Properties { _Color(\"Color\", Color) = (0, 1, 1, 1)   } SubShader {  Lighting Off Color[_Color] Pass {} } }";
	
	void drawMesh ()
	{
		List<Triangle3D> triangles3D = new List<Triangle3D> ();
		
		for (int i=0; i < meshes.Count; i++) {
			PVector center = (PVector)centers [i];
			HE_Mesh mesh = (HE_Mesh)meshes [i];
			/*
			//mesh.drawEdges();
			foreach (HE_Edge e in mesh.edges) {
				Vector3 p0 = e.halfEdge.vert.pos;
				Vector3 p1 = e.halfEdge.pair.vert.pos;
				Debug.DrawLine (p0 + center.pos * 0.2f, p1 + center.pos * 0.2f, Color.red);
			}
			*/
			Triangle3D tri = mesh.draw ();
			triangles3D.Add (tri);
		}
		
		
		List<Vector3> vertices = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		List<Vector2> uvs = new List<Vector2> ();
		
		int indexCount = 0;
		for (int i = 0; i < triangles3D.Count; i++) {
			Triangle3D tri = triangles3D [i];
			PVector center = (PVector)centers [i];
			
			for (int n = 0; n < tri.vertices.Count; n++) {
				HE_Vertex hV = tri.vertices [n];
				vertices.Add (hV.pos + center.pos*0.2f);
				triangles.Add (indexCount);
				uvs.Add (Vector2.zero);
				indexCount ++;
			}
		}
		
		if (generateMesh == null) {
			generateMesh = new Mesh ();
			generateMesh.name = "generateMesh";
		}
		generateMesh.Clear (false);
		
		generateMesh.vertices = vertices.ToArray ();
		generateMesh.triangles = triangles.ToArray ();
		generateMesh.uv = uvs.ToArray ();
		//generateMesh.SetIndices(triangles.ToArray(), MeshTopology.Triangles, 0);
		generateMesh.RecalculateNormals ();
		
		meshFilter.mesh = generateMesh;
		
		//Material tempmaterial = new Material(lineshader);
		//renderer.material = tempmaterial;
	}
	
	
	public GUISkin skin;
	void OnGUI()
	{
		GUI.skin = skin;
		
		GUI.Label(new Rect(10,20,200,50), "Click: slice");
	}
}
