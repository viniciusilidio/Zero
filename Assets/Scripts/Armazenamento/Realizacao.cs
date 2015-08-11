using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Realizacao
{
	// Tipo
	// p, f, b, n, v, m, r, z, e, a, t, i, c
	// placar X, fundir +X -X, bloco X, naomultiplicar X,
	// varios X Y Z..., maiorque X, resposta X, zerar X
	// exatoplacar X, absolutobloco X, tempo X (em segundos)
	// indiferentefundir X Y Resultado, completar 1

	public enum Tipo {
		Fundir = 0, Placar = 1, BlocoNormal = 2, BlocoMaiorQue = 3,
		BlocoSemMultiplicar = 4, VariosBlocos = 5, Resposta = 6,
		Zerar = 7, PlacarExato = 8, BlocoAbsoluto = 9, TempoTotal = 10,
		FundirIndiferente = 11, CompletarMissoes = 12
	}

	public int indice = 0;
	public string titulo = "Missão";
	public string descricao = "Descrição";
	public Tipo tipo = Tipo.Fundir;
	public long [] objetivos;
	public int pontos = 0;


	public System.DateTime 	dataCompleto	= System.DateTime.Now;
	public bool				completa		= false;

	public bool Verificar(Tipo tipo, long [] valores)
	{
		if (completa)
		{
			return false;
		}

		if (this.tipo == tipo)
		{
			switch(tipo){
			case Tipo.Fundir:
				return VerificarFundir(valores);
			case Tipo.BlocoSemMultiplicar:
				return VerificarBlocoSemMultiplicar(valores);
			case Tipo.BlocoMaiorQue:
				return VerificarBlocoMaiorQue(valores);
			case Tipo.BlocoNormal:
				return VerificarBlocoNormal(valores);
			case Tipo.VariosBlocos:
				return VerificarVariosBlocos(valores);
			case Tipo.Resposta:
				return VerificarResposta(valores);
			case Tipo.Zerar:
				return VerificarZerar(valores);
			case Tipo.PlacarExato:
				return VerificarPlacarExato(valores);
			case Tipo.BlocoAbsoluto:
				return VerificarBlocoAbsoluto(valores);
			case Tipo.TempoTotal:
				return VerificarTempoTotal(valores);
			case Tipo.FundirIndiferente:
				return VerificarFundirIndiferente(valores);
			case Tipo.CompletarMissoes: 
				return VerificarMissoes(valores);
			default:
				return VerificarPlacar(valores);
			}
		}

		return false;
	}

	bool VerificarMissoes(long [] valores)
	{
		if (valores.Length >= 1 && valores[0] == objetivos[0])
		{
			Completou();
		}
		
		return completa;
	}

	bool VerificarTempoTotal(long [] valores)
	{
		if (valores.Length >= 1 && valores[0] >= objetivos[0])
		{
			Completou();
		}
		
		return completa;
	}

	bool VerificarZerar(long [] valores)
	{
		if (valores.Length >= 1 && valores[0] >= objetivos[0])
		{
			Completou();
		}
		
		return completa;
	}

	bool VerificarResposta(long [] valores)
	{
		if (valores.Length == 1 && valores[0] == 42)
		{
			Completou();
		}
		
		return completa;
	}

	bool VerificarVariosBlocos(long [] valores)
	{
		if (valores.Length < objetivos.Length)
		{
			return false;
		}

		List<long> listaValores = new List<long>();
		for(int i = 0; i < valores.Length; i++){
			long valor = valores[i];
			if (valor < 0)
			{
				valor = -valor;
			}
			listaValores.Add(valor);
		}
		List<long> listaObjetivos = new List<long>();
		for(int i = 0; i < objetivos.Length; i++){
			listaObjetivos.Add(objetivos[i]);
		}

		while(listaObjetivos.Count > 0)
		{
			long objetivo = listaObjetivos[0];
			listaObjetivos.RemoveAt(0);
			if (listaValores.Remove(objetivo) == false)
			{
				return false;
			}
		}

		Completou();
		
		return completa;
	}

	bool VerificarBlocoSemMultiplicar(long [] valores)
	{
		if (valores.Length >= 1)
		{
			long valor = valores[0];
			if (valor < 0)
			{
				valor = -valor;
			}

			if (valor == objetivos[0])
			{
				Completou();
			}
		}
		
		return completa;
	}

	bool VerificarBlocoMaiorQue(long [] valores)
	{
		if (valores.Length >= 1)
		{
			if ((valores[0] > 0 && valores[0] > objetivos[0]) ||
			    (valores[0] < 0 && valores[0] < objetivos[0]))
			{
				Completou();
			}
		}
		
		return completa;
	}


	bool VerificarBlocoAbsoluto(long [] valores)
	{
		if (valores.Length >= 1)
		{
			long valor = valores[0];
			if (valor < 0)
			{
				valor = -valor;
			}
			long objetivo = objetivos[0];
			if (objetivo < 0)
			{
				objetivo = -objetivo;
			}

			if (valor == objetivo)
			{
				Completou();
			}
		}
		
		return completa;
	}

	bool VerificarBlocoNormal(long [] valores)
	{
		if (valores.Length >= 1 && valores[0] == objetivos[0])
		{
			Completou();
		}
		
		return completa;
	}

	bool VerificarPlacar(long [] valores)
	{
		if (valores.Length >= 1 && valores[0] >= objetivos[0])
		{
			Completou();
		}
		
		return completa;
	}

	bool VerificarPlacarExato(long [] valores)
	{
		if (valores.Length >= 1 && valores[0] == objetivos[0])
		{
			Completou();
		}
		
		return completa;
	}

	bool VerificarFundir(long [] valores)
	{
		if (valores.Length >= 2)
		{
			if (((valores[0] == objetivos[0] && 
			      valores[1] == objetivos[1])   ||
			     (valores[1] == objetivos[0] &&
			 	  valores[0] == objetivos[1])))
			{
				Completou();
			}
		}

		return completa;
	}

	bool VerificarFundirIndiferente(long [] valores)
	{
		if (valores.Length >= 2)
		{
			long v1Abs = valores[0];
			long v2Abs = valores[1];
			if (v1Abs < 0)
			{
				v1Abs = -v1Abs;
			}
			if (v2Abs < 0)
			{
				v2Abs = -v2Abs;
			}

			if ((v1Abs == objetivos[0] && v2Abs == objetivos[1]) ||
			    (v1Abs == objetivos[1] && v2Abs == objetivos[0]))
			{
				long resAbs = valores[0] + valores[1];
				if (resAbs < 0)
				{
					resAbs = -resAbs;
				}
				if (resAbs == objetivos[2])
				{
					Completou();
				}
			}
		}
		
		return completa;
	}

	void Completou()
	{
		dataCompleto = System.DateTime.Now;
		completa = true;
	}

	public Realizacao(
		int indice, string titulo, string descricao,
		string tipo, long [] objetivos, int pontos)
	{
		this.indice = indice;
		this.titulo = titulo;
		this.descricao = descricao;
		this.tipo = PegarTipo(tipo);
		this.objetivos = objetivos;
		this.pontos = pontos;

		//Imprimir();
	}

	Tipo PegarTipo(string t)
	{
		switch(t.ToLower()[0]){
		case 'f': return Tipo.Fundir;
		case 'b': return Tipo.BlocoNormal;
		case 'n': return Tipo.BlocoSemMultiplicar;
		case 'm': return Tipo.BlocoMaiorQue;
		case 'v': return Tipo.VariosBlocos;
		case 'r': return Tipo.Resposta;
		case 'z': return Tipo.Zerar;
		case 'e': return Tipo.PlacarExato;
		case 'a': return Tipo.BlocoAbsoluto;
		case 'i': return Tipo.FundirIndiferente;
		case 't': return Tipo.TempoTotal;
		case 'c': return Tipo.CompletarMissoes;
		default:  return Tipo.Placar;
		}
	}

	public void Imprimir()
	{
		string mens = 
			"Realização "+indice+
			": "+titulo+
			"; Descrição: "+descricao+
			"; Tipo: "+tipo+
			"; Objetivos:";

		foreach(int obj in objetivos)
		{
			mens += " " + obj;
		}

		mens += "; Completa: "+completa;

		Utilidade.DebugMensagem(mens);
	}
}

