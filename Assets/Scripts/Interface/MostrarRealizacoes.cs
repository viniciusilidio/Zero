using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MostrarRealizacoes : MonoBehaviour
{
	static MostrarRealizacoes instancia = null;

	public float posicaoInicial = -60;
	public float tamanho = 100;
	public float distancia = 10;
	public float tamanhoPainel = 480;

	public RectTransform painel;
	public RectTransform painelIngles;
	public RectTransform painelPortugues;

	public GameObject realizacao;

	public string [] tituloJanela = {"ACHIEVEMENTS","CONQUISTAS"};
	public Text txtJanela;

	public Text [] txtTitulos;
	public Text [] txtDescricoes;
	public Text [] txtPontos;
	public Image [] imgPontos;

	string nomePontos = "txtPontos";
	string nomeTitulo = "txtTitulo";
	string nomeDescricao = "txtDescricao";
	string nomeImagem = "imgPontos";

	static Text [] textosDescricao;
	static Text [] textosTitulos;
	static Image [] imagensPontos;

	static bool pronto = false;
	bool carregado = false;

	static int quantidade = 42;

	void Awake()
	{
		instancia = this;
		Ativar();
	}

	public void Ativar()
	{
		/*
		if (txtJanela)
		{
			txtJanela.text = tituloJanela[Dados.linguaAtual];
		}
		//*/

		if (Application.loadedLevelName != Constantes.telaJogo)
		{
			return;
		}

		/*
		if (carregado)
		{
			return;
		}
		//*/
		carregado = true;
		//*/

		/*
		if (Dados.linguaAtual == 1)
		{
			painel = painelPortugues;
		}
		else
		{
			painel = painelIngles;
		}
		//*/

		painel = painelPortugues;

		txtJanela.text = tituloJanela[Dados.linguaAtual];

		if (Dados.realizacoes.Count == 0)
		{
			Dados.realizacoes = GerArquivo.CarregarRealizacoes();
		}

		float posAtual = posicaoInicial;
		float tamanhoTotal = 0;

		quantidade = txtTitulos.Length;

		//textosDescricao = new Text[Dados.realizacoes.Count];
		//textosTitulos = new Text[Dados.realizacoes.Count];
		//imagensPontos = new Image[Dados.realizacoes.Count];

		//for(int i = 0; i < Dados.realizacoes.Count; i++)
		for(int i = 0; i < quantidade; i++)
		{
			txtTitulos[i].text		= Dados.realizacoes[i].titulo;
			txtDescricoes[i].text	= Dados.realizacoes[i].descricao;
			txtPontos[i].text		= "" + Dados.realizacoes[i].pontos;

			if (Dados.realizacoes[i].completa)
			{
				txtTitulos[i].color		= Constantes.corMultiplicador;
				txtDescricoes[i].color	= Constantes.corMultiplicador;
				imgPontos[i].color		= Constantes.corMultiplicador;
			}
			else
			{
				txtTitulos[i].color		= Constantes.corZero;
				txtDescricoes[i].color	= Constantes.corZero;
				imgPontos[i].color		= Constantes.corZero;
			}
			/*
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
			//*/
		}

		/*
		painel.sizeDelta = new Vector2(
			painel.sizeDelta.x, tamanhoTotal + 15);
		//*/

		//painel.sizeDelta = new Vector2(
		//	painel.sizeDelta.x, 4140);

		pronto = true;

		//Utilidade.DebugMensagem("Painel: "+painel.sizeDelta.y);
	}

	public static void VerificarCor()
	{
		if (pronto)
		{
			//for(int i = 0; i < Dados.realizacoes.Count; i++)
			for(int i = 0; i < quantidade; i++)
			{
				/*
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
				//*/
				if (Dados.realizacoes[i].completa)
				{
					instancia.txtTitulos[i].color		=
						Constantes.corMultiplicador;
					instancia.txtDescricoes[i].color	=
						Constantes.corMultiplicador;
					instancia.imgPontos[i].color		=
						Constantes.corMultiplicador;
				}
				else
				{
					instancia.txtTitulos[i].color		=
						Constantes.corZero;
					instancia.txtDescricoes[i].color	= 
						Constantes.corZero;
					instancia.imgPontos[i].color		= 
						Constantes.corZero;
				}
			}
		}
	}
}

