using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour
{
	public GerJogo 		gerJogo;

	public GameObject	interfaceJogo;
	public GameObject	painelContador;
	public Animator		animPainelContador;
	public GameObject	btRealizacoes;
	public GameObject	btMissoes;
	public GameObject	btConfig;
	public GameObject	btDebug;

	public GameObject 	teo;
	public Image		painelFundo;
	public Text			texto;

	public Transform	localTeo;

	public GameObject	rastro;

	public float		tempoAparecer			= 1;
	public float		tempoAteFalar			= 1;
	public float		tempoFalarPrimeiraVez	= 1;
	public float 		tempoReduzir			= 0.66f;
	public float 		tempoRastro				= 2;

	public Image		teo2;
	public Text			texto2;

	public string []	mensagens;

	Animator teoAnimator;
	Animator textoAnimator;

	float 	proximoTempo = 0;
	float 	tempoClicar = 0;

	int passoAtual = 0;

	bool reduzindo = false;
	bool aparecendo = false;
	bool podeClicar = false;
	bool esperandoJuntar = false;

	public static bool podeInteragirBlocos = false;
	public static bool podeMostrarConquista = false;

	bool pronto = false;

	public void Comecar ()
	{
		if (Dados.tutorialCompleto)
		{
			DestroyImmediate(gameObject);
		}

		textoAnimator = texto.GetComponent<Animator>();
		teoAnimator = teo.GetComponent<Animator>();

		painelFundo.enabled = true;
		teo.SetActive(false);
		texto.enabled = false;
		//texto.gameObject.SetActive (false);

		//interfaceJogo.SetActive (false);
		painelContador.SetActive (false);
		btRealizacoes.SetActive (false);
		btMissoes.SetActive (false);
		btConfig.SetActive (false);
		btDebug.SetActive (false);

		texto2.text = "";

		podeClicar = true;

		proximoTempo = tempoAparecer + Time.time;
		tempoClicar = Time.time;

		pronto = true;
	}
	
	void Update ()
	{
		if (Dados.tutorialCompleto || pronto == false) return;

		if (aparecendo) {
			if (Time.time > proximoTempo) {
				aparecendo = false;
				ProximoPasso ();
				podeClicar = true;
			}
		} else if (reduzindo) {
			Vector2 vel = localTeo.position - teo.transform.position;
			teo.transform.Translate (
				vel * Time.deltaTime * 3 / tempoReduzir);
			if (Time.time > proximoTempo) {
				podeClicar = false;
				reduzindo = false;
				teo.SetActive (false);
				painelFundo.enabled = false;
				teoAnimator = teo2.GetComponent<Animator> ();
				texto.enabled = false;
				//texto.gameObject.SetActive (false);
				//texto = texto2;
				textoAnimator = texto2.GetComponent<Animator> ();
				ProximoPasso ();
			}
		} else if (passoAtual == 0 || passoAtual == 2) {
			if (Time.time > proximoTempo ||
				(Clique () && Time.time > tempoClicar)) {
				ProximoPasso ();
			}
		} 
		else if (passoAtual >= 3 && passoAtual <= 6)
		{
			if (Time.time > proximoTempo) 
			{
				ProximoPasso();
			}
		}
		else if (esperandoJuntar && passoAtual < 10)
		{
			if (GerJogo.PosicoesOcupadas() == 0)
			{
				esperandoJuntar = false;
				podeInteragirBlocos = false;
				PararDeFalar();
				texto2.text = "";
				ProximoPasso();
			}
			else if (Time.time > proximoTempo)
			{
				CriarRastro();
				proximoTempo = tempoRastro + Time.time;
			}
		}
		else if (passoAtual >= 7 && passoAtual <= 12)
		{
			if (Time.time > proximoTempo)
			{
				ProximoPasso();
			}
		}
		else if (esperandoJuntar && passoAtual <= 17)
		{
			if (passoAtual == 15)
			{
				if (Time.time > proximoTempo)
				{
					ProximoPasso();
				}
			}
			else if (GerJogo.PosicoesOcupadas() > 0)
			{
				if (GerJogo.PosicoesOcupadas() == 1)
				{
					esperandoJuntar = false;
					podeInteragirBlocos = false;
					PararDeFalar();
					texto2.text = "";
					ProximoPasso();
				}
				else if (Time.time > proximoTempo)
				{
					CriarRastro();
					proximoTempo = tempoRastro + Time.time;
				}
			}
		}
		else if (esperandoJuntar && passoAtual == 18)
		{
			if (GerJogo.PosicoesOcupadas() == 0)
			{
				esperandoJuntar = false;
				podeInteragirBlocos = false;
				PararDeFalar();
				texto2.text = "";
				ProximoPasso();
			}
			else if (Time.time > proximoTempo)
			{
				CriarRastro();
				proximoTempo = tempoRastro + Time.time;
			}
		}
		else if (passoAtual <= 23)
		{
			if (Time.time > proximoTempo)
			{
				ProximoPasso();
			}
		}
	}

	bool Clique()
	{
		return podeClicar && Input.GetMouseButtonUp(0);
	}

	void ProximoPasso()
	{
		switch(passoAtual)
		{
		case 0: Aparecer(); 				break;
		case 1: FalarPrimeiraVez ();		break;
		case 2: Reduzir(); 					break;
		case 3: CriarBloco();				break;
		case 4: AparecerBlocoPositivo();	break;
		case 5: AparecerBlocoNegativo();	break;
		case 6: FalarJuntar();				break;
		case 7: MostrarPontos();			break;
		case 8: Pontuar();					break;
		case 9: Conquista();				break;
		case 10:MostrarConquista();			break;
		case 11:EsconderConquista();		break;
		case 12:BlocosIguais();				break;
		case 13:Multiplicador();			break;
		case 14:Esperar();					break;
		case 15:DoisSub();					break;
		case 16:MultiSub();					break;
		case 17:BotaoQuatro();				break;
		case 18:CompletarMissao();			break;
		case 19:MostrarMissao();			break;
		case 20:MostrarBotoes();			break;
		case 21:FalaFinal();				break;
		case 22:ComecarJogo();				break;
		}
		passoAtual++;
	}

	void Aparecer()
	{
		GerJogo.GuardarCenario();
		podeClicar = false;
		teo.SetActive(true);
		texto.text = texto.text.ToUpper();
		teoAnimator.Play("Aparecer");
		proximoTempo = Time.time + tempoAteFalar;
		aparecendo = true;
	}

	void FalarPrimeiraVez()
	{
		texto.enabled = true;
		//texto.gameObject.SetActive (true);
		//texto.text = mensagens[0];
		textoAnimator.Play("Aparecer");
		Falar ();
		proximoTempo = Time.time + tempoFalarPrimeiraVez;
		tempoClicar = Time.time + 1;
	}

	void Reduzir()
	{
		textoAnimator.Play ("Reduzir");
		reduzindo = true;
		//texto.enabled = false;
		proximoTempo = Time.time + tempoReduzir;
		teoAnimator.Play("Reduzir");
	}

	void CriarBloco()
	{
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [1].ToUpper();
		Falar();
		GerJogo.AdicionarNaGrade(1,1,0);
		proximoTempo = Time.time + 1.5f;
	}

	void AparecerBlocoPositivo()
	{
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [2].ToUpper();
		Falar();
		proximoTempo = Time.time + 3;
	}

	void AparecerBlocoNegativo()
	{
		GerJogo.AdicionarNaGrade(2,-1,15);
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [3].ToUpper();
		Falar();
		proximoTempo = Time.time + 3;
	}

	void FalarJuntar()
	{
		esperandoJuntar = true;
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [4].ToUpper();
		Falar();
		podeInteragirBlocos = true;
		proximoTempo = tempoRastro + Time.time;
	}

	void MostrarPontos()
	{
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [5].ToUpper();
		Falar();
		painelContador.SetActive(true);
		proximoTempo = Time.time + 1;
	}

	void Pontuar()
	{
		animPainelContador.Play("Aparecer");
		Dados.pontosAtuais++;
		proximoTempo = Time.time + 2;
	}

	void Conquista()
	{
		painelContador.SetActive(false);
		//textoAnimator.Play ("Aparecer");
		texto2.text = "";
		//texto.gameObject.SetActive (true);
		texto.enabled = true;
		texto.text = mensagens [6].ToUpper();
		texto.GetComponent<Animator>().Play("Reduzir");
		texto.GetComponent<Animator>().Play("Aparecer");
		Falar();
		proximoTempo = Time.time + 1;
	}

	void MostrarConquista()
	{
		PararDeFalar();
		//btRealizacoes.SetActive(true);
		//btRealizacoes.GetComponent<Animator>().Play("Aparecer");

		podeMostrarConquista = true;
		long [] valores = new long[1];
		valores[0] = Dados.pontosAtuais;
		foreach(Realizacao re in Dados.realizacoes)
		{
			if (re.indice == 1)
			{
				re.Completou();
				GerMensagens.AdicionarMensagem(
					re.titulo, MensagensImagens.realizacao);
				break;
			}
		}
		podeMostrarConquista = false;

		proximoTempo = Time.time + 3;
	}

	void BlocosIguais()
	{
		esperandoJuntar = true;
		podeInteragirBlocos = true;
		//btRealizacoes.SetActive(false);
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [7].ToUpper();
		Falar();
		GerJogo.AdicionarNaGrade(1,1,15);
		GerJogo.AdicionarNaGrade(1,1,0);
		proximoTempo = tempoRastro + Time.time;
	}

	void Multiplicador()
	{
		esperandoJuntar = true;
		podeInteragirBlocos = true;
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [8].ToUpper();
		Falar();
		gerJogo.AdicionarBotaoMulti();
		proximoTempo = tempoRastro + Time.time;
	}

	void Esperar()
	{
		PararDeFalar();
		texto2.text = "";
		proximoTempo = Time.time + 1.25f;
	}

	void DoisSub()
	{
		GerJogo.Limpar();
		esperandoJuntar = true;
		podeInteragirBlocos = true;
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [9].ToUpper();
		Falar();
		GerJogo.AdicionarNaGrade(2,-1,3);
		GerJogo.AdicionarNaGrade(2,-1,12);
		proximoTempo = tempoRastro + Time.time;
	}

	void MultiSub()
	{
		esperandoJuntar = true;
		podeInteragirBlocos = true;
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [10].ToUpper();
		Falar();
		gerJogo.AdicionarBotaoMulti();
		proximoTempo = tempoRastro + Time.time;
	}

	void BotaoQuatro()
	{
		painelContador.SetActive(true);
		esperandoJuntar = true;
		podeInteragirBlocos = true;
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [11].ToUpper();
		Falar();
		gerJogo.AdicionarBotao(+4);
		proximoTempo = tempoRastro + Time.time;
	}

	void CompletarMissao()
	{
		Dados.pontosAtuais += 4;
		//textoAnimator.Play ("Aparecer");
		texto2.text = "";
		//texto.gameObject.SetActive (true);
		texto.enabled = true;
		texto.text = mensagens [12].ToUpper();
		//texto.GetComponent<Animator>().Play("Reduzir");
		texto.GetComponent<Animator>().Play("Aparecer");
		Falar();
		proximoTempo = Time.time + 1;
	}

	void MostrarMissao()
	{
		painelContador.SetActive(false);
		PararDeFalar();
		podeMostrarConquista = true;

		Dados.missoes[0].Verificar(4);
		GerMensagens.AdicionarMensagem(
			Dados.missoes[0].Mensagem());

		podeMostrarConquista = false;
		
		proximoTempo = Time.time + 3;
	}

	void MostrarBotoes()
	{
		btMissoes.SetActive(true);
		btRealizacoes.SetActive(true);
		btMissoes.GetComponent<Animator>().Play("Aparecer");
		btRealizacoes.GetComponent<Animator>().Play("Aparecer");
		texto.text = "";
		texto.enabled = false;
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [13].ToUpper();
		Falar();
		proximoTempo = Time.time + 3;
	}

	void FalaFinal()
	{
		textoAnimator.Play ("Aparecer");
		texto2.text = mensagens [14].ToUpper();
		Falar();
		proximoTempo = Time.time + 2;
	}

	void ComecarJogo()
	{
		btConfig.SetActive(true);
		btConfig.GetComponent<Animator>().Play("Aparecer");
		painelContador.SetActive(true);
		GerJogo.RecuperarCenario();
		Destruir();
	}

	void EsconderConquista()
	{
		texto.text = "";
		texto.enabled = false;
	}

	void Falar()
	{
		teoAnimator.SetBool("rodando",true);
	}

	void PararDeFalar()
	{
		teoAnimator.SetBool("rodando",false);
	}

	void Destruir()
	{
		PararDeFalar();
		Dados.tutorialCompleto = true;
		gameObject.SetActive(false);
		//Destroy(gameObject);
	}


	Transform pini = null;
	Transform pfim = null;
	void AjeitarPs()
	{
		int bt = 0;
		foreach(GerBotao b in GerJogo.objetos)
		{
			if (b != null)
			{
				if (bt == 0)
				{
					bt = 1;
					pini = b.transform;
				}
				else
				{
					pfim = b.transform;
					break;
				}
			}
		}
	}

	void CriarRastro()
	{
		AjeitarPs();

		if (pini == null || pfim == null)
		{
			return;
		}

		GameObject ras = Instantiate<GameObject>(rastro);
		ras.GetComponent<GerRastro>().
			CriarRastroTutorial(pini, pfim, 2);
	}

	public void Saltar()
	{
		btMissoes.SetActive(true);
		btRealizacoes.SetActive(true);
		btMissoes.GetComponent<Animator>().Play("Aparecer");
		btRealizacoes.GetComponent<Animator>().Play("Aparecer");
		btConfig.SetActive(true);
		btConfig.GetComponent<Animator>().Play("Aparecer");
		painelContador.SetActive(true);
		texto.text = "";
		texto.enabled = false;
		texto2.text = "";
		texto2.enabled = false;

		Dados.tutorialCompleto = true;
		GerJogo.RecuperarCenario();
	}
}

