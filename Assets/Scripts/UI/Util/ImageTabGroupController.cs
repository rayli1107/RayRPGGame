using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageTabGroupController : MonoBehaviour
{
    [SerializeField]
    private ImageTabController _defaultImageTab;

    [field: SerializeField]
    public Sprite imageOn { get; private set; }

    [field: SerializeField]
    public Sprite imageOff { get; private set; }


    private List<ImageTabController> _tabs;

    public void RegisterImageTab(ImageTabController imageTab)
    {
        imageTab.tabSelected = imageTab == _defaultImageTab;

        if (_tabs == null)
        {
            _tabs = new List<ImageTabController>();
        }
        if (!_tabs.Contains(imageTab))
        {
            _tabs.Add(imageTab);
        }
    }

    public void UnregisterImageTab(ImageTabController imageTab)
    {
        _tabs.Remove(imageTab);
    }

    public void SetImageTab(ImageTabController activeImageTab)
    {
        foreach (ImageTabController imageTab in _tabs)
        {
            imageTab.tabSelected = imageTab == activeImageTab;
        }
    }
}
