using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MonsterBattleHandler : MonoBehaviour
{
    private List<Monster> detectedMonsters = new List<Monster>();
    private bool battleHappened = false; // Controle para evitar múltiplas batalhas

    void Start()
    {
        Debug.Log("Startou!");

        // Obtém todos os image targets na cena
        ObserverBehaviour[] observers = FindObjectsOfType<ObserverBehaviour>();

        // Inscreve-se nos eventos de detecção de cada target
        foreach (var observer in observers)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour target, TargetStatus status)
    {
        // Verifica se o target foi detectado
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            AddMonster(target);
        }
        else
        {
            RemoveMonster(target);
        }

        // Se dois monstros forem detectados e a batalha ainda não ocorreu, inicia a corrotina para aguardar o estado "Idle"
        if (detectedMonsters.Count == 2 && !battleHappened)
        {
            StartCoroutine(WaitForBothMonstersToBeIdle());
        }
    }

    // Coroutine que espera os dois monstros estarem em "Idle" antes de começar a batalha
    private IEnumerator WaitForBothMonstersToBeIdle()
    {
        Debug.Log("Esperando os dois monstros ficarem em Idle...");

        // Espera até que ambos os monstros estejam no estado "Idle"
        while (!BothMonstersIdle())
        {
            yield return null; // Aguarda o próximo frame
        }

        Debug.Log("Os dois monstros estão em Idle, iniciando a batalha.");
        CompareMonsters();
        battleHappened = true; // Marca a batalha como realizada
    }

    private bool BothMonstersIdle()
    {
        var bothAreIdle = detectedMonsters[0].IsIdle() && detectedMonsters[1].IsIdle();
        Debug.Log("Ambos Idle: " + bothAreIdle);
        return bothAreIdle;
    }

    private void AddMonster(ObserverBehaviour target)
    {
        Monster monster = target.GetComponentInChildren<Monster>();
        if (monster != null && !detectedMonsters.Contains(monster))
        {
            detectedMonsters.Add(monster);
            Debug.Log("Monstro detectado: " + monster.name);
        }
    }

    private void RemoveMonster(ObserverBehaviour target)
    {
        Monster monster = target.GetComponentInChildren<Monster>();
        if (monster != null && detectedMonsters.Contains(monster))
        {
            detectedMonsters.Remove(monster);
            Debug.Log("Monstro perdido: " + monster.name);
            battleHappened = false; // Reseta o controle da batalha
        }
    }

    private void CompareMonsters()
    {
        Monster monster1 = detectedMonsters[0];
        Monster monster2 = detectedMonsters[1];

        if (monster1.attackValue > monster2.attackValue)
        {
            Debug.Log("1 atacou 2");
            monster1.Attack(monster2); // Passando o oponente correto
            monster2.Guard();
        }
        else if (monster1.attackValue < monster2.attackValue)
        {
            Debug.Log("2 atacou 1");
            monster1.Guard();
            monster2.Attack(monster1); // Passando o oponente correto
        }
        else
        {
            Debug.Log("Ambos os monstros têm o mesmo valor de ataque.");
        }
    }
}
