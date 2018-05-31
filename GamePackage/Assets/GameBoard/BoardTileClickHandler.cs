using UnityEngine;

public interface IBoardTileClickHandler
{
  void HandleTile(BoardTile tile);
}

public class BoardTileClickHandler : MonoBehaviour, IBoardTileClickHandler
{
  public virtual BoardTile GetTile()
  {
    return null;
  }

  public virtual void HandleTile(BoardTile tile)
  {
    Debug.LogError("Default BoardTileClickHandler shouldn't be used.");
  }
}