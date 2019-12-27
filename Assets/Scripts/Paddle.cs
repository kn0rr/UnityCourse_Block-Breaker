using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{   
    // Configuration Parameters
    [SerializeField] float screenWidthInUnits=16f;
    // paddle has a size of 2 and therefore middle point is 1f
    [SerializeField] float xMin = 1f;
    [SerializeField] float xMax = 15f;

    //cached Reference
    // "FindObjectOfType" is really expensive so it makes sense to cache it!
    GameSession theGameSession;
    Ball theBall;

    // Start is called before the first frame update
    void Start()
    {
        theGameSession = FindObjectOfType<GameSession>();
        theBall = FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);
        paddlePos.x = Mathf.Clamp(GetXPos(),xMin,xMax);
        transform.position = paddlePos;
    }

    private float GetXPos()
    {
        if (theGameSession.IsAutoPlayEnabled()) 
        {
            return theBall.transform.position.x;
        }
        else
        {
            return Input.mousePosition.x / Screen.width * screenWidthInUnits;
        }
    }

    public void ExpandPaddle()
    {
        transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
    }
    public void ShrinkPaddle()
    {
        transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
    }
}
