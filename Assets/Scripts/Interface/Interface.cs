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
	public GameObject painelCreditos;

	public GameObject painelConfig_en;
	public GameObject painelMissoes_en;
	public GameObject painelRealizacoes_en;
	public GameObject painelCreditos_en;

	static bool podeTocar = false;

	void Awake()
	{
		podeTocar = false;
		FecharConfig();
		FecharMissoes();
		FecharDebug();
		FecharRealizacoes();
		FecharPlacar();
		FecharCreditos();
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


		GerJogo.Salvar();
		//GerJogo.Limpar();

		Application.LoadLevel(Constantes.telaMenu);
	}

	//
	public void AbrirRealizacoes()
	{
		if (Dados.linguaAtual == 1)
		{
			if (painelRealizacoes)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelRealizacoes.SetActive(true);
				MostrarRealizacoes.VerificarCor();
			}
		}
		else
		{
			if (painelRealizacoes)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelRealizacoes.SetActive(true);
				MostrarRealizacoes.VerificarCor();
			}
			/*
			if (painelRealizacoes_en)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelRealizacoes_en.SetActive(true);
				MostrarRealizacoes.VerificarCor();
			}
			//*/
		}
	}
	
	public void FecharRealizacoes()
	{
		if (Dados.linguaAtual == 1)
		{
			if (painelRealizacoes)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelRealizacoes.SetActive(false);
			}
		}
		else
		{
			if (painelRealizacoes)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelRealizacoes.SetActive(false);
			}
			/*
			if (painelRealizacoes_en)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelRealizacoes_en.SetActive(false);
			}
			//*/
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
		if (Dados.linguaAtual == 1)
		{
			if (painelConfig)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelConfig.SetActive(true);
			}
		}
		else
		{
			if (painelConfig_en)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelConfig_en.SetActive(true);
			}
		}
	}

	public void FecharConfig()
	{
		if (Dados.linguaAtual == 1)
		{
			if (painelConfig)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelConfig.SetActive(false);
			}
		}
		else
		{
			if (painelConfig_en)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelConfig_en.SetActive(false);
			}
		}
	}

	//
	public void AbrirMissoes()
	{
		if (Dados.linguaAtual == 1)
		{
			if (painelMissoes)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelMissoes.SetActive(true);
			}
		}
		else
		{
			if (painelMissoes_en)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelMissoes_en.SetActive(true);
			}
		}
	}

	public void FecharMissoes()
	{
		if (Dados.linguaAtual == 1)
		{
			if (painelMissoes)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelMissoes.SetActive(false);
			}
		}
		else
		{
			if (painelMissoes_en)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelMissoes_en.SetActive(false);
			}
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

	public void AbrirCreditos()
	{
		if (Dados.linguaAtual == 1)
		{
			if (painelCreditos)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelCreditos.SetActive(true);
			}
		}
		else
		{
			if (painelCreditos_en)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelCreditos_en.SetActive(true);
			}
		}
	}
	
	public void FecharCreditos()
	{
		if (Dados.linguaAtual == 1)
		{
			if (painelCreditos)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelCreditos.SetActive(false);
			}
		}
		else
		{
			if (painelCreditos_en)
			{
				if (podeTocar)
					EfeitosSonoros.Tocar();
				painelCreditos_en.SetActive(false);
			}
		}
	}
}

