using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary.Graphics
{
    /// <summary>
    /// Defines a textured 1x1x1 hexagon, centred on origin, aligned with XYZ axis
    /// </summary>
    public class HexagonMesh : Mesh, ICloneable
    {
        protected override void CreateGeometry()
        {
            #region positions
            Vector3 topFrontMiddle = new Vector3(0.5f, 0.5f, 0);
            Vector3 bottomFrontMiddle = new Vector3(0.5f, -0.5f, 0);

            Vector3 topBackMiddle = new Vector3(-0.5f, 0.5f, 0);
            Vector3 bottomBackMiddle = new Vector3(-0.5f, -0.5f, 0);

            Vector3 topLeftFront = new Vector3(0.25f, 0.5f, -0.25f);
            Vector3 bottomLeftFront = new Vector3(0.25f, -0.5f, -0.25f);

            Vector3 topLeftBack = new Vector3(-0.25f, 0.5f, -0.25f);
            Vector3 bottomLeftBack = new Vector3(-0.25f, -0.5f, -0.25f);

            Vector3 topRightFront = new Vector3(-0.25f, 0.5f, -0.25f);
            Vector3 bottomRightFront = new Vector3(-0.25f, -0.5f, -0.25f);

            Vector3 topRightBack = new Vector3(-0.25f, 0.5f, -0.25f);
            Vector3 bottomRightBack = new Vector3(-0.25f, -0.5f, -0.25f);
            #endregion

            #region UVs
            /*
            Vector2 TtopLeftBack = new Vector2(0.0f, 0.0f);
            Vector2 TtopRightBack = new Vector2(1.0f, 0.0f);
            Vector2 TtopLeftFront = new Vector2(0.0f, 1.0f);
            Vector2 TtopRightFront = new Vector2(1.0f, 1.0f);

            Vector2 TbottomLeftBack = new Vector2(1.0f, 1.0f);
            Vector2 TbottomLeftFront = new Vector2(0.0f, 1.0f);
            Vector2 TbottomRightBack = new Vector2(1.0f, 0.0f);
            Vector2 TbottomRightFront = new Vector2(0.0f, 0.0f);

            Vector2 frontTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 frontTopRight = new Vector2(1.0f, 0.0f);
            Vector2 frontBottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 frontBottomRight = new Vector2(1.0f, 1.0f);

            Vector2 backTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 backTopRight = new Vector2(1.0f, 0.0f);
            Vector2 backBottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 backBottomRight = new Vector2(1.0f, 1.0f);

            Vector2 rightTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 rightTopRight = new Vector2(1.0f, 0.0f);
            Vector2 rightBottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 rightBottomRight = new Vector2(1.0f, 1.0f);

            Vector2 leftTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 leftTopRight = new Vector2(1.0f, 0.0f);
            Vector2 leftBottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 leftBottomRight = new Vector2(1.0f, 1.0f);
            */
            #endregion UVs


            #region Hexagon UV

            Vector2 frontBottomUV = new Vector2(0f, 0f);
            Vector2 frontTopUV = new Vector2(0f, 1f);

            Vector2 rectangleBottomUV = new Vector2(2f, 0f);
            Vector2 rectangleTopUV = new Vector2(2f, 1f);

            Vector2 squareBottomUV = new Vector2(1f, 0f);
            Vector2 squareTopUV = new Vector2(1f, 1f);

            Vector2 topOneUV = new Vector2(1f, .25f);
            Vector2 topTwoUV = new Vector2(1f, .75f);

            #endregion


            #region Normals

            Vector3 frontNormal = new Vector3(0, 0, 1);
            Vector3 backNormal = new Vector3(0, 0, -1);
            Vector3 frontLeftNormal = new Vector3(0.5f, 0, 0.5f);
            Vector3 backLeftNormal = new Vector3(0.5f, 0, -0.5f);

            Vector3 frontRightNormal = new Vector3(0.5f, 0, -0.5f);
            Vector3 backRightNormal = new Vector3(-0.5f, 0, -0.5f);

            Vector3 rightNormal = new Vector3(-1, 0, 0);
            Vector3 leftNormal = new Vector3(1, 0, 0);

            Vector3 topNormal = new Vector3(0, 1, 0);
            Vector3 bottomNormal = new Vector3(0, -1, 0);

            #endregion Normals

            //Vector2 x = new Vector2(0, 0);
            vertices = new VertexPositionNormalTexture[]
            {
                
                // Front Right Surface
                new VertexPositionNormalTexture(bottomRightFront,frontRightNormal,rectangleBottomUV),
                new VertexPositionNormalTexture(topRightFront ,frontRightNormal,rectangleTopUV),
                new VertexPositionNormalTexture(topFrontMiddle,frontRightNormal,frontTopUV),
                new VertexPositionNormalTexture(bottomFrontMiddle,frontRightNormal,frontBottomUV),
                
                // Front Left Surface
                new VertexPositionNormalTexture(bottomLeftFront,frontLeftNormal,rectangleBottomUV),
                new VertexPositionNormalTexture(topLeftFront ,frontLeftNormal,rectangleTopUV),
                new VertexPositionNormalTexture(topFrontMiddle,frontLeftNormal,frontTopUV),
                new VertexPositionNormalTexture(bottomFrontMiddle,frontLeftNormal,frontBottomUV),

                // Back Right Surface
                new VertexPositionNormalTexture(bottomRightBack,backRightNormal,rectangleBottomUV),
                new VertexPositionNormalTexture(topRightBack,backRightNormal,rectangleTopUV),
                new VertexPositionNormalTexture(topBackMiddle,backRightNormal,frontTopUV),
                new VertexPositionNormalTexture(bottomBackMiddle,backRightNormal,frontBottomUV),

                 // Back Left Surface
                new VertexPositionNormalTexture(bottomLeftBack,backLeftNormal,rectangleBottomUV),
                new VertexPositionNormalTexture(topLeftBack,backLeftNormal,rectangleTopUV),
                new VertexPositionNormalTexture(topBackMiddle,backLeftNormal,frontTopUV),
                new VertexPositionNormalTexture(bottomBackMiddle,backLeftNormal,frontBottomUV),

                // Right Surface
                new VertexPositionNormalTexture(bottomRightFront,rightNormal,frontBottomUV),
                new VertexPositionNormalTexture(topRightFront,rightNormal,frontTopUV),
                new VertexPositionNormalTexture(topRightBack,rightNormal,squareTopUV),
                new VertexPositionNormalTexture(bottomRightBack,rightNormal,squareBottomUV),


                // Left Surface
                new VertexPositionNormalTexture(bottomLeftFront,leftNormal,frontBottomUV),
                new VertexPositionNormalTexture(topLeftFront,leftNormal,frontTopUV),
                new VertexPositionNormalTexture(topLeftBack,leftNormal,squareTopUV),
                new VertexPositionNormalTexture(bottomLeftBack,leftNormal,squareBottomUV),

                // Top Surface - Right
                new VertexPositionNormalTexture(topBackMiddle,topNormal,frontBottomUV),
                new VertexPositionNormalTexture(topRightBack,topNormal,topTwoUV),
                new VertexPositionNormalTexture(topRightFront,topNormal,topOneUV),
                new VertexPositionNormalTexture(topFrontMiddle,topNormal,frontTopUV),

                // Top Surface - Left
                new VertexPositionNormalTexture(topBackMiddle,topNormal,frontBottomUV),
                new VertexPositionNormalTexture(topLeftBack,topNormal, topTwoUV),
                new VertexPositionNormalTexture(topLeftFront,topNormal, topOneUV),
                new VertexPositionNormalTexture(topFrontMiddle,topNormal,frontTopUV),

                // Bottom Surface - Right
                new VertexPositionNormalTexture(bottomBackMiddle,bottomNormal,frontBottomUV),
                new VertexPositionNormalTexture(bottomRightBack,bottomNormal,topTwoUV),
                new VertexPositionNormalTexture(bottomRightFront,bottomNormal,topOneUV),
                new VertexPositionNormalTexture(bottomFrontMiddle,bottomNormal,frontTopUV),

                // Bottom Surface - Left
                new VertexPositionNormalTexture(bottomBackMiddle,bottomNormal,frontBottomUV),
                new VertexPositionNormalTexture(bottomLeftBack,bottomNormal,topTwoUV),
                new VertexPositionNormalTexture(bottomLeftFront,bottomNormal,topOneUV),
                new VertexPositionNormalTexture(bottomFrontMiddle,bottomNormal,frontTopUV),

            };

            indices = new ushort[] {
                //0, 1, 2, 2, 1, 3,
                //2, 3, 4, 4, 5, 3,
                //8, 9, 10, 10, 9, 11,
                //12, 13, 14, 14, 13, 15,
                //16, 17, 18, 18, 17, 19,
                //20, 21, 22, 22, 21, 23
               
                0, 1, 2, 2, 1, 3,
                2, 3, 4, 4, 5, 3,
                4, 5, 6, 6, 7, 5,
                6, 7, 8, 8, 7, 9,
                8, 9, 10, 10, 9, 11,
                10, 11, 0, 0, 1, 11
            };
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }

        
    }
}
