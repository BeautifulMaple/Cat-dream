using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField] private Transform[] pictures1;
    [SerializeField] private Transform[] pictures2;
    [SerializeField] private Transform[] pictures3;

    [SerializeField] private GameObject catImage1;
    [SerializeField] private GameObject catImage2;
    [SerializeField] private GameObject catImage3;

    [SerializeField] private GameObject normalStatue;
    [SerializeField] private GameObject glowingStatue;

    [SerializeField] private AudioClip puzzleCompleteSound; // Sound to play when a puzzle is completed
    [SerializeField] private AudioClip glowingStatueSound; // Sound to play when glowing statue is activated

    public static bool youWin;
    public static bool isGlowingStatueActive;

    private AudioSource audioSource; // Audio source for playing sound
    private int currentStage = 1;

    private bool glowingStatueSoundPlayed = false; // To ensure the glowing statue sound plays only once

    // Start is called before the first frame update
    void Start()
    {
        normalStatue.SetActive(true);
        glowingStatue.SetActive(false);
        youWin = false;
        isGlowingStatueActive = false;

        // Add an AudioSource component to the GameControl object
        audioSource = gameObject.AddComponent<AudioSource>();

        ActivateCurrentStage();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStage == 1 && AllPicturesAligned(pictures1))
        {
            DeactivateCurrentStage();
            currentStage = 2;
            ActivateCurrentStage();
        }
        else if (currentStage == 2 && AllPicturesAligned(pictures2))
        {
            DeactivateCurrentStage();
            currentStage = 3;
            ActivateCurrentStage();
        }
        else if (currentStage == 3 && AllPicturesAligned(pictures3))
        {
            ActivateGlowingStatue();
        }
    }

    private bool AllPicturesAligned(Transform[] pictures)
    {
        for (int i = 0; i < pictures.Length; i++)
        {
            if (pictures[i].rotation.z != 0)
            {
                return false;
            }
        }
        return true;
    }

    private void ActivateCurrentStage()
    {
        catImage1.SetActive(currentStage == 1);
        catImage2.SetActive(currentStage == 2);
        catImage3.SetActive(currentStage == 3);
    }

    private void DeactivateCurrentStage()
    {
        if (currentStage == 1)
        {
            catImage1.SetActive(false);
        }
        else if (currentStage == 2)
        {
            catImage2.SetActive(false);
        }
        else if (currentStage == 3)
        {
            catImage3.SetActive(false);
        }

        // Play the puzzle completion sound effect
        if (puzzleCompleteSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(puzzleCompleteSound);
        }
    }

    private void ActivateGlowingStatue()
    {
        if (!isGlowingStatueActive)
        {
            youWin = true;
            normalStatue.SetActive(false);
            glowingStatue.SetActive(true);
            isGlowingStatueActive = true;

            // Play the glowing statue sound effect only once
            if (glowingStatueSound != null && audioSource != null && !glowingStatueSoundPlayed)
            {
                audioSource.PlayOneShot(glowingStatueSound);
                glowingStatueSoundPlayed = true;
            }
        }
    }
}
