using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.SceneManagement; // Import necessário para carregar cenas

public class MonsterBattleHandler : MonoBehaviour
{
    private List<Monster> detectedMonsters = new List<Monster>();
    private bool battleHappened = false;

    public int player1Life = 4000;
    public int player2Life = 4000;

    public Text player1LifeText;
    public Text player2LifeText;

    public SpriteSwitcher PlayerOneProfile;
    public SpriteSwitcher PlayerTwoProfile;

    public Canvas gameOverCanvas;

    private bool gameEnded;

    void Start()
    {
        ObserverBehaviour[] observers = FindObjectsOfType<ObserverBehaviour>();

        foreach (var observer in observers)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        UpdateLifeUI();
    }

    void Update()
    {
        // Verifica se o jogo acabou e se a tecla Espaço foi pressionada
        if (gameEnded && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MainMenu"); // Carrega o menu principal
        }
    }

    private void UpdateLifeUI()
    {
        player1LifeText.text = player1Life.ToString();
        player2LifeText.text = player2Life.ToString();
    }

    private void OnTargetStatusChanged(ObserverBehaviour target, TargetStatus status)
    {
        if (gameEnded)
        {
            return;
        }

        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            AddMonster(target);
        }
        else
        {
            RemoveMonster(target);
        }

        if (detectedMonsters.Count == 2 && !battleHappened)
        {
            StartCoroutine(WaitForBothMonstersToBeIdle());
        }
    }

    private IEnumerator WaitForBothMonstersToBeIdle()
    {
        while (!BothMonstersIdle())
        {
            yield return null;
        }

        CompareMonsters();
        battleHappened = true;
    }

    private bool BothMonstersIdle()
    {
        return detectedMonsters[0].IsIdle() && detectedMonsters[1].IsIdle();
    }

    private void AddMonster(ObserverBehaviour target)
    {
        Monster monster = target.GetComponentInChildren<Monster>();
        if (monster != null && !detectedMonsters.Contains(monster))
        {
            detectedMonsters.Add(monster);
            monster.DoTypeSound();
        }
    }

    private void RemoveMonster(ObserverBehaviour target)
    {
        Monster monster = target.GetComponentInChildren<Monster>();
        if (monster != null && detectedMonsters.Contains(monster))
        {
            monster.RestoreInitialScale();

            detectedMonsters.Remove(monster);
            battleHappened = false;
        }
    }

    private void CompareMonsters()
    {
        Monster monster1 = detectedMonsters[0];
        Monster monster2 = detectedMonsters[1];

        bool isMonster1OnLeft = IsMonsterOnLeft(monster1.transform);
        bool isMonster2OnLeft = IsMonsterOnLeft(monster2.transform);

        if (monster1.attackValue > monster2.attackValue)
        {
            StartCoroutine(HandleBattleSequence(monster1, monster2, isMonster2OnLeft));
        }
        else if (monster1.attackValue < monster2.attackValue)
        {
            StartCoroutine(HandleBattleSequence(monster2, monster1, isMonster1OnLeft));
        }
        else
        {
            Tie(monster1, monster2);
        }
    }

    private IEnumerator Tie(Monster monster1, Monster monster2)
    {
        yield return StartCoroutine(ApplyPopoutEffect(monster1, monster2, 0.5f));
    }

    private IEnumerator HandleBattleSequence(Monster attacker, Monster defender, bool isDefenderOnLeft)
    {
        attacker.Attack(defender);
        defender.Guard();

        int damage = attacker.attackValue - defender.attackValue;
        if (isDefenderOnLeft)
        {
            player1Life -= damage;
            PlayerOneProfile.SwitchSprite(2);
            PlayerTwoProfile.SwitchSprite(3);
        }
        else
        {
            player2Life -= damage;
            PlayerOneProfile.SwitchSprite(3);
            PlayerTwoProfile.SwitchSprite(2);
        }

        yield return new WaitUntil(() => !attacker.IsAttacking() && !attacker.IsAttackingProperty());

        defender.EndGuard();

        yield return new WaitUntil(() => !defender.IsDefending());

        PlayerOneProfile.SwitchSprite(1);
        PlayerTwoProfile.SwitchSprite(1);

        yield return StartCoroutine(ApplyPopoutEffect(attacker, defender, 0.5f));

        UpdateLifeUI();

        CheckGameOver();
    }

    private IEnumerator ApplyPopoutEffect(Monster attacker, Monster defender, float duration)
    {
        float elapsedTime = 0f;

        // Escala final (zero)
        Vector3 targetScale = Vector3.zero;

        // Aplica o efeito de popout
        while (elapsedTime < duration)
        {
            attacker.transform.localScale = Vector3.Lerp(attacker.GetInitialScale(), targetScale, elapsedTime / duration);
            defender.transform.localScale = Vector3.Lerp(defender.GetInitialScale(), targetScale, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garante que ambos atinjam a escala final
        attacker.transform.localScale = targetScale;
        defender.transform.localScale = targetScale;

        Debug.Log("Popout Effect concluído.");
    }

    private void CheckGameOver()
    {
        if (player1Life <= 0)
        {
            EndGame("Player 2 venceu!");

            PlayerOneProfile.SwitchSprite(2);
            PlayerTwoProfile.SwitchSprite(1);
        }
        else if (player2Life <= 0)
        {
            EndGame("Player 1 venceu!");

            PlayerOneProfile.SwitchSprite(1);
            PlayerTwoProfile.SwitchSprite(2);
        }
    }

    private void EndGame(string message)
    {
        gameOverCanvas.gameObject.SetActive(true);

        Text gameOverText = gameOverCanvas.GetComponentInChildren<Text>();
        if (gameOverText != null)
        {
            gameOverText.text = message;
        }

        gameEnded = true;
    }

    private bool IsMonsterOnLeft(Transform monsterTransform)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(monsterTransform.position);
        return screenPos.x < Screen.width / 2;
    }
}
