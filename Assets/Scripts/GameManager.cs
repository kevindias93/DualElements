using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
            textoIgnis.text = "Ignis: " + cristaisIgnis + "/" + totalCristaisIgnis;
        if (textoAqua != null)
            textoAqua.text = "Aqua: " + cristaisAqua + "/" + totalCristaisAqua;
        if (textoDourado != null)
            textoDourado.text = "Dourado: " + cristaisDourados + "/" + totalCristaisDourados;
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