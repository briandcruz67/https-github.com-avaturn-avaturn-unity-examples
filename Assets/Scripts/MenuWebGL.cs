using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuWebGL : MonoBehaviour
{
    [SerializeField] private KeyCode _key;
    [SerializeField] private Text _menuHint;
    [SerializeField] private string _openMenu, _closeMenu;
    [SerializeField] private FrameController _frameController;
    [SerializeField] private bool _isOpen;
    
    void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            _isOpen = !_isOpen;
            _menuHint.text = !_isOpen ? _openMenu : _closeMenu;
            _frameController.ChangeVisibility();
            Cursor.visible = _isOpen;
            Cursor.lockState = _isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
    
    
}
