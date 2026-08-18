using UnityEngine;
using UnityEngine.SceneManagement;

public class DificuldadeSelectController : MonoBehaviour
{
    public string cenaSelecaoFases = "SelecaoFases";
    public string cenaMenu = "MainMenu";

    public void EscolherFacil() => Escolher(SessaoJogo.Facil);
    public void EscolherMedio() => Escolher(SessaoJogo.Medio);
    public void EscolherDificil() => Escolher(SessaoJogo.Dificil);

    void Escolher(string dificuldade)
    {
        SessaoJogo.Dificuldade = dificuldade;
        SceneManager.LoadScene(cenaSelecaoFases);
    }

    public void Voltar()
    {
        SceneManager.LoadScene(cenaMenu);
    }
}
