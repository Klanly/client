using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextLanguage : MonoBehaviour {

	// Use this for initialization
	void Start () {

        var panelManager = SimpleFramework.LuaHelper.GetPanelManager();

        if ( panelManager == null )
        {
            GameObject.DestroyObject(  this.gameObject.GetComponent<TextLanguage>() );

            return;
        }

        var  textLst = this.gameObject.GetComponentsInChildren<Text>(true);

        for ( int i = 0 ; i < textLst.Length; i++ )
        {

           var textValue =  panelManager.getCont( textLst[i].text == null ? "" : textLst[ i ].text );

            textLst[ i ].text = textValue;

        }

        

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
