using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public delegate void TeamCardHandler(Action action, int teamId, int playerId);
    public event TeamCardHandler OnCardActivate;

    public delegate void TeamPlayerEmpty(int teamId);
    public event TeamPlayerEmpty OnTeamPlayerHealthEmpty;
    public event TeamPlayerEmpty OnTeamPlayerStaminaEmpty;

    [SerializeField] private Player _playerPrefab = null;
    private int _id = -1;
    private List<Player> _players = new List<Player>();
    private int _playerAmount = 1;
    private RectTransform _rectTransform = null;

    public void Initialize(int id)
    {
        _id = id;
        _rectTransform = gameObject.GetComponent<RectTransform>();
        Debug.Assert(_rectTransform, "No RectTransform found on Team Object");
        for (int i = 0; i < _playerAmount; i++)
        {
            Player newPlayer = GameObject.Instantiate(_playerPrefab);
            newPlayer.Initialize(i);
            newPlayer.OnActivate += DoCardActivate;
            newPlayer.OnPlayerHealthEmpty += OnPlayerHealthEmpty;
            newPlayer.OnPlayerStaminaEmpty += OnPlayerStaminaEmpty;
            _players.Add(newPlayer);
            newPlayer.transform.SetParent(gameObject.transform);
        }        
    }

    public void Tick()
    {

    }

    public void ApplyHealthChange(float healthChange, int playerIdx)
    {
        _players[playerIdx].DoHealthChange(healthChange);
    }

    public void ApplyStaminaChange(float staminaChange, int playerIdx)
    {
        _players[playerIdx].DoStaminaChange(staminaChange);
    }

    public void RechargeTeamStamina()
    {
        foreach(Player player in _players)
        {
            player.RechargeStamina();
        }
    }

    public void OnPlayerHealthEmpty()
    {
        OnTeamPlayerHealthEmpty.Invoke(_id);
    }

    public void OnPlayerStaminaEmpty()
    {
        OnTeamPlayerStaminaEmpty.Invoke(_id);
    }

    public void SetPosition(Vector2 newPosition)
    {
        _rectTransform.anchoredPosition = newPosition;
    }

    public void SetRotation(float angle)
    {
        _rectTransform.Rotate(new Vector3(0, 0, angle));
    }

    public int GetPlayerAmount()
    {
        return _playerAmount;
    }

    public void EnableTeamCardInput(bool isEnabled)
    {
        foreach(Player player in _players)
        {
            player.EnableCardInput(isEnabled);
        }
    }

    private void DoCardActivate(Action action, int playerId)
    {
        OnCardActivate?.Invoke(action, _id, playerId);
    }
}
