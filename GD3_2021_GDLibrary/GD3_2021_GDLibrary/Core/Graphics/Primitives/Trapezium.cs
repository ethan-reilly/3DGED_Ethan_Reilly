using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary.Graphics
{
    /// <summary>
    /// Defines a textured 1x1x1 Trapezium, centred on origin, aligned with XYZ axis
    /// </summary>
    public class Trapezium : Mesh, ICloneable
    {
        protected override void CreateGeometry()
        {
            Vector3 TFrontLeft = new Vector3(-0.75f, 0.5f, 0.35f);
            Vector3 BFrontLeft = new Vector3(-0.75f, -0.5f, 0.35f);

            Vector3 TFrontRight = new Vector3(0.75f, 0.5f, 0.35f);
            Vector3 BFrontRight = new Vector3(0.75f, -0.5f, 0.35f);


            Vector3 TBackLeft = new Vector3(-0.25f, 0.5f, -0.15f);
            Vector3 BBackLeft = new Vector3(-0.25f, -0.5f, -0.15f);

            Vector3 TBackRight = new Vector3(0.25f, 0.5f, -0.15f);
            Vector3 BBackRight = new Vector3(0.25f, -0.5f, -0.15f);


            #region UVs

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

            #endregion UVs


            #region Normals

            Vector3 frontNormal = new Vector3(0, 0, 1);
            Vector3 backNormal = new Vector3(0, 0, -1);
            Vector3 leftNormal = new Vector3(-1, 0, 0);
            Vector3 rightNormal = new Vector3(1, 0, 0);
            Vector3 topNormal = new Vector3(0, 1, 0);
            Vector3 bottomNormal = new Vector3(0, -1, 0);

            #endregion Normals

            vertices = new VertexPositionNormalTexture[]
            {
                //Front Surface
                new VertexPositionNormalTexture(BFrontLeft,frontNormal,frontBottomLeft),
                new VertexPositionNormalTexture(TFrontLeft ,frontNormal,frontTopLeft),
                new VertexPositionNormalTexture(BFrontRight,frontNormal,frontBottomRight),
                new VertexPositionNormalTexture(TFrontRight,frontNormal,frontTopRight),

                //Back Surface
                new VertexPositionNormalTexture(BBackRight,backNormal,backBottomLeft),
                new VertexPositionNormalTexture(TBackRight,backNormal,backTopLeft),
                new VertexPositionNormalTexture(BBackLeft,backNormal,backBottomRight),
                new VertexPositionNormalTexture(TBackLeft,backNormal,backTopRight),

                //Left Surface
                 new VertexPositionNormalTexture(BBackLeft,leftNormal,frontBottomLeft),
                new VertexPositionNormalTexture(TBackLeft,leftNormal,frontTopLeft),
                new VertexPositionNormalTexture(TFrontLeft,leftNormal,frontBottomRight),
                new VertexPositionNormalTexture(BFrontLeft,leftNormal,frontTopRight),

                //Right Surface
                 new VertexPositionNormalTexture(BBackRight,rightNormal,frontBottomRight),
                new VertexPositionNormalTexture(TBackRight,rightNormal,frontTopRight),
                new VertexPositionNormalTexture(TFrontRight,rightNormal,frontBottomLeft),
                new VertexPositionNormalTexture(BFrontRight,rightNormal,frontTopLeft),

                //Top Surface
                new VertexPositionNormalTexture(TFrontLeft,topNormal,TtopLeftFront),
                new VertexPositionNormalTexture(TBackLeft,topNormal,TtopLeftBack),
                new VertexPositionNormalTexture(TFrontRight,topNormal,TtopRightFront),
                new VertexPositionNormalTexture(TBackRight,topNormal,TtopRightBack),

                //Bottom Surface
                new VertexPositionNormalTexture(BBackLeft,bottomNormal,TbottomRightBack),
                new VertexPositionNormalTexture(BFrontLeft,bottomNormal,TbottomRightFront),
                new VertexPositionNormalTexture(BBackRight,bottomNormal,TbottomLeftBack),
                new VertexPositionNormalTexture(BFrontRight,bottomNormal,TbottomLeftFront)

            };

            indices = new ushort[] {
                0, 1, 2, 2, 1, 3,
                4, 5, 6, 6, 5, 7,
                8, 9, 10, 8, 10, 11,
                15, 13, 12, 14, 13, 15,
                16, 17, 18, 18, 17, 19,
                20, 21, 22, 22, 21, 23

            };
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }

       
    }
}
