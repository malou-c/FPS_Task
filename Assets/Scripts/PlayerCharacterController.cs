using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(CharacterController), typeof(PlayerInputHandler), typeof(AudioSource))]
public class PlayerCharacterController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private PauseMenu _pauseMenu;

    [Header("Character")]
    [SerializeField] private float _speed;
    private float _xRotation;
    private Vector3 _velocity;
    private Health _health;
    private CharacterController _characterController;
    private PlayerInputHandler _playerInput;

    [Header("Audio")]
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _shootClip;
    private AudioSource _audioSource;
    private AudioActiveStatus _audioActive;

    [Header("Weapon")]
    [SerializeField] private int _damage;
    [SerializeField] private float _firingRange;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private GameObject _smokeEffect;
    [SerializeField] private GameObject _hitEffect;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _groundDistance;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float G;
    [SerializeField] private LayerMask _groundMask;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput= GetComponent<PlayerInputHandler>();
        _audioSource = GetComponent<AudioSource>();
        _health = GetComponent<Health>();
        _audioActive = GameManager.Instance.AudioActive;
        Debug.Log(_audioActive);

    }

    private void Update()
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = CheckGround();
        if (wasGrounded && !_isGrounded && Cursor.lockState == CursorLockMode.Locked)
            _velocity.y = -0.05f;

        HandleCharacterMovement();
        HandleFire();
        HandleUI();
    }

    private bool CheckGround() => Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

    private void HandleCharacterMovement()
    {
        // Look
        float mouseX = _playerInput.GetLookInputsHorizontal() * Time.deltaTime;
        float mouseY = _playerInput.GetLookInputsVertical() * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -89f, 89f);
        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movement
        Vector3 move = _playerInput.GetMoveInput();
        _characterController.Move(move * _speed * Time.deltaTime);
        if (!_isGrounded)
        {
            _velocity.y += G * Time.deltaTime;
            _characterController.Move(_velocity);
        }
        // Jump
        if (_isGrounded && _playerInput.GetJumpInputDown())
        {
            _velocity.y = _jumpHeight;
            _characterController.Move(_velocity);
            _isGrounded = false;
        }
    }

    private void HandleFire()
    {
        if (_playerInput.GetFireInputDown())
        {
            _muzzleFlash.Play();
            if (_audioActive == AudioActiveStatus.ON)
            {
                Debug.Log($"BAS");
                _audioSource.PlayOneShot(_shootClip);
            }

            RaycastHit hit;
            if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _firingRange))
            {
                EnemyController _enemy;
                if (hit.collider.gameObject.TryGetComponent(out _enemy))
                {
                    Instantiate(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    _enemy.Hit(_damage, hit.point);
                }
                else if (hit.collider.tag != "Barier")
                {
                    Instantiate(_smokeEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
    }

    private void HandleUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            _pauseMenu.gameObject.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        _health.OnTakeDamage(damage);
        if (_audioActive == AudioActiveStatus.ON)
            _audioSource.PlayOneShot(_hitClip);

        if (_health.IsDie())
        {
            GlobalEventsManager.DiePlayerEvent.Invoke();
            _pauseMenu.gameObject.SetActive(true);
            _pauseMenu.SetActivePlayButton(false);
        }
    }

    public void ResetPlayer(Transform spawnPoint)
    {
        _health.ResetHp();
        var move = spawnPoint.position - transform.position;
        _characterController.Move(move);
    }
}
