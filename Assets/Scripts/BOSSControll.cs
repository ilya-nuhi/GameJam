using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSControll : MonoBehaviour
{
    public Transform player;
    [SerializeField] GameObject bulletBoss;
    [SerializeField] Transform[] gunBoss;
    public bool canAttack=true;
	public bool isFlipped = false;

    public Vector2 direction;
	public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}

    public void Attack(){
        StartCoroutine(startAttack());
    }

    IEnumerator startAttack()
    {
        direction = new Vector2(player.position.x-transform.position.x,player.position.y-transform.position.y);
        direction.Normalize();
        canAttack = false;
        Instantiate(bulletBoss, gunBoss[0].position, Quaternion.identity);
        Instantiate(bulletBoss, gunBoss[1].position, Quaternion.identity);
        Instantiate(bulletBoss, gunBoss[2].position, Quaternion.identity);
        yield return new WaitForSeconds(2);
        canAttack = true;
    }
}
