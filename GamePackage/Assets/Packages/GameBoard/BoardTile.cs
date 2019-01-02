/*******************************************************************************
File:         BoardTile.cs
Author:       ...
Date Created: #CREATIONDATE#

Description: A brief description about what this file does, or what the purpose 
             of this class is. 

Copyright: All content © #YEAR# DigiPen (USA) Corporation, all rights reserved.
*******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using NTBUtils;
using UnityEngine;

[Serializable]
public abstract class TileAction
{
  public enum Actions
  {
    NONE,
  }

  public abstract void Process(GameObject target);
  public abstract void Init();

  public static TileAction Create(Actions initAction, BoardTile tile = null)
  {
    switch (initAction)
    {
    }

    return new BaseTileAction();
  }
}

public class BaseTileAction : TileAction
{
  public BaseTileAction()
  { }

  public override void Init()
  {
  }

  public override void Process(GameObject target)
  {
  }
}

public class BoardTile : MonoBehaviour
{
  public int x = -1;
  public int y = -1;
  public SpriteColorMangler ColorManager;
  public TileAction Action = new BaseTileAction();
  public float TimeActive = -1.0f;
  public ZTimer Timer = new ZTimer();
  public bool DoesNotExpire = false;
  public GameObject Occupant = null;

  public void Tick()
  {
    if (this.TimeActive < 0)
    {
      this.DoesNotExpire = true;
      return;
    }
    this.Timer.Start(this.TimeActive);
  }

  void Update()
  {
    this.Timer.Tick(Time.deltaTime);
  }

  public string Print()
  {
    return this.x + ", " + this.y;
  }

  public bool Expired()
  {
    return (!this.DoesNotExpire && (this.TimeActive < 0.0f || this.Timer.Done()));
  }
}
