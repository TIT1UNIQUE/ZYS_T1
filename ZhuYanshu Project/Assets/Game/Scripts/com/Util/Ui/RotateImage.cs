using UnityEngine;

namespace com
{
    public class RotateImage : MonoBehaviour
    {
        public RectTransform trans;
        public bool useRawTime = true;
        public Vector3 Speed = new Vector3(0, 0, 1);

        void Start()
        {
            if (trans == null)
                trans = GetComponent<RectTransform>();
        }

        private void Update()
        {
            var t = useRawTime ? Time.deltaTime : GameTime.deltaTime;
            trans.localEulerAngles += Speed * t;
            trans.Rotate(Speed * t, Space.Self);
        }
    }
}
