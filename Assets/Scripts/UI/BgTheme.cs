using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgTheme : MonoBehaviour {

	private SpriteRenderer m_SpriteRenderer;
	private ManagerVars Vars;//bgTheme容器
	private void Awake() {
		Vars = ManagerVars.GetManagerVars();
		m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
		int ranValue = Random.Range(0,Vars.bgSpriteList.Count);
		m_SpriteRenderer.sprite = Vars.bgSpriteList[ranValue];
	}
}
