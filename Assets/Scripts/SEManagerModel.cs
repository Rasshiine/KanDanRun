using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManagerModel : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip startButton;
    [SerializeField] private AudioClip backButton;
    [SerializeField] private AudioClip levelUp;
    [SerializeField] private AudioClip changeWeather;
    [SerializeField] private AudioClip changeCloth;
    [SerializeField] private AudioClip damaged_Hot;
    [SerializeField] private AudioClip damaged_Cold;
    [SerializeField] private AudioClip gameOver;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartButton() => audioSource.PlayOneShot(startButton);
    public void BackButton() => audioSource.PlayOneShot(backButton);
    public void LevelUp() => audioSource.PlayOneShot(levelUp);
    public void ChangeWeather() => audioSource.PlayOneShot(changeWeather);
    public void ChangeCloth() => audioSource.PlayOneShot(changeCloth);
    public void Damaged_Hot() => audioSource.PlayOneShot(damaged_Hot);
    public void Damaged_Cold() => audioSource.PlayOneShot(damaged_Cold);
    public void GameOverSound() => audioSource.PlayOneShot(gameOver); 
}
