using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;

public class PaniniProjectionFeature : ScriptableRendererFeature
{
    class PassData
    {
        public TextureHandle source;
        public Material material;
    }

    public Material material;

    class PaniniPass : ScriptableRenderPass
    {
        private Material material;

        public PaniniPass(Material material)
        {
            this.material = material;
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

            TextureHandle source = resourceData.activeColorTexture;

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("Panini Projection", out var passData))
            {
                passData.source = source;
                passData.material = material;

                builder.UseTexture(passData.source);
                builder.SetRenderAttachment(passData.source, 0);

                builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
                {
                    Blitter.BlitTexture(
                        context.cmd,
                        data.source,
                        new Vector4(1, 1, 0, 0),
                        data.material,
                        0
                    );
                });
            }
        }
    }

    PaniniPass pass;

    public override void Create()
    {
        pass = new PaniniPass(material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}