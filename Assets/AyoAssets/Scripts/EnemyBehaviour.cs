using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Instantiator instantiator;
    private Animator anim;
    bool shouldMove = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if(anim == null)
        {
            Debug.Log("No anim");
        }
        else
        {
            Debug.Log("Anim");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldMove == true)
        {
            transform.Translate(Vector2.left * instantiator.activeRate * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ScreenX"))
        {
            Destroy(this.gameObject);
            //Debug.Log("Collided with x");
        }
        if (collision.gameObject.CompareTag("ScreenY"))
        {
            instantiator.DelayedInstantiation();
            //Debug.Log("Collided with y");
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with player");
            anim.SetTrigger("OnEnemyDeath");
            shouldMove = false;
            Destroy(this.gameObject, .667f);
        }
    }
}
