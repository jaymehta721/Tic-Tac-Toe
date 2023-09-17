using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {
   [Header ("UI References :")]
   [SerializeField] private GameObject uiCanvas ;
   [SerializeField] private TextMeshProUGUI uiWinnerText ;
   [SerializeField] private Button uiRestartButton ;

   [Header ("Board Reference :")]
   [SerializeField] private Board board ;

   private void Start () {
      uiRestartButton.onClick.AddListener (() => SceneManager.LoadScene (0)) ;
      board.OnWinAction += OnWinEvent ;

      uiCanvas.SetActive (false) ;
   }

   private void OnWinEvent (StateMark stateMark, Color color) {
      uiWinnerText.text = (stateMark == StateMark.None) ? "Nobody Wins" : stateMark.ToString () + " Wins." ;
      uiWinnerText.color = color ;

      uiCanvas.SetActive (true) ;
   }

   private void OnDestroy () {
      uiRestartButton.onClick.RemoveAllListeners () ;
      board.OnWinAction -= OnWinEvent ;
   }
}
