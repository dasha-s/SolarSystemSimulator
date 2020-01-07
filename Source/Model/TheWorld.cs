using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TheWorld : MonoBehaviour {

    public SceneNode RootNode;
    public Camera NodeCam;
    public LineSegment LineOfSight;
    private float kSightLength = 20f;
    private float kNodeCamPos = 3f;
    public GameObject rocket;
    public GameObject alien;
    public GameObject explosionModel;
    private GameObject explosion;

    // Use this for initialization
    void Start () {
        Debug.Assert(RootNode != null);
        Debug.Assert(NodeCam != null);
        Debug.Assert(LineOfSight != null);
        LineOfSight.SetWidth(0.05f);
        SceneNode.selected = RootNode;
        rocket = Instantiate(Resources.Load("AtomRocket")) as GameObject;
        rocket.SetActive(false);

        alien = Instantiate(Resources.Load("Alien")) as GameObject;
        alien.SetActive(false);
        explosion = Instantiate(explosionModel, rocket.transform.position, Quaternion.identity) as GameObject;

    }

    void Update()
    {
        Vector3 pos, dir;
        Matrix4x4 m = Matrix4x4.identity;
        RootNode.CompositeXform(ref m);
        if (rocket.activeSelf)
        {
            pos = rocket.transform.position;
            dir = rocket.transform.up;
        }
        else
        {
            pos = SceneNode.selected.snOrigin;
            dir = SceneNode.selected.snUp;
        }

        Vector3 p1 = pos;
        Vector3 p2 = pos + kSightLength * dir;
        LineOfSight.SetEndPoints(p1, p2);

        // Now update NodeCam
        NodeCam.transform.localPosition = pos + kNodeCamPos * dir;
        //NodeCam.transform.localPosition = pos;
        // NodeCam.transform.LookAt(p2, Vector3.up);
        //NodeCam.transform.forward = (p2 - NodeCam.transform.localPosition).normalized;
        NodeCam.transform.forward = dir.normalized;


        if (rocket.activeSelf)
        {
            if (Vector3.Distance(rocket.transform.position, RootNode.transform.position) < 10)
            {
                Destroy(explosion);
                explosion = Instantiate(explosionModel, rocket.transform.position, Quaternion.identity) as GameObject;
                rocket.SetActive(false);
            }
            if (Vector3.Distance(rocket.transform.position, alien.transform.position) < 2)
            {
                Destroy(explosion);
                explosion = Instantiate(explosionModel, rocket.transform.position, Quaternion.identity) as GameObject;
                alien.SetActive(false);
            }
            else
            {
                CheckCollision(RootNode.transform);
            }
        }
    }

    private void CheckCollision(Transform t)
    {
        foreach (Transform child in t)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                //Debug.Log(Vector3.Distance(rocket.transform.position, cn.transform.position));
                if (Vector3.Distance(rocket.transform.position, cn.transform.position) < 3)
                {
                    Destroy(explosion);
                    explosion = Instantiate(explosionModel, rocket.transform.position, Quaternion.identity) as GameObject;
                    rocket.SetActive(false);
                    break;
                }
                CheckCollision(child);
            }
        }
    }

    public SceneNode GetRootNode() { return RootNode; }

    public void ProcessUserSelection(string objType)
    {
        if (objType.Equals("AtomRocket"))
        {
            Vector3 p = RootNode.transform.localPosition;
            p.y = 21.0f;
            p.z = -50;
            rocket.transform.localPosition = p;
            rocket.SetActive(true);
        }
        if (objType.Equals("Alien"))
        {
            Vector3 alienPos = RootNode.transform.localPosition;
            alienPos.y = 23;
            alienPos.z = -60;
            alien.transform.localPosition = alienPos;
            alien.SetActive(true);
        } 
    }
}
