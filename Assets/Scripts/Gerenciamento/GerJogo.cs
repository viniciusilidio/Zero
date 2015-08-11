using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GerJogo : MonoBehaviour
{
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
	static List<int> posicoesLivres = new List<int>();
	static List<int> posicoesOcupadas = new List<int>();

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

	void Awake()
	{
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

		for (int i = 0; i < qtdMaxima; i++)
		{
			grade[i] = gradePosicoes[i].localPosition;
			posicoesLivres.Add(i);
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

		//PlayerPrefs.DeleteAll();
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
		proximoSalvar = Time.time + tempoSalvar;
	}

	static bool adicionarNovoBlocoAleatorio = false;
	void Update()
	{
		VerificarCriarBlocos();

		VerificarSalvar();

		if (adicionarNovoBlocoAleatorio)
		{
			adicionarNovoBlocoAleatorio = false;
			AdicionarEmPosicaoAleatoria();
		}

	}

	void VerificarSalvar()
	{
		if (Time.time > proximoSalvar)
		{
			long [] valores = new long[1];
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
			
			Dados.tempoAtualDeJogo = Time.time;
			Dados.tempoTotalDeJogo += (ulong)Dados.tempoAtualDeJogo;
			
			Armazenador.SalvarDados();
			//Utilidade.DebugMensagem ("Dados salvos "+Time.time);
		}
	}

	void OnDestroy()
	{
		Dados.tempoAtualDeJogo = Time.time;
		Dados.tempoTotalDeJogo += (ulong)Dados.tempoAtualDeJogo;
		
		Armazenador.SalvarDados();
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
		if (posicoesLivres.Count == 0 ||
		    posicoesOcupadas.Count > qtdMaxima)
		{
			return -1;
		}
		
		int ind = Utilidade.AleatorioLista<int>(posicoesLivres);
		posicoesOcupadas.Add(ind);

		return ind;
	}

	static void LiberarPosicao(int p)
	{
		posicoesOcupadas.Remove(p);
		posicoesLivres.Add(p);
	}

	static bool PosicaoOcupada(int p)
	{
		return posicoesOcupadas.Contains(p);
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

	public static void AdicionarNaGrade(int tipo, int valor, int pos)
	{
		if (posicoesLivres.Count > 0)
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

			posicoesLivres.Remove(pos);
			posicoesOcupadas.Add(pos);
			
			objetos.Add(botao.GetComponent<GerBotao>());
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
			GameObject botao = (GameObject) Instantiate(
				botaoBaseEstatico, Vector3.zero, Quaternion.identity);
			
			//botao.transform.SetParent(transformEstatico, false);
			
			botao.GetComponent<GerBotao>()
				.Inicializar(transformEstatico, pos, 
				             painelRastrosEstatico);

			botao.GetComponent<GerBotao>().Mudar(valor);
			//botao.GetComponent<GerBotao>().posicaoGrade = pos;
			botao.transform.localPosition = grade[pos];
			
			objetos.Add(botao.GetComponent<GerBotao>());
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
			
			objetos.Add(botao.GetComponent<GerBotao>());
		}
		else
		{
			// Lista esta cheia!
			MensagemListaCheia();
		}
	}

	static void CriarBotao(int p)
	{
		GameObject botao = (GameObject) Instantiate(
			botaoBaseEstatico, Vector3.zero, Quaternion.identity);

		botao.GetComponent<GerBotao>()
			.Inicializar(transformEstatico, p, 
			             painelRastrosEstatico);

		//botao.GetComponent<GerBotao>().posicaoGrade = p;
		botao.transform.localPosition = grade[p];

		objetos.Add(botao.GetComponent<GerBotao>());
	}

	public static void SoltarObjeto(GerBotao ativo)
	{
		bool juntou = false;
		foreach(GerBotao obj in objetos)
		{
			if (obj != ativo)
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

		posicoesOcupadas.Add(p);
		posicoesLivres.Remove(p);
	}

	static void MostrarGrade()
	{
		string saida = "";
		int i = 0;
		for (int y = 0; y < 4; y++)
		{
			for (int x = 0; x < 4; x++)
			{
				string v = "x";
				if (posicoesLivres.Contains(i)) v = "_";
				saida += " " + v;
				i++;
			}
			saida += "\n";
		}

		saida += "Livres: ";
		foreach(int l in posicoesLivres)
		{
			saida += "" + l + ", ";
		}

		saida += "\nOcups: ";
		foreach(int o in posicoesOcupadas)
		{
			saida += "" + o + ", ";
		}

		Utilidade.DebugMensagem ("Grade: \n"+saida);

	}

	static int PegarPosicaoMaisProxima(Vector3 pos)
	{
		int retorno = -1;
		float d = float.MaxValue;

		foreach(int i in posicoesLivres)
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

		objetos.Remove(juntado);
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
			objetos.Remove(parado);
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

		Dados.quantidadeDeResets += resets;

		AjeitarPontosTotais();

		if (resets > 0)
		{
			ResetouJogo();
		}
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

	static void VerificarRealizacoes(
		Realizacao.Tipo tipo, long [] valores)
	{
		int quantidadeCompletas = 0;
		foreach(Realizacao re in Dados.realizacoes)
		{
			if (re.Verificar(tipo, valores))
			{
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

	static void VerificarMissao(int missao, long valor)
	{
		if (Dados.missoes[missao].Verificar((int)valor) > 0)
		{
			CompletouMissao(missao);
		}
	}

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
			gb.Destruir();
		}
		objetos.Clear();
		posicoesOcupadas.Clear();
		posicoesLivres.Clear();
		Dados.pontosAtuais = 0;
		Dados.pontosTotaisReais = 0;
		Dados.quantidadeDeResets = 0;
		Dados.tempoTotalDeJogo = 0;
		Dados.tempoAtualDeJogo = 0;

		for (int i = 0; i < qtdMaxima; i++)
		{
			posicoesLivres.Add(i);
		}

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

	public static void Limpar()
	{
		for (int i = 0; i < objetos.Count; i++)
		{
			GerBotao gb = objetos[i];
			//Utilidade.DebugMensagem ("Objeto: "+gb.posicaoGrade);
			gb.Destruir();
		}
		objetos.Clear();
		posicoesOcupadas.Clear();
		posicoesLivres.Clear();
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
}

