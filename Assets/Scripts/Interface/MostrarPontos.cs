using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MostrarPontos : MonoBehaviour 
{
	public Text [] textos;

	string mascaraPontos;
	string pontos;

	void Awake()
	{
		mascaraPontos = "";
		for (int i = 0; i < textos.Length; i++)
		{
			mascaraPontos += "0";
		}
	}

	void Update()
	{
		pontos = Dados.pontosAtuais.ToString(mascaraPontos);
		for (int i = 0; i < textos.Length; i++)
		{
			textos[i].text = pontos[textos.Length - i - 1]
				.ToString();
		}
	}
}
