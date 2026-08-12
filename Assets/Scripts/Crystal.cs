using UnityEngine;

public class Crystal : MonoBehaviour
{
    public string donoTag = "Ignis";  // "Ignis", "Aqua" ou "Dourado"

    void OnTriggerEnter2D(Collider2D col)
    {
        bool podeColetar = false;

        if (donoTag == "Dourado")
        {
            if (col.CompareTag("Ignis") || col.CompareTag("Aqua"))
                podeColetar = true;
        }
        else
        {
            if (col.CompareTag(donoTag))
                podeColetar = true;
        }

        if (podeColetar)
        {
            GameManager.instance.ColetarCristal(donoTag);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, 90 * Time.deltaTime);
    }
}