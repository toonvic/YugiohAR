using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int attackValue;
    private Animator modelAnimator;

    // Definindo os nomes dos estados do Animator
    private string idleStateName = "Idle";
    private string attackStateName = "Attack";
    private string guardStateName = "Guard";

    private bool isAttacking = false;
    private bool isGuarding = false;

    void Start()
    {
        modelAnimator = GetComponent<Animator>();
    }

    // Verifica se o monstro está no estado "idle"
    public bool IsIdle()
    {
        AnimatorStateInfo currentState = modelAnimator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName(idleStateName);
    }

    // Ataca e sincroniza o tempo do ataque com a defesa do oponente
    public void Attack(Monster opponent)
    {
        if (IsIdle() && !isAttacking)
        {
            isAttacking = true;
            modelAnimator.SetTrigger("Attack");
            Debug.Log(gameObject.name + " está atacando!");

            // Aguarda o fim da animação de ataque e então reseta o estado
            StartCoroutine(EndAttack(opponent));
        }
    }

    // Defende até que o oponente termine de atacar
    public void Guard()
    {
        if (IsIdle() && !isGuarding)
        {
            isGuarding = true;
            modelAnimator.SetTrigger("Guard");
            Debug.Log(gameObject.name + " está defendendo!");
        }
    }

    // Coroutine para controlar o término do ataque
    private IEnumerator EndAttack(Monster opponent)
    {
        // Espera até o final da animação de ataque
        AnimatorStateInfo attackStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(attackStateInfo.length);

        isAttacking = false;
        opponent.EndGuard(); // Oponente sai da defesa

        Debug.Log(gameObject.name + " terminou o ataque e está voltando ao estado idle.");
        ResetToIdle();
    }

    // Função chamada pelo oponente quando o ataque termina
    public void EndGuard()
    {
        if (isGuarding)
        {
            isGuarding = false;
            Debug.Log(gameObject.name + " terminou a defesa e está voltando ao estado idle.");
            ResetToIdle();
        }
    }

    // Reseta o monstro para o estado "idle"
    public void ResetToIdle()
    {
        isAttacking = false;
        isGuarding = false;
    }
}
