using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    [Header("References")]
    [SerializeField] private EggCounterUI _eggCounterUI;
    [SerializeField] private WinLoseUI _winLoseUI;

    [Header("Settings")]
    [SerializeField] private int _maxEggCount = 5;

    private int _currentEggCount = 0;

    private GameState _currentGameState;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        ChangeGameState(GameState.Play);
    }

    public void ChangeGameState(GameState gameState)
    {
        OnGameStateChanged?.Invoke(gameState);
        _currentGameState = gameState;
        Debug.Log("Game State: " + gameState);
    }

    public void OnEggCollected()
    {
        Debug.Log("OnEggCollected ÇAĞRILDI!");

        _currentEggCount++;
        _eggCounterUI.SetEggCounterText(_currentEggCount, _maxEggCount);

        if (_currentEggCount == _maxEggCount)
        {
           //win
            _eggCounterUI.SetEggCompleted();
            ChangeGameState(GameState.GameOver);
            _winLoseUI.OnGameWin();
        }

        Debug.Log("Egg Count: " + _currentEggCount);
    }
    public GameState GetCurrentGameState()
{
    return _currentGameState;
}

}
