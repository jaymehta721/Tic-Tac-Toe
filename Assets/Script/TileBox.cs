using UnityEngine ;
using UnityEngine.Serialization;

public class TileBox : MonoBehaviour {
   public int index ;
   [FormerlySerializedAs("mark")] public StateMark stateMark ;
   public bool isMarked ;

   private SpriteRenderer spriteRenderer ;

   private void Awake () {
      spriteRenderer = GetComponent<SpriteRenderer> () ;

      index = transform.GetSiblingIndex () ;
      stateMark = StateMark.None ;
      isMarked = false ;
   }

   public void SetAsMarked (Sprite sprite, StateMark stateMark, Color color) {
      isMarked = true ;
      this.stateMark = stateMark ;

      spriteRenderer.color = color ;
      spriteRenderer.sprite = sprite ;

      //Disable the CircleCollider2D (to avoid marking it twice)
      GetComponent<CircleCollider2D> ().enabled = false ;
   }
}
