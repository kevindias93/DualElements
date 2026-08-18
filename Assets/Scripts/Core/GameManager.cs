using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Disparado quando os personagens morrem e a fase reinicia (respawn cooperativo).
    // Qualquer mecanica que precise voltar ao estado inicial (caixas, plataformas frageis, etc)
    // pode assinar este evento em OnEnable/OnDisable em vez de o GerenciadorFase precisar conhece-la.
    public static event Action AoReiniciarFase;

    public static void DispararReinicioDeFase()
    {
        AoReiniciarFase?.Invoke();
    }

    public int cristaisIgnis = 0;
    public int cristaisAqua = 0;
    public int cristaisDourados = 0;

    [Header("Quantos cristais precisam ser pegos")]
    public int totalCristaisIgnis = 0;
    public int totalCristaisAqua = 0;
    public int totalCristaisDourados = 0;

    [Header("Textos na tela")]
    public TextMeshProUGUI textoIgnis;
    public TextMeshProUGUI textoAqua;
    public TextMeshProUGUI textoDourado;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Crystal[] cristais = FindObjectsOfType<Crystal>();
        foreach (Crystal c in cristais)
        {
            if (c.donoTag == "Ignis") totalCristaisIgnis++;
            else if (c.donoTag == "Aqua") totalCristaisAqua++;
            else if (c.donoTag == "Dourado") totalCristaisDourados++;
        }
        AtualizarTextos();
    }

    public void ColetarCristal(string donoTag)
    {
        if (donoTag == "Ignis") cristaisIgnis++;
        else if (donoTag == "Aqua") cristaisAqua++;
        else if (donoTag == "Dourado") cristaisDourados++;

        AtualizarTextos();
    }

    void AtualizarTextos()
    {
        if (textoIgnis != null)
            textoIgnis.text = cristaisIgnis + "/" + totalCristaisIgnis;
        if (textoAqua != null)
            textoAqua.text = cristaisAqua + "/" + totalCristaisAqua;
        if (textoDourado != null)
            textoDourado.text = cristaisDourados + "/" + totalCristaisDourados;
    }

    public bool TodosCristaisColetados()
    {
        return cristaisIgnis >= totalCristaisIgnis
            && cristaisAqua >= totalCristaisAqua
            && cristaisDourados >= totalCristaisDourados;
    }

    public void ResetarCristais()
    {
        cristaisIgnis = 0;
        cristaisAqua = 0;
        cristaisDourados = 0;

        Crystal[] cristais = Resources.FindObjectsOfTypeAll<Crystal>();
        foreach (Crystal c in cristais)
        {
            if (c.gameObject.scene.IsValid())
                c.gameObject.SetActive(true);
        }

        AtualizarTextos();
        Debug.Log("Cristais resetados!");
    }
}