using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    public Sprite sprite1; // Primeiro sprite (exemplo: jaden1)
    public Sprite sprite2; // Segundo sprite (exemplo: jaden2)
    public Sprite sprite3; // Segundo sprite (exemplo: jaden2)

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Obtém o SpriteRenderer anexado ao GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer não encontrado no objeto " + gameObject.name);
            return;
        }

        // Define o sprite inicial
        if (sprite1 != null)
        {
            spriteRenderer.sprite = sprite1;
        }
    }

    // Método para alternar o sprite com base em uma condição
    public void SwitchSprite(int num)
    {
        if (num == 1)
        {
            spriteRenderer.sprite = sprite1; // Define o segundo sprite
            return;
        }

        if (num == 2)
        {
            spriteRenderer.sprite = sprite2; // Define o segundo sprite
            return;
        }

        spriteRenderer.sprite = sprite3; // Volta ao primeiro sprite
    }
}
