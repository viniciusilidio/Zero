using UnityEngine;
using System.Collections;

public class Musica : MonoBehaviour
{
	public static Musica _instancia = null;

	static AudioSource tocador;

	void Awake ()
	{
		if (_instancia == null || _instancia == this)
		{
			_instancia = this;
			DontDestroyOnLoad(gameObject);
			
			tocador = GetComponent<AudioSource>();
			if (Dados.musicaLigado)
				tocador.Play();
		}
		else
		{
			DestroyImmediate(gameObject);
		}
	}

	void Update()
	{
		if (_instancia == this && tocador)
		{
			if (Dados.musicaLigado)
			{
				if (tocador.isPlaying == false)
				{
					tocador.UnPause();
				}
			}
			else if (tocador.isPlaying)
			{
				tocador.Pause();
			}
		}
	}
}

