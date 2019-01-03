using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSelectMenu : MonoBehaviour
{
    public int ButtonsPerRow = 4;
    [Header("Prefabs")]
    public GameObject ButtonPrefab;
    public GameObject RowPrefab;
    [Header("Runtime Objects")]
    public GameObject MenuCanvas;
    public Scrollbar Scrollbar;
    public GameObject Content;
    [Header("Data")]
    public List<string> Games;

    private void Start()
    {
        this.populateMenu();
            //Move menu to top
        this.Scrollbar.value = 1f;
    }

    private void loadFromRegistrar(string name)
    {
        GameMenuAssetList gmal = GameMenuAssetList.Get();
        gmal.Init();
        GameRegistration reg = gmal.Registrar(name);
        this.MenuCanvas.SetActive(false);
        SceneManager.LoadScene(reg.Scene, LoadSceneMode.Additive);
    }

    private void populateMenu()
    {
        var columnTicker = 0;

        var row = Instantiate(this.RowPrefab);
        row.transform.SetParent(this.Content.transform, false);

        foreach (var game in this.Games)
        {
            columnTicker++;

            var obj = Instantiate(this.ButtonPrefab);
            obj.transform.SetParent(row.transform, false);

            Text text = obj.GetComponentInChildren<Text>();
            text.text = game;

            var butt = obj.GetComponent<Button>();
            butt.onClick.AddListener(delegate { this.loadFromRegistrar(game); });

            if (columnTicker == this.ButtonsPerRow)
            {
                columnTicker = 0;
                row = Instantiate(this.RowPrefab);
                row.transform.SetParent(this.Content.transform, false);
            }
        }
    }
}