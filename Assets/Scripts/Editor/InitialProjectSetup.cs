using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OneJob.Editor
{
    [InitializeOnLoad]
    internal static class InitialProjectSetup
    {
        private const string SettingsFolder = "Assets/Settings";
        private const string RendererPath = SettingsFolder + "/ONEJOB_UniversalRenderer.asset";
        private const string PipelinePath = SettingsFolder + "/ONEJOB_UniversalRenderPipeline.asset";

        static InitialProjectSetup()
        {
            EditorApplication.delayCall += EnsureUniversalRenderPipeline;
        }

        private static void EnsureUniversalRenderPipeline()
        {
            if (GraphicsSettings.defaultRenderPipeline is UniversalRenderPipelineAsset)
            {
                return;
            }

            EnsureSettingsFolder();

            UniversalRenderPipelineAsset pipelineAsset =
                AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelinePath);

            if (pipelineAsset == null)
            {
                UniversalRendererData rendererData =
                    AssetDatabase.LoadAssetAtPath<UniversalRendererData>(RendererPath);

                if (rendererData == null)
                {
                    rendererData = ScriptableObject.CreateInstance<UniversalRendererData>();
                    rendererData.name = "ONEJOB Universal Renderer";
                    AssetDatabase.CreateAsset(rendererData, RendererPath);
                }

                pipelineAsset = UniversalRenderPipelineAsset.Create(rendererData);
                pipelineAsset.name = "ONEJOB Universal Render Pipeline";
                pipelineAsset.useSRPBatcher = true;
                pipelineAsset.supportsHDR = true;
                pipelineAsset.shadowDistance = 40f;
                AssetDatabase.CreateAsset(pipelineAsset, PipelinePath);
            }

            GraphicsSettings.defaultRenderPipeline = pipelineAsset;
            QualitySettings.renderPipeline = pipelineAsset;
            PlayerSettings.colorSpace = ColorSpace.Linear;
            AssetDatabase.SaveAssets();

            Debug.Log("ONE JOB: created and assigned default URP settings.");
        }

        private static void EnsureSettingsFolder()
        {
            if (!AssetDatabase.IsValidFolder(SettingsFolder))
            {
                AssetDatabase.CreateFolder("Assets", "Settings");
            }
        }
    }
}
