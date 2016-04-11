using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// http://www.wblut.com/2009/05/04/snippets-ii/
// http://hemesh.wblut.com/
//Rudimentary plane class
public class Plane
{
	public float A, B, C, D;

	public Plane (PVector p0, PVector n)
	{
		PVector norm = n.get ();
		norm.normalize ();
		A = norm.x;
		B = norm.y;
		C = norm.z;
		D = -A * p0.x - B * p0.y - C * p0.z;
	}

	public Plane (PVector p0, PVector p1, PVector p2)
	{
		A = p0.y * (p1.z - p2.z) + p1.y * (p2.z - p0.z) + p2.y * (p0.z - p1.z);
		B = p0.z * (p1.x - p2.x) + p1.z * (p2.x - p0.x) + p2.z * (p0.x - p1.x);
		C = p0.x * (p1.y - p2.y) + p1.x * (p2.y - p0.y) + p2.x * (p0.y - p1.y);
		PVector norm = new PVector (A, B, C);
		norm.normalize ();
		A = norm.x;
		B = norm.y;
		C = norm.z;
		D = -A * p0.x - B * p0.y - C * p0.z;
	}

	void flipNormal ()
	{
		A *= -1;
		B *= -1;
		C *= -1;
		D *= -1;
	}

	//returns 1 if p is on same side as normal, -1 if on opposite side, 0 if on the plane
	public float side (PVector p)
	{
		float tmp = A * p.x + B * p.y + C * p.z + D;
		if (tmp < -HE_Mesh.DELTA) {
			tmp = -1f; 
		} else if (tmp > HE_Mesh.DELTA) {
			tmp = 1f; 
		} else {
			tmp = 0f; 
		}
		return tmp;
	}
}

//Half-Edge Mesh
//Source: http://www.flipcode.com/archives/The_Half-Edge_Data_Structure.shtml

public class HE_Mesh
{
	public static float DELTA = 0.001f;
	public List<HE_Vertex> vertices;
	public List<HE_Face> faces;
	public List<HE_HalfEdge> halfEdges;
	public List<HE_Edge> edges;

	public HE_Mesh ()
	{
		vertices = new List<HE_Vertex> ();
		faces = new List<HE_Face> ();
		halfEdges = new List<HE_HalfEdge> ();
		edges = new List<HE_Edge> ();
	}
	
	// draw as a triangular mesh.
	public Triangle3D draw ()
	{
		Triangle3D triangle3d = new Triangle3D ();
		
		for (int i = 0; i < faces.Count; i++) {
			HE_Face face = faces [i];
			HE_HalfEdge halfEdge = face.halfEdge;
			HE_Vertex midFace = new HE_Vertex (0, 0, 0, 0);
			int c = 0;
			do {
				HE_Vertex v = halfEdge.vert;
				midFace.x += v.x;
				midFace.y += v.y;
				midFace.z += v.z;
				halfEdge = halfEdge.next;
				c++;
			} while(halfEdge!=face.halfEdge);
			midFace.x /= c;
			midFace.y /= c;
			midFace.z /= c;
			halfEdge = face.halfEdge;
	
			HE_Vertex vv;
			do {
				HE_Vertex v0 = halfEdge.vert;
				HE_Vertex v1 = halfEdge.next.vert;
				
				triangle3d.vertices.Add (midFace);
				triangle3d.vertices.Add (v1);
				triangle3d.vertices.Add (v0);
				
				halfEdge = halfEdge.next;
			} while(halfEdge!=face.halfEdge);
			
			//beginShape(TRIANGLE_STRIP);
			
			/*
			GL.Begin(GL.TRIANGLE_STRIP);
			HE_Vertex vv;
			do{              
				vv=halfEdge.vert; 
				GL.Vertex3(vv.x,vv.y,vv.z);
				GL.Vertex3(midFace.x,midFace.y,midFace.z);
				halfEdge= halfEdge.next;
			}
			while(halfEdge!=face.halfEdge);
			vv=halfEdge.vert; 
			GL.Vertex3(vv.x,vv.y,vv.z);
			//endShape();
			GL.End();
			*/
			
			/*
			HE_Vertex vv;
			do{              
				vv=halfEdge.vert; 
				//GL.Vertex3(vv.x,vv.y,vv.z);
				//GL.Vertex3(midFace.x,midFace.y,midFace.z);
				triangle3d.vertices.Add(vv);
				triangle3d.vertices.Add(midFace);
				
				halfEdge= halfEdge.next;
			}
			while(halfEdge!=face.halfEdge);
			vv=halfEdge.vert;
			
			//GL.Vertex3(vv.x,vv.y,vv.z);
			triangle3d.vertices.Add(vv);
			*/
		}
		
		return triangle3d;
	}

	void drawVertices ()
	{
		//Iterator vertexItr = vertices.iterator();
		//while(vertexItr.hasNext()){
		//HE_Vertex v=vertexItr.next();
			
		/*
		foreach(HE_Vertex v in vertices){
		  pushMatrix();
		  translate(v.x,v.y,v.z);  
		  box(5);
		  popMatrix();
		}
		noFill();
		*/
	}

	public void drawEdges ()
	{
		//Iterator eItr = edges.iterator();
		//while(eItr.hasNext()){
		//HE_Edge e=eItr.next();
		/*
		foreach(HE_Edge e in edges){
		  line(e.halfEdge.vert.x,e.halfEdge.vert.y,e.halfEdge.vert.z,
			   e.halfEdge.pair.vert.x,e.halfEdge.pair.vert.y,e.halfEdge.pair.vert.z);
		}
		*/
		
		/*
		foreach(HE_Edge e in edges){
			Vector3 p0 = e.halfEdge.vert.pos;
			Vector3 p1 = e.halfEdge.pair.vert.pos;
			Debug.DrawLine(p0, p1, Color.red);
		}
		*/
	}

	public void drawCurves ()
	{
		//Iterator faceItr = faces.iterator();
		//while(faceItr.hasNext()){
		//HE_Face face=faceItr.next();
		/*  
		foreach(HE_Face face in faces){
		  HE_HalfEdge halfEdge=face.halfEdge;
		  HE_Vertex u=halfEdge.vert;
		  HE_Vertex v=halfEdge.next.vert;
			
		beginShape();
		  vertex(0.5*(u.x+v.x),0.5*(u.y+v.y),0.5*(u.z+v.z));     
		  do{
		    u=halfEdge.next.vert;
		    v=halfEdge.next.next.vert;      
		    bezierVertex(u.x,u.y,u.z,u.x,u.y,u.z,0.5*(u.x+v.x),0.5*(u.y+v.y),0.5*(u.z+v.z));
		    halfEdge=halfEdge.next;
		  }
		  while(halfEdge!=face.halfEdge);
		  endShape(CLOSE);
		  
		}
		*/
	}
	
	public void buildMesh (float[][] simpleVertices, int[][] simpleFaces)
	{
		/*
		vertices = new List<HE_Vertex>();
		faces=new List<HE_Face>();
		halfEdges=new List<HE_HalfEdge>();
		edges=new List<HE_Edge>();
		*/
		vertices.Clear ();
		faces.Clear ();
		halfEdges.Clear ();
		edges.Clear ();

		//Add all input vertices to the mesh, the original index of the vertex is stored as an id.
		for (int i=0; i < simpleVertices.Length; i++) {  
			vertices.Add (new HE_Vertex (simpleVertices [i] [0], simpleVertices [i] [1], simpleVertices [i] [2], i)); 
		}

		//Create a loop of halfedges for each face. We assume the vertices are given in a consistent order.
		// To extend this for a non-ordered list, sort the vertices of the input vertices here.
		HE_HalfEdge he;
		for (int i=0; i<simpleFaces.Length; i++) {
			List<HE_HalfEdge> faceEdges = new List<HE_HalfEdge> ();
			HE_Face hef = new HE_Face ();
			faces.Add (hef);
			//TODO: sort face vertices here
			for (int j=0; j<simpleFaces[i].Length; j++) {
				he = new HE_HalfEdge ();
				faceEdges.Add (he);
				he.face = hef;
				if (hef.halfEdge == null)
					hef.halfEdge = he;
				he.vert = vertices [simpleFaces [i] [j]];
				if (he.vert.halfEdge == null)
					he.vert.halfEdge = he;  
			}
			cycleHalfEdges (faceEdges, false);    
			halfEdges.AddRange (faceEdges);
		}
		//associate each halfedge with its pair (belonging to adjacent face)
		pairHalfEdges (halfEdges);
		//associate each pair of halfedges to a physical edge
		createEdges (halfEdges, edges);
		reindex ();
	}
	
	// get clone
	public HE_Mesh get ()
	{
		HE_Mesh result = new HE_Mesh ();
		reindex ();
	
		for (int i=0; i<vertices.Count; i++) {
			result.vertices.Add ((vertices [i]).getClone ());
		}

		for (int i=0; i<faces.Count; i++) {
			result.faces.Add (new HE_Face ());
		}
		for (int i=0; i<halfEdges.Count; i++) {
			result.halfEdges.Add (new HE_HalfEdge ());
		}
		for (int i=0; i<edges.Count; i++) {
			result.edges.Add (new HE_Edge ());
		}
		//
		for (int i=0; i<vertices.Count; i++) {
			HE_Vertex sv = vertices [i];
			HE_Vertex tv = result.vertices [i];
			tv.halfEdge = result.halfEdges [sv.halfEdge.id];
		}
		for (int i=0; i<faces.Count; i++) {
			HE_Face sf = faces [i];
			HE_Face tf = result.faces [i];
			tf.id = i;
			tf.halfEdge = result.halfEdges [sf.halfEdge.id];
		}
		for (int i=0; i<edges.Count; i++) {
			HE_Edge se = edges [i];
			HE_Edge te = result.edges [i];
			te.halfEdge = result.halfEdges [se.halfEdge.id];
			te.id = i;
		}
		for (int i=0; i<halfEdges.Count; i++) {
			HE_HalfEdge she = halfEdges [i];
			HE_HalfEdge the = result.halfEdges [i];
			the.pair = result.halfEdges [she.pair.id];
			the.next = result.halfEdges [she.next.id];
			the.prev = result.halfEdges [she.prev.id];
			the.vert = result.vertices [she.vert.id];
			the.face = result.faces [she.face.id];
			the.edge = result.edges [she.edge.id];
			the.id = i;
		}
		return result;
	}

	HE_Mesh getDual ()
	{
		HE_Mesh result = new HE_Mesh ();
		reindex ();
		for (int i=0; i<faces.Count; i++) {
			HE_Face f = faces [i];
			HE_HalfEdge he = f.halfEdge;
			PVector faceCenter = new PVector ();
			int n = 0;
			do {
				faceCenter.add (he.vert);
				he = he.next;
				n++;
			} while(he!=f.halfEdge);
			
			faceCenter.div (n);
			result.vertices.Add (new HE_Vertex (faceCenter.x, faceCenter.y, faceCenter.z, i));
		}
		//for(int i=0;i<vertices.Count;i++){
		//HE_Vertex v=getVertex(i);
		foreach (HE_Vertex v in vertices) {
			HE_HalfEdge he = v.halfEdge;
			HE_Face f = he.face;
			List<HE_HalfEdge> faceHalfEdges = new List<HE_HalfEdge> ();
			HE_Face nf = new HE_Face ();
			result.faces.Add (nf);
			do {
				HE_HalfEdge hen = new HE_HalfEdge ();
				faceHalfEdges.Add (hen);
				hen.face = nf;
				hen.vert = result.vertices [f.id];
				if (hen.vert.halfEdge == null)
					hen.vert.halfEdge = hen;
				if (nf.halfEdge == null)
					nf.halfEdge = hen;
				he = he.pair.next;
				f = he.face;
			} while(he != v.halfEdge);
			
			cycleHalfEdges (faceHalfEdges, false);    
			result.halfEdges.AddRange (faceHalfEdges);
		}
		result.pairHalfEdges (result.halfEdges);
		result.createEdges (result.halfEdges, result.edges);
		result.reindex ();
		return result;
	
	}

	void cycleHalfEdges (List<HE_HalfEdge> halfEdges, bool reverse)
	{   
		HE_HalfEdge he;
		int n = halfEdges.Count;
		if (!reverse) {
			he = halfEdges [0];
			he.next = halfEdges [1];
			he.prev = halfEdges [n - 1];
			for (int j=1; j<n-1; j++) {
				he = halfEdges [j];
				he.next = halfEdges [j + 1];
				he.prev = halfEdges [j - 1];
			}
			he = halfEdges [n - 1];
			he.next = halfEdges [0];
			he.prev = halfEdges [n - 2];
		} else {
			he = halfEdges [0];
			he.prev = halfEdges [1];
			he.next = halfEdges [n - 1];
			for (int j=1; j<n-1; j++) {
				he = halfEdges [j];
				he.prev = halfEdges [j + 1];
				he.next = halfEdges [j - 1];
			}
			he = halfEdges [n - 1];
			he.prev = halfEdges [0];
			he.next = halfEdges [n - 2];
		}
	
	}

	//go through all the halfedges and find matching pairs
	void pairHalfEdges (List<HE_HalfEdge> halfEdges)
	{
		int n = halfEdges.Count;
		for (int i=0; i<n; i++) {
			HE_HalfEdge he = halfEdges [i];
			if (he.pair == null) {
				for (int j=0; j<n; j++) {
					if (i != j) {
						HE_HalfEdge he2 = halfEdges [j];
						if ((he2.pair == null) && (he.vert == he2.next.vert) && (he2.vert == he.next.vert)) {
							he.pair = he2;
							he2.pair = he;
							break;
						}
					}
				}
			}
		}
	}

	//associate each pair of halfedges to a physical edge.
	void createEdges (List<HE_HalfEdge> halfEdges, List<HE_Edge> target)
	{
		int n = halfEdges.Count;
		for (int i=0; i<n; i++) {
			HE_HalfEdge he = halfEdges [i];     
			for (int j=0; j<n; j++) {
				if (i != j) {
					HE_HalfEdge he2 = halfEdges [j];
					if (he.pair == he2) {
						HE_Edge e = new HE_Edge ();
						e.halfEdge = he;
						target.Add (e);
						he.edge = e;
						he2.edge = e;
						break;              
					}
				}        
			}
		}
	}


	// check all edges for intersection wit a plan
	List<SplitEdge> retrieveSplitEdges (Plane P)
	{
		List<SplitEdge> splitEdges = new List<SplitEdge> ();
		for (int i=0; i<edges.Count; i++) {
			HE_Edge edge = edges [i];
			/*
			PVector intersection = planeEdgeIntersection(edge,P);
			if(intersection != null){    
				splitEdges.Add(new SplitEdge(edge,intersection));
			}
			*/
			PVector intersection;
			bool b = planeEdgeIntersection (out intersection, edge, P);
			if (b) {
				splitEdges.Add (new SplitEdge (edge, intersection));
			}
		}
		return splitEdges;
	}

	void reindex ()
	{
		for (int i=0; i<vertices.Count; i++) {
			(vertices [i]).id = i;
		}
		for (int i=0; i<faces.Count; i++) {
			(faces [i]).id = i;
		}
		for (int i=0; i<halfEdges.Count; i++) {
			(halfEdges [i]).id = i;
		}
		for (int i=0; i<edges.Count; i++) {
			(edges [i]).id = i;
		}
	}


	// Split the mesh in half, retain the part on the same side as the point "center".
	// Works only on a convex mesh !!!
	public void cutMesh (Plane P, PVector center)
	{
		float centerside = P.side (center);
		if (centerside != 0) {// if center is on the plane, we can't decide which part to keep, ignore.
			List<HE_Vertex> newVertices = new List<HE_Vertex> ();
			List<HE_Face> newFaces = new List<HE_Face> ();
			List<HE_HalfEdge> newHalfEdges = new List<HE_HalfEdge> ();
			List<HE_Edge> newEdges = new List<HE_Edge> ();

			// get all split edges
			List<SplitEdge> splitEdges = retrieveSplitEdges (P);           

			//check if the plane cuts the mesh at all, at least one point should be on the other side of the plane.
			//compared to the first point
			float[] sides = new float[vertices.Count];
			//bool cut = false;// add inok
			for (int i=0; i<vertices.Count; i++) {
				HE_Vertex v = vertices [i];
				sides [i] = P.side (v);
				//if(sides[0]*sides[i]<=0f) cut=true;// add inok
			}
			//loop through all faces.
			for (int i=0; i<faces.Count; i++) {
				HE_Face face = faces [i];
				HE_HalfEdge halfEdge = face.halfEdge;      
				List<HE_Vertex> newFaceVertices1 = new List<HE_Vertex> ();// vertices on the correct side.
				List<HE_Vertex> newFaceVertices2 = new List<HE_Vertex> ();// vertices on the wrong side, not used right now.

				//for each face, loop through all vertices and retain the vertices on the correct side. If the edge
				//is cut, insert the new point in the appropriate place.   
				do { 
					if (sides [halfEdge.vert.id] * centerside >= 0f) {
						newFaceVertices1.Add (halfEdge.vert);
					}
					if (sides [halfEdge.vert.id] * centerside <= 0f) {
						newFaceVertices2.Add (halfEdge.vert);
					}

					for (int j=0; j < splitEdges.Count; j++) {// loop through all split edges to check for the current edge.
						SplitEdge se = splitEdges [j];
						if (halfEdge.edge == se.edge) {
							newFaceVertices1.Add (se.splitVertex);
							newFaceVertices2.Add (se.splitVertex);
							break;
						}
					}
					halfEdge = halfEdge.next;
				} while(halfEdge!=face.halfEdge);

				//Create a new face form the vertices we retained,ignore degenerate faces with less than 3 vertices.
				//Add all face-related information to the data-structure.
				if (newFaceVertices1.Count > 2) {
					HE_Face newFace = new HE_Face ();
					newFaces.Add (newFace);
					List<HE_HalfEdge> faceEdges = new List<HE_HalfEdge> ();
					for (int j=0; j<newFaceVertices1.Count; j++) {
						HE_Vertex v = newFaceVertices1 [j];
						if (!newVertices.Contains (v))
							newVertices.Add (v);
						HE_HalfEdge newHalfEdge = new HE_HalfEdge ();
						faceEdges.Add (newHalfEdge);
						newHalfEdge.vert = v;

						v.halfEdge = newHalfEdge;
						newHalfEdge.face = newFace;
						if (newFace.halfEdge == null)
							newFace.halfEdge = newHalfEdge;
					}
					cycleHalfEdges (faceEdges, false);
					newHalfEdges.AddRange (faceEdges); 
				}
			}

			//Add missing information to the datastructure
			int n = newHalfEdges.Count;
			pairHalfEdges (newHalfEdges);
			createEdges (newHalfEdges, newEdges);

			//Cutting the mesh not only cuts the faces, it also creates one new planar face looping through all new cutpoints(in a convex mesh).
			//This hole in the mesh is identified by unpaired halfedges remaining after the pairibg operation.
			//This part needs to rethought to extend to concave meshes!!!
			List<HE_HalfEdge> unpairedEdges = new List<HE_HalfEdge> ();
			for (int i=0; i<n; i++) {
				HE_HalfEdge he = newHalfEdges [i];
				if (he.pair == null)
					unpairedEdges.Add (he);
			}
			if (unpairedEdges.Count > 0) {
				//Create a closed loop out of the collection of unpaired halfedges and associate a new face with this.
				//Easy to explain with a drawing, beyond my skill with words.
				HE_Face cutFace = new HE_Face (); 
				List<HE_HalfEdge> faceEdges = new List<HE_HalfEdge> ();  
				HE_HalfEdge he = unpairedEdges [0];
				HE_HalfEdge hen = he;
				do {
					HE_HalfEdge _hen = he.next;
					HE_HalfEdge _hep = he.next.pair;
					if(_hep != null){//add inok
						hen = he.next.pair.next;
						while (!unpairedEdges.Contains(hen)){
							hen = hen.pair.next;
						}
					}else{
						hen = hen.next;
						Debug.LogWarning("LogWarning: null");
					}
					HE_HalfEdge newhe = new HE_HalfEdge ();
					faceEdges.Add (newhe);
					if (cutFace.halfEdge == null)
						cutFace.halfEdge = newhe;
					newhe.vert = hen.vert;
					newhe.pair = he;
					he.pair = newhe;
					HE_Edge e = new HE_Edge ();
					e.halfEdge = newhe;
					he.edge = e;
					newhe.edge = e;
					newEdges.Add (e);
					newhe.face = cutFace;
					he = hen;
				} while(hen!=unpairedEdges[0]);

				cycleHalfEdges (faceEdges, true);
				newHalfEdges.AddRange (faceEdges); 
				newFaces.Add (cutFace);
			}              
      
			// update the mesh
			vertices = newVertices;
			faces = newFaces;
			halfEdges = newHalfEdges;
			
			edges = newEdges;
			reindex ();

		}
	}

	void splitSurface (Plane P)
	{
		List<HE_Vertex> newVertices = new List<HE_Vertex> ();
		List<HE_Face> newFaces = new List<HE_Face> ();
		List<HE_HalfEdge> newHalfEdges = new List<HE_HalfEdge> ();
		List<HE_Edge> newEdges = new List<HE_Edge> ();

		// get all split edges
		List<SplitEdge> splitEdges = retrieveSplitEdges (P);           

		//check if the plane cuts the mesh at all, at least one point should be on the other side of the plane.
		//compared to the first point
		float[] sides = new float[vertices.Count];
		//bool cut=false;         
		for (int i=0; i<vertices.Count; i++) {
			HE_Vertex v = vertices [i];
			sides [i] = P.side (v);
			//if(sides[0]*sides[i]<=0f) cut=true;
		}
		
		//loop through all faces.
		for (int i=0; i<faces.Count; i++) {
			HE_Face face = faces [i];
			HE_HalfEdge halfEdge = face.halfEdge;      
			List<HE_Vertex> newFaceVertices1 = new List<HE_Vertex> ();
			List<HE_Vertex> newFaceVertices2 = new List<HE_Vertex> ();
			List<HE_Vertex> currentFace = newFaceVertices1;
			//for each face, loop through all vertices and retain the vertices on the correct side. If the edge
			//is cut, insert the new point in the appropriate place.   
			do {         
				currentFace.Add (halfEdge.vert); 
				for (int j=0; j<splitEdges.Count; j++) {// loop through all split edges to check for the current edge.
					SplitEdge se = (SplitEdge)splitEdges [j];
					if (halfEdge.edge == se.edge) {
						newFaceVertices1.Add (se.splitVertex);
						newFaceVertices2.Add (se.splitVertex);
						if (currentFace == newFaceVertices1) {
							currentFace = newFaceVertices2;
						} else {
							currentFace = newFaceVertices1;
						}
						break;
					}
				}
				halfEdge = halfEdge.next;
			} while(halfEdge!=face.halfEdge);

			//Create a new face form the vertices we retained,ignore degenerate faces with less than 3 vertices.
			//Add all face-related information to the data-structure.

			HE_Face newFace = new HE_Face ();
			newFaces.Add (newFace);
			List<HE_HalfEdge> faceEdges = new List<HE_HalfEdge> ();
			for (int j=0; j<newFaceVertices1.Count; j++) {
				HE_Vertex v = newFaceVertices1 [j];
				if (!newVertices.Contains (v))
					newVertices.Add (v);
				HE_HalfEdge newHalfEdge = new HE_HalfEdge ();
				faceEdges.Add (newHalfEdge);
				newHalfEdge.vert = v;

				v.halfEdge = newHalfEdge;
				newHalfEdge.face = newFace;
				if (newFace.halfEdge == null)
					newFace.halfEdge = newHalfEdge;
			}
			cycleHalfEdges (faceEdges, false);
			newHalfEdges.AddRange (faceEdges); 
			if (newFaceVertices2.Count > 0) {
				newFace = new HE_Face ();
				newFaces.Add (newFace);
				faceEdges = new List<HE_HalfEdge> ();
				for (int j=0; j<newFaceVertices2.Count; j++) {
					HE_Vertex v = newFaceVertices2 [j];
					if (!newVertices.Contains (v))
						newVertices.Add (v);
					HE_HalfEdge newHalfEdge = new HE_HalfEdge ();
					faceEdges.Add (newHalfEdge);
					newHalfEdge.vert = v;

					v.halfEdge = newHalfEdge;
					newHalfEdge.face = newFace;
					if (newFace.halfEdge == null)
						newFace.halfEdge = newHalfEdge;
				}
				cycleHalfEdges (faceEdges, false);
				newHalfEdges.AddRange (faceEdges); 
         
				//}//test
			}

			//Add missing information to the datastructure
			pairHalfEdges (newHalfEdges);
			createEdges (newHalfEdges, newEdges);
		}
      
		// update the mesh
		vertices = newVertices;
		faces = newFaces;
		halfEdges = newHalfEdges;
		edges = newEdges;
		reindex ();
	}

	public void roundCorners (float d)
	{
		if(d <= 0){ return; }
		
		List<Plane> cutPlanes = new List<Plane> ();
		PVector center = new PVector ();
		for (int i=0; i<vertices.Count; i++) {
			HE_Vertex v = vertices [i];   
			center.add (v);
		} 
		center.div (vertices.Count);
		for (int i=0; i<vertices.Count; i++) {
			HE_Vertex v = vertices [i];   
			PVector n = PVector.sub (v, center);
			
			//float distanceToVertex=n.mag();
			//if(distanceToVertex>d){
			float distanceToVertex = n.magSq ();
			if (distanceToVertex > d * d) {
				float ratio = (distanceToVertex - d) / distanceToVertex;
				PVector origin = PVector.mult (n, ratio);
				origin.add (center);
				cutPlanes.Add (new Plane (origin, n));
			}
		}
		for (int i=0; i<cutPlanes.Count; i++) {
			Plane P = cutPlanes [i];
			cutMesh (P, center);
		}
	}

	//FLAWED
	public void roundEdges (float d)
	{
		if(d <= 0){ return; }
		
		List<Plane> cutPlanes = new List<Plane> ();
		PVector center = new PVector ();
		for (int i=0; i<vertices.Count; i++) {
			HE_Vertex v = vertices [i];   
			center.add (v);
		} 
		center.div (vertices.Count);
    
		for (int i=0; i<edges.Count; i++) {
			HE_Edge e = edges [i];   
			HE_Vertex v1 = e.halfEdge.vert;
			HE_Vertex v2 = e.halfEdge.pair.vert;
			HE_Vertex v = new HE_Vertex (0.5f * (v1.x + v2.x), 0.5f * (v1.y + v2.y), 0.5f * (v1.z + v2.z), 0);

			PVector n = PVector.sub (v, center);
			//float distanceToVertex=n.mag();
			//if(distanceToVertex>d){
			float distanceToVertex = n.magSq ();
			if (distanceToVertex > d * d) {
				float ratio = (distanceToVertex - d) / distanceToVertex;
				PVector origin = PVector.mult (n, ratio);
				origin.add (center);
				cutPlanes.Add (new Plane (origin, n));
			}
		}
		for (int i=0; i<cutPlanes.Count; i++) {
			Plane P = cutPlanes [i];
			cutMesh (P, center);
		}
	}
	
	
		
	//Paul Bourke, http://local.wasp.uwa.edu.au/~pbourke/geometry/planeline/
	PVector planeEdgeIntersection (HE_Edge e, Plane P)
	{
		PVector p0 = e.halfEdge.vert;
		PVector p1 = e.halfEdge.pair.vert;
		float denom = P.A * (p0.x - p1.x) + P.B * (p0.y - p1.y) + P.C * (p0.z - p1.z);
		if ((denom < HE_Mesh.DELTA) && (denom > -HE_Mesh.DELTA))
			return null;
		float u = (P.A * p0.x + P.B * p0.y + P.C * p0.z + P.D) / denom;
		if ((u < 0.0f) || (u > 1.0f))
			return null;
		return new PVector (p0.x + u * (p1.x - p0.x), p0.y + u * (p1.y - p0.y), p0.z + u * (p1.z - p0.z));
	}
	
	bool planeEdgeIntersection (out PVector p, HE_Edge e, Plane P)
	{
		PVector p0 = e.halfEdge.vert;
		PVector p1 = e.halfEdge.pair.vert;
		float denom = P.A * (p0.x - p1.x) + P.B * (p0.y - p1.y) + P.C * (p0.z - p1.z);
		if ((denom < HE_Mesh.DELTA) && (denom > -HE_Mesh.DELTA)) {
			p = null;
			return false;
		}
		float u = (P.A * p0.x + P.B * p0.y + P.C * p0.z + P.D) / denom;
		if ((u < 0.0f) || (u > 1.0f)) {
			p = null;
			return false;
		}
		p = new PVector (p0.x + u * (p1.x - p0.x), p0.y + u * (p1.y - p0.y), p0.z + u * (p1.z - p0.z));
		return true;
	}
}

public class HE_HalfEdge
{
	public int id;
	public HE_Vertex vert;
	public HE_HalfEdge pair;
	public HE_Face face;
	public HE_HalfEdge next;
	public HE_HalfEdge prev;
	public HE_Edge edge;
}

public class HE_Edge
{
	public int id;
	public HE_HalfEdge halfEdge;
}

public class HE_Vertex : PVector
{
	public int id;
	public HE_HalfEdge halfEdge;
	
	public HE_Vertex (float x, float y, float z, int id): base(x, y, z)
	{
		this.id = id;
	}
	
	// clone
	public HE_Vertex getClone ()
	{
		return new HE_Vertex (x, y, z, id); 
	}
}

public class HE_Face
{
	public int id;
	public HE_HalfEdge halfEdge;
}

public class SplitEdge
{
	public HE_Edge edge;
	public HE_Vertex splitVertex;

	public SplitEdge (HE_Edge e, PVector p)
	{
		edge = e;
		splitVertex = new HE_Vertex (p.x, p.y, p.z, 0);
	}  
}


//
public class Triangle3D
{
	public List<HE_Vertex> vertices;
	
	public Triangle3D ()
	{
		vertices = new List<HE_Vertex> ();
	}
}
