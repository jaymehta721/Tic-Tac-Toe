using System.Collections;
using TicTacToe.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TicTacToe
{
   public class Board : MonoBehaviour
   {
      
      [Header("Tile Prefab")]
      [SerializeField] private GameObject tilePrefab;
      
      [Header ("Input Settings : ")]
      [SerializeField] private LayerMask boxesLayerMask ;
      [SerializeField] private float touchRadius ;

      [Header ("Mark Sprites : ")]
      [SerializeField] private Sprite spriteX ;
      [SerializeField] private Sprite spriteO ;

      [Header ("Mark Colors : ")]
      [SerializeField] private Color colorX ;
      [SerializeField] private Color colorO ;

      public UnityAction<StateMark,Color> OnWinAction ;

      private Camera cam ;
      private StateMark[] marks ;
      private StateMark currentPlayerMark ;
      private bool canPlay ;
      private LineRenderer lineRenderer ;
      private int marksCount = 0 ;
      
      [Header("Board Size")]
      [SerializeField] private int bordSize = 3; // Adjust as needed
    
      private void Start()
      {
         cam = Camera.main;
         lineRenderer = GetComponent<LineRenderer>();
         lineRenderer.enabled = false;
         currentPlayerMark = StateMark.X;
         marks = new StateMark[9];
         canPlay = true;

         InstantiateBoard();
      }
      
      private void InstantiateBoard()
      {
         float tileSizeX = 0.35f; // Adjust as needed
         float tileSizeY = 0.35f; // Adjust as needed

         int rows = bordSize;
         int columns = bordSize;
         
         for (int row = 0; row < rows; row++)
         {
            for (int col = 0; col < columns; col++)
            {
               Vector3 position = new Vector3(
                  col * tileSizeX - (tileSizeX * (columns - 1) / 2),
                  -row * tileSizeY + (tileSizeY * (rows - 1) / 2),
                  0
               );

               GameObject tile = Instantiate(tilePrefab, transform);
               tile.transform.localPosition = position;
               tile.GetComponent<TileBox>().index = row * columns + col;
            }
         }
      }

      private void Update()
      {
         if (canPlay && Input.GetMouseButtonUp(0))
         {
            HandleClick();
         }
      }
   
      private void HandleClick()
      {
         Vector2 touchPosition = cam.ScreenToWorldPoint(Input.mousePosition);
         Collider2D hit = Physics2D.OverlapCircle(touchPosition, touchRadius, boxesLayerMask);

         if (hit)
         {
            HitBox(hit.GetComponent<TileBox>());
         }
      }

      private void HitBox(TileBox tileBox)
      {
         if (!tileBox.isMarked)
         {
            marks[tileBox.index] = currentPlayerMark;
            tileBox.SetAsMarked(GetSprite(), currentPlayerMark, GetColor());
            marksCount++;

            if (CheckForWin())
            {
               HandleWin(currentPlayerMark);
            }
            else if (marksCount == 9)
            {
               HandleDraw();
            }
            else
            {
               SwitchPlayer();
            }
         }
      }
   
      private void HandleWin(StateMark winner)
      {
         if (OnWinAction != null)
         {
            OnWinAction.Invoke(winner, GetColor());
         }

         Debug.Log(winner.ToString() + " Wins.");
         canPlay = false;
      }
   
      private void HandleDraw()
      {
         if (OnWinAction != null)
         {
            OnWinAction.Invoke(StateMark.None, Color.white);
         }

         Debug.Log("Nobody Wins.");
         canPlay = false;
      }
   
      private bool CheckForWin()
      {
         for (int i = 0; i < bordSize; i++)
         {
            if (CheckRow(i) || CheckColumn(i))
            {
               return true;
            }
         }

         return CheckDiagonals();
      }
   
      private bool CheckRow(int row)
      {
         int startIndex = row * bordSize;
         return AreBoxesMatched(startIndex, startIndex + 1, startIndex + 2);
      }

      private bool CheckColumn(int col)
      {
         return AreBoxesMatched(col, col + bordSize, col + bordSize * 2);
      }

      private bool CheckDiagonals()
      {
         return AreBoxesMatched(0, 4, 8) || AreBoxesMatched(2, 4, 6);
      }

      private bool AreBoxesMatched(int i, int j, int k)
      {
         bool matched = marks[i] == currentPlayerMark && marks[j] == currentPlayerMark && marks[k] == currentPlayerMark;

         if (matched)
         {
            StartCoroutine(DrawLine(i, k));
         }

         return matched;
      }

      private IEnumerator DrawLine(int i, int k)
      {
         lineRenderer.SetPosition(0, transform.GetChild(i).position);
         lineRenderer.SetPosition(1, transform.GetChild(i).position); // Start with both points at the same position

         Color color = GetColor();
         color.a = 0.3f;
         lineRenderer.startColor = color;
         lineRenderer.endColor = color;
         lineRenderer.enabled = true;

         float duration = 0.5f; // Adjust as needed
         float startTime = Time.time;

         while (Time.time - startTime < duration)
         {
            float t = (Time.time - startTime) / duration;
            Vector3 endPosition = Vector3.Lerp(transform.GetChild(i).position, transform.GetChild(k).position, t);
            lineRenderer.SetPosition(1, endPosition);
            yield return null;
         }

         // Ensure the line reaches the final position
         lineRenderer.SetPosition(1, transform.GetChild(k).position);
      }

      private void SwitchPlayer () 
      {
         currentPlayerMark = (currentPlayerMark == StateMark.X) ? StateMark.O : StateMark.X ;
      }

      private Color GetColor () 
      {
         return (currentPlayerMark == StateMark.X) ? colorX : colorO ;
      }
   
      private Sprite GetSprite () 
      {
         return (currentPlayerMark == StateMark.X) ? spriteX : spriteO ;
      }
   }
}
