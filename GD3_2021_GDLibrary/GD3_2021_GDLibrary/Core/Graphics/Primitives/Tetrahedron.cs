using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary.Graphics
{
    /// <summary>
    /// Defines a textured 1x1x1 cube, centred on origin, aligned with XYZ axis
    /// </summary>
    public class Tetrahedron : Mesh, ICloneable
    {
       

        protected override void CreateGeometry()
        {
            Vector3 backLeft = new Vector3(-0.5f, 0, -0.5f);
            Vector3 backRight = new Vector3(0.5f, 0, -0.5f);

            Vector3 frontLeft = new Vector3(-0.5f, 0, 0.5f);
            Vector3 frontRight = new Vector3(0.5f, 0, 0.5f);

            Vector3 topPoint = new Vector3(0, 1f, 0);


            Vector2 backLeftUV = new Vector2(0f, 0f);
            Vector2 backRightUV = new Vector2(0f, 1f);
            Vector2 frontLeftUV = new Vector2(1f, 0f);
            Vector2 frontRightUV = new Vector2(1f, 1f);
            Vector2 topUV = new Vector2(0.5f, 1f);

            Vector3 frontNormal = new Vector3(0, 0, 1);
            Vector3 backNormal = new Vector3(0, 0, -1);
            Vector3 leftNormal = new Vector3(-1, 0, 0);
            Vector3 rightNormal = new Vector3(1, 0, 0);
            Vector3 topNormal = new Vector3(0, 1, 0);
            Vector3 bottomNormal = new Vector3(0, -1, 0);

            vertices = new VertexPositionNormalTexture[]
           {
               // Bottom Surface
                new VertexPositionNormalTexture(backLeft,bottomNormal,backLeftUV),
                new VertexPositionNormalTexture(frontLeft,bottomNormal,frontLeftUV),
                new VertexPositionNormalTexture(backRight,bottomNormal,backRightUV),
                new VertexPositionNormalTexture(frontRight,bottomNormal,frontRightUV),

                // Front face
                new VertexPositionNormalTexture(frontLeft, frontNormal, frontLeftUV),
                new VertexPositionNormalTexture(topPoint, frontNormal, topUV),
                new VertexPositionNormalTexture(frontRight, frontNormal, frontRightUV),
                

               // Left face
                new VertexPositionNormalTexture(frontLeft, leftNormal, frontLeftUV),
                new VertexPositionNormalTexture(topPoint, leftNormal, topUV),
                new VertexPositionNormalTexture(backLeft, leftNormal, backLeftUV),

                // Back face
                new VertexPositionNormalTexture(backLeft, backNormal, backLeftUV),
                new VertexPositionNormalTexture(topPoint, backNormal, topUV),
                new VertexPositionNormalTexture(backRight, backNormal, backRightUV),

                // Right face
                new VertexPositionNormalTexture(frontRight, rightNormal, frontRightUV),
                new VertexPositionNormalTexture(topPoint, rightNormal, topUV),
                new VertexPositionNormalTexture(backRight, rightNormal, backRightUV)
           };


            indices = new ushort[]
            {
                //0, 1, 2, 2, 1, 3,
                //0, 1, 4,
                //0, 2, 4,
                //2, 3, 4,
                //1, 3, 4

                //1, 2, 3, 3, 2, 4,
                //1, 5, 2,
                //1, 5, 3,
                //3, 5, 4,
                //2, 5, 4

                0, 1, 2, 2, 1, 3,
                1, 5, 3,
                2, 5, 0,
                0, 5, 1,
                3, 5, 2
            };

       }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
