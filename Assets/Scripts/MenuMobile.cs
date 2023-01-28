using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuMobile : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private Text _menuButtonText;
    [SerializeField] private string _openMenu, _closeMenu;
    [SerializeField] private GameObject _frameButton, _controlCanvas;
    [SerializeField] private bool _isOpen;


    private void Start()
    {
        _menuButton.onClick.AddListener(OnMenuClick);
    }

    private void OnMenuClick()
    {
        _isOpen = !_isOpen;
        _menuButtonText.text = !_isOpen ? _openMenu : _closeMenu;
        _frameButton.SetActive(_isOpen);
        _controlCanvas.SetActive(!_isOpen);
        Cursor.visible = _isOpen;
        Cursor.lockState = _isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}