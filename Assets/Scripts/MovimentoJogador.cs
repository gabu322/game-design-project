using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class MovimentoJogador : MonoBehaviour
{
    // Declara��o das vari�veis
    private CharacterController controller; // Controlador do personagem
    private Transform myCamera; // Transform da c�mera principal
    private Animator animator; // Controlador de anima��es

    public int VidaPlayer = 100;
    public bool colliding = false;
    
    [SerializeField] private float forcaY; // For�a aplicada no eixo Y para simular a gravidade e o salto
    [SerializeField] private float gravidade = -9.81f; // Valor da gravidade
    [SerializeField] private float forcaSalto = 2f; // For�a de salto
    [SerializeField] private float velocidadeMovimento = 5f; // Velocidade de movimento

    [SerializeField] private float distanciaChecagemChao = 0.1f; // Dist�ncia para checar o ch�o
    [SerializeField] private LayerMask colisaoLayer; // Layer para detectar colis�es com o ch�o


    // Start � chamado antes da primeira atualiza��o do frame
    void Start()
    {
        // Inicializa o controlador do personagem, a c�mera principal e o controlador de anima��es
        controller = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();
    }

    // Update � chamado uma vez por frame
    void Update()
    {
        // Obt�m o input do teclado para movimenta��o
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Cria um vetor de movimento baseado no input do jogador
        Vector3 movimento = new Vector3(horizontal, 0, vertical);

        // Transforma o vetor de movimento baseado na dire��o da c�mera
        movimento = myCamera.TransformDirection(movimento);
        movimento.y = 0;

        // Move o personagem
        controller.Move(movimento * Time.deltaTime * velocidadeMovimento);

        // Se o personagem estiver se movendo, altera sua rota��o para olhar na dire��o do movimento
        if (movimento != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), Time.deltaTime * 10);
        }

        // Define a anima��o de movimento
        animator.SetBool("Mover", movimento != Vector3.zero);

        // Verifica se o personagem est� no ch�o usando um Raycast
        RaycastHit hit;
        bool estaNoChao = controller.isGrounded || Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, distanciaChecagemChao + 0.1f, colisaoLayer);

        animator.SetBool("EstaNoChao", estaNoChao);

        // Se o jogador pressionar a tecla de espa�o e o personagem estiver no ch�o, aplica a for�a de salto
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            forcaY = Mathf.Sqrt(forcaSalto * -2f * gravidade); // Calcular a for�a de salto para alcan�ar a altura desejada
            animator.SetTrigger("Saltar");
        }

        // Aplica a gravidade ao personagem
        if (!estaNoChao)
        {
            forcaY += gravidade * Time.deltaTime;
        }
        else if (forcaY < 0)
        {
            forcaY = -2f; // Garante que o personagem fique no ch�o
        }

        // Move o personagem no eixo Y baseado na for�a aplicada (gravidade e salto)
        controller.Move(new Vector3(0, forcaY, 0) * Time.deltaTime);
        if(VidaPlayer <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Desenha o Raycast de checagem do ch�o no editor para visualiza��o
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distanciaChecagemChao);
    }

    void OnTriggerEnter(Collider other)
    {
            colliding = true;
        if (other.CompareTag("Inimigo"))
        {
            VidaPlayer -= 10;
        }
    }


}
