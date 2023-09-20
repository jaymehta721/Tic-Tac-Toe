using System.Collections.Generic;
using TicTacToe;
using UnityEngine;
using TicTacToe.Enums;

public class TicTacToeAI : MonoBehaviour
{
    public int MakeMove(Board board)
    {
        // Check if the AI can win in the next move
        for (int i = 0; i < board.marks.Length; i++)
        {
            if (board.marks[i] == StateMark.None)
            {
                board.MakeMove(i, StateMark.O);
                if (board.CheckForWin())
                {
                    board.UndoMove(i);
                    return i;
                }
                board.UndoMove(i);
            }
        }

        // Check if the player is about to win and block them
        for (int i = 0; i < board.marks.Length; i++)
        {
            if (board.marks[i] == StateMark.None)
            {
                board.MakeMove(i, StateMark.X);
                if (board.CheckForWin())
                {
                    board.UndoMove(i);
                    return i;
                }
                board.UndoMove(i);
            }
        }

        // Try to create two-in-a-row opportunities
        int[] moveOrder = { 4, 0, 2, 6, 8, 1, 3, 5, 7 };

        foreach (int i in moveOrder)
        {
            if (board.marks[i] == StateMark.None)
            {
                board.MakeMove(i, StateMark.O);
                bool twoInARow = CheckTwoInARow(board, StateMark.O);
                board.UndoMove(i);
                if (twoInARow)
                {
                    return i;
                }
            }
        }

        // If none of the above conditions are met, make a random move
        int randomMove;
        do
        {
            randomMove = Random.Range(0, board.marks.Length);
        } while (board.marks[randomMove] != StateMark.None);

        return randomMove;
    }
    
    private bool CheckTwoInARow(Board board, StateMark mark)
    {
        for (int i = 0; i < 3; i++)
        {
            // Check rows
            if (board.AreBoxesMatched(i * 3, i * 3 + 1, i * 3 + 2, mark))
            {
                return true;
            }

            // Check columns
            if (board.AreBoxesMatched(i, i + 3, i + 6, mark))
            {
                return true;
            }
        }

        // Check diagonals
        if (board.AreBoxesMatched(0, 4, 8, mark) || board.AreBoxesMatched(2, 4, 6, mark))
        {
            return true;
        }

        return false;
    }
}
