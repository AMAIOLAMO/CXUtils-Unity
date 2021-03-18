using UnityEngine;
using System.Collections.Generic;
using System;

namespace CXUtils.CodeUtils
{
    ///<summary> CX's Helper Mesh Utils and extensions </summary>
    public static class MeshUtils
    {
        #region Mesh Construction

        /// <summary>
        /// This will create a connected quad mesh (which u use the mesh to just display one big texture only (not for single quad grid uv))
        /// </summary>
        public static void CreateEmptyConnectedQuadMeshArrays(Vector2Int size, out Vector3[] verticies, out int[] triangles, out Vector2[] uvs)
        {
            int totVerticiesCount = ( size.x + 1 ) * ( size.y + 1 );

            verticies = new Vector3[totVerticiesCount];
            triangles = new int[size.x * size.y * 2];
            uvs = new Vector2[totVerticiesCount];
        }

        /// <summary>
        /// This will create a connected quad mesh but... independent with each other quad meshes
        /// (which u use the mesh to display grid like tiles and other awesome stuff)
        /// </summary>
        public static void CreateEmptyQuadMeshArrays(Vector2Int size, out Vector3[] verticies, out int[] triangles, out Vector2[] uvs)
        {
            int totVerticiesCount = size.x * size.y * 4; // we times four is because each quad has 4 dots (in seperation so we could correctly use uvs lol)

            verticies = new Vector3[totVerticiesCount];

            //triangles remain the same size because ....yea there are just that many triangles what do u think lolol
            //(why draw triangles in between lol)
            triangles = new int[size.x * size.y * 2];
            uvs = new Vector2[totVerticiesCount];
        }

        #endregion

        #region All Mesh

        /// <summary> Adds a triangle mesh on a mesh </summary>
        public static void AddTriangleMesh(this Mesh mesh, Vector3 PT1, Vector3 PT2, Vector3 PT3, int TriangleSubMeshIndex)
        {
            List<Vector3> verticies = new List<Vector3>();
            List<int> triangles = new List<int>();

            foreach ( var vert in mesh.vertices )
                verticies.Add(vert);

            foreach ( var tris in mesh.triangles )
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
            mesh.SetTriangles(triangles, TriangleSubMeshIndex);
        }

        /// <summary> Adds a rectangular mesh on (facing on left down , left up , right up || right down, left down, right up)</summary> 
        public static void AddRectangleMesh(this Mesh mesh, Vector3 leftDown, Vector3 leftUp, Vector3 rightUp,
            Vector3 rightDown, int TriangleSubMeshesIndex)
        {
            AddTriangleMesh(mesh, leftDown, leftUp, rightUp, TriangleSubMeshesIndex);
            AddTriangleMesh(mesh, rightDown, leftDown, rightUp, TriangleSubMeshesIndex);
        }

        #endregion

        #region Add Grid Mesh

        /// <summary> Adds a rectangular grid mesh (In order [verticies are in 1D array with orders]) </summary>
        public static void AddGridMeshInOrder(this Mesh mesh, float eachGridSize, Vector2Int wholeGridSize,
         int TriangleSubmesh)
        {
            List<Vector3> verticies = new List<Vector3>();
            List<int> triangles = new List<int>();

            //vetrticies
            for ( float z = 0; z <= wholeGridSize.y; z += eachGridSize )
                for ( float x = 0; x <= wholeGridSize.x; x += eachGridSize )
                    verticies.Add(new Vector3(x, 0, z));

            //triangles
            int vert = 0;
            for ( int z = 0; z < wholeGridSize.y; z++ )
            {
                for ( int x = 0; x < wholeGridSize.x; x++ )
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
            mesh.RecalculateAll();
        }

        /// <summary> Adds a rectangular grid mesh (In order [verticies are in 1D array with orders]) Out verticies </summary>
        public static void AddGridMeshInOrder(this Mesh mesh, float eachGridSize, Vector2Int wholeGridSize,
         int TriangleSubmesh, out List<Vector3> verticies)
        {
            verticies = new List<Vector3>();
            List<int> triangles = new List<int>();

            //vetrticies
            for ( float z = 0; z <= wholeGridSize.y; z += eachGridSize )
                for ( float x = 0; x <= wholeGridSize.x; x += eachGridSize )
                    verticies.Add(new Vector3(x, 0, z));

            //triangles
            int vert = 0;
            for ( int z = 0; z < wholeGridSize.y; z++ )
            {
                for ( int x = 0; x < wholeGridSize.x; x++ )
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

        /// <summary> Adds a rectangular grid mesh (In order [verticies are in 1D array with orders]) Out verticies and triangles </summary>
        public static void AddGridMeshInOrder(this Mesh mesh, float eachGridSize, Vector2Int wholeGridSize,
         int TriangleSubmesh, out List<Vector3> verticies, out List<int> triangles)
        {
            verticies = new List<Vector3>();
            triangles = new List<int>();

            //vetrticies
            for ( float z = 0; z <= wholeGridSize.y; z += eachGridSize )
                for ( float x = 0; x <= wholeGridSize.x; x += eachGridSize )
                    verticies.Add(new Vector3(x, 0, z));

            //triangles
            int vert = 0;
            for ( int z = 0; z < wholeGridSize.y; z++ )
            {
                for ( int x = 0; x < wholeGridSize.x; x++ )
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
            mesh.RecalculateAll();
        }
        #endregion

        #region Script Methods

        /// <summary> Recalculates all the bounds, normals and tangents of the mesh </summary>
        public static void RecalculateAll(this Mesh mesh)
        {
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }

        #endregion
    }
}