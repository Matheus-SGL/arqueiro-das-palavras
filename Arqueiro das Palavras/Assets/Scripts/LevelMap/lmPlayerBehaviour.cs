using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class pathWay
{
    public Transform curva;
}

public class lmPlayerBehaviour : MonoBehaviour
{

    public List<pathWay> pathWay = new List<pathWay>();
    public Rigidbody2D playerBody;

    public bool moveToNext;
    public int passCurves = 1;
    public float distMinCurva;
	public bool atualFase;

    // Use this for initialization
    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        SpriteRenderer playerMash = GetComponent<SpriteRenderer>();

        if (movementVector != Vector2.zero)
        {
            if (movementVector.x < 0)
			{
                playerMash.flipX = true;
			}
            else
			{
                playerMash.flipX = false;
				if (atualFase)
					moveToNext = true;
			}
        }

        if (moveToNext)
        {
			if (passCurves == 6)
			{
				moveToNext = false;
				passCurves = 6;
				atualFase = true;
			}
			transform.position = Vector2.MoveTowards(transform.position, pathWay[passCurves].curva.position, 0.05f);

            float distancia = Vector2.Distance(transform.position, pathWay[passCurves].curva.position);
			
            if (distancia <= distMinCurva)
            {
                passCurves++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
