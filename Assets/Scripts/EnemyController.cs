using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _attackDamage;
    [SerializeField] private GameObject _dieEffect;
    [SerializeField] private float _rechargeTime;
    private int _hp;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _player;
    private Collider _collider;
    public bool isRun = true;
    public bool isRecharging = false;
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = GameManager.Instance.Player.GetComponent<Transform>();
        _collider = GetComponent<Collider>();
        _agent.SetDestination(_player.position);
        _hp = _maxHealth;
        StartCoroutine(UpdateDectinatoinAgent());
    }

    void FixedUpdate()
    {
        var speed = _agent.velocity.magnitude;
        _animator.SetFloat("Speed", speed);

        // Анимация Idle слишком долгая делаем автовключение анимации ходьбы вручную
        if (speed > 0.01)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyIdle"))
                _animator.Play("EnemyWalking");
        }
    }

    private void OnEnable()
    {
        if (_agent != null)
            StartCoroutine(UpdateDectinatoinAgent());
        isRecharging = false;
    }

    private void OnDisable()
    {
        _hp = _maxHealth;
    }

    public IEnumerator UpdateDectinatoinAgent()
    {
        _agent.SetDestination(_player.position);
        yield return new WaitForSeconds(0.7f);
        if (isRun)
            yield return StartCoroutine(UpdateDectinatoinAgent());
        else
            yield break;
    }
    public IEnumerator RechargeAttack()
    {
        isRecharging = true;
        yield return new WaitForSeconds(_rechargeTime);
        isRecharging = false;
        yield break;
    }

    public void OnTriggerStay(Collider other)
    {
        PlayerCharacterController _playerController;
        if (other.gameObject.TryGetComponent<PlayerCharacterController>(out _playerController))
        {
            if (!isRecharging)
            {
                _playerController.TakeDamage(_attackDamage);
                StartCoroutine(RechargeAttack());
            }
        }
    }

    public void Hit(int damage, Vector3 hitPoint)
    {
        var head = _collider.bounds.max;
        // Если дистанция до вершины коллайдера меньше X % 
        if (Mathf.Abs(head.y - hitPoint.y) < head.y * 0.15)
            damage *= 2;
        _hp -= damage;
        if (IsDie())
        {
            GlobalEventsManager.KillEnemy();
            var spawnPositoin = gameObject.transform.position;
            spawnPositoin.y += 1;
            Instantiate(_dieEffect, _collider.bounds.center, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    public bool IsDie() => _hp <= 0;

   


}
