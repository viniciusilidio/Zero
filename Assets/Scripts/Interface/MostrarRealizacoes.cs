using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MostrarRealizacoes : MonoBehaviour
{
	public float posicaoInicial = -60;
	public float tamanho = 100;
	public float distancia = 10;
	public float tamanhoPainel = 480;

	public RectTransform painel;

	public GameObject realizacao;

	string nomePontos = "txtPontos";
	string nomeTitulo = "txtTitulo";
	string nomeDescricao = "txtDescricao";
	string nomeImagem = "imgPontos";

	static Text [] textosDescricao;
	static Text [] textosTitulos;
	static Image [] imagensPontos;

	static bool pronto = false;
	bool carregado = false;

	void Awake()
	{
		Ativar();
	}

	public void Ativar()
	{
		if (carregado)
		{
			return;
		}
		carregado = true;

		if (Dados.realizacoes.Count == 0)
		{
			Dados.realizacoes = GerArquivo.CarregarRealizacoes();
		}

		float posAtual = posicaoInicial;
		float tamanhoTotal = 0;

		textosDescricao = new Text[Dados.realizacoes.Count];
		textosTitulos = new Text[Dados.realizacoes.Count];
		imagensPontos = new Image[Dados.realizacoes.Count];

		for(int i = 0; i < Dados.realizacoes.Count; i++)
		{
			GameObject reali = (GameObject) Instantiate(
				realizacao);
			reali.transform.SetParent(painel, false);
			reali.GetComponent<RectTransform>().localPosition =
				new Vector3(0, posAtual, 0);

			tamanhoTotal += tamanho + distancia;
			posAtual = posicaoInicial - tamanhoTotal;

			textosDescricao[i] = reali.transform.FindChild(nomeDescricao)
				.GetComponent<Text>();
			textosTitulos[i] = reali.transform.FindChild(nomeTitulo)
				.GetComponent<Text>();
			imagensPontos[i] = reali.transform.FindChild(nomeImagem)
				.GetComponent<Image>();

			reali.transform.FindChild(nomePontos)
				.GetComponent<Text>().text = "" + 
					Dados.realizacoes[i].pontos;
			textosTitulos[i].text = Dados.realizacoes[i].titulo;
			textosDescricao[i].text = Dados.realizacoes[i].descricao;

			if (Dados.realizacoes[i].completa)
			{
				textosTitulos[i].color = Constantes.corMultiplicador;
				textosDescricao[i].color = Constantes.corMultiplicador;
				imagensPontos[i].color = Constantes.corMultiplicador;
			}
		}

		painel.sizeDelta = new Vector2(
			painel.sizeDelta.x, tamanhoTotal + 15);
		//painel.sizeDelta = new Vector2(
		//	painel.sizeDelta.x, 4140);

		pronto = true;

		Utilidade.DebugMensagem("Painel: "+painel.sizeDelta.y);
	}

	public static void VerificarCor()
	{
		if (pronto)
		{
			for(int i = 0; i < Dados.realizacoes.Count; i++)
			{
				if (Dados.realizacoes[i].completa)
				{
					textosTitulos[i].color =
						Constantes.corMultiplicador;
					textosDescricao[i].color =
						Constantes.corMultiplicador;
					imagensPontos[i].color =
						Constantes.corMultiplicador;
				}
				else
				{
					textosTitulos[i].color =
						Constantes.corZero;
					textosDescricao[i].color =
						Constantes.corZero;
					imagensPontos[i].color =
						Constantes.corZero;
				}
			}
		}
	}
}

