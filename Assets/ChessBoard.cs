using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ChessBoardPos
{
    public Transform piece;
    public Vector3 startingPos;
}

public class ChessBoard : MonoBehaviour
{
    public List<ChessBoardPos> allPieces;

    public int threshold = 8;

    public AudioSource audio;

    public void Start()
    {
        StartCoroutine("UpdateChess");
    }

    public IEnumerator UpdateChess()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < allPieces.Count; i++)
        {
            ChessBoardPos piece = allPieces[i];
            piece.startingPos = allPieces[i].piece.position;
            allPieces[i] = piece;
        }

        do
        {
            //yield return new WaitForSeconds(0.1f);
            yield return new WaitForEndOfFrame();
            bool moved = false;
            for (int i = 0; i < allPieces.Count && i <= threshold; i++)
            {
                if(allPieces[i].startingPos != allPieces[i].piece.position)
                {
                    allPieces.RemoveAt(i);
                    i--;
                    moved = true;
                }
            }
            if(moved)
                audio.Play();
        } while (allPieces.Count > threshold);

        GameController.Instance.spawner.spawnAt("Madness", this.transform.position, Vector3.up, GameController.Instance.transform);
        GameController.Instance.pictureTaker.takePicture(GameController.Instance.playerCat.transform, this.transform, "Checkmate... MATE!");
    }
}
