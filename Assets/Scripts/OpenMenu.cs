using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OpenMenu : MonoBehaviour
{
    public static OpenMenu instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        //SceneManager.LoadScene("Battle_1");
        OpenMenu.instance.ShowBattleMenus();
    }

    public GameObject battleMenu;

    public void HideMenus()
    {
        battleMenu.SetActive(false);
    }

    public void ShowBattleMenus()
    {
        battleMenu.SetActive(true);
    }
    public void EnterBattle()
    {
        SceneManager.LoadScene("Battle_1");
    }


}