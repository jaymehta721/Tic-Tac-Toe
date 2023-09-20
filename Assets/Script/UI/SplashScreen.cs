using System;
using System.Collections;
using System.Collections.Generic;
using TicTacToe;
using TicTacToe.Enums;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
   [SerializeField] private Board board;
   [SerializeField] private Canvas splashCanvas;
   [SerializeField] private Button crossButton;
   [SerializeField] private Button circleButton;

   private void Start()
   {
      crossButton.onClick.AddListener(() => OnStartButtonClick("x"));
      circleButton.onClick.AddListener(() => OnStartButtonClick("o"));
   }


   private void OnStartButtonClick(string val)
   {
      splashCanvas.enabled = false;
      board.PlayerMark = val.Equals("x") ? StateMark.X : StateMark.O;
      board.AiMark = val.Equals("o") ? StateMark.O: StateMark.X;
   }


}
