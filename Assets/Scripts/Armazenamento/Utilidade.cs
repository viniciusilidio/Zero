using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utilidade
{
	/// <summary>
	/// Gera um número aleatório entre zero e um,
	/// com 50% de chance de retornar verdadeiro
	/// </summary>
	public static bool MeiaChance()
	{
		return Random.value < 0.5f;
	}

	public static void DebugMensagem(string mensagem, int t = 0)
	{
		Dados.totalDebug++;
		mensagem = "\n" + "<color=cyan>"+
			Dados.totalDebug + "</color>" + ". " + mensagem+
				" || "+System.DateTime.Now;
		Dados.debugMensagens += mensagem;

		if (t == 0)
			Debug.Log(mensagem);
		else if (t == 1)
			Debug.LogWarning(mensagem);
		else
			Debug.LogError(mensagem);
	}

	public static void DebugAviso(string mensagem)
	{
		DebugMensagem("<color=#ffff00ff>"+mensagem+"</color>", 1);
	}

	public static void DebugErro(string mensagem)
	{
		DebugMensagem("<color=#c05436ff>"+mensagem+"</color>", 2);
	}

	/// <summary>
	/// Pega um item aleatório de uma lista, e remove este item
	/// da lista. Usado para tirar vários itens aleatórios de 
	/// uma mesma lista, sem repetir nenhum.
	/// </summary>
	/// <returns>Item aleatório da lista, ou null</returns>
	/// <param name="lista">A lista</param>
	/// <typeparam name="T">Tipo genérico</typeparam>
	public static T AleatorioLista<T>(List<T> lista)
	{
		if (lista.Count == 0)
			return default(T);

		int pos = Random.Range(0, lista.Count);
		T retorno = lista[pos];
		lista.RemoveAt(pos);
		return retorno;
	}

	/// <summary>
	/// Gera um tipo de botão aleatório, baseado na chance
	/// do multiplicador. Se não for multiplicador, tem 50%
	/// de chance de voltar positivo ou negativo.
	/// </summary>
	/// <returns>The botao.</returns>
	public static Tipos.Botao GerarTipoBotao()
	{
		Tipos.Botao tipo = Tipos.Botao.Multiplicador;

		if (Random.value < 1 - Dados.chanceMultiplicador)
		{
			if (MeiaChance())
			{
				tipo = Tipos.Botao.Positivo;
			}
			else
			{
				tipo = Tipos.Botao.Negativo;
			}
		}

		return tipo;
	}

	/// <summary>
	/// Retorna uma cor baseada no tipo do botão.
	/// </summary>
	/// <returns>Cor relativa ao tipo</returns>
	/// <param name="tipo">Tipo do botão</param>
	public static Color CorPorTipo(Tipos.Botao tipo)
	{
		switch(tipo)
		{
		case Tipos.Botao.Positivo:
			return Constantes.corPositivo;
		case Tipos.Botao.Negativo:
			return Constantes.corNegativo;
		case Tipos.Botao.Multiplicador:
			return Constantes.corMultiplicador;
		}

		return Constantes.corZero;
	}

	/// <summary>
	/// Gera um valor aleatório, baseado no tipo.
	/// </summary>
	/// <returns>Valor em pontos</returns>
	/// <param name="tipo">Tipo</param>
	public static int PegarValor(Tipos.Botao tipo)
	{
		switch(tipo)
		{
		case Tipos.Botao.Positivo: 		
			return ValorSomaSubtracao();
		case Tipos.Botao.Negativo: 		
			return ValorSomaSubtracao() * -1;
		case Tipos.Botao.Multiplicador: 
			return ValorMultiplicador();
		}

		return 0;
	}
	static int ValorSomaSubtracao()
	{
		return Random.Range(
			(int) Dados.valorBotaoNormalMin,
			(int) Dados.valorBotaoNormalMax + 1);
	}
	static int ValorMultiplicador()
	{
		return Random.Range(
			(int) Dados.valorBotaoMultipMin,
			(int) Dados.valorBotaoMultipMax + 1);
	}
}

