using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Camera_Target : MonoBehaviour {
	public Transform m_TargetOffset;

	void LateUpdate(){
		transform.LookAt (m_TargetOffset);
	}

}
