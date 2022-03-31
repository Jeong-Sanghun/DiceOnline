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
    int[] eyes;

    public void Start()
    {
        Time.timeScale = 3.0f;
        eyes = new int[dice.Length];
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

        while (true)
        {
            bool stopped = true;
            for (int i = 0; i < dice.Length; i++)
            {
                if (diceRigidbody[i].velocity.sqrMagnitude > 0.1f)
                {
                    stopped = false;
                    break;
                }
            }
            if (stopped)
            {
                break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }


        for(int i = 0; i < dice.Length; i++)
        {
            eyes[i] = -1;
            eyes[i] = EyeReader(i)+1;
        }
        myText.text = "´«Àº";
        for (int i = 0; i < dice.Length; i++)
        {
            myText.text += eyes[i].ToString();
            if(i == dice.Length-1)
            {
                myText.text += "ÀÌ¿¡¿ä";
            }
            else
            {
                myText.text += "ÀÌ¶û ";
            }
                
        }

        RollButtonActive(true);

        

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
