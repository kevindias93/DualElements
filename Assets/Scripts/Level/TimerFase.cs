using UnityEngine;
using TMPro;

public class TimerFase : MonoBehaviour
{
    [Header("Tempo base da fase, em segundos (dificuldade Medio)")]
    public float tempoTotal = 30f;

    [Header("Multiplicadores por dificuldade (Facil = sem tempo)")]
    public float multiplicadorMedio = 1.5f;
    public float multiplicadorDificil = 0.6f;

    [Header("Texto na tela")]
    public TextMeshProUGUI textoTempo;

    [Header("Mensagem de tempo esgotado")]
    public GameObject mensagemTempoEsgotado;

    private float tempoRestante;
    private bool acabou = false;
    private bool semTempo;

    void Start()
    {
        semTempo = SessaoJogo.Dificuldade == SessaoJogo.Facil;

        if (semTempo)
        {
            if (textoTempo != null)
                textoTempo.gameObject.SetActive(false);
            return;
        }

        tempoTotal *= SessaoJogo.Dificuldade == SessaoJogo.Dificil ? multiplicadorDificil : multiplicadorMedio;
        tempoRestante = tempoTotal;
    }

    void Update()
    {
        if (semTempo || acabou) return;

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
        if (semTempo) return;

        tempoRestante = tempoTotal;
        acabou = false;

        if (textoTempo != null)
            textoTempo.color = Color.white;
    }
}