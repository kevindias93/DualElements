using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleFase : MonoBehaviour
{
    [Header("Cena para onde o botao Sair volta")]
    public string cenaMenu = "MainMenu";

    [Header("Cena de selecao de fases (quando acaba a ultima)")]
    public string cenaSelecaoFases = "SelecaoFases";

    public void JogarDeNovo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ProximaFase()
    {
        Time.timeScale = 1f;

        int proximo = GerenciadorFase.NumeroDaFaseAtual() + 1;
        if (proximo <= ProgressoJogo.TotalFases)
        {
            SessaoJogo.FaseAtual = proximo;
            SceneManager.LoadScene("Fase_" + proximo.ToString("D2"));
        }
        else
        {
            SceneManager.LoadScene(cenaSelecaoFases); // zerou todas as fases dessa dificuldade
        }
    }

    public void Sair()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaMenu);
    }
}