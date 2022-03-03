using System.Collections.Generic;
using UnityEngine;

namespace CXUtils.Common
{
	///<summary> CX's Helper Mesh Utils and extensions </summary>
	public static class MeshUtils
	{
		#region Script Methods

		/// <summary> Recalculates all the bounds, normals and tangents of the mesh </summary>
		public static void RecalculateAll(this Mesh mesh)
		{
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
		}

		#endregion

		#region Mesh Construction

		/// <summary>
		///     This will create a connected quad mesh (which uses the mesh to just display one big texture only (not for single
		///     quad grid uv))
		/// </summary>
		public static void CreateEmptyConnectedQuadMeshArrays(Vector2Int size, out Vector3[] vertices, out int[] triangles, out Vector2[] uvs)
		{
			int totVerticesCount = (size.x + 1) * (size.y + 1);

			vertices = new Vector3[totVerticesCount];
			triangles = new int[size.x * size.y * 2];
			uvs = new Vector2[totVerticesCount];
		}

		/// <summary>
		///     This will create a connected quad mesh but independent with each other quad meshes
		///     (which u use the mesh to display grid like tiles and other awesome stuff)
		/// </summary>
		public static void CreateQuadMeshArrays(Vector2Int size, out Vector3[] vertices, out int[] triangles, out Vector2[] uvs)
		{
			int totVerticesCount = size.x * size.y * 4;

			vertices = new Vector3[totVerticesCount];

			triangles = new int[size.x * size.y * 2];
			uvs = new Vector2[totVerticesCount];
		}

		#endregion

		#region All Mesh

		/// <summary>
		///     Adds a triangle mesh on a mesh
		/// </summary>
		public static void AddTriangle(this Mesh mesh, Vector3 a, Vector3 b, Vector3 c, int subMesh)
		{
			var vertices = new List<Vector3>();
			var triangles = new List<int>();

			mesh.GetVertices(vertices);
			mesh.GetTriangles(triangles, subMesh);

			mesh.Clear();

			vertices.Add(a);
			triangles.Add(vertices.Count - 1);

			vertices.Add(b);
			triangles.Add(vertices.Count - 1);

			vertices.Add(c);
			triangles.Add(vertices.Count - 1);

			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles, subMesh);
		}

		/// <summary> Adds a rectangular mesh on (facing on left down , left up , right up || right down, left down, right up)</summary>
		public static void AddRect(this Mesh mesh, Vector3 leftDown, Vector3 leftUp, Vector3 rightUp,
			Vector3 rightDown, int subMesh)
		{
			var vertices = new List<Vector3>();
			var triangles = new List<int>();

			mesh.GetVertices(vertices);
			mesh.GetTriangles(triangles, subMesh);

			mesh.Clear();

			vertices.Add(leftDown);
			triangles.Add(vertices.Count - 1);

			vertices.Add(leftUp);
			triangles.Add(vertices.Count - 1);

			vertices.Add(rightUp);
			triangles.Add(vertices.Count - 1);
			
			vertices.Add(rightDown);
			triangles.Add(vertices.Count - 1);
			triangles.Add(vertices.Count - 4); // leftDown
			triangles.Add(vertices.Count - 2); // leftDown

			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles, subMesh);
			
			// AddTriangle(mesh, leftDown, leftUp, rightUp, subMesh);
			// AddTriangle(mesh, rightDown, leftDown, rightUp, subMesh);
		}

		#endregion

		#region Add Grid Mesh

		/// <summary> Adds a rectangular grid mesh (In order [vertices are in 1D array with orders]) </summary>
		public static void AddGridMeshInOrder(this Mesh mesh, float eachGridSize, Vector2Int wholeGridSize,
			int triangleSubMesh)
		{
			var vertices = new List<Vector3>();
			var triangles = new List<int>();

			//vertices
			for (float z = 0; z <= wholeGridSize.y; z += eachGridSize)
				for (float x = 0; x <= wholeGridSize.x; x += eachGridSize)
					vertices.Add(new Vector3(x, 0, z));

			//triangles
			int vert = 0;
			for (int z = 0; z < wholeGridSize.y; z++)
			{
				for (int x = 0; x < wholeGridSize.x; x++)
				{
					triangles.Add(vert + 1);
					triangles.Add(vert);
					triangles.Add(vert + wholeGridSize.x + 1);

					triangles.Add(vert + 1);
					triangles.Add(vert + wholeGridSize.x + 1);
					triangles.Add(vert + wholeGridSize.x + 2);
					vert++;
				}
				vert++;
			}

			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles.ToArray(), triangleSubMesh);
			//and recalculate
			mesh.RecalculateAll();
		}

		/// <summary> Adds a rectangular grid mesh (In order [vertices are in 1D array with orders]) Out vertices </summary>
		public static void AddGridMeshInOrder(this Mesh mesh, float eachGridSize, Vector2Int wholeGridSize,
			int triangleSubMesh, out List<Vector3> vertices)
		{
			vertices = new List<Vector3>();
			var triangles = new List<int>();

			//vetrticies
			for (float z = 0; z <= wholeGridSize.y; z += eachGridSize)
				for (float x = 0; x <= wholeGridSize.x; x += eachGridSize)
					vertices.Add(new Vector3(x, 0, z));

			//triangles
			int vert = 0;
			for (int z = 0; z < wholeGridSize.y; z++)
			{
				for (int x = 0; x < wholeGridSize.x; x++)
				{
					triangles.Add(vert + 1);
					triangles.Add(vert);
					triangles.Add(vert + wholeGridSize.x + 1);
					triangles.Add(vert + 1);
					triangles.Add(vert + wholeGridSize.x + 1);
					triangles.Add(vert + wholeGridSize.x + 2);
					vert++;
				}
				vert++;
			}

			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles.ToArray(), triangleSubMesh);

			//and recalculate
			RecalculateAll(mesh);
		}

		/// <summary> Adds a rectangular grid mesh (In order [vertices are in 1D array with orders]) Out vertices and triangles </summary>
		public static void AddGridMeshInOrder(this Mesh mesh, float eachGridSize, Vector2Int wholeGridSize,
			int triangleSubMesh, out List<Vector3> vertices, out List<int> triangles)
		{
			vertices = new List<Vector3>();
			triangles = new List<int>();

			//vertices
			for (float z = 0; z <= wholeGridSize.y; z += eachGridSize)
				for (float x = 0; x <= wholeGridSize.x; x += eachGridSize)
					vertices.Add(new Vector3(x, 0, z));

			//triangles
			int vert = 0;
			for (int z = 0; z < wholeGridSize.y; z++)
			{
				for (int x = 0; x < wholeGridSize.x; x++)
				{
					triangles.Add(vert + 1);
					triangles.Add(vert);
					triangles.Add(vert + wholeGridSize.x + 1);
					triangles.Add(vert + 1);
					triangles.Add(vert + wholeGridSize.x + 1);
					triangles.Add(vert + wholeGridSize.x + 2);
					vert++;
				}
				vert += 2;
			}

			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles.ToArray(), triangleSubMesh);

			//and recalculate
			mesh.RecalculateAll();
		}

		#endregion
	}
}
