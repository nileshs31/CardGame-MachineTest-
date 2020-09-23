using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    #region Variables
    // Start is called before the first frame update
    CardsManager cardMan;
    public Animator CamAnim;
    public GameObject MainMenuScreen;
    public GameObject GameScreen, GameOverScreen;
    public Button p1But, p2But;
    public bool p1Turn, GameStarted = false;
    public Text TurnText, p1CardCount, p2CardCount, timerText, GameOverText;
    public Image TimerImage;
    public int v1 = 0, v2 = 0;
    float timeLeft = 60;
    #endregion

    void Start()
    {
        cardMan = gameObject.GetComponent<CardsManager>();
        p1But.interactable = false;
        p2But.interactable = false;
        MainMenuScreen.SetActive(true);
        GameScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        CamAnim.SetBool("GameStarted", false);
    }

    public void playButton()
    {
        CamAnim.SetBool("GameStarted", true);
        MainMenuScreen.SetActive(false);
        GameScreen.SetActive(true);
        StartCoroutine(CardsAllocate());
    }

    public void backButton()
    {
        /*
        CamAnim.SetBool("GameStarted", false);
        MainMenuScreen.SetActive(true);
        GameScreen.SetActive(false);
        StopAllCoroutines();*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quitButton()
    {
        Application.Quit();
    }

    IEnumerator CardsAllocate()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(cardMan.CardsAllocateAndMover());
    }
    
    public void GameStart()
    {
        if (p1Turn)
        {
            p1But.interactable = true;
            TurnText.text = "Player 1's Turn";
            p2But.interactable = false;
        }
        else
        {
            p1But.interactable = false;
            p2But.interactable = true;
            TurnText.text = "Player 2's Turn";
        }
    }

    public void p1Button()
    {
        cardMan.p1Cards[0].transform.position = cardMan.p1pos2.transform.position;
        cardMan.p1Cards[0].transform.rotation = Quaternion.Euler(180, 0, 0);
        v1 = cardMan.p1Cards[0].GetComponent<Card>().cardValue;
        p1Turn = false;
        GameStart();

    }

    public void p2Button()
    {
        cardMan.p2Cards[0].transform.position = cardMan.p2pos2.transform.position;
        cardMan.p2Cards[0].transform.rotation = Quaternion.Euler(180, 0, 0);
        v2 = cardMan.p2Cards[0].GetComponent<Card>().cardValue;
        StartCoroutine(Checker());
    }

    private IEnumerator Checker()
    {

        p2But.interactable = false;
        CamAnim.SetBool("CloseUp", true);
        yield return new WaitForSeconds(2f);

        if (v1 > v2)
        {
            TurnText.text = "Player 1 Wins This Round";
            var tempC1 = cardMan.p1Cards[0];
            var tempC2 = cardMan.p2Cards[0];
            cardMan.p1Cards.RemoveAt(0);
            cardMan.p2Cards.RemoveAt(0);

            for(int i=0;i< cardMan.p1Cards.Count; i++)
            {
                cardMan.p1Cards[i].transform.position += new Vector3(0, 2*0.006f, 0);
            }
            tempC1.transform.rotation = Quaternion.Euler(0, 0, 0);
            tempC2.transform.rotation = Quaternion.Euler(0, 0, 0);
            var tempPos = cardMan.p1pos1.transform.position;
            while (tempC2.transform.position != tempPos)
            {
                tempC1.transform.position = Vector3.MoveTowards(tempC1.transform.position, tempPos + new Vector3(0, 0.006f, 0), 2.5f * Time.deltaTime);
                tempC2.transform.position = Vector3.MoveTowards(tempC2.transform.position, tempPos, 2.5f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            cardMan.p1Cards.Add(tempC1);
            cardMan.p1Cards.Add(tempC2);

        }
        else if (v1 < v2)
        {
            TurnText.text = "Player 2 Wins This Round";
            var tempC1 = cardMan.p1Cards[0];
            var tempC2 = cardMan.p2Cards[0];
            cardMan.p1Cards.RemoveAt(0);
            cardMan.p2Cards.RemoveAt(0);

            for (int i = 0; i < cardMan.p2Cards.Count; i++)
            {
                cardMan.p2Cards[i].transform.position += new Vector3(0, 2 * 0.006f, 0);
            }
            tempC1.transform.rotation = Quaternion.Euler(0, 0, 0);
            tempC2.transform.rotation = Quaternion.Euler(0, 0, 0);
            var tempPos = cardMan.p2pos1.transform.position;
            while (tempC1.transform.position != tempPos)
            {
                tempC2.transform.position = Vector3.MoveTowards(tempC2.transform.position, tempPos + new Vector3(0, 0.006f, 0), 2.5f * Time.deltaTime);
                tempC1.transform.position = Vector3.MoveTowards(tempC1.transform.position, tempPos, 2.5f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            cardMan.p2Cards.Add(tempC2);
            cardMan.p2Cards.Add(tempC1);
        }
        else if (v1 == v2)
        {
            TurnText.text = "Round Draw";
            var tempC1 = cardMan.p1Cards[0];
            var tempC2 = cardMan.p2Cards[0];
            cardMan.p1Cards.RemoveAt(0);
            cardMan.p2Cards.RemoveAt(0);

            for (int i = 0; i < cardMan.p1Cards.Count; i++)
            {
                cardMan.p1Cards[i].transform.position += new Vector3(0, 0.006f, 0);
            }

            for (int i = 0; i < cardMan.p2Cards.Count; i++)
            {
                cardMan.p2Cards[i].transform.position += new Vector3(0, 0.006f, 0);
            }

            tempC1.transform.rotation = Quaternion.Euler(0, 0, 0);
            tempC2.transform.rotation = Quaternion.Euler(0, 0, 0);
            while (tempC2.transform.position != cardMan.p2pos1.transform.position)
            {
                tempC2.transform.position = Vector3.MoveTowards(tempC2.transform.position, cardMan.p2pos1.transform.position, 2.5f * Time.deltaTime);
                tempC1.transform.position = Vector3.MoveTowards(tempC1.transform.position, cardMan.p1pos1.transform.position, 2.5f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            cardMan.p1Cards.Add(tempC1);
            cardMan.p2Cards.Add(tempC2);
        }
        
        if(cardMan.p1Cards.Count == 0 || cardMan.p2Cards.Count == 0)
        {
            GameOver();
        }
        else
        {
            yield return new WaitForSeconds(0.8f);
            CamAnim.SetBool("CloseUp", false);
            p1Turn = true;
            GameStart();
        }
    
    }

    public void GameOver()
    {
        TurnText.text = "Game Over";
        p1But.interactable = false;
        p2But.interactable = false;
        if (cardMan.p1Cards.Count > cardMan.p2Cards.Count)
            GameOverText.text = "Player 1 Wins!";
        else if (cardMan.p1Cards.Count < cardMan.p2Cards.Count)
            GameOverText.text = "Player 2 Wins!";
        else if (cardMan.p1Cards.Count == cardMan.p2Cards.Count)
            GameOverText.text = "Game Draw!";
        GameOverScreen.SetActive(true);

    }

    private void Update()
    {
        
        p1CardCount.text = "Cards: "+cardMan.p1Cards.Count;
        p2CardCount.text = "Cards: "+cardMan.p2Cards.Count;

        if (GameStarted)
        {
            TimerImage.fillAmount = timeLeft/60;
            timerText.text = ((int)timeLeft).ToString();
            if (timeLeft > 0)
                timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                StopAllCoroutines();
                GameOver();
            }
        }
    }
}
