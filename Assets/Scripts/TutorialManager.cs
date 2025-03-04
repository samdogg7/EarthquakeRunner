﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialPopups;
    public GameObject finalTutorialPopup;
    public PlayerController playerController;
    public int popupIndex = 0;
    private int hunger = 0;
    private bool calledIncrement = false;
    private float slowMoScale = 0.6f;
    private int pretutorialBirdConsumed = 0;

    private void Start()
    {
        if(Settings.instance != null && Settings.instance.isTutorial)
        {
            Time.timeScale = slowMoScale;
            playerController.hungerEnabled = false;
        }
    }

    void Update()
    {
        if(Settings.instance != null && Settings.instance.isTutorial)
        {
            for (int i = 0; i < tutorialPopups.Length; i++)
            {
                if (i == popupIndex)
                {
                    tutorialPopups[i].SetActive(true);
                }
                else
                {
                    tutorialPopups[i].SetActive(false);
                }
            }

            if (popupIndex == 0)
            {
                if (Input.touchCount > 0 && transform.position.x < 8f)
                {
                    foreach (Touch touch in Input.touches)
                    {
                        if ((touch.position.x < Screen.width / 2) && !calledIncrement)
                        {
                            calledIncrement = true;
                            StartCoroutine(delayIndexIncrement(3));
                        }
                    }
                }
            }

            if (popupIndex == 1)
            {
                if (Input.touchCount > 0 && transform.position.x < 8f)
                {
                    foreach (Touch touch in Input.touches)
                    {
                        if (touch.position.x > Screen.width / 2 && touch.position.y < (3 * Screen.height / 4) && !calledIncrement)
                        {
                            calledIncrement = true;
                            StartCoroutine(delayIndexIncrement(2));
                            playerController.hungerEnabled = true;
                        }
                    }
                }
            }

            if (popupIndex == 2)
            {
                //Check if the player has consumed a bird
                if (playerController.GetNumberOfBirdsConsumed() > pretutorialBirdConsumed && !calledIncrement)
                {
                    StartCoroutine(delayIndexIncrement(2));
                    calledIncrement = true;
                }
            }

            if (popupIndex == 3)
            {
                if (!PlayerPrefs.HasKey("Tutorial") && Settings.instance != null)
                {
                    Settings.instance.isTutorial = false;
                    PlayerPrefs.SetString("Tutorial", "false");
                }
                StartCoroutine(finalTutorialPrompt());
            }
        }
    }

    IEnumerator delayIndexIncrement(int waitTime)
    {
        yield return new WaitForSeconds(.3f);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(waitTime);
        Time.timeScale = slowMoScale;
        popupIndex++;
        pretutorialBirdConsumed = playerController.GetNumberOfBirdsConsumed();
        calledIncrement = false;
    }

    IEnumerator finalTutorialPrompt()
    {
        foreach(GameObject popup in tutorialPopups)
        {
            popup.SetActive(false);
        }
        finalTutorialPopup.SetActive(true);
        yield return new WaitForSeconds(3);
        finalTutorialPopup.SetActive(false);
        Time.timeScale = 1f;
    }
}
