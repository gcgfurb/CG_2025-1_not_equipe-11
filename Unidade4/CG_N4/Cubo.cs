#define CG_Debug
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace gcgcg
{
  internal class Cubo : Objeto
  {
    CuboFaces[] faces;
    Ponto4D[] vertices;
    int[][] texturas;
    int[][] normais;

    Shader _shaderTextura;
    Texture _textura;

    public Cubo(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.TriangleFan;
      PrimitivaTamanho = 10;

      vertices = new[]
      {
        new Ponto4D(-1.0f, -1.0f,  1.0f),
        new Ponto4D( 1.0f, -1.0f,  1.0f),
        new Ponto4D( 1.0f,  1.0f,  1.0f),
        new Ponto4D(-1.0f,  1.0f,  1.0f),
        new Ponto4D(-1.0f, -1.0f, -1.0f),
        new Ponto4D( 1.0f, -1.0f, -1.0f),
        new Ponto4D( 1.0f,  1.0f, -1.0f),
        new Ponto4D(-1.0f,  1.0f, -1.0f)
    };

      // --- CORREÇÃO 1: SINTAXE DO ARRAY ---
      texturas = new int[][]
      {
        new int[] { 0, 1, 2, 2, 3, 0 },
        new int[] { 0, 1, 2, 2, 3, 0 },
        new int[] { 0, 1, 2, 2, 3, 0 },
        new int[] { 0, 1, 2, 2, 3, 0 },
        new int[] { 0, 1, 2, 2, 3, 0 },
        new int[] { 0, 1, 2, 2, 3, 0 }
      };

      normais = new int[][]
      {
        new int[] { 0, 0, 0, 0, 0, 0 },
        new int[] { 1, 1, 1, 1, 1, 1 },
        new int[] { 2, 2, 2, 2, 2, 2 },
        new int[] { 3, 3, 3, 3, 3, 3 },
        new int[] { 4, 4, 4, 4, 4, 4 },
        new int[] { 5, 5, 5, 5, 5, 5 }
      };

      // A criação das faces estava correta
      var faceFrente = new CuboFaces(this, ref _rotulo, new[] { vertices[0], vertices[1], vertices[2], vertices[2], vertices[3], vertices[0] }, texturas[0], normais[0]);
      var faceDireita = new CuboFaces(this, ref _rotulo, new[] { vertices[1], vertices[5], vertices[6], vertices[6], vertices[2], vertices[1] }, texturas[1], normais[1]);
      var faceFundo = new CuboFaces(this, ref _rotulo, new[] { vertices[5], vertices[4], vertices[7], vertices[7], vertices[6], vertices[5] }, texturas[2], normais[2]);
      var faceEsquerda = new CuboFaces(this, ref _rotulo, new[] { vertices[4], vertices[0], vertices[3], vertices[3], vertices[7], vertices[4] }, texturas[3], normais[3]);
      var faceCima = new CuboFaces(this, ref _rotulo, new[] { vertices[3], vertices[2], vertices[6], vertices[6], vertices[7], vertices[3] }, texturas[4], normais[4]);
      var faceBaixo = new CuboFaces(this, ref _rotulo, new[] { vertices[0], vertices[4], vertices[5], vertices[5], vertices[1], vertices[0] }, texturas[5], normais[5]);

      faces = new[]
      {
        faceFrente,
        faceDireita,
        faceFundo,
        faceEsquerda,
        faceCima,
        faceBaixo
      };

      // --- CORREÇÃO 2: DESCOMENTAR A CRIAÇÃO DO SHADER ---
      _shaderTextura = new Shader("Shaders/shaderTextura.vert", "Shaders/shaderTextura.frag");
      _shaderTextura.Use();

      // O carregamento das texturas estava correto
      var texturaFrente = Texture.LoadFromFile("Resources/dalton.jpg");
      var texturaDireita = Texture.LoadFromFile("Resources/gabriel-klauck.jpg");
      var texturaFundo = Texture.LoadFromFile("Resources/joao-krapp.jpg");
      var texturaEsquerda = Texture.LoadFromFile("Resources/martin.jpg");
      var texturaCima = Texture.LoadFromFile("Resources/nicolas.jpg");
      var texturaBaixo = Texture.LoadFromFile("Resources/rodacki.jpg");

      var texturasCarregadas = new[]
      {
        texturaFrente,
        texturaDireita,
        texturaFundo,
        texturaEsquerda,
        texturaCima,
        texturaBaixo
      };

      // A atribuição estava correta
      for (int i = 0; i < faces.Length; i++)
      {
        faces[i].TexturaDaFace = texturasCarregadas[i];
        faces[i].shaderCor = _shaderTextura;
      }
    }

    private void Atualizar()
    {
      base.ObjetoAtualizar();
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Cubo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif

  }
}
