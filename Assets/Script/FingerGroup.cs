using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe
{
    public class FingerGroup : NodeAnnotationController
    {
        public static List<NodeAnnotationController> thumb = new List<NodeAnnotationController>();
        public static List<NodeAnnotationController> index_finger = new List<NodeAnnotationController>();
        public static List<NodeAnnotationController> middle_finger = new List<NodeAnnotationController>();
        public static List<NodeAnnotationController> ring_finger = new List<NodeAnnotationController>();
        public static List<NodeAnnotationController> pinky = new List<NodeAnnotationController>();

        public void getFingers()
        {

            if (GameObject.FindGameObjectsWithTag("Finish").Length > 0)
            {
                GameObject[] _fingers = GameObject.FindGameObjectsWithTag("Finish");

                for (int i = 0; i <=4; i++)
                {
                    thumb.Add(_fingers[i].GetComponentInParent<NodeAnnotationController>());
                }
                
                for (int i = 5; i <= 8; i++)
                {
                    index_finger.Add(_fingers[i].GetComponentInParent<NodeAnnotationController>());
                }
                
                for (int i = 9; i <= 12; i++)
                {
                    middle_finger.Add(_fingers[i].GetComponentInParent<NodeAnnotationController>());
                }

                for (int i = 13; i <= 16; i++)
                {
                    ring_finger.Add(_fingers[i].GetComponentInParent<NodeAnnotationController>());
                }

                for (int i = 17; i <= 20; i++)
                {
                    pinky.Add(_fingers[i].GetComponentInParent<NodeAnnotationController>());
                }
                
            }

        }

    }
}