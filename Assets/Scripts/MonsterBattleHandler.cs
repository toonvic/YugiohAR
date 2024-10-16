using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia; // Necessário para usar Vuforia e ObserverBehaviour

public class MonsterBattleHandler : MonoBehaviour
{
    // Lista de todos os monstros detectados
    private List<Monster> detectedMonsters = new List<Monster>();

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

    // Função chamada quando o status de qualquer image target muda
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

        // Verifica se dois monstros foram detectados
        if (detectedMonsters.Count == 2)
        {
            CompareMonsters();
        }
    }

    // Função para adicionar um monstro à lista quando o target é detectado
    private void AddMonster(ObserverBehaviour target)
    {
        Monster monster = target.GetComponentInChildren<Monster>();
        if (monster != null && !detectedMonsters.Contains(monster))
        {
            detectedMonsters.Add(monster);
            Debug.Log("Monstro detectado: " + monster.name);
        }
    }

    // Função para remover um monstro da lista quando o target é perdido
    private void RemoveMonster(ObserverBehaviour target)
    {
        Monster monster = target.GetComponentInChildren<Monster>();
        if (monster != null && detectedMonsters.Contains(monster))
        {
            detectedMonsters.Remove(monster);
            Debug.Log("Monstro perdido: " + monster.name);
        }
    }

    // Função para comparar os monstros e iniciar a lógica de ataque
    private void CompareMonsters()
    {
        Monster monster1 = detectedMonsters[0];
        Monster monster2 = detectedMonsters[1];

        if (monster1.attackValue > monster2.attackValue)
        {
            monster1.Attack();
            monster2.Guard();
        }
        else if (monster1.attackValue < monster2.attackValue)
        {
            monster1.Guard();
            monster2.Attack();
        }
        else
        {
            Debug.Log("Ambos os monstros têm o mesmo valor de ataque.");
        }
    }
}
