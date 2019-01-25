using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class BallScripts : MonoBehaviour
{
    public GameObject[] bombs;
    public GameObject sparks;
    public GameObject explosion;
    public GameObject background;

    GameObject initialSparks = null;
    GameObject exp = null;

    int rowCount = 0;

    bool isAllowOneDown = false;
    private bool isCatchedValue = false;

    public bool IsCatchedValue
    {
        get { return isCatchedValue; }
        set { isCatchedValue = value; }
    }

    List<GameObject> leftBombs = new List<GameObject>();
    List<GameObject> rightBombs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        BuildBlocks();
        letsMove();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildBlocks(bool isOneLine = false)
    {

        float bombWidth = bombs[0].GetComponent<Renderer>().bounds.size.x;
        float ballWidth = gameObject.GetComponent<Renderer>().bounds.size.x;

        Vector3 upperCornet = new Vector3(Screen.width, Screen.height, 0);
        Vector3 targetWidth = Camera.main.ScreenToWorldPoint(upperCornet);

        rowCount = (int) (targetWidth.y / bombWidth);

        int startValue = 0;

        if (isOneLine)
        {
            startValue = rowCount - 1;
        }

        for (int i = startValue; i < rowCount; i++)
        {

            int randomBombIndex = UnityEngine.Random.Range(0, bombs.Length);
            GameObject obj = null;

            obj = (GameObject) Instantiate(bombs[randomBombIndex], new Vector3(-targetWidth.x + (bombWidth / 2), i * bombWidth, 0), Quaternion.identity);
            if (isOneLine)
            {
                leftBombs[i] = obj;
            }
            else
            {
                leftBombs.Add(obj);
            }

            obj = Instantiate(bombs[(randomBombIndex + 1) % bombs.Length], new Vector3(+targetWidth.x - (bombWidth / 2), i * bombWidth, 0), Quaternion.identity);
            if (isOneLine)
            {
                rightBombs[i] = obj;
            }
            else
            {
                rightBombs.Add(obj);
            }
        }

        if (!initialSparks)
        {
            Vector3 initialPosition;
            if (UnityEngine.Random.Range(0, 100) > 50)
            {
                initialPosition = leftBombs[0].transform.position;
            }
            else
            {
                initialPosition = rightBombs[0].transform.position;
            }

            initialSparks = (GameObject) Instantiate(sparks, initialPosition + new Vector3(bombWidth/7, bombWidth/3, 0), Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        moveDownBombs();

        if (Input.touchCount == 1 && (Input.GetTouch(0).deltaPosition.x > 0.1f || Input.GetTouch(0).deltaPosition.x <-0.1))
        {
            switchDirection(); 
        }
    }

    private void moveDownBombs()
    {

        if (isAllowOneDown)
        {
            if (!isCatchedValue)
            {
                for (int i = rowCount - 1; i > 0; i--)
                {

                    leftBombs[i].GetComponent<Rigidbody2D>().MovePosition(leftBombs[i].GetComponent<Rigidbody2D>().position + new Vector2(0, -5 * Time.deltaTime));
                    rightBombs[i].GetComponent<Rigidbody2D>().MovePosition(rightBombs[i].GetComponent<Rigidbody2D>().position + new Vector2(0, -5 * Time.deltaTime));

                    background.transform.Translate(new Vector3(0, -0.5f * Time.deltaTime, 0));
                    if (background.transform.position.y < -5.7f)
                    {
                        background.transform.position = new Vector3(0, 4.5f, 0);
                    }
                }

            }
            else
            {
                Destroy(leftBombs[0].gameObject);
                Destroy(rightBombs[0].gameObject);

                for (int i = rowCount -1; i > 0; i--)
                {
                    leftBombs[i].GetComponent<Rigidbody2D>().MovePosition(leftBombs[i].GetComponent<Rigidbody2D>().position + new Vector2(0, 0.2f));
                    rightBombs[i].GetComponent<Rigidbody2D>().MovePosition(rightBombs[i].GetComponent<Rigidbody2D>().position + new Vector2(0, 0.2f));
                }

                isAllowOneDown = false;
                isCatchedValue = false;

                Destroy(initialSparks);
                initialSparks = null;

                Destroy(exp);

                for (int i = 0; i < rowCount -1; i++)
                {
                    leftBombs[i] = leftBombs[i + 1];
                    rightBombs  [i] = rightBombs[i + 1];
                }

                BuildBlocks(true);

                GetComponent<Rigidbody2D>().position = new Vector2(0, 0);
                letsMove();
            }
        }
    }

    void letsMove()
    {

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        float force = 0;

        if (UnityEngine.Random.Range(0, 100) < 50)
        {
            force = 200f;
        }
        else
        {
            force = -200f;
        }

        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0));

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.x * initialSparks.transform.position.x > 0)
        {
            exp = Instantiate(explosion, transform.position, Quaternion.identity);
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().position = new Vector2(0, -5);


        isAllowOneDown = true;

    }

    private void OnMouseDown()
    {
        switchDirection();
    }

    void switchDirection()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(-GetComponent<Rigidbody2D>().velocity.x * 5, 0);
    }
}
