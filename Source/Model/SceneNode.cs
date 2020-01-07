using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour {

    protected Matrix4x4 mCombinedParentXform;
    
    public Transform AxisFrame;
    public Vector3 NodeOrigin = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;
    public Vector3 snOrigin, snUp;
    public static SceneNode selected;

    const float kAxisFrameSize = 5f;

    void Awake()
    {
        UnSelect();
    }

	// Use this for initialization
	protected void Start () {
        Debug.Assert(AxisFrame != null);
        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SetToSelect() { AxisFrame.gameObject.SetActive(true); }
    public void UnSelect() { AxisFrame.gameObject.SetActive(false); }

    private void InitializeSceneNode()
    {
        mCombinedParentXform = Matrix4x4.identity;
    }

    // tipPos: is the origin of this scene node
    // topDir: is the y-direction of this node
    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        mCombinedParentXform = parentXform * orgT * trs;

        // let's decompose the combined matrix into R, and S
        Vector3 c0 = mCombinedParentXform.GetColumn(0);
        Vector3 c1 = mCombinedParentXform.GetColumn(1);
        Vector3 c2 = mCombinedParentXform.GetColumn(2);
        Vector3 s = new Vector3(c0.magnitude, c1.magnitude, c2.magnitude);
        Quaternion q = Quaternion.LookRotation(c2, c1); // creates a rotation matrix with c2-Forward, c1-up

        snOrigin = mCombinedParentXform.GetColumn(3);
        snUp = c0;

        AxisFrame.transform.localPosition = snOrigin;  // our location is Pivot 
        AxisFrame.transform.localScale = s * kAxisFrameSize;
        AxisFrame.transform.localRotation = q;

        // propagate to all children
        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }
        
        // disenminate to primitives
        foreach (NodePrimitive np in PrimitiveList)
        {
            np.LoadShaderMatrix(ref mCombinedParentXform);
        }
    }
}
 