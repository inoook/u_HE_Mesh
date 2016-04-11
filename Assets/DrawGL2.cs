using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// no use.
public class DrawGL2 : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		//voronoiTest.transform.Rotate(new Vector3(10, 10,0)* Time.deltaTime);
	}
	
	static Material glMaterial;

	static void CreateGLMaterial ()
	{
		if (!glMaterial) {
			glMaterial = new Material ("Shader \"Lines/Colored Blended\" {" +
	            "SubShader { Pass { " +
	            "    Blend SrcAlpha OneMinusSrcAlpha " +
	            "    ZWrite Off Cull Off Fog { Mode Off } " +
	            "    BindChannels {" +
	            "      Bind \"vertex\", vertex Bind \"color\", color }" +
	            "} } }");
			glMaterial.hideFlags = HideFlags.HideAndDontSave;
			glMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}
	
	public buckySlice voronoiTest;
	
	void OnPostRender ()
	{
		
		List<HE_Mesh> meshes = voronoiTest.meshes;
		List<PVector> centers = voronoiTest.centers;
		
		CreateGLMaterial ();
		glMaterial.SetPass (0);
		
		GL.PushMatrix ();
		
		Matrix4x4 mtx = voronoiTest.transform.localToWorldMatrix;
		
		//draw //////////////////////////
		GL.Begin (GL.LINES);
		for (int i=0; i < meshes.Count; i++) {
			PVector center = (PVector)centers [i];
			HE_Mesh mesh = (HE_Mesh)meshes [i];
			
			Color color = Color.Lerp (Color.red, Color.blue, (float)i / (float)meshes.Count);
			
			//mesh.drawEdges();
			int edgeNum = mesh.edges.Count;
			for (int j = 0; j < edgeNum; j++) {
				HE_Edge e = mesh.edges [j];
				
				GL.Color (color);
				Vector3 pos0 = mtx.MultiplyPoint3x4 (e.halfEdge.vert.pos + center.pos * 0.2f);
				Vector3 pos1 = mtx.MultiplyPoint3x4 (e.halfEdge.pair.vert.pos + center.pos * 0.2f);
				//Debug.DrawLine (p0 + center.pos * 0.2f, p1 + center.pos * 0.2f, Color.red);
				GL.Vertex (pos0);
				GL.Vertex (pos1);
			}
		}
		
		/*
        for(int i=0; i < voronoiCells.Length; i++)
        {
            int edgeNum = voronoiCells[i].edges.Count;
			
            for(int j = 0; j < edgeNum; j++){
                HE_Edge e = voronoiCells[i].edges[j];
				Color color = Color.Lerp(Color.red, Color.blue, (float)j / (float)edgeNum);
				//Debug.DrawLine(e.halfEdge.vert.pos, e.halfEdge.pair.vert.pos, color);
				
				GL.Color(color);
				Vector3 pos0 = mtx.MultiplyPoint3x4(e.halfEdge.vert.pos);
				Vector3 pos1 = mtx.MultiplyPoint3x4(e.halfEdge.pair.vert.pos);
				GL.Vertex(pos0);
				GL.Vertex(pos1);
            }
        }
        */
		GL.End ();
		
		GL.PopMatrix ();
	}
	
	void OnGUI ()
	{
		
	}
}
