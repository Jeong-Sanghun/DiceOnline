using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : Photon.MonoBehaviour
{
    ScoreTable myScoreTable;
    ScoreTable enemyScoreTable;
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
    Text[] myScoreTextArray;

    [SerializeField]
    Text[] enemyScoreTextArray;
    [SerializeField]
    GameObject[] pickButtonArray;

    public void Start()
    {
        GameInit();
    }

    public void GameInit()
    {
        myScoreTable = new ScoreTable();
        enemyScoreTable = new ScoreTable();
        eyesArray = new int[diceObjectArray.Length];
        RollButtonActive(false);
        PickButtonActive(false);

        for (int i = 0; i < enemyScoreTextArray.Length; i++)
        {
            enemyScoreTextArray[i].text = null;
            myScoreTextArray[i].text = null;
        }

        nowChoosedDiceIndex = new List<int>();

        for (int i = 0; i < diceObjectArray.Length; i++)
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
        myScoreTable = new ScoreTable();
        string json = JsonUtility.ToJson(myScoreTable);
        photonView.RPC("YourTurn", PhotonTargets.Others,json);
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
        PickButtonActive(false);
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
            PickButtonActive(true);
            rollCount--;
            myText.text = "남은 돌리기 : " + rollCount.ToString();

            ChangeMyText();
            if (rollCount == 0)
            {
                nowChoosing = false;
            }
            else
            {
                nowChoosing = true;
                RollButtonActive(true);

            }
            //photonView.RPC("YourTurn", PhotonTargets.Others);
        }
        else
        {
            ChangeEnemyText();
        }
    }

    void ChangeMyText()
    {
        for(int i = 0; i < myScoreTextArray.Length; i++)
        {
            if ((ScoreKind)i == ScoreKind.Bonus)
            {
                int score = myScoreTable.OnScoreCalculate((ScoreKind)i, eyesArray);
                if (score < 0)
                {
                    myScoreTextArray[i].color = Color.red;
                }
                else
                {
                    myScoreTextArray[i].color = Color.black;
                }
                myScoreTextArray[i].text = score.ToString();
            }
            else if ((ScoreKind)i == ScoreKind.TotalScore)
            {
                myScoreTextArray[i].text = myScoreTable.OnScoreCalculate((ScoreKind)i, eyesArray).ToString();
            }
            else if (myScoreTable.checkedScoreArray[i] == false)
            {
                myScoreTextArray[i].color = Color.green;
                myScoreTextArray[i].text = myScoreTable.OnScoreCalculate((ScoreKind)i, eyesArray).ToString();
            }
            else
            {
                myScoreTextArray[i].color = Color.black;
            }
        }
        
    }

    void ChangeEnemyText()
    {
        for (int i = 0; i < enemyScoreTextArray.Length; i++)
        {
            if ((ScoreKind)i == ScoreKind.Bonus )
            {
                int score = enemyScoreTable.OnScoreCalculate((ScoreKind)i, eyesArray);
                if (score < 0)
                {
                    enemyScoreTextArray[i].color = Color.red;
                }
                else
                {
                    enemyScoreTextArray[i].color = Color.black;
                }
                enemyScoreTextArray[i].text = score.ToString();
            }
            else if ((ScoreKind)i == ScoreKind.TotalScore)
            {
                enemyScoreTextArray[i].text = enemyScoreTable.OnScoreCalculate((ScoreKind)i, eyesArray).ToString();
            }
            else if (enemyScoreTable.checkedScoreArray[i] == false)
            {
                enemyScoreTextArray[i].color = Color.green;
                enemyScoreTextArray[i].text = enemyScoreTable.OnScoreCalculate((ScoreKind)i, eyesArray).ToString();
            }
            else
            {
                enemyScoreTextArray[i].color = Color.black;
            }
        }

    }

    void BackToOriginMyText()
    {
        for (int i = 0; i < myScoreTextArray.Length; i++)
        {
            if ((ScoreKind)i == ScoreKind.Bonus)
            {
                int score = myScoreTable.OnScoreCalculate((ScoreKind)i, eyesArray);
                if (score < 0)
                {
                    myScoreTextArray[i].color = Color.red;
                }
                else
                {
                    myScoreTextArray[i].color = Color.black;
                }
                myScoreTextArray[i].text = score.ToString();
            }
            else if ((ScoreKind)i == ScoreKind.TotalScore)
            {
                myScoreTextArray[i].text = myScoreTable.OnScoreCalculate((ScoreKind)i, eyesArray).ToString();
            }
            else if (myScoreTable.checkedScoreArray[i] == false)
            {
                myScoreTextArray[i].text = null;
            }
            else
            {
                myScoreTextArray[i].color = Color.black;
                myScoreTextArray[i].text = myScoreTable.scoreArray[i].ToString();
            }
        }

    }
    void BackToOriginEnemyText()
    {
        for (int i = 0; i < enemyScoreTextArray.Length; i++)
        {
            if ((ScoreKind)i == ScoreKind.Bonus)
            {
                int score = enemyScoreTable.OnScoreCalculate((ScoreKind)i, eyesArray);
                if (score < 0)
                {
                    enemyScoreTextArray[i].color = Color.red;
                }
                else
                {
                    enemyScoreTextArray[i].color = Color.black;
                }
                enemyScoreTextArray[i].text = score.ToString();
            }
            else if ((ScoreKind)i == ScoreKind.TotalScore)
            {
                enemyScoreTextArray[i].text = enemyScoreTable.OnScoreCalculate((ScoreKind)i, eyesArray).ToString();
            }
            else if (enemyScoreTable.checkedScoreArray[i] == false)
            {
                enemyScoreTextArray[i].text = null;
            }
            else
            {
                enemyScoreTextArray[i].color = Color.black;
                enemyScoreTextArray[i].text = enemyScoreTable.scoreArray[i].ToString();
            }
        }

    }

    public void PickScore(int kindNum)
    {
        ScoreKind kind = (ScoreKind)kindNum;
        PickButtonActive(false);
        TurnChange(false);
        myScoreTable.OnScoreButton(kind, eyesArray);
        BackToOriginMyText();
        string json = JsonUtility.ToJson(myScoreTable);
        photonView.RPC("YourTurn", PhotonTargets.Others, json);
    }

    void PickButtonActive(bool active)
    {
        if (active==true)
        {
            for (int i = 0; i < pickButtonArray.Length; i++)
            {
                if (pickButtonArray[i] != null && myScoreTable.checkedScoreArray[i] == false)
                {
                    pickButtonArray[i].SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < pickButtonArray.Length; i++)
            {
                if (pickButtonArray[i] != null)
                {
                    pickButtonArray[i].SetActive(false);
                }
            }
        }
    }


    [PunRPC]
    void YourTurn(string jsonEnemyTable)
    {
        if(jsonEnemyTable == null)
        {
            enemyScoreTable = new ScoreTable();
        }
        else
        {
            ScoreTable enemyTable = JsonUtility.FromJson<ScoreTable>(jsonEnemyTable);
            enemyScoreTable = enemyTable;
        }
        BackToOriginEnemyText();
        TurnChange(true);
    }

    [PunRPC]
    void RollDice(int[] nowChoosedDiceArray, int[] nowEyesArray)
    {
        RollEnemyDice(nowChoosedDiceArray, nowEyesArray);
    }

}
