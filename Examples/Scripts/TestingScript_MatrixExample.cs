using UnityEngine;
using CXUtils.CodeUtils;
using CXUtils.GridSystem;
namespace CXUtils.Test
{
    public class TestingScript_MatrixExample : MonoBehaviour
    {
#pragma warning disable IDE0044
        Matrix newMatrix = new Matrix(5, 10);
        CXGrid<float> newMatrixGrid = default;
        [SerializeField] private Transform textsTrans = default;

        private void Start()
        {
            newMatrix.Map((x, y) => (y * newMatrix.Height + x + 1));
            NewMatrixGrid();
            newMatrixGrid.DrawText(textsTrans, 50);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                newMatrix.Transpose();
                NewMatrixGrid();

                for (int i = 0; i < textsTrans.childCount; i++)
                    Destroy(textsTrans.GetChild(i).gameObject);

                newMatrixGrid.DrawText(textsTrans, 50);
            }
        }

        private void OnDrawGizmos() =>
            Init();

        #region Script Methods

        private void Init()
        {
            NewMatrixGrid();
            newMatrixGrid.DrawDebug();
        }

        private void NewMatrixGrid() =>
            newMatrixGrid = new CXGrid<float>(newMatrix.Width, newMatrix.Height, 10f, Vector3.zero, (x, y) => (newMatrix[x, y]));

        #endregion
    }
}