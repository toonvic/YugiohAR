using UnityEngine;

public class Monster : MonoBehaviour
{
    // Valor de ataque fixo do monstro
    public int attackValue;

    // Referência ao Animator do monstro
    private Animator modelAnimator;

    void Start()
    {
        // Obtém o Animator do monstro
        modelAnimator = GetComponent<Animator>();
    }

    // Método que executa a animação de ataque
    public void Attack()
    {
        modelAnimator.SetTrigger("Attack");
        Debug.Log(gameObject.name + " está atacando!");
    }

    // Método que executa a animação de guarda
    public void Guard()
    {
        modelAnimator.SetTrigger("Guard");
        Debug.Log(gameObject.name + " está defendendo!");
    }
}
