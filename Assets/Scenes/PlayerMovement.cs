using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 14f;

    [Header("Pulo Duplo")]
    public float forcaSegundoPulo = 9f;

    [Header("Controles")]
    public KeyCode teclaEsquerda = KeyCode.LeftArrow;
    public KeyCode teclaDireita = KeyCode.RightArrow;
    public KeyCode teclaPulo = KeyCode.UpArrow;

    [Header("Chao")]
    public Transform checkChao;
    public LayerMask oQueEChao;

    private Rigidbody2D rb;
    private bool noChao;
    private int pulosDados = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = 0f;
        if (Input.GetKey(teclaEsquerda)) x = -1f;
        if (Input.GetKey(teclaDireita)) x = 1f;

        rb.velocity = new Vector2(x * speed, rb.velocity.y);

        noChao = Physics2D.OverlapCircle(checkChao.position, 0.12f, oQueEChao);

        if (noChao && rb.velocity.y <= 0.1f)
            pulosDados = 0;

        if (Input.GetKeyDown(teclaPulo) && pulosDados < 2)
        {
            if (pulosDados == 0)
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            else
                rb.velocity = new Vector2(rb.velocity.x, forcaSegundoPulo);

            pulosDados++;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 2f * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ignis") || col.gameObject.CompareTag("Aqua"))
        {
            if (col.transform.position.y < transform.position.y)
                transform.SetParent(col.transform);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ignis") || col.gameObject.CompareTag("Aqua"))
            transform.SetParent(null);
    }

    void OnDrawGizmos()
    {
        if (checkChao != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(checkChao.position, 0.12f);
        }
    }
}