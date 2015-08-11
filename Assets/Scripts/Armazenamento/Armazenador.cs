using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Armazenador
{
	static string divisor = "|";

	/* Estrutura do arquivo
	 * tempoTotalDeJogo|pontos|quantidadeDeObjetosNoCenario|
	 * valorBloco0|tipoBloco0|posicaoGrade0|
	 * valorBloco1|tipoBloco1|posicaoGrade1...<qtdvariavel>...|
	 * quantidadeDeMissoesCompletas|indiceMissao0|dataMissao0|
	 * indiceMissao1|DataMissao11...<qtdvariavel>...
	 */
	public static void SalvarDados()
	{
		string dados = CriarStringSalvar();
		//Utilidade.DebugMensagem ("Dados: "+dados);
		PlayerPrefs.SetString(Dados.nomeArquivo, dados);
	}

	public static void CarregarDados()
	{
		string carregar = CarregarStringDeArquivo();
		if (string.IsNullOrEmpty(carregar))
		{
			return;
		}

		//Utilidade.DebugMensagem ("Dados Carregados: "+carregar);

		string [] dados = carregar.Split(divisor[0]);

		int objts = 0;
		int indiceAtual = 0;

		// Tempo e pontos
		Dados.tempoTotalDeJogo		= ulong.Parse(dados[indiceAtual]);
		Dados.pontosAtuais			= int.Parse(dados[indiceAtual + 1]);
		Dados.quantidadeDeResets	= ulong.Parse(dados[indiceAtual + 2]);
		Dados.pontosTotaisReais 	= ulong.Parse(dados[indiceAtual + 3]);
		indiceAtual += 4;

		// Objetos no cenário
		objts = int.Parse(dados[indiceAtual]) * 3;
		objts += indiceAtual;
		indiceAtual++;
		
		while (indiceAtual < objts)
		{
			int valor = int.Parse(dados[indiceAtual]);
			int tipo = int.Parse(dados[indiceAtual + 1]);
			int pos = int.Parse(dados[indiceAtual + 2]);
			
			GerJogo.AdicionarNaGrade(tipo, valor, pos);
			indiceAtual += 3;
		}

		// Missões
		objts = int.Parse(dados[indiceAtual]) * 2;
		objts += indiceAtual;
		indiceAtual++;

		while (indiceAtual < objts)
		{
			int indice = int.Parse(dados[indiceAtual]);
			int nivel = int.Parse(dados[indiceAtual + 1]);

			foreach(Missao m in Dados.missoes)
			{
				if ((int)m.tipo == indice)
				{
					m.Nivel(nivel);
					m.AjeitarValores();
				}
			}
			indiceAtual += 2;
		}

		// Realizações
		objts = int.Parse(dados[indiceAtual]) * 3;
		objts += indiceAtual;
		indiceAtual++;
		
		while (indiceAtual < objts)
		{
			int indice = int.Parse(dados[indiceAtual]);
			bool completo = bool.Parse(dados[indiceAtual + 1]);
			long data = long.Parse(dados[indiceAtual + 2]);

			foreach(Realizacao r in Dados.realizacoes)
			{
				if (r.indice == indice)
				{
					r.completa = completo;
					r.dataCompleto = System.DateTime.FromFileTime(data);
					break;
				}
			}

			indiceAtual += 3;
		}
	}

	static string CarregarStringDeArquivo()
	{
		return PlayerPrefs.GetString(Dados.nomeArquivo);
	}

	static string CriarStringSalvar()
	{
		// Tempo de jogo
		string dados = "" + Dados.tempoTotalDeJogo;

		// Pontos
		dados += divisor + Dados.pontosAtuais;
		dados += divisor + Dados.quantidadeDeResets;
		dados += divisor + Dados.pontosTotaisReais;

		// Objetos no cenário
		dados += divisor + GerJogo.objetos.Count;
		foreach(GerBotao bloco in GerJogo.objetos)
		{
			dados += divisor + bloco.valor + divisor + 
				((int)bloco.tipo) + divisor + bloco.posicaoGrade;
		}

		// Missões
		dados += divisor + Dados.missoes.Count;
		foreach(Missao m in Dados.missoes)
		{
			dados += divisor + (int) m.tipo;
			dados += divisor + m.Nivel();
		}

		// Realizações
		dados += divisor + Dados.realizacoes.Count;
		foreach(Realizacao r in Dados.realizacoes)
		{
			dados += divisor + r.indice;
			dados += divisor + r.completa;
			dados += divisor + r.dataCompleto.ToFileTime();
		}

		return dados;
	}
}

