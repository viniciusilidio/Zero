using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GerRastro : MonoBehaviour
{
	public float distanciaMinimaSeguir = 0.01f;
	public float velocidade = 4;

	Transform pai = null;
	Image imagem;
	bool destruir = false;

	static Transform painelRastros = null;

	//Transform ini;
	Transform fim;

	bool tutorial = false;

	public void CriarRastroTutorial(
		Transform ini, Transform fim, float vel = 9)
	{
		if (painelRastros == null)
		{
			painelRastros = GameObject.Find ("PainelRastros").transform;
		}
		transform.SetParent(painelRastros, false);
		transform.position = ini.position;
		//this.ini = ini;
		this.fim = fim;
		velocidade = vel;

		tutorial = true;
	}

	public void Inicializar(Transform p, Color cor, Transform avo)
	{
		pai = p;
		transform.SetParent(avo, false);
		transform.position = pai.position;
		imagem = GetComponent<Image>();
		imagem.color = new Color(cor.r, cor.g, cor.b, imagem.color.a);
	}

	public void MudarCor(Color cor)
	{
		imagem.color = new Color(cor.r, cor.g, cor.b, imagem.color.a);
	}

	void Update()
	{
		if (destruir)
		{
			Destroy(gameObject);
			return;
		}

		if (tutorial)
		{
			Tutorial();
		}
		else if (pai != null &&
		    Vector3.Distance(pai.position, transform.position) > 
		    distanciaMinimaSeguir)
		{
			Vector3 dif = pai.position - transform.position;
			transform.Translate(dif * Time.deltaTime * velocidade);
		}
	}

	void Tutorial()
	{
		if (fim == null)
		{
			Destruir();
		}
		else
		{
			Vector3 dif = fim.position - transform.position;
			transform.Translate(dif * Time.deltaTime * velocidade);

			if (Vector3.Distance(fim.position, 
			    	transform.position) <= 0.15f)
			{
				Destruir();
			}
		}
	}

	public void Destruir()
	{
		destruir = true;
	}
}

