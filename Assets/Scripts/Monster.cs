using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int attackValue;
    private Animator modelAnimator;

    // Nomes dos estados no Animator
    private string idleStateName = "Idle";
    private string attackStateName = "Attack";
    private string guardStateName = "Guard";

    private bool isAttacking = false;
    private bool isDefending = false;

    public AudioSourceManager audioManager;

    private Vector3 initialScale; // Escala original do monstro

    void Start()
    {
        modelAnimator = GetComponent<Animator>();
        initialScale = transform.localScale;
    }

    // Verifica se o monstro está no estado "Idle"
    public bool IsIdle()
    {
        AnimatorStateInfo currentState = modelAnimator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName(idleStateName);
    }

    // Verifica se o monstro está no estado de ataque
    public bool IsAttacking()
    {
        AnimatorStateInfo currentState = modelAnimator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName(attackStateName);
    }

    public bool IsAttackingProperty()
    {
        return isAttacking;
    }

    // Verifica se o monstro está no estado de defesa
    public bool IsDefending()
    {
        AnimatorStateInfo currentState = modelAnimator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName(guardStateName);
    }

    // Inicia o ataque
    public void Attack(Monster opponent)
    {
        if (IsIdle() && !isAttacking)
        {
            isAttacking = true;
            modelAnimator.SetTrigger("Attack");
            Debug.Log($"{gameObject.name} está atacando!");
            StartCoroutine(EndAttack());
        }
    }

    // Inicia a defesa
    public void Guard()
    {
        if (IsIdle() && !isDefending)
        {
            isDefending = true;
            modelAnimator.SetTrigger("Guard");
            Debug.Log($"{gameObject.name} está defendendo!");
        }
    }

    // Finaliza o estado de ataque após a animação
    private IEnumerator EndAttack()
    {
        yield return new WaitUntil(() => IsAttacking());
        yield return new WaitUntil(() => !IsAttacking());
        isAttacking = false;
        Debug.Log($"{gameObject.name} terminou o ataque.");
    }

    // Finaliza a defesa
    public void EndGuard()
    {
        if (isDefending)
        {
            isDefending = false;
            Debug.Log($"{gameObject.name} terminou a defesa.");
        }
    }

    // Função chamada ao detectar o tipo do monstro (apenas um exemplo de som, pode ser expandido)
    public void DoTypeSound()
    {
        if (audioManager != null)
            audioManager.PlayAudio(0);
    }

    public void SaveInitialScale()
    {
        initialScale = transform.localScale;
    }

    // Obtém a escala inicial
    public Vector3 GetInitialScale()
    {
        return initialScale;
    }

    // Restaura a escala inicial
    public void RestoreInitialScale()
    {
        Debug.Log(initialScale);
        transform.localScale = initialScale;
    }
}
