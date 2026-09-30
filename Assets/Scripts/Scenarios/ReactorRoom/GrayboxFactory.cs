using UnityEngine;
using UnityEngine.Rendering;

namespace OneJob.Scenarios.ReactorRoom
{
    internal sealed class GrayboxFactory
    {
        private readonly Transform root;
        private readonly Shader litShader;

        public GrayboxFactory(Transform root)
        {
            this.root = root;
            litShader = FindLitShader();
        }

        public GameObject CreatePrimitive(
            PrimitiveType primitiveType,
            string objectName,
            Vector3 position,
            Vector3 scale,
            Color color,
            Transform parent = null)
        {
            GameObject instance = GameObject.CreatePrimitive(primitiveType);
            instance.name = objectName;
            instance.transform.SetParent(parent != null ? parent : root, false);
            instance.transform.localPosition = position;
            instance.transform.localScale = scale;

            Renderer objectRenderer = instance.GetComponent<Renderer>();
            if (objectRenderer != null && litShader != null)
            {
                Material material = new Material(litShader)
                {
                    name = $"{objectName} Graybox Material",
                    color = color
                };

                if (material.HasProperty("_BaseColor"))
                {
                    material.SetColor("_BaseColor", color);
                }

                objectRenderer.sharedMaterial = material;
            }

            return instance;
        }

        public Transform CreateEmpty(string objectName, Vector3 position, Transform parent = null)
        {
            GameObject instance = new GameObject(objectName);
            instance.transform.SetParent(parent != null ? parent : root, false);
            instance.transform.localPosition = position;
            return instance.transform;
        }

        public TextMesh CreateLabel(string text, Vector3 position, float characterSize = 0.14f, Transform parent = null)
        {
            Transform labelTransform = CreateEmpty($"Label - {text}", position, parent);
            TextMesh label = labelTransform.gameObject.AddComponent<TextMesh>();
            label.text = text;
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = characterSize;
            label.fontSize = 48;
            label.fontStyle = FontStyle.Bold;
            label.color = Color.white;
            return label;
        }

        public Light CreatePointLight(string objectName, Vector3 position, Color color, float range, float intensity)
        {
            Transform lightTransform = CreateEmpty(objectName, position);
            Light light = lightTransform.gameObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.range = range;
            light.intensity = intensity;
            light.shadows = LightShadows.None;
            return light;
        }

        private static Shader FindLitShader()
        {
            if (GraphicsSettings.currentRenderPipeline != null)
            {
                Shader urpShader = Shader.Find("Universal Render Pipeline/Lit");
                if (urpShader != null)
                {
                    return urpShader;
                }
            }

            return Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit");
        }
    }
}
