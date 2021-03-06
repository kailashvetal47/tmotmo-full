using UnityEngine;

public class ImageMaterial {
	private Mesh mesh;
	public Material material { private set; get; }

	public ImageMaterial (Mesh mesh)
		: this(mesh, DefaultMaterial()) {
	}
	
	public ImageMaterial (Mesh mesh, Material material) {
		this.mesh = mesh;
		this.material = material;
	}
	
	public void RenderTo(GameObject gameObject) {
		var meshFilter = gameObject.GetComponent<MeshFilter>();
		if (meshFilter == null) {
			meshFilter = gameObject.AddComponent<MeshFilter>();
		}
		meshFilter.mesh = mesh;

		var meshRenderer = gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer == null) {
			meshRenderer = gameObject.AddComponent<MeshRenderer>();

			meshRenderer.material = material;
			meshRenderer.receiveShadows = false;
			meshRenderer.castShadows = false;
		} else {
			this.material = meshRenderer.sharedMaterial;
		}
	}
	
	public void SetTexture(Texture2D texture) {
		material.mainTexture = texture;
	}
	
	public void SetUVTiled() {
		// set uv coordinates so the texture repeats when the size of the mesh is larger than the size of the texture
		
		Vector2[] uvs = new Vector2[mesh.vertices.Length];

		// vertices[3] and vertices[0] are the extrema
		var meshSize = mesh.vertices[3] - mesh.vertices[0];

		int i = 0;
        while (i < uvs.Length) {
			// arrived at purely through intuition and trial and error. I will attempt to explain:
			// - I divide the distance from the minimum coordinate and the current vertex by the size of the shape (max - min)
			// - This gives us coordinates between zero and one that correspond to the ratio of the textures width and height
			var p = new Vector2((mesh.vertices[3].x - mesh.vertices[i].x) / meshSize.x,
				(mesh.vertices[3].y - mesh.vertices[i].y) / meshSize.y);
            uvs[i] = new Vector2(p.x, p.y);
            i++;
        }
        mesh.uv = uvs;
	}

	public void SetUVToGridCell(Grid grid, int i, int j) {
		// show only the rectangle at the (i, j) grid coordinate on this mesh
		
		var textureWidth = material.mainTexture.width;
		var textureHeight = material.mainTexture.height;
		
		var corner = grid.PixelAtCell(i, j);
		var cellAsUV = new Rect(corner.x / textureWidth, corner.y / textureHeight,
			grid.cellWidth / textureWidth, grid.cellHeight / textureHeight);
		
		Vector2[] uvs = new Vector2[4];
		uvs[0] = new Vector2(cellAsUV.xMax, cellAsUV.yMax);
        uvs[1] = new Vector2(cellAsUV.xMin, cellAsUV.yMax);
        uvs[2] = new Vector2(cellAsUV.xMax, cellAsUV.yMin);
        uvs[3] = new Vector2(cellAsUV.xMin, cellAsUV.yMin);

        mesh.uv = uvs;
	}

	public void SetUVStretched() {
		// set uv coordinates so the texture stretches across the surface
		Vector2[] uvs = new Vector2[4];
        uvs[0] = new Vector2(1, 1);
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(1, 0);
        uvs[3] = new Vector2(0, 0);
        mesh.uv = uvs;
	}
	
	public static Material DefaultMaterial() {
		var shader = Shader.Find("Mobile/Transparent/Vertex Color");
		if (shader == null) {
			Debug.LogError("Shader (" + shader.name + ") not found.");
		}
			
		Material material = new Material(shader);
		material.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1.0f));
		return material;
	}
}
