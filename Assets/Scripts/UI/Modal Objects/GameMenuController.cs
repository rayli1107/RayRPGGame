using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggleStatus;
    [SerializeField]
    private Toggle _toggleQuests;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void OnTabValueChange(Toggle toggle)
    {
        Debug.Log("Tab: " + toggle + " " + toggle.isOn);
    }
}
