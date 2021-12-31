using System;
using System.Collections.Generic;
using UnityEngine;

public static class Board
{
    public static readonly int[,] startingBoardArray;
    public static readonly int[,] boardArray;
    private static readonly Actions actions;
    public static GameObject movedTokenPrefab;
    public static Vector2Int SelectedCoords { get; set; }
    public static int NumPlayers { get; set; }
    public static int Turn { get; set; }

    public static readonly List<int>[] turnColors;
    public enum PieceTypes
    {

        NotAvailable = -1,
        Empty = 0,
        Black = 1,
        Magenta = 2,
        Red = 3,
        Green = 4,
        Blue = 5,
        Yellow = 6,
        Hole = 7,
        Wall = 8,
        SafeEntry = 9
    }

    static Board()
    {
        NumPlayers = -1;
        boardArray = new int[,]
        {
                {-1, -1, -1, -1, -1, -1, -1, -1, -1,  1, -1, -1, -1},
                {-1, -1, -1, -1, -1, -1, -1, -1,  1,  1, -1, -1, -1},
                {-1, -1, -1, -1, -1, -1, -1,  1,  1,  1, -1, -1, -1},
                {-1, -1, -1,  6,  6,  6,  0,  0,  0,  0,  3,  3,  3},
                {-1, -1, -1,  6,  6,  0,  0,  0,  0,  0,  3,  3, -1},
                {-1, -1, -1,  6,  0,  0,  0,  0,  0,  0,  3, -1, -1},
                {-1, -1, -1,  0,  0,  0,  0,  0,  0,  0, -1, -1, -1},
                {-1, -1,  4,  0,  0,  0,  0,  0,  0,  5, -1, -1, -1},
                {-1,  4,  4,  0,  0,  0,  0,  0,  5,  5, -1, -1, -1},
                { 4,  4,  4,  0,  0,  0,  0,  5,  5,  5, -1, -1, -1},
                {-1, -1, -1,  2,  2,  2, -1, -1, -1, -1, -1, -1, -1},
                {-1, -1, -1,  2,  2, -1, -1, -1, -1, -1, -1, -1, -1},
                {-1, -1, -1,  2, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        };
        startingBoardArray = new int[,]
        {
                {-1, -1, -1, -1, -1, -1, -1, -1, -1,  1, -1, -1, -1},
                {-1, -1, -1, -1, -1, -1, -1, -1,  1,  1, -1, -1, -1},
                {-1, -1, -1, -1, -1, -1, -1,  1,  1,  1, -1, -1, -1},
                {-1, -1, -1,  6,  6,  6,  0,  0,  0,  0,  3,  3,  3},
                {-1, -1, -1,  6,  6,  0,  0,  0,  0,  0,  3,  3, -1},
                {-1, -1, -1,  6,  0,  0,  0,  0,  0,  0,  3, -1, -1},
                {-1, -1, -1,  0,  0,  0,  0,  0,  0,  0, -1, -1, -1},
                {-1, -1,  4,  0,  0,  0,  0,  0,  0,  5, -1, -1, -1},
                {-1,  4,  4,  0,  0,  0,  0,  0,  5,  5, -1, -1, -1},
                { 4,  4,  4,  0,  0,  0,  0,  5,  5,  5, -1, -1, -1},
                {-1, -1, -1,  2,  2,  2, -1, -1, -1, -1, -1, -1, -1},
                {-1, -1, -1,  2,  2, -1, -1, -1, -1, -1, -1, -1, -1},
                {-1, -1, -1,  2, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        };
        actions = new Actions();
        Turn = -1;
        movedTokenPrefab = null;
        turnColors = new List<int>[]
        {
            new List<int> { },
            new List<int> { },
            new List<int> {1, 2 },
            new List<int> {1, 5, 4 },
            new List<int> {3, 5, 4, 6},
            new List<int> { },
            new List<int> {1, 3, 5, 2, 4, 6 }
        };
    }

    public static void SetNumPlayers(int numPlayers)
    {
        NumPlayers = numPlayers;
        for (int x=0; x<GetBoardLength(); x++)
        {
            for (int z=0; z<GetBoardLength(); z++)
            {
                Vector2Int coords = new Vector2Int(x, z);
                if (GetSpot(coords) != -1 && !turnColors[NumPlayers].Contains(GetSpot(coords))){
                    SetSpot(coords, 0);
                }
            }
        }
    }

    public static int GetBoardLength()
    {
        return boardArray.GetLength(0);
    }

    public static int GetStartingSpot(Vector2Int coords)
    {
        return startingBoardArray[coords.x, coords.y];
    }

    public static int GetSpot(Vector2Int coords)
    {
        return boardArray[coords.x,coords.y];
    }

    public static void SetSpot(Vector2Int coords, int value)
    {
        boardArray[coords.x, coords.y] = value;
    }

    public static bool Move(GameObject tokenPrefab, Vector2Int moveCoords)
    {
        if (SelectedCoords.x == -1 || !IsValidMove(tokenPrefab, moveCoords))
        {
            return false;
        }
        actions.Add(new Move(tokenPrefab, SelectedCoords, moveCoords, Turn==-1));
        return true;
    }

    private static bool IsValidMove(GameObject tokenPrefab, Vector2Int moveCoords)
    {
        if ((GetSpot(moveCoords) != 0 && GetSpot(moveCoords) != (int)PieceTypes.SafeEntry) ||
            (Turn != -1 && tokenPrefab != movedTokenPrefab) ||
            (GetSpot(moveCoords) == (int) PieceTypes.SafeEntry && !tokenPrefab.GetComponent<Token>().vaccinated))
        {
            return false;
        }
        if (SelectedCoords.y == moveCoords.y)
        {
            int tokens = 0;
            int blockages = 0;
            Vector2 middle = Vector2.zero;
            for (int x = Math.Min(SelectedCoords.x, moveCoords.x) + 1; x < Math.Max(SelectedCoords.x, moveCoords.x); x++)
            {
                Vector2Int currCoords = new Vector2Int(x, moveCoords.y);

                if (GetSpot(currCoords) == (int)PieceTypes.Wall)
                {
                    blockages++;
                    tokens += 2;
                    middle += currCoords;
                }
                else if (GetSpot(currCoords) != 0
                    && GetSpot(currCoords) != (int)PieceTypes.Hole
                    && GetSpot(currCoords) != (int)PieceTypes.SafeEntry)
                {
                    blockages++;
                    tokens += 1;
                    middle += currCoords;
                }                
            }

            if (tokens == 1 || (tokens == 2 && tokenPrefab.GetComponent<Token>().jumpBoosted))
            {
                middle /= blockages;
                return middle.x - SelectedCoords.x == moveCoords.x - middle.x;
            }
            else return (Math.Abs(moveCoords.x - SelectedCoords.x) == 1 && Turn == -1);
        }
        else if (SelectedCoords.x == moveCoords.x)
        {
            int tokens = 0;
            int blockages = 0;
            Vector2 middle = Vector2.zero;
            for (int y = Math.Min(SelectedCoords.y, moveCoords.y) + 1; y < Math.Max(SelectedCoords.y, moveCoords.y); y++)
            {
                Vector2Int currCoords = new Vector2Int(moveCoords.x, y);

                if (GetSpot(currCoords) == (int)PieceTypes.Wall)
                {
                    blockages++;
                    tokens += 2;
                    middle += currCoords;
                }
                else if (GetSpot(currCoords) != 0
                    && GetSpot(currCoords) != (int)PieceTypes.Hole
                    && GetSpot(currCoords) != (int)PieceTypes.SafeEntry)
                {
                    blockages++;
                    tokens += 1;
                    middle += currCoords;
                }
            }

            if (tokens == 1 || (tokens == 2 && tokenPrefab.GetComponent<Token>().jumpBoosted))
            {
                middle /= blockages;
                return middle.y - SelectedCoords.y == moveCoords.y - middle.y;
            }
            else return (Math.Abs(moveCoords.y - SelectedCoords.y) == 1 && Turn == -1);
        }
        else if (SelectedCoords.x + SelectedCoords.y == moveCoords.x + moveCoords.y)
        {
            int tokens = 0;
            int blockages = 0;
            Vector2 middle = Vector2.zero;
            for (int x = Math.Min(SelectedCoords.x, moveCoords.x) + 1; x < Math.Max(SelectedCoords.x, moveCoords.x); x++)
            {
                Vector2Int currCoords = new Vector2Int(x, moveCoords.x + moveCoords.y - x);

                if (GetSpot(currCoords) == (int)PieceTypes.Wall)
                {
                    blockages++;
                    tokens += 2;
                    middle += currCoords;
                }
                else if (GetSpot(currCoords) != 0
                    && GetSpot(currCoords) != (int)PieceTypes.Hole
                    && GetSpot(currCoords) != (int)PieceTypes.SafeEntry)
                {
                    blockages++;
                    tokens += 1;
                    middle += currCoords;
                }
            }

            if (tokens == 1 || (tokens == 2 && tokenPrefab.GetComponent<Token>().jumpBoosted))
            {
                middle /= blockages;
                return middle.x - SelectedCoords.x == moveCoords.x - middle.x;
            }
            else return (Math.Abs(moveCoords.x - SelectedCoords.x) == 1 && Turn == -1);
        }
        return false;
    }

    public static Actions GetActions()
    {
        return actions;
    }

    public static void NextTurn()
    {
        if (Turn == -1)
        {
            return;
        }
        actions.Add(new Turn(movedTokenPrefab, Turn));
    }


    public static string toString()
    {
        string debug = "";
        for (int x = 0; x < GetBoardLength(); x++)
        {
            for (int y = 0; y < GetBoardLength(); y++)
            {
                if (boardArray[x, y] != -1)
                {
                    debug += boardArray[x, y];
                }
                debug += "\t";
            }
            debug += "\n";
        }
        return debug;
    }
}