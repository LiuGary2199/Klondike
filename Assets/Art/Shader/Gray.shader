Shader "Gray" 
{
    Properties
    {
        _MainTex ("纹理", 2D) = "white" {} // 定义了主贴图
        _Color ("颜色", Color) = (1,1,1,1) // 定义了颜色调整，默认是白色不做调整
    }

    SubShader 
    {
        Tags 
        { 
            "Queue"="Transparent" // 渲染队列设置为透明，保证透明度能正确处理
            "IgnoreProjector"="True" // 不接受投影仪效果
            "RenderType"="Transparent" // 渲染类型为透明
            "PreviewType"="Plane" // 预览时的对象类型为平面
            "CanUseSpriteAtlas"="True" // 可以使用精灵图集
        }

        Cull Off // 不进行面剔除，正面和背面都渲染
        Lighting Off // 关闭光照效果
        ZWrite Off // 不写入深度缓冲区
        ZTest [unity_GUIZTestMode] // ZTest模式为默认的GUI测试模式
        Blend SrcAlpha OneMinusSrcAlpha // 混合模式，适用于透明效果
        ColorMask RGBA // 颜色蒙版，写入所有通道

        Pass 
        {
            Name "Default" 
            CGPROGRAM 
            // 指定编译指标
            #pragma vertex vert // 指定顶点着色器函数
            #pragma fragment frag // 指定片元着色器函数
            #pragma target 2.0 // 指定Shader模型

            #include "UnityCG.cginc" // 包含Unity的CG包
            #include "UnityUI.cginc" // 包含Unity的UI CG包

            struct appdata_t 
            {
                float4 vertex : POSITION; // 顶点位置
                float4 color : COLOR; // 顶点颜色
                float2 texcoord : TEXCOORD0; // 纹理坐标
            };

            struct v2f 
            {
                float4 vertex : SV_POSITION; // 裁剪空间下的顶点位置
                fixed4 color : COLOR; // 顶点颜色
                float2 texcoord : TEXCOORD0; // 纹理坐标
            };

            sampler2D _MainTex; // 纹理采样器
            fixed4 _Color; // Shader属性-颜色

            // 顶点着色器
            v2f vert(appdata_t v) 
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(v.vertex); // 计算顶点在裁剪空间的位置
                OUT.texcoord = v.texcoord; // 传递纹理坐标
                OUT.color = v.color * _Color; // 计算顶点的最终颜色
                return OUT;
            }

            // 片元着色器
            fixed4 frag(v2f IN) : SV_Target 
            {
                fixed4 color = tex2D(_MainTex, IN.texcoord) * IN.color; // 采样纹理并乘以顶点颜色
                float gray = dot(color.rgb, float3(0.299, 0.587, 0.114)); // RGB转灰度值
                color.rgb = gray; // 将灰度值赋值给RGB，实现置灰效果
                return color; // 返回最终颜色
            }
            ENDCG 
        }
    }
    FallBack "Diffuse" // 如果Shader渲染失败，则回退到Diffuse Shader
}