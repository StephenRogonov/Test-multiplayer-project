using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private List<Menu> _menuScreens;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < _menuScreens.Count; i++)
        {
            if (_menuScreens[i].menuName == menuName)
            {
                _menuScreens[i].Open();
            }
            else if (_menuScreens[i].open)
            {
                _menuScreens[i].Close();
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < _menuScreens.Count; i++)
        {
            if (_menuScreens[i].open)
            {
                CloseMenu(_menuScreens[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
