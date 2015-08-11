using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MostrarDebug : MonoBehaviour
{
	void OnEnable()
	{
		gameObject.GetComponent<Text>().text = Dados.debugMensagens;
	}
}
