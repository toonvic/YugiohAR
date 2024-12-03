using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    // Configurações do efeito de flutuação
    public float verticalAmplitude = 0.1f; // Altura do movimento para cima e para baixo
    public float horizontalAmplitude = 0.1f; // Largura do movimento para os lados
    public float frequency = 1f; // Velocidade do movimento

    private Vector3 startPosition;

    void Start()
    {
        // Armazena a posição inicial do objeto
        startPosition = transform.position;
    }

    void Update()
    {
        // Calcula o deslocamento vertical (senoidal)
        float verticalOffset = Mathf.Sin(Time.time * frequency) * verticalAmplitude;

        // Calcula o deslocamento horizontal (cossenoidal para criar o padrão de "H")
        float horizontalOffset = Mathf.Cos(Time.time * frequency) * horizontalAmplitude;

        // Aplica o movimento combinando ambos os eixos
        transform.position = startPosition + new Vector3(horizontalOffset, verticalOffset, 0);
    }
}