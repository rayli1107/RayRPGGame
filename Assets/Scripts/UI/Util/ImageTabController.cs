using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageTabController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private ImageTabGroupController _imageTabGroup;
    [SerializeField]
    private RectTransform _tabPanel;

    private bool _tabSelected;
    public bool tabSelected
    {
        get => _tabSelected;
        set
        {
            _tabSelected = value;
            _imageTab.sprite = tabSelected ? _imageTabGroup.imageOn : _imageTabGroup.imageOff;
            _tabPanel.gameObject.SetActive(tabSelected);
        }
    }

    private Image _imageTab;

    private void Awake()
    {
        _imageTab = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _imageTabGroup.RegisterImageTab(this);
    }

    private void OnDisable()
    {
        _imageTabGroup.UnregisterImageTab(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!tabSelected)
        {
            _imageTabGroup.SetImageTab(this);
        }
    }
}
