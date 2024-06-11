using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MovimentoInimigos : MonoBehaviour
{
    private Animator animInimigo;
    private NavMeshAgent navMesh;
    private GameObject player;
    public float velocidadeInimigo;
    public bool isAttacking = false;
    public bool isWalking = false;
    private Coroutine attackCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        animInimigo = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        navMesh.speed = velocidadeInimigo;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking)
        {
            navMesh.destination = player.transform.position;
            animInimigo.SetBool("walk", true);
            isWalking = true;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 1.5f && !isAttacking)
        {
            isAttacking = true;
            navMesh.speed = 0;
            animInimigo.SetBool("attack", true);
            animInimigo.SetBool("walk", false);
            attackCoroutine = StartCoroutine(Ataque());
        }
    }

    private IEnumerator Ataque()
    {
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
        yield return new WaitForSeconds(1.0f);
        animInimigo.SetBool("attack", false);
        navMesh.speed = velocidadeInimigo;
    }
}
