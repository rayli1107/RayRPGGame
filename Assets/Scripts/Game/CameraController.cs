using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private string[] _fadeLayerNames;
    [SerializeField]
    private float _fadeAlpha = 0.4f;
    [SerializeField]
    private float _raycastTimeout = 0.1f;

    private CinemachineBrain _cinemachineBrain;
    private float _lastRaycastTime;
    private Transform _cameraTarget => _cinemachineBrain.ActiveVirtualCamera.LookAt;
    private int _layerMask;

    private void Awake()
    {
        _cinemachineBrain = GetComponent<CinemachineBrain>();
        _layerMask = 0;
        foreach (string layerName in _fadeLayerNames)
        {
            _layerMask |= 1 << LayerMask.NameToLayer(layerName);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _lastRaycastTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float timeNow = Time.time;
        if (timeNow - _lastRaycastTime > _raycastTimeout)
        {
            checkForFadeOutObjects();
            _lastRaycastTime = timeNow;
        }
    }

    private void checkForFadeOutObjects()
    {
        Vector3 delta = _cameraTarget.position - transform.position;

        RaycastHit[] results = Physics.RaycastAll(
            transform.position, delta, delta.magnitude, _layerMask);
        foreach (RaycastHit result in results)
        {
            FadingObjectController fadeController = result.collider.GetComponent<FadingObjectController>();
            if (fadeController == null)
            {
                fadeController = result.collider.gameObject.AddComponent<FadingObjectController>();
                fadeController.enabled = true;
            }
            fadeController.FadeOut(_fadeAlpha);
            /*
                        if (_fadedObjects.Contains(result.collider))
                        {
                            continue;
                        }

                        _fadedObjects.Add(result.collider);
                        MeshRenderer meshRenderer = result.collider.GetComponent<MeshRenderer>();
                        Material material = meshRenderer.material;
                        material.SetInt(
                            "_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        material.SetInt(
                            "_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        material.SetInt("_ZWrite", 0);
                        if (true)
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
                                material.color.r, material.color.g, material.color.b, _fadeAlpha);
                        }
            */
        }
    }
}
