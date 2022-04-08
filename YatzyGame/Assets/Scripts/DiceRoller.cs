using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : Photon.MonoBehaviour
{
    [SerializeField]
    GameObject[] diceObjectArray;
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject rollButton;
    [SerializeField]
    Text myText;
    int[] eyesArray;
    Vector3[] diceEyeAngle = {new Vector3(0,0,0), new Vector3(-180, -360, 0) , new Vector3(-90, -360, 0) , new Vector3(-360, -90, 0) , new Vector3(-360, -270, 0)
    ,new Vector3(-270,-360,0),new Vector3(-360,0,0)};

    List<int> nowChoosedDiceIndex;
    bool isMyTurn;
    GameObject touchedObject;                   //터치를 위한 raycastHit
    bool nowChoosing;
    int rollCount;
    [SerializeField]
    GameObject endRollButton;
    [SerializeField]
    GameObject pickButton;

    public void Start()
    {
        eyesArray = new int[diceObjectArray.Length];
        RollButtonActive(false);
        
        nowChoosedDiceIndex = new List<int>();

        for(int i = 0; i < diceObjectArray.Length; i++)
        {
            diceObjectArray[i].transform.localEulerAngles = diceEyeAngle[Random.Range(1, 7)];
        }
    }

    public void TurnChange(bool myTurn)
    {
        isMyTurn = myTurn;
        RollButtonActive(myTurn);
        if(myTurn == true)
        {
            nowChoosing = false;
            rollCount = 3;
            myText.text = "남은 돌리기 : " + rollCount.ToString();
            nowChoosedDiceIndex.Clear();
            SetDiceBeforeRoll();
        }
        else
        {
            myText.text = "상대편";
        }
    }

    public void MasterStart()
    {
        photonView.RPC("YourTurn", PhotonTargets.Others);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0)&&nowChoosing && isMyTurn)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                touchedObject = hit.collider.gameObject;
                int diceIndex = -1;
                //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
                if (int.TryParse(touchedObject.tag, out diceIndex) == true)
                {
                    if (nowChoosedDiceIndex.Contains(diceIndex))
                    {
                        diceObjectArray[diceIndex].transform.localScale = Vector3.one * 0.5f;
                        nowChoosedDiceIndex.Remove(diceIndex);
                    }
                    else
                    {
                        diceObjectArray[diceIndex].transform.localScale = Vector3.one;
                        nowChoosedDiceIndex.Add(diceIndex);
                    }
                    if (nowChoosedDiceIndex.Count == 0)
                    {
                        RollButtonActive(false);
                    }
                    else
                    {
                        RollButtonActive(true);
                    }
                }
            }
        }
    }

    void SetDiceBeforeRoll()
    {
        nowChoosedDiceIndex.Clear();
        for (int i = 0; i < diceObjectArray.Length; i++)
        {
            diceObjectArray[i].transform.localScale = Vector3.one;
            nowChoosedDiceIndex.Add(i);
        }
    }



    public void RollButtonActive(bool active)
    {
        rollButton.SetActive(active);
    }

    public void RollMyDice()
    {
        nowChoosing = false;
        RollButtonActive(false);
        int[] sendingDiceIndex = nowChoosedDiceIndex.ToArray();
        for (int i = 0; i < nowChoosedDiceIndex.Count; i++)
        {
            int diceIndex = nowChoosedDiceIndex[i];
            eyesArray[diceIndex] = Random.Range(1, 7);
            
        }
        photonView.RPC("RollDice", PhotonTargets.Others, sendingDiceIndex, eyesArray);
        if(nowChoosedDiceIndex.Count != 0)
        {
            StartCoroutine(RollCoroutine());
        }
    }

    void RollEnemyDice(int[] enemyChoosedDice,int[] enemyEyes)
    {
        nowChoosedDiceIndex.Clear();
        for(int i = 0; i < enemyChoosedDice.Length; i++)
        {
            nowChoosedDiceIndex.Add(enemyChoosedDice[i]);
        }
        
        eyesArray = enemyEyes;
        if (nowChoosedDiceIndex.Count != 0)
        {
            StartCoroutine(RollCoroutine());
        }
    }


//1 180,0,0
//2 -90,0,0
//3 0,-90,0
//4 0,90,0
//5 90,0,0
//6 0,0,0


    IEnumerator RollCoroutine()
    {
        //안 낑궜는지 판단해주는 코루틴.
        //yield return new WaitForSeconds(1f);
        List<Vector3> originLerp = new List<Vector3>();
        List<Vector3> endLerp = new List<Vector3>();
        float timer = 0;
        for (int i = 0; i < nowChoosedDiceIndex.Count; i++)
        {
            int diceIndex = nowChoosedDiceIndex[i];
            originLerp.Add(diceObjectArray[diceIndex].transform.localEulerAngles);
            int eye = eyesArray[diceIndex];
            endLerp.Add(diceEyeAngle[eye]);
        }



        float speed = 0f;

        while (timer < 1)
        {
            
            for (int i = 0; i < nowChoosedDiceIndex.Count; i++)
            {
                int diceIndex = nowChoosedDiceIndex[i];
                diceObjectArray[diceIndex].transform.localEulerAngles= Vector3.Lerp(originLerp[i], endLerp[i], timer);
            }
            if (speed < 1.5f)
            {
                speed += Time.deltaTime * 6f;
            }
            else
            {
                speed -= Time.deltaTime * 100f;
            }
            if (speed < 0.1f)
            {
                speed = 0.1f;
            }
            timer += Time.deltaTime * speed;
            yield return null;
        }
        for (int i = 0; i < nowChoosedDiceIndex.Count; i++)
        {
            int diceIndex = nowChoosedDiceIndex[i];
            diceObjectArray[diceIndex].transform.localEulerAngles = endLerp[i];
        }

        
       

        if (isMyTurn == true)
        {
            SetDiceBeforeRoll();
            rollCount--;
            myText.text = "남은 돌리기 : " + rollCount.ToString();
            if (rollCount == 0)
            {
                nowChoosing = false;
                EndRoll();
            }
            else
            {
                nowChoosing = true;
                endRollButton.SetActive(true);
                RollButtonActive(true);

            }
            //photonView.RPC("YourTurn", PhotonTargets.Others);
        }
    }

    public void EndRoll()
    {
        endRollButton.SetActive(false);
        pickButton.SetActive(true);
    }


    public void PickScore()
    {
        pickButton.SetActive(false);
        TurnChange(false);
        photonView.RPC("YourTurn", PhotonTargets.Others);
    }



    [PunRPC]
    void YourTurn()
    {
        TurnChange(true);
    }

    [PunRPC]
    void RollDice(int[] nowChoosedDiceArray, int[] nowEyesArray)
    {
        RollEnemyDice(nowChoosedDiceArray, nowEyesArray);
    }

}
