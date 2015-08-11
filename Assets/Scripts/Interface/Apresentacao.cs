using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Apresentacao : MonoBehaviour
{
	public float tempoComecarSumir = 3;
	public float tempoMudarDeTela = 4;

	float tempo = 0;
	float alfa = 0;
	float multiAlfa = 1;
	bool sumindo = false;
	Image imagem;

	void Awake()
	{
		if (Dados.textosMensagens == null)
		{
			Dados.textosMensagens = GerArquivo.CarregarMensagens();
		}
		
		if (Dados.realizacoes.Count == 0)
		{
			Dados.realizacoes = GerArquivo.CarregarRealizacoes();
		}
		
		if (Dados.missoes.Count == 0)
		{
			Dados.missoes = GerArquivo.CarregarMissoes();
		}

		imagem = GetComponent<Image>();
		multiAlfa = 1.5f / tempoMudarDeTela;
		tempo = Time.time + tempoComecarSumir;
	}

	void Update ()
	{
		if (Time.time > tempo)
		{
			if (sumindo == false)
			{
				sumindo = true;
				tempo = Time.time + tempoMudarDeTela;
			}
			else
			{
				Application.LoadLevel("Menu");
			}
		}

		if (sumindo)
		{
			AtualizarAlfa();
			if (Input.GetMouseButtonUp(0))
			{
				Application.LoadLevel("Menu");
			}
		}
	}

	void AtualizarAlfa()
	{
		alfa += Time.deltaTime * multiAlfa;
		if (alfa > 1)
		{
			alfa = 1;
		}
		imagem.color = new Color(
			imagem.color.r,
			imagem.color.g,
			imagem.color.b,
			alfa);
	}
}

