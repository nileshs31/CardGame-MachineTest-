using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] faceCards; 
    public GameObject cardPrefab, mainPos, p1pos1, p1pos2, p2pos1, p2pos2;
    public List<GameObject> Cards, p1Cards, p2Cards;


    void Start()
    {
        initializeCards();
    }

    int RREx(int i, int j, List<int> k)//Random Range Exclusion
    {         
        int num;
        do
        {
            num = Random.Range(i, j);
        } while (k.Contains(num));
        return num;
    }
    
    public void initializeCards()
    {
        List<int> TempFaceCards = new List<int>();
        Vector3 tempos = mainPos.transform.position;
        for (int i = 0; i < 52; i++)
        {
            Cards.Add(Instantiate(cardPrefab, tempos, Quaternion.identity));
            tempos += new Vector3(0, 0.006f, 0);

            int Temp = RREx(0, faceCards.Length, TempFaceCards);
            TempFaceCards.Add(Temp);
            Cards[i].GetComponent<Card>().faceSide.sprite = faceCards[Temp];
            
            // GIVING VALUES TO THE CARDS
            if (Temp == 0 || Temp == 13 || Temp == 26 || Temp == 39)
                Cards[i].GetComponent<Card>().cardValue = 0;
            else if (Temp == 1 || Temp == 14 || Temp == 27 || Temp == 40)
                Cards[i].GetComponent<Card>().cardValue = 1;
            else if (Temp == 2 || Temp == 15 || Temp == 28 || Temp == 41)
                Cards[i].GetComponent<Card>().cardValue = 2;
            else if (Temp == 3 || Temp == 16 || Temp == 29 || Temp == 42)
                Cards[i].GetComponent<Card>().cardValue = 3;
            else if (Temp == 4 || Temp == 17 || Temp == 30 || Temp == 43)
                Cards[i].GetComponent<Card>().cardValue = 4;
            else if (Temp == 5 || Temp == 18 || Temp == 31 || Temp == 44)
                Cards[i].GetComponent<Card>().cardValue = 5;
            else if (Temp == 6 || Temp == 19 || Temp == 32 || Temp == 45)
                Cards[i].GetComponent<Card>().cardValue = 6;
            else if (Temp == 7 || Temp == 20 || Temp == 33 || Temp == 46)
                Cards[i].GetComponent<Card>().cardValue = 7;
            else if (Temp == 8 || Temp == 21 || Temp == 34 || Temp == 47)
                Cards[i].GetComponent<Card>().cardValue = 8;
            else if (Temp == 9 || Temp == 22 || Temp == 35 || Temp == 48)
                Cards[i].GetComponent<Card>().cardValue = 9;
            else if (Temp == 10 || Temp == 23 || Temp == 36 || Temp == 49)
                Cards[i].GetComponent<Card>().cardValue = 10;
            else if (Temp == 11 || Temp == 24 || Temp == 37 || Temp == 50)
                Cards[i].GetComponent<Card>().cardValue = 11;
            else if (Temp == 12 || Temp == 25 || Temp == 38 || Temp == 51)
                Cards[i].GetComponent<Card>().cardValue = 12;
        }

        Cards.Reverse();

    }

    public IEnumerator CardsAllocateAndMover()
    {
        Vector3 tempPos1 = p1pos1.transform.position;
        Vector3 tempPos2 = p2pos1.transform.position;
        for (int i = 0; i < 52; i++)
        {
            if (i % 2 == 0)
            {                
                p1Cards.Add(Cards[i]);
                while (Cards[i].transform.position != tempPos1)
                {
                    Cards[i].transform.position = Vector3.MoveTowards(Cards[i].transform.position, tempPos1, 8f * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                tempPos1 += new Vector3(0, 0.006f, 0);
            }
            else if((i % 2 != 0))
            {
                p2Cards.Add(Cards[i]);
                while (Cards[i].transform.position != tempPos2)
                {
                    Cards[i].transform.position = Vector3.MoveTowards(Cards[i].transform.position, tempPos2, 8f * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                tempPos2 += new Vector3(0, 0.006f, 0);
            }
        }

        p1Cards.Reverse();
        p2Cards.Reverse();

        gameObject.GetComponent<LevelManager>().p1Turn = true;
        gameObject.GetComponent<LevelManager>().GameStart();
        gameObject.GetComponent<LevelManager>().GameStarted = true;

    }

}
