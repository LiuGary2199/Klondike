using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// UGUI描边
/// </summary>
public class MossMistake : BaseMeshEffect
{
    public Color OutlineColor = Color.white;
    [Range(0, 6)]
    public float OutlineWidth = 2;
    private static List<UIVertex> m_MercyTrip= new List<UIVertex>();

    protected override void Start()
    {
        base.Start();
        var shader = Shader.Find("Outline");
        base.graphic.material = new Material(shader);
        var v1 = base.graphic.canvas.additionalShaderChannels;
        var v2 = AdditionalCanvasShaderChannels.TexCoord1;
        if ((v1 & v2) != v2)
            base.graphic.canvas.additionalShaderChannels |= v2;
        v2 = AdditionalCanvasShaderChannels.TexCoord2;
        if ((v1 & v2) != v2)
            base.graphic.canvas.additionalShaderChannels |= v2;
        this.Balcony();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if (base.graphic.material != null)
            this.Balcony();
    }
#endif

    public void Balcony()
    {
        base.graphic.material.SetColor("_OutlineColor", this.OutlineColor);
        base.graphic.material.SetFloat("_OutlineWidth", this.OutlineWidth);
        base.graphic.SetVerticesDirty();
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        vh.GetUIVertexStream(m_MercyTrip);
        this._HemlockMolecule();
        vh.Clear();
        vh.AddUIVertexTriangleStream(m_MercyTrip);
    }

    private void _HemlockMolecule()
    {
        for (int i = 0, count = m_MercyTrip.Count - 3; i <= count; i += 3)
        {
            var v1 = m_MercyTrip[i];
            var v2 = m_MercyTrip[i + 1];
            var v3 = m_MercyTrip[i + 2];
            // 计算原顶点坐标中心点
            var minX = _Bay(v1.position.x, v2.position.x, v3.position.x);
            var minY = _Bay(v1.position.y, v2.position.y, v3.position.y);
            var maxX = _Gel(v1.position.x, v2.position.x, v3.position.x);
            var maxY = _Gel(v1.position.y, v2.position.y, v3.position.y);
            var posCenter = new Vector2(minX + maxX, minY + maxY) * 0.5f;
            // 计算原始顶点坐标和UV的方向
            Vector2 triX, triY, uvX, uvY;
            Vector2 pos1 = v1.position;
            Vector2 pos2 = v2.position;
            Vector2 pos3 = v3.position;
            if (Mathf.Abs(Vector2.Dot((pos2 - pos1).normalized, Vector2.right))
                > Mathf.Abs(Vector2.Dot((pos3 - pos2).normalized, Vector2.right)))
            {
                triX = pos2 - pos1;
                triY = pos3 - pos2;
                uvX = v2.uv0 - v1.uv0;
                uvY = v3.uv0 - v2.uv0;
            }
            else
            {
                triX = pos3 - pos2;
                triY = pos2 - pos1;
                uvX = v3.uv0 - v2.uv0;
                uvY = v2.uv0 - v1.uv0;
            }
            // 计算原始UV框
            var uvMin = _Bay(v1.uv0, v2.uv0, v3.uv0);
            var uvMax = _Gel(v1.uv0, v2.uv0, v3.uv0);
            var uvOrigin = new Vector4(uvMin.x, uvMin.y, uvMax.x, uvMax.y);
            // 为每个顶点设置新的Position和UV，并传入原始UV框
            v1 = _CudGunSumBusUV(v1, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvOrigin);
            v2 = _CudGunSumBusUV(v2, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvOrigin);
            v3 = _CudGunSumBusUV(v3, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvOrigin);
            // 应用设置后的UIVertex
            m_MercyTrip[i] = v1;
            m_MercyTrip[i + 1] = v2;
            m_MercyTrip[i + 2] = v3;
        }
    }

    private static UIVertex _CudGunSumBusUV(UIVertex pVertex, float pOutLineWidth,
        Vector2 pPosCenter,
        Vector2 pTriangleX, Vector2 pTriangleY,
        Vector2 pUVX, Vector2 pUVY,
        Vector4 pUVOrigin)
    {
        // Position
        var pos = pVertex.position;
        var posXOffset = pos.x > pPosCenter.x ? pOutLineWidth : -pOutLineWidth;
        var posYOffset = pos.y > pPosCenter.y ? pOutLineWidth : -pOutLineWidth;
        pos.x += posXOffset;
        pos.y += posYOffset;
        pVertex.position = pos;
        // UV
        var uv = pVertex.uv0;
        uv += (Vector4)pUVX / pTriangleX.magnitude * posXOffset * (Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1);
        uv += (Vector4)pUVY / pTriangleY.magnitude * posYOffset * (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1);
        pVertex.uv0 = uv;
        // 原始UV框
        pVertex.uv1 = new Vector2(pUVOrigin.x, pUVOrigin.y);
        pVertex.uv2 = new Vector2(pUVOrigin.z, pUVOrigin.w);
        return pVertex;
    }
    private static float _Bay(float pA, float pB, float pC)
    {
        return Mathf.Min(Mathf.Min(pA, pB), pC);
    }
    private static float _Gel(float pA, float pB, float pC)
    {
        return Mathf.Max(Mathf.Max(pA, pB), pC);
    }
    private static Vector2 _Bay(Vector2 pA, Vector2 pB, Vector2 pC)
    {
        return new Vector2(_Bay(pA.x, pB.x, pC.x), _Bay(pA.y, pB.y, pC.y));
    }
    private static Vector2 _Gel(Vector2 pA, Vector2 pB, Vector2 pC)
    {
        return new Vector2(_Gel(pA.x, pB.x, pC.x), _Gel(pA.y, pB.y, pC.y));
    }
}
