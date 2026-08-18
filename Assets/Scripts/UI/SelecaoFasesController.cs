using UnityEngine;
using UnityEngine.SceneManagement;

public class SelecaoFasesController : MonoBehaviour
{
    public string cenaDificuldade = "DificuldadeSelect";

    public void EscolherFase(int numero)
    {
        if (!ProgressoJogo.EstaDesbloqueada(SessaoJogo.Dificuldade, numero))
            return;

        SessaoJogo.FaseAtual = numero;
        SceneManager.LoadScene("Fase_" + numero.ToString("D2"));
    }

    public void Voltar()
    {
        SceneManager.LoadScene(cenaDificuldade);
    }
}
