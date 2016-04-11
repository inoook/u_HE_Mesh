using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class stickyCube : Processing
{
	public int numPoints = 20; //number of points in the container
	int currentSelection = 0;
	// simple arrays to store the properties of the points
	public PVector[] points;// 
	PVector[] vels;
	HE_Mesh container;
	// coordinates of the containervertices
	float[][] containerVertices;
	// simple face array for the container. Each subarray contains the indices of the vertices.
	int[][] containerFaces;
	float bufferMouseX, bufferMouseY;
	
	//half-edge mesh, one for each voronoi cell
	public HE_Mesh[] voronoiCells;
	PVector[] centers;
	//representation of a plane, used to cut the mesh
	Plane P = new Plane (new PVector (0, 0, 0), new PVector (0, 1, 0));
	
	//size of box
	public float S = 2;
	
	public override void setup ()
	{
		//frameRate(60);
		
		points = new PVector[numPoints];
		vels = new PVector[numPoints];
		
		voronoiCells = new HE_Mesh[numPoints];
		centers = new PVector[numPoints];
		
		//size(800,800,OPENGL);
		//background(Color.blue);
		//smooth();
		//hint(ENABLE_OPENGL_4X_SMOOTH);
	
		initializePoints ();
		buildContainer ();
		buildVoronoi ();
	}
	
	public override void draw ()
	{
		//background(255);
		//lights();
	  
		updatePoints ();
		buildVoronoi ();
		
		drawMesh (); // debug
	}
	
	void initializePoints ()
	{
		for (int i=0; i<numPoints; i++) {
			points [i] = new PVector (random (-S, S), random (-S, S), random (-S, S));
			vels [i] = new PVector (random (-1, 1), random (-1, 1), random (-1, 1));
			vels [i].normalize ();
			vels [i].mult (0.005f);
		}
	}
	
	
	// container = cube, 8 vertices, 6 faces of 4 vertices each
	void buildContainer ()
	{
		float[][] tmpv = new float[][]{
	    new float[]{ S,S,S },
	    new float[]{ -S,S,S },
	    new float[]{ -S,S,-S },
	    new float[]{ S,S,-S },
	    new float[]{ S,-S,S },
	    new float[]{ -S,-S,S },
	    new float[]{ -S,-S,-S },
	    new float[]{ S,-S,-S }
	  };
		containerVertices = tmpv;
		//  vertices need to be in a consistent order (clockwise, or counterclockwise around the face)
		int[][] tmpf = new int[][]{
	    new int[]{ 0,1,2,3 },
	    new int[]{ 1,0,4,5 },
	    new int[]{ 0,3,7,4 },
	    new int[]{ 2,1,5,6 },
	    new int[]{ 5,4,7,6 },
	    new int[]{ 3,2,6,7 }
	  };
		containerFaces = tmpf;  
		container = new HE_Mesh ();
		container.buildMesh (containerVertices, containerFaces);
		//container.roundEdges(100);
		//container.roundCorners(40);
	}
	
	public float offset_size = 0;
	// A hack-n-slash approach to voronoi, literally. The individual cells are created by iteratively splitting
	// the container mesh by the bisector planes of all other points. (Bisector plane = plane perpendicular to line
	// between two points, positioned halfway between the points)
	
	void buildVoronoi ()
	{
		for (int i=0; i<numPoints; i++) {
			// each Voronoi cell starts as the entire container
			voronoiCells [i] = container.get ();
		}
		for (int i=0; i<numPoints; i++) {
			for (int j=0; j<numPoints; j++) {
				if (i != j) {
					PVector N = PVector.sub (points [j], points [i]); // plane normal=normalized vector pinting from point i to point j
					N.normalize ();
					PVector O = PVector.add (points [j], points [i]); // plane origin=point halfway between point i and point j
					O.mult (0.5f);
					
					O.x += N.x * offset_size;
					O.y += N.y * offset_size;
					O.z += N.z * offset_size;
			
					P = new Plane (O, N);
					voronoiCells [i].cutMesh (P, points [i]);
				}
			}
		} 
	
		for (int i=0; i<numPoints; i++) {   
			centers [i] = new PVector ();
			for (int j=0; j<voronoiCells[i].vertices.Count; j++) {
				HE_Vertex v = (HE_Vertex)voronoiCells [i].vertices [j];   
				centers [i].add (v);
			} 
			centers [i].div (voronoiCells [i].vertices.Count);
		}
	}
	
	private Mesh mesh;
	public MeshFilter meshFilter;
	string lineshader = "Shader \"Unlit/Color\" { Properties { _Color(\"Color\", Color) = (0, 1, 1, 1)   } SubShader {  Lighting Off Color[_Color] Pass {} } }";

	public void drawMesh ()
	{
		List<Triangle3D> triangles3D = new List<Triangle3D> ();
		
		List<Vector3> vertices = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		List<Vector2> uvs = new List<Vector2> ();
		
		for (int i=0; i < voronoiCells.Length; i++) {
			Triangle3D tri = voronoiCells [i].draw ();
			triangles3D.Add (tri);
		}
		
		int indexCount = 0;
		for (int i = 0; i < triangles3D.Count; i++) {
			Triangle3D tri = triangles3D [i];
			
			for (int n = 0; n < tri.vertices.Count; n++) {
				HE_Vertex hV = tri.vertices [n];
				vertices.Add (hV.pos);
				triangles.Add (indexCount);
				uvs.Add (Vector2.zero);
				indexCount ++;
			}
		}
		
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
		
		//Material tempmaterial = new Material(lineshader);
		//renderer.material = tempmaterial;
	}
	
	void updatePoints ()
	{
		for (int i=0; i<numPoints; i++) {
			points [i].add (vels [i]);
			if (points [i].x < -S) { 
				points [i].x = -2 * S - points [i].x;
				vels [i].x *= -1;
			}
			if (points [i].y < -S) { 
				points [i].y = -2 * S - points [i].y;
				vels [i].y *= -1;
			}
			if (points [i].z < -S) { 
				points [i].z = -2 * S - points [i].z;
				vels [i].z *= -1;
			}
			if (points [i].x > S) { 
				points [i].x = 2 * S - points [i].x;
				vels [i].x *= -1;
			}
			if (points [i].y > S) { 
				points [i].y = 2 * S - points [i].y;
				vels [i].y *= -1;
			}
			if (points [i].z > S) { 
				points [i].z = 2 * S - points [i].z;
				vels [i].z *= -1;
			}
		}  
	}
	
	public override void mousePressed ()
	{
		initializePoints ();
		buildVoronoi ();
	}
	
	public override void keyPressed ()
	{
		currentSelection++;
		if (currentSelection == numPoints)
			currentSelection = 0;
	
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
		GUILayout.Label("offset size: "+offset_size.ToString("0.00"));
        offset_size = GUILayout.HorizontalSlider(offset_size, -1.0f, 0);
        
        GUI.DragWindow();
    }
}
