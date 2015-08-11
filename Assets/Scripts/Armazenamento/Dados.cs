using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dados
{
	// Armazenamento
	public static string nomeArquivo = "dados";
	public static string nomeVersaoSalvar = "versao";
	public static string versaoSalvar = "1.1";

	// Tempo
	public static ulong tempoTotalDeJogo = 0;
	public static float tempoAtualDeJogo = 0;

	// Pontuação
	public static ulong pontosTotaisReais = 0;
	public static long pontosParaReset = 1000000000;
	public static ulong quantidadeDeResets = 0;
	public static ulong maximoResetsMostrar = 21;
	public static string caractereReset = ".";
	public static long pontosAtuais = 0;

	// Blocos
	public static bool blocosCriarPorTempo = true;
	public static string blocoTextoMaximoValor = "1 bi";
	public static float chanceCriarNovoBloco = 0.1f;
	public static float tempoCriarBlocos = 5;
	public static long blocoMaiorValor = 1000000000;

	// Chance de vir um objeto multiplicador, entre zero e um.
	// Valor padrão está em 5% (0.05f)
	public static float chanceMultiplicador = 0.05f;

	// Valores possíveis dos pontos, soma e subtração
	public static int valorBotaoNormalMin = 1;
	public static int valorBotaoNormalMax = 1;

	// Valores possíveis dos pontos, multiplicação
	public static int valorMultInicialMaximo = 16;
	public static int valorBotaoMultipMin = 2;
	public static int valorBotaoMultipMax = 2;

	// Missões
	public static string arquivoMissoes = "missoes";
	public static string textoMissaoCompleta = "Missão completa.";
	public static List<Missao> missoes = new List<Missao>();

	// Realizações
	public static string arquivoRealizacoes = "realizacoes";
	public static List<Realizacao> realizacoes = new List<Realizacao>();

	// Mensagens
	public static string arquivoMensagens = "mensagens";
	public static string [] textosMensagens = null;

	// Som e música
	public static bool somLigado = true;
	public static bool musicaLigado = true;

	// Debug
	public static long totalDebug = 0;
	public static string debugMensagens =
		"<color=#ffff00>Debug:</color>";
}
