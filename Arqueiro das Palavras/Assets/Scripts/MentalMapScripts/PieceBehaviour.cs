using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]

public class PieceBehaviour : MonoBehaviour {
	
	Vector3 posStart;
	Vector3 posInicial;
	Vector3 posDestino;
	Vector3 vetorDirecao;

	Rigidbody2D pieceBody;
	BoxCollider2D pieceCollider;
	bool estaArrastando;
	float distancia;

	public MapaMentalController mapaMentalController;

	public string pieceName;

	public bool estaConectado;
	public float velocidadeMovimento = 10;
	public Transform conector;
	public SpriteRenderer conectorSprite;
	public float distMinConector;


	void Start () {
		mapaMentalController = mapaMentalController.GetComponent<MapaMentalController>();
		pieceBody = transform.GetComponent<Rigidbody2D>();
		pieceCollider = transform.GetComponent<BoxCollider2D>();
		pieceBody.gravityScale = 0f;
		posStart = transform.position;
		Physics2D.IgnoreLayerCollision(15,15, true);
		estaArrastando = false;
		estaConectado = false;
	}
	
	private void OnMouseDown() {
		posInicial = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
		estaArrastando = true;
		estaConectado = false;
	}

	private void OnMouseDrag() {
		posDestino = posInicial + Camera.main.ScreenToWorldPoint(Input.mousePosition);
		vetorDirecao = posDestino - transform.position;
		pieceBody.velocity = vetorDirecao * velocidadeMovimento;
	}

	private void OnMouseUp() {
		estaArrastando = false;
		pieceBody.velocity = Vector2.zero;
	}

	private void FixedUpdate() {
		if (!estaArrastando && !estaConectado)
		{
			distancia = Vector2.Distance(transform.position, conector.position);

			//proximo
			if (distancia <= distMinConector)
			{
				pieceBody.velocity = Vector2.zero;
				transform.position = Vector2.MoveTowards(transform.position, conector.position, 0.05f);
			}

			//errou
			else if (distancia > distMinConector)
			{
				transform.position = Vector2.MoveTowards(transform.position, posStart, 0.5f);
			}

			//em cima
			if (distancia < 0.01f)
			{
				estaConectado = true;
				transform.position = conector.position;
				pieceCollider.enabled = false;
				conectorSprite.enabled = false;
				SavePos();
			}
		}
	}

	public void SavePos()
	{
		mapaMentalController.SavingPiecePos(pieceName, transform.position);
	}
}
