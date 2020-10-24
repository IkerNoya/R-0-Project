using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public GameObject CamvasGameOver;
    private void OnEnable()
    {
        PlayerController.OnDiePlayer += ActivateCamvasGameOver;
    }
    private void OnDisable()
    {
        PlayerController.OnDiePlayer -= ActivateCamvasGameOver;
    }
    void ActivateCamvasGameOver(PlayerController pc)
    {
        CamvasGameOver.SetActive(true);
    }
}
