using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class GerJogo : MonoBehaviour
{
	static GerJogo instancia = null;

	public GameObject objTutorial;

	public GameObject botaoBase;
	public float distanciaPorcentagem = 0.42f;
	public int quantidadeInicial = 3;
	public RectTransform [] gradePosicoes;
	public Transform painelRastros;
	public float tempoSalvar = 5;
	public float intervaloMensagemDeListaCheia = 10;

	public Text textoResetAsteriscos;
	//public Text textoResetValor;

	float proximoSalvar = 0;

	public static List<GerBotao> objetos = new List<GerBotao>();
	static GameObject botaoBaseEstatico;
	static Transform transformEstatico;
	static float distanciaJuntar = 0;
	static float ladoBotao = 90;

	static Vector3 [] grade;
	static int qtdMaxima = 1;
	//static List<int> posicoesLivres = new List<int>();
	//static List<int> posicoesOcupadas = new List<int>();

	public float intervaloCriarBloco = 10.0f;
	float tempoProximoBloco = 0;

	static float tempoMensagemListaCheia = 0;
	static float intervaloMensagemListaCheia = 10;

	static Transform painelRastrosEstatico;

	static Text textoResetAsteriscosEstatico;
	//static Text textoResetValorEstatico;

	static int missaoZerar;
	static int missaoBloco;
	static int missaoPlacar;
	static int missaoMultiplicador;

	public bool resetarDadosAoIniciar = false;

	void Awake()
	{
		instancia = this;

		botaoBaseEstatico = botaoBase;
		transformEstatico = transform;
		painelRastrosEstatico = painelRastros;

		textoResetAsteriscosEstatico = textoResetAsteriscos;
		//textoResetValorEstatico = textoResetValor;

		intervaloMensagemListaCheia = intervaloMensagemDeListaCheia;

		ladoBotao = botaoBase.GetComponent<RectTransform>().sizeDelta.x;

		distanciaJuntar = distanciaPorcentagem * ladoBotao;

		qtdMaxima = gradePosicoes.Length;

		grade = new Vector3[qtdMaxima];

		/*
		posicoesLivres.Clear();
		if (posicoesLivres.Count == 0)
		{
			for (int i = 0; i < qtdMaxima; i++)
			{
				grade[i] = gradePosicoes[i].localPosition;
				posicoesLivres.Add(i);
			}
		}
		*/
		for (int i = 0; i < qtdMaxima; i++)
		{
			grade[i] = gradePosicoes[i].localPosition;
			objetos.Add(null);
		}

		if (Dados.textosMensagens == null)
		{
			Dados.textosMensagens = GerArquivo.CarregarMensagens();
		}

		if (Dados.realizacoes.Count == 0)
		{
			Dados.realizacoes = GerArquivo.CarregarRealizacoes();
		}
		GetComponent<MostrarRealizacoes>().Ativar();

		if (Dados.missoes.Count == 0)
		{
			Dados.missoes = GerArquivo.CarregarMissoes();
		}

		for(int i = 0; i < Dados.missoes.Count; i++)
		{
			switch(Dados.missoes[i].tipo){
			case Missao.Tipo.Zerar: 
				missaoZerar = i; 
				break;
			case Missao.Tipo.Bloco: 
				missaoBloco = i; 
				break;
			case Missao.Tipo.Placar: 
				missaoPlacar = i; 
				break;
			case Missao.Tipo.Multiplicador: 
				missaoMultiplicador = i; 
				break;
			}
		}

#if UNITY_EDITOR
		if (resetarDadosAoIniciar)
		{
			PlayerPrefs.DeleteAll();
		}
#endif

		if (PlayerPrefs.GetString(Dados.nomeVersaoSalvar)
		    == Dados.versaoSalvar)
		{
			Armazenador.CarregarDados();
			AjeitarResets();
		}
		else
		{
			PlayerPrefs.DeleteAll();

			PlayerPrefs.SetString(
				Dados.nomeVersaoSalvar,
				Dados.versaoSalvar);

			for(int i = 0; i < quantidadeInicial; i++)
			{
				AdicionarEmPosicaoAleatoria();
			}
		}

		tempoProximoBloco = Time.time + Dados.tempoCriarBlocos;
		proximoSalvar = 0;

		if (Dados.tutorialCompleto == false || 
		    Dados.refazerTutorial == true)
		{
			Dados.refazerTutorial = false;
			Dados.tutorialCompleto = false;
			objTutorial.SetActive(true);
			objTutorial.GetComponent<Tutorial>().Comecar();
			proximoSalvar = Time.time + 10;
			tempoProximoBloco = Time.time + 10;
		}
		else
		{
			objTutorial.SetActive(false);
		}

		Dados.tempoAnteriorDeJogo = Time.time;
	}

	static bool adicionarNovoBlocoAleatorio = false;
	void Update()
	{
		if (Dados.tutorialCompleto)
		{
			VerificarCriarBlocos();

			VerificarSalvar();

			if (adicionarNovoBlocoAleatorio)
			{
				adicionarNovoBlocoAleatorio = false;
				AdicionarEmPosicaoAleatoria();
			}
		}
	}

	public static void Salvar()
	{
		instancia.proximoSalvar = 0;
		instancia.VerificarSalvar();
	}

	void VerificarSalvar()
	{
		if (Time.time > proximoSalvar)
		{
			AjeitarTempo();

			long [] valores = new long[1];
			if (Dados.tempoTotalDeJogo >= ulong.MaxValue ||
			    Dados.tempoTotalDeJogo < 1)
			{
				Dados.tempoTotalDeJogo = 1;
			}

			if (Dados.tempoTotalDeJogo < (ulong) long.MaxValue)
			{
				valores[0] = (long) Dados.tempoTotalDeJogo;
			}
			else
			{
				valores[0] = long.MaxValue;
			}
			VerificarRealizacoes(
				Realizacao.Tipo.TempoTotal, valores);

			proximoSalvar = Time.time + tempoSalvar;

			
			Armazenador.SalvarDados();
			//Utilidade.DebugMensagem ("Dados salvos "+Time.time);
		}
	}

	static void AjeitarTempo()
	{
		Debug.Log ("ALTERANDO TEMPO DE JOGO. Tempo Atual: "+
		           Dados.tempoAtualDeJogo+", Tempo Total: "+
		           Dados.tempoTotalDeJogo+", Tempo Anterior: "+
		           Dados.tempoAnteriorDeJogo+ 
		           ", Time.time: "+Time.time);

		//Dados.tempoAtualDeJogo = Time.time - Dados.tempoAtualDeJogo;

		Dados.tempoAtualDeJogo = Time.time - Dados.tempoAnteriorDeJogo;
		Dados.tempoAnteriorDeJogo = Time.time;

		Dados.tempoTotalDeJogo += (ulong)Dados.tempoAtualDeJogo;

		Debug.Log ("ALTERANDO TEMPO DE JOGO. Tempo Atual: "+
		           Dados.tempoAtualDeJogo+", Tempo Total: "+
		           Dados.tempoTotalDeJogo+", Tempo Anterior: "+
		           Dados.tempoAnteriorDeJogo+ 
		           ", Time.time: "+Time.time);
	}

	void OnDestroy()
	{
		//AjeitarTempo();
		//Armazenador.SalvarDados();
		Salvar();
	}

	void VerificarCriarBlocos()
	{
		VerificarAdicionarLista();
		if (Dados.blocosCriarPorTempo &&
		    Time.time > tempoProximoBloco)
		{
			tempoProximoBloco = Time.time + Dados.tempoCriarBlocos;
			//AdicionarEmPosicaoAleatoria();
			adicionarNovoBlocoAleatorio = true;
		}
	}

	static int PegarProximaPosicaoLivre()
	{
		/*
		if (posicoesLivres.Count == 0 ||
		    posicoesOcupadas.Count > qtdMaxima)
		{
			return -1;
		}
		*/
		if (PosicoesOcupadas() >= qtdMaxima)
		{
			return -1;
		}
		
		//int ind = Utilidade.AleatorioLista<int>(posicoesLivres);
		//posicoesOcupadas.Add(ind);
		int ind = Random.Range(0, qtdMaxima);
		int indAnterior = ind;
		while(objetos[ind] != null)
		{
			ind++;
			if (ind >= qtdMaxima)
			{
				ind = 0;
			}
			if (ind == indAnterior)
			{
				ind = -1;
				break;
			}
		}

		return ind;
	}

	static void LiberarPosicao(int p)
	{
		//posicoesOcupadas.Remove(p);
		//posicoesLivres.Add(p);
		if (objetos[p] != null)
		{
			//objetos[p].Destruir();
			objetos[p] = null;
		}
	}

	static bool PosicaoOcupada(int p)
	{
		//return posicoesOcupadas.Contains(p);
		return objetos[p] != null;
	}

	static List<int> listaPosicoesAdicionar = new List<int>();
	static void VerificarAdicionarLista()
	{
		if (listaPosicoesAdicionar.Count > 0)
		{
			int pos = listaPosicoesAdicionar[0];
			CriarBotao(pos);
			listaPosicoesAdicionar.RemoveAt(0);
		}
	}

	static void AdicionarEmPosicaoAleatoria()
	{
		int pos = PegarProximaPosicaoLivre();
		if (pos != -1)
		{
			listaPosicoesAdicionar.Add(pos);
		}
		else
		{
			// Lista esta cheia!
			MensagemListaCheia();
		}
	}

	//static int cont = 0;
	public static void AdicionarNaGrade(int tipo, int valor, int pos)
	{
		//cont++;
		//Debug.Log ("Entoru aqui pera "+cont+" vez!");
		//if (posicoesLivres.Count > 0)
		bool listaCheia = true;
		foreach(GerBotao b in objetos)
		{
			if (b == null)
			{
				listaCheia = false;
				break;
			}
		}

		if (listaCheia == false)
		{
			bool cheio = false;
			int posAnterior = pos;
			while(objetos[pos] != null)
			{
				pos++;
				if (pos >= qtdMaxima)
				{
					pos = 0;
				}
				if (pos == posAnterior)
				{
					cheio = true;
					break;
				}
			}

			if (cheio == false)
			{

				Tipos.Botao tipoBloco = Tipos.Botao.Zero;
				switch(tipo){
				case 0: tipoBloco = Tipos.Botao.Zero; break;
				case 1: tipoBloco = Tipos.Botao.Positivo; break;
				case 2: tipoBloco = Tipos.Botao.Negativo; break;
				case 3: tipoBloco = Tipos.Botao.Multiplicador; break;
				}
				
				GameObject botao = (GameObject) Instantiate(
					botaoBaseEstatico, Vector3.zero, Quaternion.identity);
				
				botao.GetComponent<GerBotao>()
					.Inicializar(transformEstatico, pos, 
					             painelRastrosEstatico);
				
				botao.GetComponent<GerBotao>().tipo	= tipoBloco;
				botao.GetComponent<GerBotao>().valor = valor;
				botao.GetComponent<GerBotao>().Alterar();
				
				if (tipoBloco == Tipos.Botao.Zero)
				{
					botao.GetComponent<GerBotao>().Zerou();
				}

				botao.transform.localPosition = grade[pos];

				//posicoesLivres.Remove(pos);
				//posicoesOcupadas.Add(pos);
				
				//objetos.Add(botao.GetComponent<GerBotao>());
				objetos[pos] = botao.GetComponent<GerBotao>();

				//RearranjarCenario();
			}
			else
			{
				MensagemListaCheia();
			}
		}
		else
		{
			MensagemListaCheia();
		}
	}

	static void MensagemListaCheia()
	{
		if (Time.time > tempoMensagemListaCheia)
		{
			GerMensagens.AdicionarMensagem(
				Mensagens.gradeCheia, MensagensImagens.aviso);

			Utilidade.DebugMensagem (
				"Não cabem mais objetos, max: "+qtdMaxima);

			tempoMensagemListaCheia = 
				Time.time + intervaloMensagemListaCheia;
		}
	}

	public void AdicionarBotao()
	{
		//AdicionarEmPosicaoAleatoria();
		adicionarNovoBlocoAleatorio = true;
	}

	public void AdicionarBotao(int v)
	{
		AdicionarBotao((long) v);
	}

	public void AdicionarBotao(long valor)
	{
		int pos = PegarProximaPosicaoLivre();
		if (pos != -1)
		{

			bool cheio = false;
			int posAnterior = pos;
			while(objetos[pos] != null)
			{
				pos++;
				if (pos >= qtdMaxima)
				{
					pos = 0;
				}
				if (pos == posAnterior)
				{
					cheio = true;
					break;
				}
			}
			
			if (cheio == false)
			{
				GameObject botao = (GameObject) Instantiate(
					botaoBaseEstatico, Vector3.zero, Quaternion.identity);
				
				//botao.transform.SetParent(transformEstatico, false);
				
				botao.GetComponent<GerBotao>()
					.Inicializar(transformEstatico, pos, 
					             painelRastrosEstatico);
				
				botao.GetComponent<GerBotao>().Mudar(valor);
				//botao.GetComponent<GerBotao>().posicaoGrade = pos;

				botao.transform.localPosition = grade[pos];
				
				//posicoesLivres.Remove(pos);
				//posicoesOcupadas.Add(pos);
				
				//objetos.Add(botao.GetComponent<GerBotao>());
				objetos[pos] = botao.GetComponent<GerBotao>();
				
				//RearranjarCenario();
			}
			else
			{
				MensagemListaCheia();
			}
		}
		else
		{
			// Lista esta cheia!
			MensagemListaCheia();
		}
	}

	public void AdicionarBotaoMulti()
	{
		AdicionarBotaoMulti(2);
	}

	public void AdicionarBotaoMulti(long v)
	{
		int pos = PegarProximaPosicaoLivre();
		if (pos != -1)
		{
			bool cheio = false;
			int posAnterior = pos;
			while(objetos[pos] != null)
			{
				pos++;
				if (pos >= qtdMaxima)
				{
					pos = 0;
				}
				if (pos == posAnterior)
				{
					cheio = true;
					break;
				}
			}
			
			if (cheio == false)
			{
				GameObject botao = (GameObject) Instantiate(
					botaoBaseEstatico, Vector3.zero, Quaternion.identity);
				
				//botao.transform.SetParent(transformEstatico, false);
				
				botao.GetComponent<GerBotao>()
					.Inicializar(transformEstatico, pos,
					             painelRastrosEstatico);
				
				botao.GetComponent<GerBotao>().tipo	=
					Tipos.Botao.Multiplicador;
				botao.GetComponent<GerBotao>().valor = v;
				botao.GetComponent<GerBotao>().Alterar();
				
				//botao.GetComponent<GerBotao>().posicaoGrade = pos;

				botao.transform.localPosition = grade[pos];
				
				//posicoesLivres.Remove(pos);
				//posicoesOcupadas.Add(pos);
				
				//objetos.Add(botao.GetComponent<GerBotao>());
				objetos[pos] = botao.GetComponent<GerBotao>();
				
				//RearranjarCenario();
			}
			else
			{
				MensagemListaCheia();
			}
		}
		else
		{
			// Lista esta cheia!
			MensagemListaCheia();
		}
	}

	static void CriarBotao(int p)
	{
		//botao.GetComponent<GerBotao>().posicaoGrade = p;
		bool cheio = false;
		int posAnterior = p;
		while(objetos[p] != null)
		{
			p++;
			if (p >= qtdMaxima)
			{
				p = 0;
			}
			if (p == posAnterior)
			{
				cheio = true;
				break;
			}
		}
		
		if (cheio == false)
		{
			GameObject botao = (GameObject) Instantiate(
				botaoBaseEstatico, Vector3.zero, Quaternion.identity);
			
			botao.GetComponent<GerBotao>()
				.Inicializar(transformEstatico, p, 
				             painelRastrosEstatico);

			botao.transform.localPosition = grade[p];
			
			//posicoesLivres.Remove(pos);
			//posicoesOcupadas.Add(pos);
			
			//objetos.Add(botao.GetComponent<GerBotao>());
			objetos[p] = botao.GetComponent<GerBotao>();
			
			//RearranjarCenario();
		}
		else
		{
			MensagemListaCheia();
		}
	}

	public static void SoltarObjeto(GerBotao ativo)
	{
		bool juntou = false;
		foreach(GerBotao obj in objetos)
		{
			if (obj != null && obj != ativo)
			{
				float distancia = Vector3.Distance(
					obj.transform.localPosition, 
					ativo.transform.localPosition);

				if (distancia <= distanciaJuntar)
				{
					JuntarObjetos(obj, ativo);
					juntou = true;
					break;
				}
				else
				{
					//Utilidade.DebugMensagem ("Distância: " + distancia);
				}
			}
		}

		if (juntou == false)
		{
			AjeitarPosicao(ativo);
		}

		//MostrarGrade();
	}

	static void AjeitarPosicao(GerBotao ativo)
	{
		LiberarPosicao(ativo.posicaoGrade);

		int p = PegarPosicaoMaisProxima(
			ativo.transform.localPosition);

		ativo.transform.localPosition = grade[p];
		ativo.posicaoGrade = p;

		objetos[p] = ativo;
		//posicoesOcupadas.Add(p);
		//posicoesLivres.Remove(p);
	}

	static int PegarPosicaoMaisProxima(Vector3 pos)
	{
		int retorno = -1;
		float d = float.MaxValue;

		List<int> posLivres = new List<int>();

		for(int i = 0; i < objetos.Count; i++)
		{
			if (objetos[i] == null)
			{
				posLivres.Add(i);
			}
		}

		foreach(int i in posLivres)
		{
			float d2 = Vector3.Distance(pos, grade[i]);
			if (d2 < d)
			{
				retorno = i;
				d = d2;
			}
		}

		if (retorno == -1)
		{
			Debug.LogWarning("Sem posições válidas!");
		}

		return retorno;
	}

	static void JuntarObjetos(GerBotao parado, GerBotao juntado)
	{
		long novoValor = 0;
		long pontos = 0;

		int pospar = parado.posicaoGrade;
		int posjun = juntado.posicaoGrade;

		bool mul = false;

		if (juntado.multiplicou)
		{
			parado.multiplicou = true;
		}

		Tipos.Botao tipo = juntado.tipo;

		if (parado.tipo == Tipos.Botao.Multiplicador ||
		    juntado.tipo == Tipos.Botao.Multiplicador)
		{
			novoValor = parado.valor * juntado.valor;
			if (novoValor > 0 && novoValor > Dados.blocoMaiorValor)
			{
				novoValor = Dados.blocoMaiorValor;
			}
			else if (novoValor < 0 && novoValor < -Dados.blocoMaiorValor)
			{
				novoValor = -Dados.blocoMaiorValor;
			}

			parado.multiplicou = true;

			if (parado.tipo == Tipos.Botao.Multiplicador &&
			    juntado.tipo == Tipos.Botao.Multiplicador)
			{
				mul = true;
			}
		}
		else
		{
			novoValor = parado.valor + juntado.valor;
			if (novoValor > 0 && novoValor > Dados.blocoMaiorValor)
			{
				novoValor = Dados.blocoMaiorValor;
			}
			else if (novoValor < 0 && novoValor < -Dados.blocoMaiorValor)
			{
				novoValor = -Dados.blocoMaiorValor;
			}
		}

		long [] valores = new long[2];
		valores[0] = parado.valor;
		valores[1] = juntado.valor;
		VerificarRealizacoes(
			Realizacao.Tipo.Fundir, valores);
		VerificarRealizacoes(
			Realizacao.Tipo.FundirIndiferente, valores);

		LiberarPosicao(posjun);

		//objetos.Remove(juntado);
		juntado.Destruir();

		if (novoValor == 0)
		{
			EfeitosSonoros.Tocar(EfeitosSonoros.Tipos.Zerar);

			adicionarNovoBlocoAleatorio = true;
			//AdicionarEmPosicaoAleatoria();

			if (parado.valor < 0)
			{
				pontos = -parado.valor;
			}
			else
			{
				pontos = parado.valor;
			}
			//objetos.Remove(parado);
			parado.Zerou();
			LiberarPosicao(pospar);

			VerificarMissao(missaoZerar, pontos);
		}
		else
		{
			switch(tipo){
			case Tipos.Botao.Positivo:
				EfeitosSonoros.Tocar(EfeitosSonoros.Tipos.Somar);
				break;
			case Tipos.Botao.Negativo:
				EfeitosSonoros.Tocar(EfeitosSonoros.Tipos.Subtrair);
				break;
			case Tipos.Botao.Multiplicador:
				EfeitosSonoros.Tocar(EfeitosSonoros.Tipos.Multiplicar);
				break;
			}


			valores = new long[1];
			valores[0] = novoValor;

			long absNovoValor = novoValor;
			if (novoValor < 0)
			{
				absNovoValor = -novoValor;
			}

			if (mul)
			{
				VerificarMissao(
					missaoMultiplicador, absNovoValor);
			}
			else
			{
				VerificarMissao(missaoBloco, absNovoValor);

				if (parado.multiplicou == false)
				{
					VerificarRealizacoes(
						Realizacao.Tipo.BlocoSemMultiplicar, valores);
				}
			}

			if (parado.tipo != Tipos.Botao.Multiplicador)
			{
				VerificarRealizacoes(
					Realizacao.Tipo.BlocoNormal, valores);
				VerificarRealizacoes(
					Realizacao.Tipo.BlocoMaiorQue, valores);
				VerificarRealizacoes(
					Realizacao.Tipo.BlocoAbsoluto, valores);
			}

			parado.Mudar(novoValor, mul);
			parado.Brilhar();

			if (Random.value <= Dados.chanceCriarNovoBloco)
			{
				//AdicionarEmPosicaoAleatoria();
				adicionarNovoBlocoAleatorio = true;
			}
		}

		if (Dados.tutorialCompleto == false)
		{
			pontos = 0;
		}

		//Cria funçao para mostrar pontos ganhos.
		Dados.pontosAtuais += pontos;
		VerificarMissao(missaoPlacar, Dados.pontosAtuais);

		valores = new long[1];
		valores[0] = Dados.pontosAtuais;
		VerificarRealizacoes(
			Realizacao.Tipo.PlacarExato, valores);

		/*
		Utilidade.DebugMensagem(
			"Pontos recebidos: "+pontos + 
			"; Nova pontuação: "+Dados.pontosAtuais+
			"; Resets: " + Dados.quantidadeDeResets +
			"; Pontos totais: "+Dados.pontosTotaisReais);
		//*/

		VerificarReset(pontos);

		valores = new long[1];
		if (Dados.pontosTotaisReais < (ulong) Dados.pontosParaReset)
		{
			valores[0] = (long) Dados.pontosTotaisReais;
		}
		else
		{
			valores[0] = Dados.pontosParaReset;
		}
		VerificarRealizacoes(
			Realizacao.Tipo.Placar, valores);

		AjeitarResets();

		List<long> listaValores = new List<long>();
		string vs = "Valores:";
		foreach(GerBotao bloco in objetos)
		{
			if (bloco == null) continue;
			if (bloco.tipo != Tipos.Botao.Multiplicador &&
			    bloco.tipo != Tipos.Botao.Zero)
			{
				vs += " " + bloco.valor;
				listaValores.Add(bloco.valor);
			}
		}
		VerificarRealizacoes(
			Realizacao.Tipo.VariosBlocos, listaValores.ToArray());
		//Utilidade.DebugMensagem (vs);

		valores = new long[1];
		valores[0] = (long) Dados.quantidadeDeResets;
		VerificarRealizacoes(Realizacao.Tipo.Zerar, valores);

		valores = new long[1];
		valores[0] = 1;
		foreach(Missao missao in Dados.missoes)
		{
			if (missao.Completa() == false)
			{
				valores[0] = 0;
				break;
			}
		}
		VerificarRealizacoes(
			Realizacao.Tipo.CompletarMissoes, valores);
	}

	static void VerificarReset(long pontos)
	{
		ulong resets = 0;

		while (Dados.pontosAtuais >= Dados.pontosParaReset)
		{
			Dados.pontosAtuais -= Dados.pontosParaReset;
			resets++;
		}

		if (Dados.quantidadeDeResets == 0 && resets > 0)
		{
			EnviarDadosUnityAnalytics();
		}

		Dados.quantidadeDeResets += resets;

		AjeitarPontosTotais();

		if (resets > 0)
		{
			ResetouJogo();
		}
	}

	static void EnviarDadosUnityAnalytics()
	{
		int realizacoesComplestas = 0;
		foreach(Realizacao r in Dados.realizacoes)
		{
			if (r.completa)
				realizacoesComplestas++;
		}
		Analytics.CustomEvent("gameOver", new Dictionary<string, object>
		                      {
			{ "realizacoes", realizacoesComplestas },
			{ "tempo", Dados.tempoTotalDeJogo }
		});
		Utilidade.DebugMensagem("Enviou pro analytics");
	}

	static void AjeitarPontosTotais()
	{
		ulong paraReset = (ulong) Dados.pontosParaReset;
		ulong pontos = (ulong) Dados.pontosAtuais;
		Dados.pontosTotaisReais = 
			Dados.quantidadeDeResets * paraReset + pontos;
	}

	static void ResetouJogo()
	{
		GerMensagens.AdicionarMensagem(Mensagens.jogoZerado);

		AjeitarResets();
	}

	static void AjeitarMultiplicadoresPorReset()
	{
		ulong max = (ulong) 
			(Dados.valorMultInicialMaximo - 
			 Dados.valorBotaoMultipMin);
		
		if (Dados.quantidadeDeResets < max)
		{
			Dados.valorBotaoMultipMax = ((int)
			                             Dados.quantidadeDeResets) + 
				Dados.valorBotaoMultipMin;
		}
		else
		{
			Dados.valorBotaoMultipMax = 
				Dados.valorMultInicialMaximo;
		}
	}

	public static void VerificarRealizacoes(
		Realizacao.Tipo tipo, long [] valores)
	{
		if (Dados.tutorialCompleto || Tutorial.podeMostrarConquista)
		{
			int quantidadeCompletas = 0;
			foreach(Realizacao re in Dados.realizacoes)
			{
				if (re.Verificar(tipo, valores))
				{
					Utilidade.DebugMensagem(re.titulo);
					GerMensagens.AdicionarMensagem(
						re.titulo, MensagensImagens.realizacao);
				}

				if (re.completa)
				{
					quantidadeCompletas++;
				}
			}

			if (quantidadeCompletas == 41)
			{
				long [] vs = { 42 };
				VerificarRealizacoes(Realizacao.Tipo.Resposta, vs);
			}
		}
	}

	static void VerificarMissao(int missao, long valor)
	{
		if (Dados.tutorialCompleto || Tutorial.podeMostrarConquista)
		{
			if (Dados.missoes[missao].Verificar((int)valor) > 0)
			{
				CompletouMissao(missao);
			}
		}
	}

	static List<GerBotao> objetosG = new List<GerBotao>();
	static List<int> posicoesLivresG = new List<int>();
	static List<int> posicoesOcupadasG = new List<int>();
	public static void GuardarCenario()
	{
		if (Dados.tempoTotalDeJogo > 1)
		{
			Salvar();
		}
		objetosG = objetos;
		foreach(GerBotao b in objetosG)
		{
			if (b != null)
			{
				b.Esconder();
			}
		}
		//posicoesLivresG = posicoesLivres;
		//posicoesOcupadasG = posicoesOcupadas;
		Limpar ();
	}
	public static void RecuperarCenario()
	{
		instancia.proximoSalvar = Time.time + 1;
		instancia.tempoProximoBloco = Time.time + Dados.tempoCriarBlocos;

		Limpar ();
		Armazenador.CarregarDados();

		Dados.pontosAtuais += 5;
		Dados.missoes[0].Verificar(4);

		long [] valores = new long[1];
		valores[0] = Dados.pontosAtuais;
		foreach(Realizacao re in Dados.realizacoes)
		{
			if (re.indice == 1)
			{
				re.Completou();
				break;
			}
		}

		instancia.objTutorial.SetActive(false);
		Dados.tutorialCompleto = true;

		/*
		Limpar();
		foreach(GerBotao b in objetosG)
		{
			if (b != null)
			{
				b.Mostrar();
			}
		}
		objetos = objetosG;
		//*/
		//posicoesLivres = posicoesLivresG;
		//posicoesOcupadas = posicoesOcupadasG;
		//RearranjarCenario();
	}

	/*
	public static void RearranjarCenario()
	{
		foreach(GerBotao o1 in objetos)
		{
			foreach(GerBotao o2 in objetos)
			{
				if (o1 != o2 && o1.posicaoGrade == o2.posicaoGrade)
				{
					Reposicionar(o1);
				}
			}
		}
	}

	static void Reposicionar(GerBotao bloco)
	{
		List<int> poslivres = new List<int>();
		for(int i = 0; i < qtdMaxima; i++)
		{
			poslivres.Add(i);
		}
		foreach(GerBotao b in objetos)
		{
			if(poslivres.Contains(b.posicaoGrade))
			{
				poslivres.Remove(b.posicaoGrade);
			}
		}
		int p = Random.Range(0, poslivres.Count);
		int novaPosicao = poslivres[p];
		bloco.posicaoGrade = novaPosicao;

		bloco.transform.position = grade[novaPosicao];

		posicoesLivres.Remove(novaPosicao);
		posicoesOcupadas.Add(novaPosicao);
	}
	*/

	static void CompletouMissao(int missao)
	{
		GerMensagens.AdicionarMensagem(
			Dados.missoes[missao].Mensagem());
	}

	public void Resetar()
	{
		for (int i = 0; i < objetos.Count; i++)
		{
			GerBotao gb = objetos[i];
			//Utilidade.DebugMensagem ("Objeto: "+gb.posicaoGrade);
			if (gb != null)
			{
				gb.Destruir();
			}
		}
		objetos.Clear();
		//posicoesOcupadas.Clear();
		//posicoesLivres.Clear();
		Dados.pontosAtuais = 0;
		Dados.pontosTotaisReais = 0;
		Dados.quantidadeDeResets = 0;
		Dados.tempoTotalDeJogo = 0;
		Dados.tempoAtualDeJogo = 0;

		//if (posicoesLivres.Count == 0)
		//{
			for (int i = 0; i < qtdMaxima; i++)
			{
				//posicoesLivres.Add(i);
				objetos.Add(null);
			}
		//}

		for(int i = 0; i < quantidadeInicial; i++)
		{
			AdicionarEmPosicaoAleatoria();
		}

		foreach(Missao missao in Dados.missoes)
		{
			missao.Nivel(0);
			missao.AjeitarValores();
		}

		foreach(Realizacao realizacao in Dados.realizacoes)
		{
			realizacao.completa = false;
		}

		AjeitarResets();
	}

	public static void Limpar(bool recriar = true)
	{
		for (int i = 0; i < objetos.Count; i++)
		{
			GerBotao gb = objetos[i];
			//Utilidade.DebugMensagem ("Objeto: "+gb.posicaoGrade);
			if (gb != null)
			{
				gb.Destruir();
			}
		}
		objetos.Clear();
		//posicoesOcupadas.Clear();
		//posicoesLivres.Clear();
		//*
		if (recriar)
		{
			for (int i = 0; i < qtdMaxima; i++)
			{
				//posicoesLivres.Add(i);
				objetos.Add(null);
			}
		}
		//*/
	}

	public void AdicionarPersonalizado(Text texto)
	{
		if (string.IsNullOrEmpty(texto.text) || 
		    texto.text.Length < 2)
		{
			return;
		}

		string tipo = texto.text.Substring(0,1);
		string resto = texto.text.Substring(1);

		long valor = 1;
		if (long.TryParse(resto, out valor) == false)
		{
			return;
		}

		if (tipo.ToLower() == "x")
		{
			AdicionarBotaoMulti(valor);
		}
		else if (tipo.ToLower() == "-")
		{
			AdicionarBotao(-valor);
		}
		else
		{
			AdicionarBotao(valor);
		}
	}

	public void AlterarBlocosPorTempo()
	{
		Dados.blocosCriarPorTempo = !Dados.blocosCriarPorTempo;
	}

	static void AjeitarResets()
	{
		AjeitarMultiplicadoresPorReset();
		/*
		if (Dados.quantidadeDeResets > Dados.maximoResetsMostrar)
		{
			textoResetAsteriscosEstatico.text = Dados.caractereReset;
			textoResetValorEstatico.text = "" + Dados.quantidadeDeResets;
		}
		else
		{
		*/
			string texto = "";
			for (ulong i = 0; i < Dados.quantidadeDeResets; i++)
			{
				texto += Dados.caractereReset;
			}
			textoResetAsteriscosEstatico.text = texto;
		/*
		}
		*/
	}

	public static int PosicoesOcupadas()
	{
		int p = 0;
		foreach(GerBotao b in objetos)
		{
			if (b != null)
			{
				p++;
			}
		}
		return p;
	}

	public static void FazerTutorial()
	{
		Dados.tutorialCompleto = false;
		Salvar();
		Application.LoadLevel(Constantes.telaJogo);
	}
}

