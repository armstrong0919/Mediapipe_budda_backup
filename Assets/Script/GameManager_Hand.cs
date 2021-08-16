using UnityEngine;

namespace Mediapipe
{
    public class GameManager_Hand : FingerGroup
    {
        [SerializeField] public GameObject Tracking_Obj;
        [SerializeField] public Vector3 ObjSize = new Vector3(2,2,2);

        public void Awake()
        {
            this.enabled = false;
        }
        public void Update()
        {
            if (middle_finger[0] != null)
            {
                Tracking_Obj.transform.position = middle_finger[0].transform.position;
                var dis = pinky[3].transform.position.y - thumb[4].transform.position.y;
                Vector3 _new = ObjSize * Mathf.Abs(dis);
                Debug.Log(_new);
                Tracking_Obj.transform.localScale = _new;
            }
            else
            {
                Debug.Log("Finger");
            }
        }
    }
}
