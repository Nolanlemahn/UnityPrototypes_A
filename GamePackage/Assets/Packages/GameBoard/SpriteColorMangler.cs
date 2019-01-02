/*******************************************************************************
File:         SpriteColorMangler.cs
Author:       ...
Date Created: #CREATIONDATE#

Description: A brief description about what this file does, or what the purpose 
             of this class is. 

Copyright: All content © #YEAR# DigiPen (USA) Corporation, all rights reserved.
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColorMangler : MonoBehaviour
{
  SpriteRenderer m_SpriteRenderer;

  void Awake()
  {
    //this.m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
  }

  public Color GetColor()
  {
    this.m_SpriteRenderer = this.GetComponent<SpriteRenderer>();

    if (this.m_SpriteRenderer != null && this.m_SpriteRenderer.material != null)
      return this.m_SpriteRenderer.material.color;

    return Color.clear;
  }

  public void ChangeColor(Color c)
  {
    this.m_SpriteRenderer = this.GetComponent<SpriteRenderer>();

    if (this.m_SpriteRenderer != null && this.m_SpriteRenderer.material != null)
      this.m_SpriteRenderer.material.color = c;
    else
    {
      Debug.Log("Had no renderer.");
    }
  }
}
