using UnityEngine ;
using UnityEngine.Events ;
using UnityEngine.UI;

public class Board : MonoBehaviour {
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

   public StateMark[] marks ;

   private Camera cam ;

   private StateMark currentStateMark ;

   private bool canPlay ;

   private LineRenderer lineRenderer ;

   private int marksCount = 0 ;

   private void Start () {
      cam = Camera.main ;
      lineRenderer = GetComponent<LineRenderer> () ;
      lineRenderer.enabled = false ;

      currentStateMark = StateMark.X ;

      marks = new StateMark[9] ;

      canPlay = true ;
   }

   private void Update () {
      if (canPlay && Input.GetMouseButtonUp (0)) {
         Vector2 touchPosition = cam.ScreenToWorldPoint (Input.mousePosition) ;
         Collider2D hit = Physics2D.OverlapCircle (touchPosition, touchRadius, boxesLayerMask) ;
   
         if (hit)
         {
            HitBox(hit.GetComponent<TileBox>());
         }
      }
   }

   private void HitBox (TileBox tileBox) {
      if (!tileBox.isMarked) {
         marks [ tileBox.index ] = currentStateMark ;

         tileBox.SetAsMarked (GetSprite (), currentStateMark, GetColor ()) ;
         marksCount++ ;

         //check if anybody wins:
         bool won = CheckIfWin () ;
         if (won) {
            if (OnWinAction != null)
               OnWinAction.Invoke (currentStateMark, GetColor ()) ;

            Debug.Log (currentStateMark.ToString () + " Wins.") ;

            canPlay = false ;
            return ;
         }

         if (marksCount == 9) {
            if (OnWinAction != null)
               OnWinAction.Invoke (StateMark.None, Color.white) ;

            Debug.Log ("Nobody Wins.") ;

            canPlay = false ;
            return ;
         }

         SwitchPlayer () ;
      }
   }

   private bool CheckIfWin () {
      return
      AreBoxesMatched (0, 1, 2) || AreBoxesMatched (3, 4, 5) || AreBoxesMatched (6, 7, 8) ||
      AreBoxesMatched (0, 3, 6) || AreBoxesMatched (1, 4, 7) || AreBoxesMatched (2, 5, 8) ||
      AreBoxesMatched (0, 4, 8) || AreBoxesMatched (2, 4, 6) ;

   }

   private bool AreBoxesMatched (int i, int j, int k) {
      StateMark m = currentStateMark ;
      bool matched = (marks [ i ] == m && marks [ j ] == m && marks [ k ] == m) ;

      if (matched)
      {
         DrawLine(i, j);
      }

      return matched ;
   }

   private void DrawLine (int i, int k) {
      lineRenderer.SetPosition (0, transform.GetChild (i).position) ;
      lineRenderer.SetPosition (1, transform.GetChild (k).position) ;
      Color color = GetColor () ;
      color.a = .3f ;
      lineRenderer.startColor = color ;
      lineRenderer.endColor = color ;

      lineRenderer.enabled = true ;
   }

   private void SwitchPlayer () {
      currentStateMark = (currentStateMark == StateMark.X) ? StateMark.O : StateMark.X ;
   }

   private Color GetColor () {
      return (currentStateMark == StateMark.X) ? colorX : colorO ;
   }

   private Sprite GetSprite () {
      return (currentStateMark == StateMark.X) ? spriteX : spriteO ;
   }
}
