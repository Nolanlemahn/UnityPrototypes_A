using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectMenu : MonoBehaviour
{
    [Serializable]
    public class GameRegistration
    {
        public string GameName = "null";
        public Sprite ButtonImage = null;
    }

    public int ButtonsPerRow = 4;
    [Header("Prefabs")]
    public GameObject ButtonPrefab;
    public GameObject RowPrefab;
    [Header("Runtime Objects")]
    public Scrollbar Scrollbar;
    public GameObject Content;
    [Header("Data")]
    public List<GameRegistration> Games;

    private void Start()
    {
        this.PopulateMenu();
            //Move menu to top
        this.Scrollbar.value = 1f;
    }

    private void PopulateMenu()
    {
        var columnTicker = 0;

        var anchor = this.Content.transform.position;
        var row = Instantiate(this.RowPrefab);
        row.transform.SetParent(this.Content.transform, false);

        foreach (var game in this.Games)
        {
            var obj = Instantiate(this.ButtonPrefab);
            obj.transform.SetParent(row.transform, false);
            columnTicker++;

            var butt = obj.GetComponent<Button>();

            if (columnTicker == this.ButtonsPerRow)
            {
                columnTicker = 0;
                row = Instantiate(this.RowPrefab);
                row.transform.SetParent(this.Content.transform, false);
            }
        }
    }
}