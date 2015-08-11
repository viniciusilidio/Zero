using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Gerenciador do botão
public class GerBotao : MonoBehaviour 
{
	// Atributos públicos
	public Tipos.Botao	tipo 		= Tipos.Botao.Multiplicador;
	public long			valor 		= 0;
	public float 		tempoSumir	= 1f;
	public float 		duracaoBrilho = 0.25f;
	public GerRastro	rastro;
	public string 		nomeTextoValor = "txtValor";
	public string 		nomeTextoSinal = "txtSinal";

	int [] tamanhosFonte = {
		75, 55, 40, 33, 26, 22, 19, 17, 16
	};
	int [] tamanhoSinal = {
		35, 30, 22, 19, 17, 16, 14, 14, 13
	};

	[HideInInspector]
	public int posicaoGrade = 0;

	[HideInInspector]
	public bool multiplicou	= false;

	// Atributos privados
	Color 	cor 		= Constantes.corZero;
	Image	imagem 		= null;
	Text	txtValor 	= null;
	Text	txtSinal   	= null;
	float 	tempo		= 0;
	float 	alfa		= 1;
	bool	sumir 		= false;

	bool 	brilhar 	= false;
	bool	desbrilhar 	= false;
	float 	tempoLerp 	= 0;

	GerRastro rastrinho;
	Outline brilho;

	Transform paiRastro;

	// Inicialização, chamada quando o objeto é criado.
	void Awake()
	{
		//Inicializar();
	}

	public void Inicializar(
		Transform pai = null, int pos = 0, Transform paiRastro = null)
	{
		tipo 	= Utilidade.GerarTipoBotao();
		valor 	= Utilidade.PegarValor(tipo);
		cor 	= Utilidade.CorPorTipo(tipo);
		this.paiRastro = paiRastro;
		
		//Utilidade.DebugMensagem ("Botão criado. Tipo: "+tipo+"; "+
		//           "Valor: "+valor+"; "+" Cor: "+cor);

		transform.SetParent(pai, false);
		posicaoGrade = pos;

		imagem = GetComponent<Image>();
		txtValor = 
			transform.FindChild(nomeTextoValor).GetComponent<Text>();
		txtSinal = 
			transform.FindChild(nomeTextoSinal).GetComponent<Text>();
		brilho = GetComponent<Outline>();
		brilho.enabled = false;

		rastrinho = Instantiate<GerRastro>(rastro);
		//rastrinho.Inicializar(transform, cor, paiRastro);

		Alterar();
	}

	void Update()
	{
		if (brilhar)
		{
			imagem.color = Color.Lerp(
				cor, Constantes.corBrilho, tempoLerp);
			if (tempoLerp < 1)
			{
				tempoLerp += Time.deltaTime / duracaoBrilho;
			}
			else
			{
				brilhar = false;
				desbrilhar = true;
			}
		}
		else if (desbrilhar)
		{
			imagem.color = Color.Lerp(
				Constantes.corBrilho, cor, tempoLerp);
			if (tempoLerp < 1)
			{
				tempoLerp += Time.deltaTime / duracaoBrilho;
			}
			else
			{
				desbrilhar = false;
				brilho.enabled = false;
			}
		}

		if (sumir)
		{
			alfa = (tempo - Time.time) / tempoSumir;
			if (alfa < 0)
			{
				Destruir();
			}
			else
			{
				imagem.color = new Color(
					imagem.color.r,
					imagem.color.g,
					imagem.color.b,
					alfa);

				txtValor.color = new Color(
					txtValor.color.r,
					txtValor.color.g,
					txtValor.color.b,
					alfa);

				txtSinal.color = new Color(
					txtSinal.color.r,
					txtSinal.color.g,
					txtSinal.color.b,
					alfa);
			}
		}
	}

	void GerarSinal()
	{
		string sinal = "";
		switch(tipo){
		case Tipos.Botao.Positivo: sinal = "+"; break;
		case Tipos.Botao.Negativo: sinal = "-"; break;
		case Tipos.Botao.Multiplicador: sinal = "x"; break;
		}

		txtSinal.text = sinal;
	}

	void GerarTexto()
	{
		string saidaaux = "";
		
		if (tipo == Tipos.Botao.Negativo)
		{
			saidaaux = valor.ToString().Substring(1);
		}
		else
		{
			saidaaux = valor.ToString();
		}

		string saida = "";
		int tamanho = 0;
		for(int i = saidaaux.Length - 1; i >= 0; i--)
		{
			if (tamanho == 3 || tamanho == 6)
			{
				saida = saidaaux[i] + " " + saida;
			}
			else
			{
				saida = saidaaux[i] + saida;
			}
			tamanho++;
		}

		if (saidaaux.Length >= Dados.blocoMaiorValor.ToString().Length)
		{
			saida = Dados.blocoTextoMaximoValor;
			txtValor.text = Dados.blocoTextoMaximoValor;
			txtValor.fontSize = tamanhosFonte[saida.Length - 1];
			txtSinal.fontSize = tamanhoSinal[saida.Length - 1];
			if (tipo != Tipos.Botao.Multiplicador)
			{
				txtSinal.fontSize = (txtSinal.fontSize * 3) / 2;
			}
			return;
		}

		txtValor.fontSize = tamanhosFonte[saidaaux.Length - 1];
		txtSinal.fontSize = tamanhoSinal[saidaaux.Length - 1];

		if (tipo != Tipos.Botao.Multiplicador)
		{
			txtSinal.fontSize = (txtSinal.fontSize * 3) / 2;
		}

		txtValor.text = saida;
	}

	public void Alterar()
	{
		cor 			= Utilidade.CorPorTipo(tipo);
		imagem.color 	= cor;
		GerarTexto();
		GerarSinal();
		rastrinho.Inicializar(transform, cor, paiRastro);
	}

	public void Mudar(long novoValor, bool mul = false)
	{
		valor = novoValor;
		if (mul)
		{
			tipo = Tipos.Botao.Multiplicador;
		}
		else if (valor > 0)
		{
			tipo = Tipos.Botao.Positivo;
		}
		else
		{
			tipo = Tipos.Botao.Negativo;
		}

		Alterar();
	}

	public void Zerou()
	{
		tipo	= Tipos.Botao.Zero;
		valor 	= 0;
		Alterar();

		sumir = true;
		tempo = Time.time + tempoSumir;
	}

	public void Destruir()
	{
		rastrinho.Destruir();
		Destroy(gameObject);
	}

	public void Carregar()
	{
		//Utilidade.DebugMensagem ("Começou a carregar");
		Transform pai = transform.parent;
		transform.SetParent(null);
		transform.SetParent(pai);

		transform.position = Input.mousePosition;
	}

	public void Mover()
	{
		transform.position = Input.mousePosition;

		// Usar este, caso o canvas esteja para a câmera
		/*
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
		myCanvas.transform as RectTransform, 
		Input.mousePosition, myCanvas.worldCamera, out pos);
		transform.position = myCanvas.transform.TransformPoint(pos);
		//*/
	}

	public void Soltar()
	{
		//Utilidade.DebugMensagem ("Soltou");
		GerJogo.SoltarObjeto(GetComponent<GerBotao>());
	}

	public void Brilhar()
	{
		brilhar = true;
		desbrilhar = false;
		brilho.enabled = true;
		tempoLerp = 0;
		rastrinho.MudarCor(cor);
	}
}
