using UnityEngine;
using System;

namespace CXUtils.CXCamera
{
    ///<summary> Cx's Camera Class </summary>
    class CXCamera
    {
        ///<summary> This determines camera's borders </summary>
        public enum Port
        { LeftUp, LeftDown, RightUp, RightDown, LeftMiddle, RightMiddle, UpMiddle, DownMiddle }
        #region MousePosition
        ///<summary> This method will get the mouse position on the scene position on the camera </summary>
        public static Vector3 GetMouseOnScenePos(Camera camera) => camera.ScreenToWorldPoint(Input.mousePosition);

        ///<summary> This method will get the mouse position on the viewport pos on the camera </summary>
        public static Vector3 GetMouseOnViewPortPos(Camera camera) => camera.ScreenToViewportPoint(Input.mousePosition);
        #endregion


        #region CameraOtherHelperMethods
        ///<summary> This method will get the edges of the camera and return the edge camera pos </summary>
        public static Vector3 GetCameraEdgePosOnWorldPos(Camera camera, Port port)
        {
            #region postions
            //positions
            Vector2 LU = new Vector2(0, camera.pixelHeight);
            Vector2 LD = new Vector2(0, 0);
            Vector2 RU = new Vector2(camera.pixelWidth, camera.pixelHeight);
            Vector2 RD = new Vector2(camera.pixelWidth, 0);
            //middles
            Vector2 MU = new Vector2(camera.pixelWidth / 2, camera.pixelHeight);
            Vector2 MD = new Vector2(camera.pixelWidth / 2, 0);
            Vector2 ML = new Vector2(0, camera.pixelHeight / 2);
            Vector2 MR = new Vector2(camera.pixelWidth, camera.pixelHeight / 2);
            #endregion

            switch (port)
            {
                case Port.LeftUp:
                return camera.ScreenToWorldPoint(LU);
                case Port.LeftDown:
                return camera.ScreenToWorldPoint(LD);
                case Port.RightUp:
                return camera.ScreenToWorldPoint(RU);
                case Port.RightDown:
                return camera.ScreenToWorldPoint(RD);
                case Port.UpMiddle:
                return camera.ScreenToWorldPoint(MU);
                case Port.DownMiddle:
                return camera.ScreenToWorldPoint(MD);
                case Port.LeftMiddle:
                return camera.ScreenToWorldPoint(ML);
                case Port.RightMiddle:
                return camera.ScreenToWorldPoint(MR);
                default:
                return default;
            }
        }

        ///<summary> Get's the Vector2 border in world space </summary>
        public static Vector2 GetCameraBounds_Vec2_Ortho(Camera camera)
        {
            if (!camera.orthographic)
                throw new Exception($"{camera.transform.name} is not orthographic! please turn on orthographic in order to use this method!");

            //getting the border of the real world space
            Vector2 BorderPositive = new Vector2(GetCameraEdgePosOnWorldPos(camera, Port.RightMiddle).x, GetCameraEdgePosOnWorldPos(camera, Port.UpMiddle).y);
            return BorderPositive - (Vector2)camera.transform.position;
        }

        ///<summary> Get's the border in world space </summary>
        public static Bounds GetCameraBorders_Ortho(Camera camera) =>
            new Bounds(camera.transform.position, GetCameraBounds_Vec2_Ortho(camera) * 2);

        #endregion

    }
}