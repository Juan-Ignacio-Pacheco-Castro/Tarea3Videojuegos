using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    [SerializeField] private ParticleSystem particles;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    public void Awake(){
      //  particles = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
   
    private void OnCollisionEnter2D(Collision2D other){
        //Debug.Log("Colisión del objeto " + other.gameObject.name + "con el bloque: " + this.gameObject.name);
        StartCoroutine(DeleteObject());
    }


    private IEnumerator DeleteObject(){
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        particles.Play();
        yield return new WaitForSeconds(particles.main.startLifetime.constantMax);
        Destroy(this.gameObject);
    }

}
