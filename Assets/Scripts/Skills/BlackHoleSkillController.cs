using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackHoleSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackHoleTimer;
    
    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotKey = true;
    private bool cloneAttackReleased;
    private bool playCanDisappear = true;
    
    private int amountOfAttack = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;
    
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public bool playerCanExitState { get; private set;}

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttack, float _cloneAttackCooldown,float _blackDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttack = _amountOfAttack;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackHoleTimer = _blackDuration;
    }
    
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if (blackHoleTimer < 0)
        {
            blackHoleTimer = Mathf.Infinity;
            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHoleAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }


        CloneAttackLogic();
        
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), Time.deltaTime * growSpeed);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,new Vector2(-1,-1), Time.deltaTime * shrinkSpeed);
            if(transform.localScale.x < 0)
                Destroy(gameObject);
        }
        
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
            return;
        
        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKey = false;

        if (playCanDisappear)
        {
            playCanDisappear = false;
            PlayerManager.Instance.player.MakeTransparent(true);
        }

    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttack > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;
            
            xOffset = (Random.Range(0,100) > 50) ? 2 : -2;
            
            SkillManager.Instance.clone?.CreateClone(targets[randomIndex],new Vector3(xOffset,0));
            amountOfAttack--;
            if (amountOfAttack <= 0)
            {
                Invoke("FinishBlackHoleAbility", 1f);
            }
            
        }
    }

    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
        
    }

    private void DestroyHotKeys()
    {
        if(createdHotKey.Count <= 0)
            return;
        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }
    
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>();

            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //TODO:没有做冻结的函数
        // if (collision.GetComponent<Enemy>() != null)
        //     collision.GetComponent<Enemy>().FreezeTime(false);
    }

    private void CreateHotKey(Collider2D collision)
    {
        if(keyCodeList.Count <= 0)
            return;
        if(!canCreateHotKey)
            return;
        
        GameObject newHotKey = Instantiate(hotKeyPrefab,collision.transform.position + new Vector3(0,2),Quaternion.identity);
        createdHotKey.Add(newHotKey);
        
        KeyCode chooseKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chooseKey);

        BlackHoleHotKeyController newHotKeyScript = newHotKey.GetComponent<BlackHoleHotKeyController>();
        newHotKeyScript.SetupHotKey(chooseKey,collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
