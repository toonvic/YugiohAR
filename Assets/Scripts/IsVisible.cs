using UnityEngine;
 
public class IsVisible : MonoBehaviour
{
    private Renderer m_Renderer;
    private Animator m_Animator;
    private bool isAnimating;
 
    // Use this for initialization
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_Animator = GetComponent<Animator>();
        isAnimating = false;  // Estado inicial da animação
    }
 
    // Update is called once per frame
    void Update()
    {
        if (m_Renderer.isVisible && !isAnimating)
        {
            // Inicia a animação quando o objeto se torna visível
            m_Animator.SetTrigger("Start");
            m_Animator.ResetTrigger("Stop");
            isAnimating = true;
        }
        else if (!m_Renderer.isVisible && isAnimating)
        {
            // Pausa ou para a animação quando o objeto não é mais visível
            m_Animator.ResetTrigger("Start");
            m_Animator.SetTrigger("Stop");
            isAnimating = false;
        }
    }
}