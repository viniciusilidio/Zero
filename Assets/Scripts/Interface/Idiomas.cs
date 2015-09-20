using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Idiomas : MonoBehaviour
{
	public Button btIngles;
	public Button btPortugues;

	public Color corSelecionado = Color.green;

	Color cor;

	void Awake()
	{
		cor = btIngles.image.color;
		TrocarIdioma(Dados.linguaAtual, false);
	}

	public void TrocarIdioma(int id)
	{
		TrocarIdioma(id, true);
	}

	void TrocarIdioma(int id, bool trocar)
	{
		if (btIngles == null || btPortugues == null) return;
		
		if (id == 1)
		{
			btIngles.image.color = cor;
			btIngles.interactable = true;
			btPortugues.image.color = corSelecionado;
			btPortugues.interactable = false;
		}
		else
		{
			btIngles.image.color = corSelecionado;
			btIngles.interactable = false;
			btPortugues.image.color = cor;
			btPortugues.interactable = true;
		}
		
		Dados.linguaAtual = id;
		PlayerPrefs.SetInt(Dados.chaveSalvarLingua, 
		                   Dados.linguaAtual);
		
		if (trocar)
		{
			Utilidade.VerificarRecarregarIdiomas(true);
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}

