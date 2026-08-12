using UnityEngine;

public class GerenciadorFase : MonoBehaviour
{
    public static GerenciadorFase instancia;

    public Porta portaIgnis;
    public Porta portaAqua;

    [Header("Tela de Vitoria")]
    public GameObject textoVitoria;
    public GameObject botaoJogarDeNovo;

    [Header("Os dois personagens")]
    public PlayerRespawn ignis;
    public PlayerRespawn aqua;

    [Header("Timer")]
    public TimerFase timer;

    void Awake()
    {
        instancia = this;
    }

    public void VerificarVitoria()
    {
        if (portaIgnis.ocupada && portaAqua.ocupada)
        {
            Debug.Log("FASE CONCLUIDA!");
            if (textoVitoria != null) textoVitoria.SetActive(true);
            if (botaoJogarDeNovo != null) botaoJogarDeNovo.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void MorreramOsDois()
    {
        Debug.Log("Um morreu! Os dois voltam e os cristais reaparecem.");
        ignis.Morrer();
        aqua.Morrer();
        GameManager.instance.ResetarCristais();

        if (timer != null)
            timer.ReiniciarTimer();
    }

    public void TempoEsgotado()
    {
        Debug.Log("GAME OVER - Tempo esgotado!");

        // Mostra o botao de jogar de novo (o mesmo da vitoria)
        if (botaoJogarDeNovo != null)
            botaoJogarDeNovo.SetActive(true);

        // Congela o jogo
        Time.timeScale = 0f;
    }

}