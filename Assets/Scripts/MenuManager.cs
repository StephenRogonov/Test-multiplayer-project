using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] private List<Menu> _menuScreens;

    public void OpenMenu(string menuName)
    {
        foreach (Menu menu in _menuScreens)
        {
            if (menu.menuName == menuName)
            {
                menu.Open();
            }
            else
            {
                menu.Close();
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }
}
