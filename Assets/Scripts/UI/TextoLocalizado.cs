using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextoLocalizado : MonoBehaviour
{
    public string chave;

    private TextMeshProUGUI texto;

    void Awake()
    {
        texto = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        Atualizar();
    }

    public void Atualizar()
    {
        if (texto != null && !string.IsNullOrEmpty(chave))
            texto.text = LocalizacaoDados.Get(chave);
    }
}
