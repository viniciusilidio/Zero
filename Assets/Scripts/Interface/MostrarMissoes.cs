using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MostrarMissoes : MonoBehaviour
{
	public Text [] titulos;
	public Text [] descricoes;
	public Text [] valor;

	void OnEnable()
	{
		if (Dados.missoes.Count == 0)
		{
			Dados.missoes = GerArquivo.CarregarMissoes();
		}

		for (int i = 0; i < titulos.Length; i++)
		{
			titulos[i].text 	= Dados.missoes[i].Titulo();
			descricoes[i].text	= Dados.missoes[i].Descricao();
			valor[i].text		= 
				Dados.missoes[i].StringValorAtual();
		}
	}
}

