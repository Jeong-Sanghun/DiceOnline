using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    [SerializeField]
    GameObject[] dice;
    [SerializeField]
     Rigidbody[] diceRigidbody;
    [SerializeField]
    GameObject rollButton;
    [SerializeField]
    Text myText;
    [SerializeField]
     GameManager gameManager;
    int[] eyes = new int[2];

    public void Start()
    {
        Time.timeScale = 3.0f;
        RollButtonActive(true);
    }

    public void RollButtonActive(bool active)
    {
        rollButton.SetActive(active);
    }

    public void RollDice()
    {
        RollButtonActive(false);
        for (int i = 0; i < dice.Length; i++)
        {
            dice[i].SetActive(true);
            diceRigidbody[i].velocity = Vector3.zero;
            diceRigidbody[i].AddTorque(Random.Range(-200f, 200f), Random.Range(-200f, 200f),
                Random.Range(-200f, 200f), ForceMode.Acceleration);

            diceRigidbody[i].AddForce(Vector3.up * Random.Range(2f, 5f), ForceMode.Impulse); ;
        }

        StartCoroutine(CheckCoroutine());
        

    }

    int EyeReader(int index)
    {

        Transform[] childEyeTransform = new Transform[6];
        float max = 0;
        int maxIndex = 0;

        for (int i = 0; i < 6; i++)
        {
            childEyeTransform[i] = dice[index].transform.GetChild(i);
        }
        max = childEyeTransform[0].position.y;
        maxIndex = 0;
        for (int i = 1; i < 6; i++)
        {
            if (max < childEyeTransform[i].position.y)
            {
                max = childEyeTransform[i].position.y;
                maxIndex = i;
            }
        }

        return maxIndex;
    }



    IEnumerator CheckCoroutine()
    {
        //¾È ³©±É´ÂÁö ÆÇ´ÜÇØÁÖ´Â ÄÚ·çÆ¾.
        yield return new WaitForSeconds(2f);
        
        while (diceRigidbody[0].velocity.sqrMagnitude > 0.1f
            || diceRigidbody[1].velocity.sqrMagnitude > 0.1f)
        {
            yield return new WaitForSeconds(1f);
        }


        for(int i = 0; i < dice.Length; i++)
        {
            eyes[i] = -1;
            eyes[i] = EyeReader(i)+1;
        }

        if(eyes[0] == eyes[1])
        {
            OnSameEye();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            OnDifferentEye();
        }
        

    }


    void OnSameEye()
    {
        myText.text = "´«ÀÌ °°¾Æ¿ä" + eyes[0].ToString() + "ÀÌ¶û "+ eyes[1].ToString();
    }

    void OnDifferentEye()
    {
        myText.text = "´Ù½Ã±¼·Á¿ä" + eyes[0].ToString() + "ÀÌ¶û " + eyes[1].ToString(); ;
        RollButtonActive(true);
    }
}
