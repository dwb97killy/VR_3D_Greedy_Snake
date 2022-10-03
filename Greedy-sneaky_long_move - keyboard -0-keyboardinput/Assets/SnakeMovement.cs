using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public List<Transform> brick;
    Coroutine[] temp;
    public float range = 10;//摆动范围
    private void Start()
    {
        temp = new Coroutine[brick.Count];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //StopAllCoroutines();
            if (temp[0] != null)
                StopCoroutine(temp[0]);
            temp[0] = StartCoroutine(FirstMove(range));
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            //StopAllCoroutines();
            if (temp[0] != null)
                StopCoroutine(temp[0]);
            temp[0] = StartCoroutine(FirstMove(0));
        }
    }

    IEnumerator FirstMove(float moveTo)
    {
        if (temp[1] != null)
            StopCoroutine(temp[1]);
        temp[1] = StartCoroutine(Follow(1, moveTo));
        float speed = 0.1f;
        while (Mathf.Abs(brick[0].position.x - moveTo) > 0.01)
        {
            Vector3 temp = brick[0].position;
            temp.x = Mathf.Lerp(temp.x, moveTo, speed);
            brick[0].position = temp;
            speed += Time.deltaTime;
            yield return null;
        }
        Vector3 temp1 = brick[0].position;
        temp1.x = moveTo;
        brick[0].position = temp1;
    }


    IEnumerator Follow(int i, float moveTo)
    {
        yield return new WaitForSeconds(0.05f);
        float speed = 0.1f;
        if (i < brick.Count - 1)
        {
            if (temp[i + 1] != null)
                StopCoroutine(temp[i + 1]);
            temp[i + 1] = StartCoroutine(Follow(i + 1, moveTo));
        }
        while (Mathf.Abs(brick[i].position.x - moveTo) > 0.05)
        {
            Vector3 temp = brick[i].position;
            temp.x = Mathf.Lerp(temp.x, moveTo, speed);
            brick[i].position = temp;
            speed += Time.deltaTime;
            yield return null;
        }
        Vector3 temp1 = brick[i].position;
        temp1.x = moveTo;
        brick[i].position = temp1;
        temp[i] = null;
    }
}