using UnityEngine;
using System;
using System.Collections.Generic;

public class BallScripts : MonoBehaviour
{
    public GameObject[] bombs;

    int rowCount = 0;

    public bool isAllowOneDown = false;
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildBlocks()
    {

        float bombWidth = bombs[0].GetComponent<Renderer>().bounds.size.x;
        float ballWidth = gameObject.GetComponent<Renderer>().bounds.size.x;

        Vector3 upperCornet = new Vector3(Screen.width, Screen.height, 0);
        Vector3 targetWidth = Camera.main.ScreenToWorldPoint(upperCornet);

        rowCount = (int) (targetWidth.y / bombWidth);

        for (int i = 0; i < rowCount; i++)
        {

            int randomBombIndex = UnityEngine.Random.Range(0, bombs.Length);

            leftBombs.Add((GameObject) Instantiate(bombs[randomBombIndex], new Vector3(-targetWidth.x + (bombWidth/2), i * bombWidth, 0), Quaternion.identity));
            rightBombs.Add(Instantiate(bombs[(randomBombIndex + 1 ) % bombs.Length], new Vector3(+targetWidth.x - (bombWidth / 2), i * bombWidth, 0), Quaternion.identity));
        }
    }

    private void FixedUpdate()
    {
        moveDownBombs();
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

                for (int i = 0; i < rowCount -1; i++)
                {
                    leftBombs[i] = leftBombs[i + 1];
                    rightBombs  [i] = rightBombs[i + 1];
                }
            }
        }

    }
}
