using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Outbreak
{
    public class OBB
    {
        Vector3 center;
        Vector3[] sideDirection;
        float[] sideHalfLength;

        public OBB()
        {
            center = Vector3.Zero;
            sideDirection = new Vector3[3];
            sideHalfLength = new float[3];

            for (int i = 0; i < 3; i++)
            {
                sideDirection[i] = Vector3.Zero;
                sideHalfLength[i] = 0;
            }
        }

        public static OBB CreateFromAABB(BoundingBox box)
        {
            OBB obb = new OBB();

            obb.center = (box.Max - box.Min) / 2 + box.Min;

            obb.sideDirection[0] = box.Max.X * Vector3.UnitX;
            obb.sideDirection[1] = box.Max.Y * Vector3.UnitY;
            obb.sideDirection[2] = box.Max.Z * Vector3.UnitZ;

            return obb;
        }

        public static OBB Translate(OBB obb, Matrix translation)
        {
            obb.center = Vector3.Transform(obb.center, translation);

            return obb;
        }

        public static OBB Rotate(OBB obb, Matrix rotation)
        {
            for (int i = 0; i < 3; i++)
            {
                obb.sideDirection[i] = Vector3.Transform(obb.sideDirection[i], rotation);
                obb.sideDirection[i].Normalize();
            }

            return obb;
        }
    }
}
