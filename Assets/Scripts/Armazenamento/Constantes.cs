using UnityEngine;
using System.Collections;

public class Constantes
{
	// Cores dos botões
	// #54 84 192 255 #3654c0ff
	public static Color corPositivo = 
		new Color(0.21f, 0.33f, 0.75f, 1);
	// #c05436ff
	public static Color corNegativo = 
		new Color(0.75f, 0.33f, 0.21f, 1);
	// #36c054ff
	public static Color corMultiplicador = 
		new Color(0.21f, 0.75f, 0.33f, 1);
	public static Color corZero = new 
		Color(0.5f,0.5f,0.5f,1);
	public static Color corBrilho = new 
		Color(1,1,1,1);

	// Animação
	public const string animGatilhoSumir = "sumir";
	public const string animNomeAparecer = "Aparecer";

	// Navegação
	public const string telaMenu = "Menu";
	public const string telaJogo = "Jogo";
}

