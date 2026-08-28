using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Elevation_Entry : MonoBehaviour
{

    public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;
    public bool isHigh=false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player"&&isHigh==false){
            foreach (Collider2D mountain in mountainColliders)
            {
                mountain.enabled = false;
            }

            foreach (Collider2D boundary in boundaryColliders)
            {
                boundary.enabled = true;
            }

            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 15;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"&&isHigh==false)
        {
            isHigh=true;
        } else if (collision.gameObject.tag == "Player"&&isHigh==true)
            {
                foreach (Collider2D mountain in mountainColliders)
                {
                    mountain.enabled = true;
                }

                foreach (Collider2D boundary in boundaryColliders)
                {
                    boundary.enabled = false;
                }

                isHigh=false;
                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
    }
}
