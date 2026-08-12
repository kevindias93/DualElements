using UnityEngine;

public class aguacolider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Ignis"))
        {
            GerenciadorFase.instancia.MorreramOsDois();
        }
    }
}