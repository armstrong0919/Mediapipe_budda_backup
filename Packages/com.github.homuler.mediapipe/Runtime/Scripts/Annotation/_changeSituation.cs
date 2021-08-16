using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe
{
    public class _changeSituation : AnnotationController
    {
        HandSituation the_laststate = 0;
        public static HandSituation the_state;

        public override void Clear()
        {
            
        }

        public enum HandSituation
        {
            Wait = 0,
            JoJo = 1,
            One_Piece = 2,
            Pretty_Soldier = 3,
        }

        private void Start()
        {
            the_state = HandSituation.Wait;
        }
        public void reset_situation()
        {
            the_state = HandSituation.Wait;
        }
        public void random()
        {
          the_state = (HandSituation)Random.Range(1, 4);
         
          if (the_state == the_laststate)
          {
              the_state = (HandSituation)Random.Range(1, 4);

          }
            the_laststate = the_state;
        }
    }
}
