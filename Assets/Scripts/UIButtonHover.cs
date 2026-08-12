using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float escalaHover = 1.06f;
    public float velocidade = 12f;

    Vector3 escalaBase;
    Vector3 escalaAlvo;

    void Awake()
    {
        escalaBase = transform.localScale;
        escalaAlvo = escalaBase;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, escalaAlvo, Time.unscaledDeltaTime * velocidade);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        escalaAlvo = escalaBase * escalaHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        escalaAlvo = escalaBase;
    }
}
