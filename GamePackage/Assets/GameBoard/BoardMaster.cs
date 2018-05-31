
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NTBUtils;
using UnityEngine;
using UnityEngine.Events;

public class BoardMaster : SingletonBehavior<BoardMaster>, IHasEditorInvokables
{
  [Serializable]
  public struct TileTask
  {
    public int x;
    public int y;
    public Color toColor;
    public TileAction action;
    public float time;

    public TileAction.Actions PresetAction;
  };

  public bool SkipRegen = false;

  public GameObject TilePrefab;
  public int width;
  public int height;
  public float xOffset;
  public float zOffset;
  public BoardTile[,] Tiles;
  public EditorInvokablesList EditorInvokables = new EditorInvokablesList();

  public List<TileTask> ColorTasks = new List<TileTask>();

  public bool TargetOnClick = false;
  public GameObject TargetGobj = null;

  public BoardTileClickHandler Callback = null;

  public void setInvokables()
  {
    this.EditorInvokables = new EditorInvokablesList();
    this.EditorInvokables.AddRange(new Action[]
    {
      this.GenerateBoard,
      this.DeleteAllChildren
    });
  }

  protected override void Awake()
  {
    base.Awake();
    if (this.SkipRegen)
    {
      this.Tiles = new BoardTile[this.width, this.height];
      List<GameObject> children = this.gameObject.transform.GetChildren();
      Debug.Log("Found " + children.Count + " children");
      foreach (GameObject d in children)
      {
        BoardTile bt = d.GetComponent<BoardTile>();
        if (bt)
        {
          bt.ColorManager.ChangeColor(Color.black);
          this.Tiles[bt.x, bt.y] = bt;
        }
        else
        {
          Debug.Log("Non-BT child found.");
        }
      }
      return;
    }
    this.DeleteAllChildren();
    this.GenerateBoard();
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit[] hits = Physics.RaycastAll(ray);
      Transform[] transforms = new Transform[hits.Length];
      for (int i = 0; i < hits.Length; i++) transforms[i] = hits[i].transform;

      Array.Sort(transforms, new ProximityPositionComparer(Camera.main.transform));

      foreach (Transform hit in transforms)
      {
        BoardTile btCandidate = hit.transform.gameObject.GetComponent<BoardTile>();
        if (btCandidate)
        {
          if (this.TargetOnClick)
          {
            Vector3 newPos = new Vector3(btCandidate.transform.position.x,
              this.TargetGobj.transform.position.y,
              btCandidate.transform.position.z);
            this.TargetGobj.transform.position = newPos;
          }

          this.Callback.HandleTile(btCandidate);
          return;
        }
      }
    }
  }

  void LateUpdate()
  {
    foreach (BoardTile tile in this.Tiles)
    {
      if (tile.Expired())
      {
        //tile.ColorManager.ChangeColor(Color.black);
        tile.Action = new BaseTileAction();
        tile.Action.Init();
      }
    }
    foreach (TileTask task in this.ColorTasks)
    {
      if (this.IsValidTask(task))
      {
        BoardTile tile = this.Tiles[task.x, task.y];
        tile.ColorManager.ChangeColor(task.toColor);
        if (task.PresetAction != TileAction.Actions.NONE)
        {
          tile.Action = TileAction.Create(task.PresetAction, tile);
        }
        else
        {
          tile.Action = task.action;
        }
        tile.Action.Init();
        tile.TimeActive = task.time;
        tile.Tick();
      }
    }
    this.ColorTasks.Clear();
  }

  public void DrawTile(int x, int y, Color c, List<Pair<int, int>> offsets = null, TileAction action = null,
    float time = 0.0f)
  {
    if (action == null)
    {
      action = new BaseTileAction();
    }
    action.Init();

    if (offsets == null)
    {
      this.ColorTasks.Add(new TileTask()
      {
        x = x,
        y = y,
        toColor = c,
        action = action,
        time = time
      });
    }
    else
    {
      foreach (Pair<int, int> offset in offsets)
      {
        this.ColorTasks.Add(new TileTask()
        {
          x = x + offset.First,
          y = y + offset.Second,
          toColor = c,
          action = action,
          time = time
        });
      }
    }
  }

  bool IsValidTask(TileTask ct)
  {
    return !(ct.x < 0 || ct.x > this.width - 1 || ct.y < 0 || ct.y > this.height - 1) ;
  }
  void DeleteAllChildren()
  {
    foreach (GameObject child in this.transform.GetChildren())
    {
      DestroyImmediate(child);
    }
  }

  [EditorInvokable]
  void GenerateBoard()
  {
    this.Tiles = new BoardTile[this.width, this.height];
    Vector3 center = this.transform.position;

    for (int i = 0; i < this.width; i++)
    {
      for (int j = 0; j < this.height; j++)
      {
        Vector3 loc = new Vector3(center.x + i * xOffset, center.y, center.z + j * zOffset);

        GameObject gobj = Instantiate(this.TilePrefab, loc, Quaternion.identity);
        gobj.name = this.TilePrefab.name + i + ", " + j;
        gobj.transform.parent = this.transform;
        BoardTile bt = gobj.GetComponent<BoardTile>() ?? gobj.AddComponent<BoardTile>();
        this.Tiles[i, j] = bt;
        this.Tiles[i, j].x = i;
        this.Tiles[i, j].y = j;
      }
    }
  }
}
