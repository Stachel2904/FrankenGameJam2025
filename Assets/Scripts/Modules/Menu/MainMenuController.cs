using DivineSkies.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _startGame, _closeGame;
    [SerializeField] private Transform _atmo;
    [SerializeField] private float _atmoRotationSpeed;
    [SerializeField] private TextMeshProUGUI _highscores;

    private void Awake()
    {
        _startGame.onClick.AddListener(StartGame);
        _closeGame.onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        Tuple<int, DateTime>[] orderedHighscores = Array.Empty<Tuple<int,DateTime>>();
        if (PlayerPrefs.HasKey("highscores"))
        {
            string raw = PlayerPrefs.GetString("highscores");
            string[] entries = raw.Split('|');
            List<Tuple<int, DateTime>> highscores = new List<Tuple<int, DateTime>>();
            foreach (string entry in entries)
            {
                string[] data = entry.Split(',');
                Tuple<int, DateTime> highscore = new Tuple<int, DateTime>(int.Parse(data[0]), DateTime.Parse(data[1]));
                highscores.Add(highscore);
            }

            orderedHighscores = highscores.OrderByDescending(t => t.Item1).ToArray();
        }

        string output = "Highscores";
        for (int i = 0; i < 10; i++)
        {
            output += "\n" + (i + 1) + ": ";
            if (i >= orderedHighscores.Length)
            {
                output += "---";
            }
            else
            {
                output += $"{orderedHighscores[i].Item1} ({orderedHighscores[i].Item2.ToShortTimeString()} {orderedHighscores[i].Item2.ToShortDateString()})";
            }
        }

        _highscores.text = output;
    }

    private void Update()
    {
        _atmo.Rotate(Vector3.up, _atmoRotationSpeed * Time.deltaTime);
    }

    private void StartGame()
    {
        ModuleController.LoadScene(SceneNames.GameScene);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
