using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum ScoreKind
{
    Ones, Twos, Threes, Fours, Fives, Sixes, Bonus, TotalScore, ThreeKind, FourKind, FullHouse, SmallStraight, LargeStraight, Yahtzee, Chance
}


[System.Serializable]
public class ScoreTable
{
    public int[] scoreArray;
    public bool[] checkedScoreArray;


    public ScoreTable()
    {
        scoreArray = new int[System.Enum.GetValues(typeof(ScoreKind)).Length];
        checkedScoreArray = new bool[System.Enum.GetValues(typeof(ScoreKind)).Length];
        for (int i = 0; i < checkedScoreArray.Length; i++)
        {
            scoreArray[i] = 0;
            checkedScoreArray[i] = false;
        }
    }

    public void OnScoreButton(ScoreKind kind,int[] eyesArray)
    {
        switch (kind)
        {
            case ScoreKind.Ones:
                break;
            case ScoreKind.Twos:
                break;
            case ScoreKind.Threes:
                break;
            case ScoreKind.Fours:
                break;
            case ScoreKind.Fives:
                break;
            case ScoreKind.Sixes:
                break;
            case ScoreKind.ThreeKind:
                break;
            case ScoreKind.FourKind:
                break;
            case ScoreKind.FullHouse:
                break;
            case ScoreKind.SmallStraight:
                break;
            case ScoreKind.LargeStraight:
                break;
            case ScoreKind.Yahtzee:
                break;
            case ScoreKind.Chance:
                break;
        }
    }

    public int OnScoreCalculate(ScoreKind kind,int[] eyesArray)
    {
        switch (kind)
        {
            case ScoreKind.Ones:
                return OnesCalculate(eyesArray);
                
            case ScoreKind.Twos:
                return TwosCalculate(eyesArray);

            case ScoreKind.Threes:
                return ThreesCalculate(eyesArray);

            case ScoreKind.Fours:
                return FoursCalculate(eyesArray);

            case ScoreKind.Fives:
                return FivesCalculate(eyesArray);

            case ScoreKind.Sixes:
                return SixesCalculate(eyesArray);

            case ScoreKind.Bonus:
                return BonusCalculate(eyesArray);

            case ScoreKind.TotalScore:
                return TotalScoreCalculate();

            case ScoreKind.ThreeKind:
                return ThreeKindCalculate(eyesArray);

            case ScoreKind.FourKind:
                return FourKindCalculate(eyesArray);

            case ScoreKind.FullHouse:
                return FullHouseCalculate(eyesArray);

            case ScoreKind.SmallStraight:
                return SmallStraightCalculate(eyesArray);

            case ScoreKind.LargeStraight:
                return LargeStraightCalculate(eyesArray);

            case ScoreKind.Yahtzee:
                return YahtzeeCalculate(eyesArray);

            case ScoreKind.Chance:
                return ChanceCalculate(eyesArray);

        }
        return 0;
    }

    public int OnesCalculate(int[] eyesArray)
    {
        int sum = 0;
        for(int i = 0; i < eyesArray.Length; i++)
        {
            if(eyesArray[i] == 1)
            {
                sum += eyesArray[i];
            }
        }
        return sum;
    }

    public int TwosCalculate(int[] eyesArray)
    {
        int sum = 0;
        for (int i = 0; i < eyesArray.Length; i++)
        {
            if (eyesArray[i] == 2)
            {
                sum += eyesArray[i];
            }
        }
        return sum;
    }

    public int ThreesCalculate(int[] eyesArray)
    {
        int sum = 0;
        for (int i = 0; i < eyesArray.Length; i++)
        {
            if (eyesArray[i] == 3)
            {
                sum += eyesArray[i];
            }
        }
        return sum;
    }

    public int FoursCalculate(int[] eyesArray)
    {
        int sum = 0;
        for (int i = 0; i < eyesArray.Length; i++)
        {
            if (eyesArray[i] == 4)
            {
                sum += eyesArray[i];
            }
        }
        return sum;
    }

    public int FivesCalculate(int[] eyesArray)
    {
        int sum = 0;
        for (int i = 0; i < eyesArray.Length; i++)
        {
            if (eyesArray[i] == 5)
            {
                sum += eyesArray[i];
            }
        }
        return sum;
    }


    public int SixesCalculate(int[] eyesArray)
    {
        int sum = 0;
        for (int i = 0; i < eyesArray.Length; i++)
        {
            if (eyesArray[i] == 6)
            {
                sum += eyesArray[i];
            }
        }
        return sum;
    }

    public int BonusCalculate(int[] eyesArray)
    {
        int totalScore = TotalScoreCalculate();
        if(totalScore>= 63)
        {
            return 35;
        }
        else
        {
            return totalScore - 63;
        }

    }

    public int TotalScoreCalculate()
    {
        int sum = 0;
        for(int i = 0; i < scoreArray.Length; i++)
        {
            sum += scoreArray[i];
        }
        return sum;
    }
    public int ThreeKindCalculate(int[] eyesArray)
    {
        int[] eyesCount = new int[7];
        for (int i = 1; i < eyesCount.Length; i++)
        {
            eyesCount[i] = 0;
        }
        for (int i = 0;i< eyesArray.Length; i++)
        {
            eyesCount[eyesArray[i]]++;
        }
        for (int i = 1; i < eyesCount.Length; i++)
        {
            if(eyesCount[i] >= 3)
            {
                return i * 3;
            }
        }
        return 0;
    }
    public int FourKindCalculate(int[] eyesArray)
    {
        int[] eyesCount = new int[7];
        for (int i = 1; i < eyesCount.Length; i++)
        {
            eyesCount[i] = 0;
        }
        for (int i = 0; i < eyesArray.Length; i++)
        {
            eyesCount[eyesArray[i]]++;
        }
        for (int i = 1; i < eyesCount.Length; i++)
        {
            if (eyesCount[i] >= 4)
            {
                return i * 4;
            }
        }
        return 0;
    }
    public int FullHouseCalculate(int[] eyesArray)
    {
        int[] eyesCount = new int[7];
        for (int i = 1; i < eyesCount.Length; i++)
        {
            eyesCount[i] = 0;
        }
        for (int i = 0; i < eyesArray.Length; i++)
        {
            eyesCount[eyesArray[i]]++;
        }
        bool three = false;
        bool two = false;
        for (int i = 1; i < eyesCount.Length; i++)
        {
            if (eyesCount[i] == 3)
            {
                three = true;
            }
            if (eyesCount[i] == 2)
            {
                two = true;
            }
            if (three && two)
            {
                return 25;
            }
        }
        return 0;
    }
    public int SmallStraightCalculate(int[] eyesArray)
    {
        int[] eyesCount = new int[7];
        for (int i = 1; i < eyesCount.Length; i++)
        {
            eyesCount[i] = 0;
        }
        for (int i = 0; i < eyesArray.Length; i++)
        {
            eyesCount[eyesArray[i]]++;
        }
        int counting = 0;
        for (int i = 1; i < eyesCount.Length; i++)
        {
            if (eyesCount[i] >= 1)
            {
                counting++;
            }
            else
            {
                counting = 0;
            }
        }
        if(counting >= 4)
        {
            return 30;
        }
        return 0;
    }
    public int LargeStraightCalculate(int[] eyesArray)
    {
        int[] eyesCount = new int[7];
        for (int i = 1; i < eyesCount.Length; i++)
        {
            eyesCount[i] = 0;
        }
        for (int i = 0; i < eyesArray.Length; i++)
        {
            eyesCount[eyesArray[i]]++;
        }
        int counting = 0;
        for (int i = 1; i < eyesCount.Length; i++)
        {
            if (eyesCount[i] >= 1)
            {
                counting++;
            }
            else
            {
                counting = 0;
            }
        }
        if (counting >= 5)
        {
            return 40;
        }
        return 0;
    }
    public int YahtzeeCalculate(int[] eyesArray)
    {
        int nowNum = eyesArray[0];
        for (int i = 1; i < eyesArray.Length; i++)
        {
            if(nowNum != eyesArray[i])
            {
                return 0;
            }
            nowNum = eyesArray[i];
        }
        return 50;
    }
    public int ChanceCalculate(int[] eyesArray)
    {
        int sum = 0;
        for (int i = 0; i < eyesArray.Length; i++)
        {
            sum += eyesArray[i];
        }
        return sum;
    }
}
