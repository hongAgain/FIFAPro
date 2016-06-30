using UnityEngine;

[ExecuteInEditMode]
public class UISpritePolygon : UISprite
{
    [RangeAttribute(3, 100)]
    public int mEdge = 3;

    public float[] mRate;

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture tex = mainTexture;
        if (tex == null) return;

        if (mSprite == null) mSprite = atlas.GetSprite(spriteName);
        if (mSprite == null) return;

        Rect outer = new Rect(mSprite.x, mSprite.y, mSprite.width, mSprite.height);
        Rect inner = new Rect(mSprite.x + mSprite.borderLeft, mSprite.y + mSprite.borderTop,
            mSprite.width - mSprite.borderLeft - mSprite.borderRight,
            mSprite.height - mSprite.borderBottom - mSprite.borderTop);

        outer = NGUIMath.ConvertToTexCoords(outer, tex.width, tex.height);
        inner = NGUIMath.ConvertToTexCoords(inner, tex.width, tex.height);

        Vector3 outterLength = Vector3.up * mWidth / 2;
        Vector3 innerLength = Vector3.up * mHeight / 2;
        float angle = 360f / mEdge * Mathf.Deg2Rad;
        for (int i = 0; i < mEdge; ++i)
        {
            verts.Add(Vector3.zero);

            Vector3 v = innerLength * Rate(i);
            Vector3 v1 = Rotate(v, angle * i);
            verts.Add(v1);

            v = innerLength * Rate(i + 1);
            Vector3 v2 = Rotate(v, angle * (i + 1));
            verts.Add(v2);

            verts.Add(Vector3.zero);

            v = outterLength * Rate(i);
            Vector3 v3 = Rotate(v, angle * i);
            verts.Add(v3);

            v = outterLength * Rate(i + 1);
            Vector3 v4 = Rotate(v, angle * (i + 1));
            verts.Add(v4);

            verts.Add(v2);
            verts.Add(v1);
        }

        //var u = Vector4.zero;
        //switch (mFlip)
        //{
        //    case Flip.Horizontally:
        //        u = new Vector4(outer.xMax, outer.yMin, outer.xMin, outer.yMax);
        //        break;
        //    case Flip.Vertically:
        //        u = new Vector4(outer.xMin, outer.yMax, outer.xMax, outer.yMin);
        //        break;
        //    case Flip.Both:
        //        u = new Vector4(outer.xMax, outer.yMax, outer.xMin, outer.yMin);
        //        break;
        //    default:
        //        u = new Vector4(outer.xMin, outer.yMin, outer.xMax, outer.yMax);
        //        break;
        //};

        for (int i = 0; i < mEdge; ++i)
        {
            //uvs.Add(new Vector2(u.x, u.y));
            //uvs.Add(new Vector2(u.x, u.y));
            //uvs.Add(new Vector2(u.x, u.y));
            //uvs.Add(new Vector2(u.x, u.y));

            //uvs.Add(new Vector2(u.z, u.w));
            //uvs.Add(new Vector2(u.z, u.w));

            //uvs.Add(new Vector2(u.z, u.w) * mHeight * 1f / mWidth);
            //uvs.Add(new Vector2(u.z, u.w) * mHeight * 1f / mWidth);

            uvs.Add(outer.min);
            uvs.Add(outer.max * mHeight * 1f / mWidth);
            uvs.Add(outer.max * mHeight * 1f / mWidth);
            uvs.Add(outer.min);

            uvs.Add(outer.max);
            uvs.Add(outer.max);

            uvs.Add(outer.max * mHeight * 1f / mWidth);
            uvs.Add(outer.max * mHeight * 1f / mWidth);
        }

        for (int i = 0; i < mEdge; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                cols.Add(mColor);
            }
            cols.Add(new Color(0f, 0f, 0f));

            for (int j = 0; j < 4; ++j)
            {
                cols.Add(mColor);
            }
        }
    }

    private float Rate(int i)
    {
        if (mRate != null)
        {
            if (i < mRate.Length)
            {
                return Mathf.Clamp01(mRate[i]);
            }
            else
            {
                return Mathf.Clamp01(mRate[0]);
            }
        }
        else
        {
            return 1f;
        }
    }

    private static Vector3 Rotate(Vector3 v, float angle)
    {
        Vector3 rotated = Vector3.zero;
        rotated.y = v.y * Mathf.Cos(angle) - v.x * Mathf.Sin(angle);
        rotated.x = v.y * Mathf.Sin(angle) + v.x * Mathf.Cos(angle);

        return rotated;
    }
}