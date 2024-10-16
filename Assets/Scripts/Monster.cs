using UnityEngine;

public class Monster : MonoBehaviour
{
    // Valor de ataque fixo do monstro
    public int attackValue;

    // Refer�ncia ao Animator do monstro
    private Animator modelAnimator;

    void Start()
    {
        // Obt�m o Animator do monstro
        modelAnimator = GetComponent<Animator>();
    }

    // M�todo que executa a anima��o de ataque
    public void Attack()
    {
        modelAnimator.SetTrigger("Attack");
        Debug.Log(gameObject.name + " est� atacando!");
    }

    // M�todo que executa a anima��o de guarda
    public void Guard()
    {
        modelAnimator.SetTrigger("Guard");
        Debug.Log(gameObject.name + " est� defendendo!");
    }
}
