using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    // Serialized Fields (config params)
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
  
    [SerializeField] Sprite[] hitSprites;
    [SerializeField] bool increaseSpeed;
    [SerializeField] bool decreaseSpeed;
    [SerializeField] bool shrinkPaddle;
    [SerializeField] bool expandPaddle;

    //cached reference
    Level level;
    GameSession gameSession;
    Paddle paddle;


    //state variables
    [SerializeField] int timesHit; //TODO only serialized for DEBUG purpose

    public void Start()
    {
        CountBreakableBlocks();
        gameSession = FindObjectOfType<GameSession>();
        paddle = FindObjectOfType<Paddle>();

    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        timesHit++;
        int maxHits = hitSprites.Length+1;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Block sprite is missing from array" + gameObject.name);
        }
    }

    private void DestroyBlock()
    {
        PlayBlockDestroySFX();
        Destroy(gameObject);
        level.BlockDestroyed();

        TriggerSparklesVFX();
        HandleGameEffects();
    }

    private void HandleGameEffects()
    {
        if (increaseSpeed)
        {
            gameSession.IncreaseGameSpeed();
        }
        else if (decreaseSpeed)
        {
            gameSession.DecreaseGameSpeed();
        }
        else if (shrinkPaddle)
        {
            paddle.ShrinkPaddle();
        }
        else if (expandPaddle)
        {
            paddle.ExpandPaddle();
        }
    }

    private void PlayBlockDestroySFX()
    {
        FindObjectOfType<GameSession>().AddToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX,transform.position,transform.rotation);
        Destroy(sparkles, 1f);
    }
}
