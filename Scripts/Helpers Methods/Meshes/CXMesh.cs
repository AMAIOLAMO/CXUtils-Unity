using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace CXUtils.CXMeshes
{
    ///<summary>
    ///Cx's Mesh managing Class
    ///</summary>
    public class CXMesh : MonoBehaviour
    {
        /// <summary>
        /// Adds a triangle mesh on a mesh
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="PT1"></param>
        /// <param name="PT2"></param>
        /// <param name="PT3"></param>
        public static void AddTriangleMesh(Mesh mesh, Vector3 PT1, Vector3 PT2, Vector3 PT3, int TriangleSubMeshIndex)
        {
            List<Vector3> verticies = new List<Vector3>();
            List<int> triangles = new List<int>();
            foreach (var vert in mesh.vertices)
                verticies.Add(vert);
            foreach (var tris in mesh.triangles)
                triangles.Add(tris);
            //clear the mesh, for less wrongs
            mesh.Clear();
            verticies.Add(PT1);
            triangles.Add(verticies.Count - 1);
            verticies.Add(PT2);
            triangles.Add(verticies.Count - 1);
            verticies.Add(PT3);
            triangles.Add(verticies.Count - 1);

            mesh.SetVertices(verticies);
            mesh.SetTriangles(triangles.ToArray(), TriangleSubMeshIndex);
        }
        /// <summary>
        /// Adds a rectangular mesh on (facing on left down , left up , right up || right down, left down, right up)
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="leftUp"></param>
        /// <param name="rightUp"></param>
        /// <param name="leftDown"></param>
        /// <param name="rightDown"></param>
        public static void AddRectangleMesh(Mesh mesh, Vector3 leftDown, Vector3 leftUp, Vector3 rightUp, Vector3 rightDown, int TriangleSubMeshesIndex)
        {
            AddTriangleMesh(mesh, leftDown, leftUp, rightUp, TriangleSubMeshesIndex);
            AddTriangleMesh(mesh, rightDown, leftDown, rightUp, TriangleSubMeshesIndex);
        }
        /// <summary>
        /// Adds a rectangular grid mesh (In order [verticies are in 1D array with orders])
        /// </summary>
        public static void AddGridMeshInOrder(Mesh mesh, float eachGridSize, Vector2Int wholeGridSize, int TriangleSubmesh)
        {
            List<Vector3> verticies = new List<Vector3>();
            List<int> triangles = new List<int>();

            //vetrticies
            for (float z = 0; z <= wholeGridSize.y; z += eachGridSize)
            {
                for (float x = 0; x <= wholeGridSize.x; x += eachGridSize)
                {
                    verticies.Add(new Vector3(x, 0, z));
                }
            }

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

            mesh.SetVertices(verticies);
            mesh.SetTriangles(triangles.ToArray(), TriangleSubmesh);
            //and recalculate
            RecalculateAll(mesh);
        }
        /// <summary>
        /// Adds a rectangular grid mesh (In order [verticies are in 1D array with orders]) Out verticies
        /// </summary>
        public static void AddGridMeshInOrder(Mesh mesh, float eachGridSize, Vector2Int wholeGridSize, int TriangleSubmesh, out List<Vector3> verticies)
        {
            verticies = new List<Vector3>();
            List<int> triangles = new List<int>();

            //vetrticies
            for (float z = 0; z <= wholeGridSize.y; z += eachGridSize)
            {
                for (float x = 0; x <= wholeGridSize.x; x += eachGridSize)
                {
                    verticies.Add(new Vector3(x, 0, z));
                }
            }

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

            mesh.SetVertices(verticies);
            mesh.SetTriangles(triangles.ToArray(), TriangleSubmesh);
            //and recalculate
            RecalculateAll(mesh);
        }
        /// <summary>
        /// Adds a rectangular grid mesh (In order [verticies are in 1D array with orders]) Out verticies and triangles
        /// </summary>
        public static void AddGridMeshInOrder(Mesh mesh, float eachGridSize, Vector2Int wholeGridSize, int TriangleSubmesh, out List<Vector3> verticies, out List<int> triangles)
        {
            verticies = new List<Vector3>();
            triangles = new List<int>();

            //vetrticies
            for (float z = 0; z <= wholeGridSize.y; z += eachGridSize)
            {
                for (float x = 0; x <= wholeGridSize.x; x += eachGridSize)
                {
                    verticies.Add(new Vector3(x, 0, z));
                }
            }

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

            mesh.SetVertices(verticies);
            mesh.SetTriangles(triangles.ToArray(), TriangleSubmesh);
            //and recalculate
            RecalculateAll(mesh);
        }

        /// <summary>
        /// Adds a height Map in the mesh to use as a displacement(not finish)
        /// </summary>
        public static void AddHeightMapInMesh()
        {

        }
        /// <summary>
        /// Recalculates all the bounds, normals and tangents of the mesh
        /// </summary>
        /// <param name="mesh"></param>
        public static void RecalculateAll(Mesh mesh)
        {
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
    }
}

