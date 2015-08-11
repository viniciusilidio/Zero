using UnityEngine;
using System.Collections;

public class EfeitosSonoros : MonoBehaviour
{
	static EfeitosSonoros _instancia = null;

	public enum Tipos {
		Somar = 0, Subtrair = 1, Multiplicar = 2, 
		Zerar = 3, Interface = 4
	}

	public AudioClip somSomar;
	public AudioClip somSubtrair;
	public AudioClip somMultiplicar;
	public AudioClip somZerar;
	public AudioClip somInterface;

	static AudioSource tocador;
	static AudioClip [] sons;

	void Awake()
	{
		if (_instancia == null || _instancia == this)
		{
			_instancia = this;
			DontDestroyOnLoad(gameObject);
			
			tocador = GetComponent<AudioSource>();
			sons = new AudioClip[5];
			sons[0] = somSomar;
			sons[1] = somSubtrair;
			sons[2] = somMultiplicar;
			sons[3] = somZerar;
			sons[4] = somInterface;
		}
		else
		{
			DestroyImmediate(gameObject);
		}
	}

	public static void Tocar(Tipos tipo = Tipos.Interface)
	{
		if (tocador && Dados.somLigado)
		{
			tocador.PlayOneShot(sons[(int) tipo]);
		}
	}
}

