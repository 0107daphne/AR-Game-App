using System.Text;
using System;
using System.Windows;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{
    private List<int> playerTaskList = new List<int>();
    private List<int> playerSequenceList = new List<int>();
    public List<AudioClip> buttonSoundsList = new List<AudioClip>();
    public List<List<Color32>> buttonColors = new List<List<Color32>>();
    public List<Button> clickableButtons;
    public List<GameObject> clickableCubes;
    public AudioClip loseSound;
    public AudioSource audioSource;
    public CanvasGroup buttons;
    public GameObject startButton;
    public TMP_Text console;
    public TMP_Text console1;
    public TMP_Text console2;
    public TMP_Text console3;
    public void Awake()
    {
        
        //add colors, color state and color clicked
        buttonColors.Add(new List<Color32> {new Color32(255, 100, 100, 255), new Color32(255, 0, 0, 255) }); //add red
        buttonColors.Add(new List<Color32> {new Color32(255, 187, 109, 255), new Color32(255, 136, 0, 255) }); //add orange
        buttonColors.Add(new List<Color32> {new Color32(162, 255, 124, 255), new Color32(72, 248, 0, 255) }); //add green
        buttonColors.Add(new List<Color32> {new Color32(57, 111, 255, 255), new Color32(0, 70, 255, 255) }); //add blue

        //setting the color of the buttons
        for(int i=0; i<4; i++)
        {
            clickableButtons[i].GetComponent<Image>().color = buttonColors[i][0];
        }
    }

    public void AddToPlayerSequenceList(int buttonId)
    {
        playerSequenceList.Add(buttonId);
        StartCoroutine(HighlightButton(buttonId));
        //need to check if player selected the correct button
        for(int i=0;i<playerSequenceList.Count;i++)
        {
            //if these are correct then the player is correct
            if(playerTaskList[i] == playerSequenceList[i])
            {
                continue;
            }
            else
            {
                //console2.text="You Lost. I am disappointed.";
                console.text="You Lost. I am disappointed.";
                StartCoroutine(PlayerLost());
                return;
            }
            
        }
        //if everything is correclty pressed/remembered then a new sequence can begin
        if(playerSequenceList.Count == playerTaskList.Count)
        {
            //ScoreManager.instance.AddPoint();
            //console1.text="Good job! Next level";
            console.text="Good job! Next level";
            StartCoroutine(StartNextRound());
        }
    }

    public void StartGame()
    {
        //ScoreManager.instance.Restart();
        StartCoroutine(StartNextRound());
        startButton.SetActive(false);
        console.text="Get ready!";
    }

    public IEnumerator HighlightButton(int buttonId)
    {
        clickableButtons[buttonId].GetComponent<Image>().color = buttonColors[buttonId][1];
        audioSource.PlayOneShot(buttonSoundsList[buttonId]);
        yield return new WaitForSeconds(0.5f);
        clickableButtons[buttonId].GetComponent<Image>().color = buttonColors[buttonId][0];
    }

    public IEnumerator PlayerLost()
    {
        audioSource.PlayOneShot(loseSound);
        playerSequenceList.Clear();
        playerTaskList.Clear();
        yield return new WaitForSeconds(2f);
        startButton.SetActive(true);
    }

    public IEnumerator StartNextRound()
    {
        playerSequenceList.Clear();
        buttons.interactable = false;
        yield return new WaitForSeconds(1f);
        playerTaskList.Add(Random.Range(0,4));
        console.text="Level " + (playerTaskList.Count); // Display the current level
        //console3.text = "Level " + (playerTaskList.Count); // Display the current level
        foreach(int index in playerTaskList)
        {
            yield return StartCoroutine(HighlightButton(index));
        }
        buttons.interactable = true;
        yield return null;
    }

}
