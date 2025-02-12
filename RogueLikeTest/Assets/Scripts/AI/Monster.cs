using System.Collections;
using System.Collections.Generic;
using AI;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(HitCollider))]
public class Monster : AbstractIA
{
    private MonsterDataInstance m_monsterDataInstance;
    
    private Vector2 basePosition;

    private bool m_moving;
    private float m_wanderingDurationRange;
    private bool isAttacking =false;
    
    protected override void Init()
    {
        base.Init();
        basePosition = transform.position;
        m_monsterDataInstance = (MonsterDataInstance)m_iaABstractDataInstance;
    }
    
    protected override void Wander()
    {
        // leave the base wander to let it go in attack state if player near
        base.Wander();

        if(m_moving)
            return;
            
        m_moving = true;
            
        Vector2 newMoveTarget = new Vector2(Random.Range(basePosition.x - m_monsterDataInstance.rangeWonder, basePosition.x + m_monsterDataInstance.rangeWonder),
            Random.Range(basePosition.y - m_monsterDataInstance.rangeWonder, basePosition.y + m_monsterDataInstance.rangeWonder ));
            
        m_transform.DOMove(newMoveTarget, 1f).OnComplete(() => m_moving = false);
    }

    protected override void Attack()
    {
        base.Attack();
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackLikeCage());
        }
        
        Vector2 direction = (playerTransform.position - m_transform.position);
        direction.Normalize();
        m_rigidbody.velocity = direction * m_monsterDataInstance.speed;
    }
    
    IEnumerator AttackLikeCage()
    {
        yield return new WaitForSeconds(m_monsterDataInstance.timeForSpawn);
        DoCageShoot();
        isAttacking = false;
    }
    
    public void DoCageShoot()
    {
        for (int i = 0; i < Random.Range(m_monsterDataInstance.spawnMin, m_monsterDataInstance.spawnMin + 1); i++)
        {
            var go = Instantiate(m_monsterDataInstance.spawn[Random.Range(0, m_monsterDataInstance.spawn.Count)], transform.parent, true);
            go.transform.position = transform.position + new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f,0.75f),0);
            go.GetComponent<AbstractIA>().ChangeState(AIStates.attacking);
        }
    }
    
}
