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
      crossButton.onClick.AddListener(() => OnStartButtonClick(StateMark.X));
      circleButton.onClick.AddListener(() => OnStartButtonClick(StateMark.O));
   }


   private void OnStartButtonClick(StateMark selectedMark)
   {
      splashCanvas.enabled = false;
      board.InitializeGame(selectedMark);

   }


}
