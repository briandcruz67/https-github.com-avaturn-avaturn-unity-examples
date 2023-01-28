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
    [SerializeField] private GameObject _frameButton;
    [SerializeField] private bool _isOpen;
    
    void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            _isOpen = !_isOpen;
            _menuHint.text = !_isOpen ? _openMenu : _closeMenu;
            _frameButton.SetActive(_isOpen);
            Cursor.visible = _isOpen;
            Cursor.lockState = _isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
    
    
}
