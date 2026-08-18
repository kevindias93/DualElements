using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorFase : MonoBehaviour
{
    public static GerenciadorFase instancia;

    public Porta portaIgnis;
    public Porta portaAqua;

    [Header("Tela de Vitoria")]
    public GameObject textoVitoria;
    public GameObject botaoJogarDeNovo;
    public GameObject botaoProximaFase;
    public GameObject botaoSair;

    [Header("Os dois personagens")]
    public PlayerRespawn ignis;
    public PlayerRespawn aqua;

    [Header("Timer")]
    public TimerFase timer;

    [Header("Som")]
    public AudioClip somVitoria;

    private AudioSource audioSource;

    void Awake()
    {
        instancia = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void VerificarVitoria()
    {
        if (portaIgnis.ocupada && portaAqua.ocupada)
        {
            Debug.Log("FASE CONCLUIDA!");
            ignis.TocarVitoria();
            aqua.TocarVitoria();
            ProgressoJogo.DesbloquearProxima(SessaoJogo.Dificuldade, NumeroDaFaseAtual());
            if (audioSource != null && somVitoria != null)
                audioSource.PlayOneShot(somVitoria);
            // A mensagem "Fase Concluida!" foi retirada: os botoes (Jogar de Novo/Proxima Fase/Sair)
            // ja deixam claro que a fase acabou, e o texto ficava atras deles na tela.
            if (botaoJogarDeNovo != null) botaoJogarDeNovo.SetActive(true);
            if (botaoProximaFase != null) botaoProximaFase.SetActive(true);
            if (botaoSair != null) botaoSair.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void MorreramOsDois()
    {
        Debug.Log("Um morreu! Os dois voltam e os cristais reaparecem.");
        ignis.Morrer();
        aqua.Morrer();
        GameManager.instance.ResetarCristais();
        GameManager.DispararReinicioDeFase();

        if (timer != null)
            timer.ReiniciarTimer();
    }

    public void TempoEsgotado()
    {
        Debug.Log("GAME OVER - Tempo esgotado!");

        // Mostra o botao de jogar de novo (o mesmo da vitoria) e o de sair
        // (Proxima Fase fica de fora aqui: so faz sentido apos vencer)
        if (botaoJogarDeNovo != null)
            botaoJogarDeNovo.SetActive(true);
        if (botaoSair != null)
            botaoSair.SetActive(true);

        // Congela o jogo
        Time.timeScale = 0f;
    }

    // Le o numero da fase a partir do nome da cena (ex: "Fase_07" -> 7)
    public static int NumeroDaFaseAtual()
    {
        string nome = SceneManager.GetActiveScene().name;
        string numeroTexto = nome.Replace("Fase_", "");
        int.TryParse(numeroTexto, out int numero);
        return numero;
    }
}