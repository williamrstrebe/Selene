using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SucessController : MonoBehaviour
{
    [Header("Itens a coletar")]
    public int madeirasSucesso;
    public int madeirasTotal = 0;

    public string nivelAtual;

    public GameObject successDialog;


    private bool sucesso = false;
    private void Update()
    {
        if (madeirasSucesso == madeirasTotal && !sucesso)
        {
            sucesso = true;
            Debug.Log(successDialog != null);

            if (ChangeQuestAtHome.currentQuest == 0)
            {
                GameState.firstLevel = false;
                ChangeQuestAtHome.currentQuest += 1;
            }
            else if (ChangeQuestAtHome.currentQuest == 2)
            {
                ChangeQuestAtHome.currentQuest += 1;
                GameState.secondLevel = false;
            }

            successDialog.SetActive(true);
            Time.timeScale = 0;

        }
    }

    public void addMadeira()
    {
        this.madeirasTotal++;
    }

    public void setState()
    {
        if (nivelAtual == "FirstLevel")
        {
            GameState.firstLevel = false;
        }
        if (nivelAtual == "SecondLevel")
        {
            GameState.secondLevel = false;
        }

    }

}
