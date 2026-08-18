using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotaoFase : MonoBehaviour
{
    public int numero;
    public Button botao;
    public TextMeshProUGUI label;

    void Start()
    {
        bool desbloqueada = ProgressoJogo.EstaDesbloqueada(SessaoJogo.Dificuldade, numero);
        if (botao != null)
            botao.interactable = desbloqueada;
        if (label != null)
            label.text = desbloqueada ? numero.ToString() : "🔒";
    }
}
