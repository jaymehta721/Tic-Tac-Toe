using System.Collections.Generic;
using TicTacToe;
using UnityEngine;
using TicTacToe.Enums;

public class TicTacToeAI : MonoBehaviour
{
    public int MakeMove(Board board)
    {
        int bestMove = -1;
        int bestScore = int.MinValue;

        // Loop through the board to find the best move
        for (int i = 0; i < board.marks.Length; i++)
        {
            if (board.marks[i] == StateMark.None)
            {
                // Try placing the AI's mark
                board.MakeMove(i, board.AiMark);
                int score = MiniMax(board, 0, false);
                board.UndoMove(i); // Undo the move

                // Update the best move if this move has a higher score
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        return bestMove;
    }
    private int MiniMax(Board board, int depth, bool isMaximizing)
    {
        if (board.CheckForWin())
        {
            return isMaximizing ? -1 : 1; // AI wants to minimize opponent's score and maximize its own score
        }
        else if (board.marks.Length == 9) // The board is full (draw)
        {
            return 0;
        }

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;

        for (int i = 0; i < board.marks.Length; i++)
        {
            if (board.marks[i] == StateMark.None)
            {
                // Try placing the mark for the current player
                board.MakeMove(i, isMaximizing ? board.AiMark : board.PlayerMark);
                int score = MiniMax(board, depth + 1, !isMaximizing);
                board.UndoMove(i); // Undo the move

                // Update the best score based on the current player
                if (isMaximizing)
                {
                    bestScore = Mathf.Max(bestScore, score);
                }
                else
                {
                    bestScore = Mathf.Min(bestScore, score);
                }
            }
        }

        return bestScore;
    }
}
