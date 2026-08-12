using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Cena do jogo")]
    public string cenaDoJogo = "SampleScene";

    [Header("Paineis")]
    public GameObject painelComoJogar;
    public GameObject painelCreditos;

    public void Jogar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaDoJogo);
    }

    public void AbrirComoJogar()
    {
        FecharPaineis();
        if (painelComoJogar != null) painelComoJogar.SetActive(true);
    }

    public void AbrirCreditos()
    {
        FecharPaineis();
        if (painelCreditos != null) painelCreditos.SetActive(true);
    }

    public void FecharPaineis()
    {
        if (painelComoJogar != null) painelComoJogar.SetActive(false);
        if (painelCreditos != null) painelCreditos.SetActive(false);
    }

    public void Sair()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
