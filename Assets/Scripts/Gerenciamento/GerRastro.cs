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

		if (pai != null &&
		    Vector3.Distance(pai.position, transform.position) > 
		    distanciaMinimaSeguir)
		{
			Vector3 dif = pai.position - transform.position;
			transform.Translate(dif * Time.deltaTime * velocidade);
		}
	}

	public void Destruir()
	{
		destruir = true;
	}
}

