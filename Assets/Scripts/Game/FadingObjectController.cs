using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingObjectController : MonoBehaviour
{
    [SerializeField]
    private float _fadeOutDuration = 0.2f;
    [SerializeField]
    private float _fadeAlpha = 0.5f;

    private List<Renderer> _renderers;
    private List<Material> _materials;
    private bool _fadedOut;
    private float _lastFadedOutTime;

    private void Awake()
    {
        _renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        _materials = new List<Material>();
        foreach (Renderer renderer in _renderers)
        {
            _materials.AddRange(renderer.materials);
        }

        foreach (Material material in _materials)
        {
            if (material.HasProperty("_Color"))
            {
                material.color = new Color(
                    material.color.r, material.color.g, material.color.b, _fadeAlpha);
            }
        }
    }

    private void OnEnable()
    {
        _fadedOut = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_fadedOut && Time.time - _lastFadedOutTime > _fadeOutDuration)
        {
            _fadedOut = false;
            foreach (Material material in _materials)
            {
                fadeInMaterial(material);
            }
        }
    }

    /*
     * Originally from SetupMaterialWithBlendMode
     * https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Inspector/StandardShaderGUI.cs
     */
    private void fadeInMaterial(Material material)
    {
        material.SetOverrideTag("RenderType", "");
        material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
        material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
        material.SetFloat("_ZWrite", 1.0f);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    }

    private void fadeOutMaterial(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetFloat("_ZWrite", 0.0f);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    public void FadeOut(float fadeAlpha)
    {
        _lastFadedOutTime = Time.time;
        if (_fadedOut)
        {
            return;
        }
        _fadedOut = true;
        foreach (Material material in _materials)
        {
            fadeOutMaterial(material);
/*
            material.SetInt(
                "_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt(
                "_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            if (false)
            {
                material.EnableKeyword("_ALPHABLEND_ON");
            }
            else
            {
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            if (material.HasProperty("_Color"))
            {
                material.color = new Color(
                    material.color.r, material.color.g, material.color.b, fadeAlpha);
            }
*/
        }
    }
}
