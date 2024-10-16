using UnityEngine;

public class AttackingCollision : MonoBehaviour
{
    // Referência ao Animator do objeto de modelo (monstro)
    public Animator modelAnimator;

    // Detecta quando ocorre a colisão através de um Trigger
    void OnTriggerEnter(Collider other)
    {
        // Chama a função para iniciar a animação de ataque
        StartAttack();
    }

    // Função que inicia a animação de ataque no objeto de modelo
    private void StartAttack()
    {
        if (modelAnimator != null)
        {
            modelAnimator.SetTrigger("StartAttacking");
            Debug.Log("Iniciando animação de ataque!");
        }
        else
        {
            Debug.LogWarning("Animator não foi atribuído!");
        }
    }
}
