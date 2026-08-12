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

        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

        if (x != 0f)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * Mathf.Sign(x);
            transform.localScale = escala;
        }

        noChao = Physics2D.OverlapCircle(checkChao.position, 0.12f, oQueEChao);

        if (noChao && rb.linearVelocity.y <= 0.1f)
            pulosDados = 0;

        if (Input.GetKeyDown(teclaPulo) && pulosDados < 2)
        {
            if (pulosDados == 0)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            else
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaSegundoPulo);

            pulosDados++;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 2f * Time.deltaTime;
        }
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