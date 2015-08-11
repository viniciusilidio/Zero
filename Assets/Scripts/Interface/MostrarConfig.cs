using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MostrarConfig : MonoBehaviour
{
	public Image somON;
	public Image somOFF;

	public Image musicaON;
	public Image musicaOFF;

	void OnEnable()
	{
		VerificarSom();
		VerificarMusica();
	}

	void VerificarSom()
	{
		if (Dados.somLigado)
		{
			somON.enabled = true;
			somOFF.enabled = false;
		}
		else
		{
			somON.enabled = false;
			somOFF.enabled = true;
		}
	}

	void VerificarMusica()
	{
		if (Dados.musicaLigado)
		{
			musicaON.enabled = true;
			musicaOFF.enabled = false;
		}
		else
		{
			musicaON.enabled = false;
			musicaOFF.enabled = true;
		}
	}

	public void Som()
	{
		Dados.somLigado = !Dados.somLigado;
		VerificarSom();
		EfeitosSonoros.Tocar();
	}

	public void Musica()
	{
		Dados.musicaLigado = !Dados.musicaLigado;
		VerificarMusica();
		EfeitosSonoros.Tocar();
	}
}

