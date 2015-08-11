using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Interface : MonoBehaviour
{
	public GameObject painelConfig;
	public GameObject painelMissoes;
	public GameObject painelDebug;
	public GameObject painelRealizacoes;
	public GameObject painelPlacar;

	static bool podeTocar = false;

	void Awake()
	{
		podeTocar = false;
		FecharConfig();
		FecharMissoes();
		FecharDebug();
		FecharRealizacoes();
		FecharPlacar();
		podeTocar = true;
	}

	//
	public void TelaJogo()
	{
		if (podeTocar)
			EfeitosSonoros.Tocar();
		Application.LoadLevel(Constantes.telaJogo);
	}

	public void TelaMenu()
	{
		if (podeTocar)
			EfeitosSonoros.Tocar();
		GerJogo.Limpar();
		Application.LoadLevel(Constantes.telaMenu);
	}

	//
	public void AbrirRealizacoes()
	{
		if (painelRealizacoes)
		{
			if (podeTocar)
				EfeitosSonoros.Tocar();
			painelRealizacoes.SetActive(true);
			MostrarRealizacoes.VerificarCor();
		}
	}
	
	public void FecharRealizacoes()
	{
		if (painelRealizacoes)
		{
			if (podeTocar)
				EfeitosSonoros.Tocar();
			painelRealizacoes.SetActive(false);
		}
	}

	//
	public void AbrirPlacar()
	{
		if (painelPlacar)
		{
			if (podeTocar)
				EfeitosSonoros.Tocar();
			painelPlacar.SetActive(true);
		}
	}
	
	public void FecharPlacar()
	{
		if (painelPlacar)
		{
			EfeitosSonoros.Tocar();
			painelPlacar.SetActive(false);
		}
	}

	//
	public void AbrirConfig()
	{
		if (painelConfig)
		{
			if (podeTocar)
				EfeitosSonoros.Tocar();
			painelConfig.SetActive(true);
		}
	}

	public void FecharConfig()
	{
		if (painelConfig)
		{
			if (podeTocar)
				EfeitosSonoros.Tocar();
			painelConfig.SetActive(false);
		}
	}

	//
	public void AbrirMissoes()
	{
		if (painelMissoes)
		{
			if (podeTocar)
				EfeitosSonoros.Tocar();
			painelMissoes.SetActive(true);
		}
	}

	public void FecharMissoes()
	{
		if (painelMissoes)
		{
			if (podeTocar)
				EfeitosSonoros.Tocar();
			painelMissoes.SetActive(false);
		}
	}

	//
	public void AbrirDebug()
	{
		if (painelDebug)
		{
			if (podeTocar)
				EfeitosSonoros.Tocar();
			painelDebug.SetActive(true);
		}
	}

	public void FecharDebug()
	{
		if (painelDebug)
		{
			if (podeTocar)
				EfeitosSonoros.Tocar();
			painelDebug.SetActive(false);
		}
	}
}

