using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private PlayerCharacterController _player;
    [SerializeField] private Score _score;

    private SpawnEnemy _enemySpawner;
    private AudioActiveStatus _audioActive;
    public PlayerCharacterController Player { get => _player; }
    public AudioActiveStatus AudioActive { get => _audioActive; }

    void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this; 
        else if (Instance == this)
            Destroy(gameObject);

        _enemySpawner = gameObject.GetComponent<SpawnEnemy>();
        _audioActive = (AudioActiveStatus)PlayerPrefs.GetInt("AUDIO_ACTIVE");
        Debug.Log(AudioActive);
    }

    public void RestartGame()
    {
        _player.ResetPlayer(_playerSpawnPoint);
        _enemySpawner.Restart();
        _score.ResetScore();
    }
}
