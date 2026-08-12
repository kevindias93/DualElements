using UnityEngine;
using TMPro;

public class TimerFase : MonoBehaviour
{
    [Header("Tempo da fase (segundos)")]
    public float tempoTotal = 30f;

    [Header("Texto na tela")]
    public TextMeshProUGUI textoTempo;

    [Header("Mensagem de tempo esgotado")]
    public GameObject mensagemTempoEsgotado;

    private float tempoRestante;
    private bool acabou = false;

    void Start()
    {
        tempoRestante = tempoTotal;
    }

    void Update()
    {
        if (acabou) return;

        tempoRestante -= Time.deltaTime;

        if (textoTempo != null)
            textoTempo.text = Mathf.Ceil(tempoRestante).ToString();

        if (textoTempo != null)
        {
            if (tempoRestante <= 5f)
                textoTempo.color = Color.red;
            else
                textoTempo.color = Color.white;
        }

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            acabou = true;
            Debug.Log("Tempo esgotado!");

            // Mostra a mensagem "TEMPO ESGOTADO!"
            if (mensagemTempoEsgotado != null)
                mensagemTempoEsgotado.SetActive(true);

            // Chama o game over (mostra botao + congela)
            GerenciadorFase.instancia.TempoEsgotado();
        }
    }

    public void ReiniciarTimer()
    {
        tempoRestante = tempoTotal;
        acabou = false;

        if (textoTempo != null)
            textoTempo.color = Color.white;
    }
}