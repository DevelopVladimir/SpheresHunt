using UnityEngine;
using System.Collections;

namespace SpheresHunt
{

    public class SphereController : MonoBehaviour
    {

        Transform trans;
        public float speed;
        GameController gc;
        float angleSpeed = 90;
        Renderer rend;
        Collider col;

        void OnEnable()
        {
            trans = gameObject.GetComponent<Transform>();
            gc = GameController.Instance;
            //speed = gc.baseSpeed * (1 + 6 * trans.localPosition.z / gc.sphereSpawnGenSize);
            rend = GetComponent<Renderer>();
            col = GetComponent<Collider>();
        }

        void Update()
        {
            trans.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
            if (-trans.localPosition.y > gc.sceneSizes.y - gc.sphereSize / 2)
                gc.sg.PushSphereToPool(this);//DeactivateSphere();//gc.sg.ResetSphere(gameObject);
            trans.Rotate(Vector3.up * Time.deltaTime * angleSpeed);
        }

        public void DeactivateSphere()
        {
            col.enabled = false;
            rend.enabled = false;
            this.enabled = false;
            trans.localPosition = Vector3.zero;
        }
    }
}


