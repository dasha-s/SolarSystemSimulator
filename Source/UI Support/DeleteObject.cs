using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    public XfromControl xf;
    public GameObject explosionModel;
    private GameObject explosion;

    private void Start()
    {
        explosion = Instantiate(explosionModel, xf.transform.position, Quaternion.identity) as GameObject;
    }

    public void DestroyObj()
    {
        Transform node = xf.getSelected();
        if (node != null)
        {
            SceneNode cn = node.GetComponent<SceneNode>();
            if (cn.name.Equals("Sun"))
            {
                foreach (Transform child in node)
                {
                    Destroy(explosion);
                    explosion = Instantiate(explosionModel, child.transform.position, Quaternion.identity) as GameObject;
                    child.gameObject.SetActive(false);
                }
            }

            else if (cn.name.Equals("Venus") || cn.name.Equals("Earth") || cn.name.Equals("Jupiter"))
            {
                Transform parent = node.parent;
                Destroy(explosion);
                explosion = Instantiate(explosionModel, node.transform.position, Quaternion.identity) as GameObject;
                parent.gameObject.SetActive(false);
            }
            else
            {
                Destroy(explosion);
                explosion = Instantiate(explosionModel, node.transform.position, Quaternion.identity) as GameObject;
                node.gameObject.SetActive(false);
            }
        }
    }
}
