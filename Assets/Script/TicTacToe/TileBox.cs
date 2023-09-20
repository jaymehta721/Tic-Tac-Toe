using TicTacToe.Enums;
using UnityEngine ;


public class TileBox : MonoBehaviour
{  
    public int index { get; set; }
    public StateMark stateMark { get; private set; }
    public bool isMarked { get; private set; }

   private SpriteRenderer spriteRenderer;
   private CircleCollider2D circleCollider;
   
   private Vector3 initialScale; 
   private Vector3 targetScale;
   private float animationDuration = 0.3f;
   [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 0.35f);

   public TileBox()
   {
      index = -1;
      stateMark = StateMark.None;
      isMarked = false;
   }

   private void Awake()
   {
      spriteRenderer = GetComponent<SpriteRenderer>();
      circleCollider = GetComponent<CircleCollider2D>();
      initialScale = transform.localScale; 
      index = transform.GetSiblingIndex();
      targetScale = Vector3.one * 0.35f;
   }

   public void SetAsMarked(Sprite sprite, StateMark stateMark, Color color)
   {
      if (!isMarked)
      {
         isMarked = true;
         stateMark = stateMark;
         
         StartCoroutine(ScaleAnimation());
        // spriteRenderer.color = color;
         spriteRenderer.sprite = sprite;
         
         if (circleCollider != null)
         {
            circleCollider.enabled = false;
         }
      }
   }
   
   public void UndoMark()
   {
      if (isMarked)
      {
         isMarked = false;
         stateMark = StateMark.None;

         StartCoroutine(ScaleAnimation()); 
         //spriteRenderer.color = Color.white; 
         spriteRenderer.sprite = null; 

         if (circleCollider != null)
         {
            circleCollider.enabled = true; 
         }
      }
   }
   
   private System.Collections.IEnumerator ScaleAnimation()
   {
      float timer = 0;
      while (timer < animationDuration)
      {
         float scaleProgress = timer / animationDuration;
         float scaleValue = scaleCurve.Evaluate(scaleProgress);
         transform.localScale = Vector3.Lerp(initialScale, targetScale, scaleValue);
         timer += Time.deltaTime;
         yield return null;
      }
      
      transform.localScale = targetScale;
   }
}
