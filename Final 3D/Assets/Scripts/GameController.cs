using System;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _maxTimeSeconds;
    [SerializeField] private GameObject _startGameUI;
    [SerializeField] private GameObject _gameOverUI;

    // Add member variables to store references to all of the UI text.
    [SerializeField] private TMP_Text _textStart;
    [SerializeField] private TMP_Text _textGameOver;
    [SerializeField] private TMP_Text _textCommonMult;
    [SerializeField] private TMP_Text _textUncommonMult;
    [SerializeField] private TMP_Text _textRareMult;
    [SerializeField] private TMP_Text _textFinalCommon;
    [SerializeField] private TMP_Text _textFinalUncommon;
    [SerializeField] private TMP_Text _textFinalRare;
    [SerializeField] private TMP_Text _textTotal;
    [SerializeField] private TMP_Text _textRestart;

    private int _commonPoints;
    private int _uncommonPoints;
    private int _rarePoints;

    private float _timeRemaining;
    private bool _runGame;

    // ------------------------------------------------------------------------
    // You don't need to code anything in this method :)
    private void Start()
    {
        SetGameEnabled(false);
        UpdatePointsText();
    }

    // ------------------------------------------------------------------------
    private void Update()
    {
        if (_runGame)
        {
            _timeRemaining -= Time.deltaTime;
            
            // Update the game timer text.

            if (_timeRemaining <= 0)
            {
                EndGame();
            }
        }
    }

    // ------------------------------------------------------------------------
    public void StartGame()
    {
        _startGameUI.SetActive(false);
        _gameOverUI.SetActive(false);
        
        SetGameEnabled(true);

        _timeRemaining = _maxTimeSeconds;
        
        // Update the game timer text.
    }

    // ------------------------------------------------------------------------
    // You don't need to code anything in this method :)
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    // ------------------------------------------------------------------------
    private void EndGame()
    {
        SetGameEnabled(false);
        _gameOverUI.SetActive(true);

        // Update the final score text in the game over UI.
        _textFinalCommon.text = _commonPoints + " common";
        _textFinalUncommon.text = _uncommonPoints + " uncommon";
        _textFinalRare.text = _rarePoints + " rare";
        _textTotal.text = "Final Score: " + _commonPoints + _uncommonPoints + _rarePoints;
    }

    // ------------------------------------------------------------------------
    // You don't need to code anything in this method :)
    private void SetGameEnabled(bool enabled)
    {
        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
        _player.enabled = enabled;
        _runGame = enabled;
    }

    // ------------------------------------------------------------------------
    // Add a paramter to this method to represent the item's rarity.
    public void CollectItem(Rarity itemRarity)
    {
        // Using a SWITCH STATEMENT,
        // based on the item's rarity, update the appropriate points variable.
        switch(itemRarity)
        {
            case Rarity.common:
                _commonPoints++;
                break;
            case Rarity.uncommon:
                _uncommonPoints++;
                break;
            case Rarity.rare:
                _rarePoints++;
                break;
        }

        UpdatePointsText();
    }

    // ------------------------------------------------------------------------
    public void RemoveItem(Rarity itemRarity)
    {
        // Using a SWITCH STATEMENT,
        // based on the item's rarity, update the appropriate points variable.
        switch(itemRarity)
        {
            case Rarity.common:
                _commonPoints--;
                break;
            case Rarity.uncommon:
                _uncommonPoints--;
                break;
            case Rarity.rare:
                _rarePoints--;
                break;
        }

        UpdatePointsText();
    }

    // ------------------------------------------------------------------------
    private void UpdatePointsText()
    {
        // Update your points text.
    }
}