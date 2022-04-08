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
        int pickedScore = 0;
        switch (kind)
        {
            case ScoreKind.Ones:
               pickedScore= OnesCalculate(eyesArray);
                break;
            case ScoreKind.Twos:
                pickedScore = TwosCalculate(eyesArray);
                break;
            case ScoreKind.Threes:
                pickedScore = ThreesCalculate(eyesArray);
                break;
            case ScoreKind.Fours:
                pickedScore = FoursCalculate(eyesArray);
                break;
            case ScoreKind.Fives:
                pickedScore = FivesCalculate(eyesArray);
                break;
            case ScoreKind.Sixes:
                pickedScore = SixesCalculate(eyesArray);
                break;

            case ScoreKind.ThreeKind:
                pickedScore = ThreeKindCalculate(eyesArray);
                break;
            case ScoreKind.FourKind:
                pickedScore = FourKindCalculate(eyesArray);
                break;
            case ScoreKind.FullHouse:
                pickedScore = FullHouseCalculate(eyesArray);
                break;
            case ScoreKind.SmallStraight:
                pickedScore = SmallStraightCalculate(eyesArray);
                break;
            case ScoreKind.LargeStraight:
                pickedScore = LargeStraightCalculate(eyesArray);
                break;
            case ScoreKind.Yahtzee:
                pickedScore = YahtzeeCalculate(eyesArray);
                break;
            case ScoreKind.Chance:
                pickedScore = ChanceCalculate(eyesArray);
                break;
        }
        if (pickedScore < 0)
        {
            pickedScore = 0;
        }
        checkedScoreArray[(int)kind] = true;
        scoreArray[(int)kind] = pickedScore;
        scoreArray[(int)ScoreKind.TotalScore] = TotalScoreCalculate();
        if(BonusCalculate() > 0)
        {
            checkedScoreArray[(int)ScoreKind.Bonus] = true;
            scoreArray[(int)ScoreKind.Bonus] = BonusCalculate();
            scoreArray[(int)ScoreKind.TotalScore] = TotalScoreCalculate();
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
                return BonusCalculate();

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

    int OnesCalculate(int[] eyesArray)
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

    int TwosCalculate(int[] eyesArray)
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

    int ThreesCalculate(int[] eyesArray)
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

    int FoursCalculate(int[] eyesArray)
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

    int FivesCalculate(int[] eyesArray)
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


    int SixesCalculate(int[] eyesArray)
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

    int BonusCalculate()
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

    int TotalScoreCalculate()
    {
        int sum = 0;
        for(int i = 0; i < scoreArray.Length; i++)
        {
            if(checkedScoreArray[i] == true)
                sum += scoreArray[i];
        }
        return sum;
    }
    int ThreeKindCalculate(int[] eyesArray)
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
    int FourKindCalculate(int[] eyesArray)
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
    int FullHouseCalculate(int[] eyesArray)
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
    int SmallStraightCalculate(int[] eyesArray)
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
                if (counting >= 4)
                {
                    return 30;
                }
            }
            else
            {
                counting = 0;
            }
        }
        return 0;
    }
    int LargeStraightCalculate(int[] eyesArray)
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
                if (counting >= 5)
                {
                    return 40;
                }
            }
            else
            {
                counting = 0;
            }
        }

        return 0;
    }
    int YahtzeeCalculate(int[] eyesArray)
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
    int ChanceCalculate(int[] eyesArray)
    {
        int sum = 0;
        for (int i = 0; i < eyesArray.Length; i++)
        {
            sum += eyesArray[i];
        }
        return sum;
    }
}
