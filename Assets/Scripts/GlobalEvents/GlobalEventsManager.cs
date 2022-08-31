using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventsManager
{
    public static readonly UnityEvent<int> AttackOnPlayerEvent = new UnityEvent<int>();
    public static readonly UnityEvent DiePlayerEvent = new UnityEvent();
    public static readonly UnityEvent KillEnemyEvent = new UnityEvent();

    public static void AttackOnPlayer(int damage) => AttackOnPlayerEvent.Invoke(damage);
    public static void DiePlayer() => DiePlayerEvent.Invoke();
    public static void KillEnemy() => KillEnemyEvent.Invoke();

}