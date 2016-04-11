using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DrawGL : MonoBehaviour {
	
	public Material boxMat;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		voronoiTest.transform.Rotate(new Vector3(10, 10,0)* Time.deltaTime);
	}
	
	static Material glMaterial;
	static void CreateGLMaterial() {
	    if( !glMaterial ) {
	        glMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
	            "SubShader { Pass { " +
	            "    Blend SrcAlpha OneMinusSrcAlpha " +
	            "    ZWrite Off Cull Off Fog { Mode Off } " +
	            "    BindChannels {" +
	            "      Bind \"vertex\", vertex Bind \"color\", color }" +
	            "} } }" );
	        glMaterial.hideFlags = HideFlags.HideAndDontSave;
	        glMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
	    }
	}
	
	public stickyCube voronoiTest;
	
	void OnPostRender() {
		
		PVector[] sites = voronoiTest.points;
		HE_Mesh[] voronoiCells = voronoiTest.voronoiCells;
		
		CreateGLMaterial();
	    glMaterial.SetPass( 0 );
		
		GL.PushMatrix();
		
		Matrix4x4 mtx = voronoiTest.transform.localToWorldMatrix;
		
		//draw sites/////////////////////////////
		float size = 0.1f;
		GL.Begin( GL.QUADS );
		
		GL.Color(Color.white);
		for(int i=0; i < sites.Length; i++)
		{
			PVector pt = sites[i];
			//Vector3 pos = pt.pos;
			Vector3 pos = mtx.MultiplyPoint3x4(pt.pos);
			GL.Vertex(pos + new Vector3(-size, -size, 0));
			GL.Vertex(pos + new Vector3( size, -size, 0));
			GL.Vertex(pos + new Vector3( size,  size, 0));
			GL.Vertex(pos + new Vector3( -size,  size, 0));
        }
        GL.End();
		
        //draw voronoi mesh//////////////////////////
		GL.Begin( GL.LINES );
		Color color = Color.white;
        for(int i=0; i < voronoiCells.Length; i++)
        {
            int edgeNum = voronoiCells[i].edges.Count;
			
            for(int j = 0; j < edgeNum; j++){
                HE_Edge e = voronoiCells[i].edges[j];
				//Color color = Color.Lerp(Color.red, Color.blue, (float)j / (float)edgeNum);
				//Debug.DrawLine(e.halfEdge.vert.pos, e.halfEdge.pair.vert.pos, color);
				
				GL.Color(color);
				Vector3 pos0 = mtx.MultiplyPoint3x4(e.halfEdge.vert.pos);
				Vector3 pos1 = mtx.MultiplyPoint3x4(e.halfEdge.pair.vert.pos);
				GL.Vertex(pos0);
				GL.Vertex(pos1);
            }
        }
		GL.End();
		
        //boxMat.SetPass(0);
		//voronoiTest.drawVoronoiBox();
		
		GL.PopMatrix();
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(0, 20, 100, 100), "num: "+voronoiTest.voronoiCells.Length.ToString());
	}
}
