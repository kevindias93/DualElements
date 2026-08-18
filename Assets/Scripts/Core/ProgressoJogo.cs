using UnityEngine;

public static class ProgressoJogo
{
    public const int TotalFases = 30;

    public static int FasesDesbloqueadas(string dificuldade)
    {
        return PlayerPrefs.GetInt("Desbloqueadas_" + dificuldade, 1);
    }

    public static bool EstaDesbloqueada(string dificuldade, int numeroFase)
    {
        return numeroFase <= FasesDesbloqueadas(dificuldade);
    }

    // Chamado ao vencer a fase "numeroFase": desbloqueia a proxima, se ainda nao estiver.
    public static void DesbloquearProxima(string dificuldade, int numeroFase)
    {
        int proxima = Mathf.Clamp(numeroFase + 1, 1, TotalFases);
        int atual = FasesDesbloqueadas(dificuldade);
        if (proxima > atual)
        {
            PlayerPrefs.SetInt("Desbloqueadas_" + dificuldade, proxima);
            PlayerPrefs.Save();
        }
    }
}
