using UnityEngine;
using System.Collections;

namespace SpheresHunt
{
    public class SphereController : MonoBehaviour
    {
        private GameController gc;
        public Transform trans;
        public Renderer rend;
        public Collider col;
        public float speed;
        public float angleSpeed;

        void Awake()
        {
            trans = GetComponent<Transform>();
            rend = GetComponent<Renderer>();
            col = GetComponent<Collider>();
            gc = GameController.Instance;
        }

        void Update()
        {
            trans.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
            trans.Rotate(Vector3.up * Time.deltaTime * angleSpeed);

            if (-trans.localPosition.y > gc.sceneSizes.y - gc.sphereSize / 2)
                gc.sg.PushSphereToPool(this);
        }
    }
}


