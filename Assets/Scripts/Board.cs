using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject titleObject;
    public float cameraSizeOffset;
    public float cameraVerticalOffset;

    public GameObject[] availablePieces;
    Tile[,] Tiles;
    Piece[,] Pieces;
    Tile startTile;
    Tile endTile;
    // Start is called before the first frame update
    void Start()
    {
        Tiles = new Tile[width,height];
        Pieces = new Piece[width,height];
        SetupBoard();
        PositionCamera();
        SetupPieces();
        
    }
    private void SetupPieces()
    {
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y <height; y++)
            {
                var selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)];
                var temporalObject = Instantiate(selectedPiece,new Vector3(x,y,-5),Quaternion.identity); 
                temporalObject.transform.parent = this.transform;
                Pieces[x,y] = temporalObject.GetComponent<Piece>();
                Pieces[x,y].Setup(x, y, this);
            }

        }
    }
    private void PositionCamera()
    {
        float newPosX = (float)width / 2f;
        float newPosY = (float)height / 2f;
        Camera.main.transform.position = new Vector3(newPosX - 0.5f, newPosY - 0.5f + cameraVerticalOffset, -10f);
        float horizontal = width+1;
        float vertical = (height/2)+1;

        Camera.main.orthographicSize = horizontal > vertical? horizontal + cameraSizeOffset : vertical + cameraSizeOffset;
    }
    private void SetupBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y <height; y++)
            {
                var temporalObject = Instantiate(titleObject,new Vector3(x,y,-5),Quaternion.identity); 
                temporalObject.transform.parent = this.transform;
                Tiles[x,y] = temporalObject.GetComponent<Tile>();
                Tiles[x,y]?.Setup(x, y, this);
            }

        }
    }

    public void TileDown(Tile tile_)
    {
        startTile = tile_;
    }
    public void TileOver(Tile tile_)
    {
        endTile = tile_;
    }
    public void TileUp(Tile tile_)
    {
        if(startTile != null && endTile != null && IsCloseTo(startTile,endTile))
        {
            SwapTiles();
        }
        startTile = null;
        endTile = null;
    }
    private void SwapTiles()
    {
        var StartPiece = Pieces[startTile.x, startTile.y];
        var EndPiece = Pieces[endTile.x, endTile.y];

        StartPiece.Move(endTile.x, endTile.y);
        EndPiece.Move(startTile.x, startTile.y);

        Pieces[startTile.x, startTile.y] = EndPiece;
        Pieces[endTile.x, endTile.y] = StartPiece;
    }
    public bool IsCloseTo(Tile start, Tile end)
    {
        if(Math.Abs(start.x -end.x) == 1 && start.y == end.y){
            return true;
        }
        if(Math.Abs((start.y -end.y)) == 1 && start.x == end.x)
        {
            return true;
        }
        return false;
    }
}
