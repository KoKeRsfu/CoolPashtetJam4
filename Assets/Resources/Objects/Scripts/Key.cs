using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject door;
    public Sprite opened;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator KillYourself(float angle)
    {
        float elapsedTime = Time.deltaTime;
        float maxTime = 5f;
        while (elapsedTime < maxTime)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, angle * elapsedTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && !collision.gameObject.GetComponent<PlayerController>().stopVariables.stop)
        {
            door.GetComponent<BoxCollider2D>().enabled = false;
            door.GetComponent<SpriteRenderer>().sprite = opened;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            this.GetComponent<Rigidbody2D>().gravityScale = 10f;
            this.GetComponent<BoxCollider2D>().enabled = false;
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10f, 10f), 20f), ForceMode2D.Impulse);
            StartCoroutine(KillYourself(Random.Range(-30f, 30f)));
        }
    }
}