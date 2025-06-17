//https://github.com/mono/opentk/blob/main/Source/Examples/Shapes/Old/Cube.cs

#define CG_Debug
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;
using System;

namespace gcgcg
{
  internal class CuboOrbital : Objeto
  {
    public Ponto4D[] vertices;
    int[] indices;
    // Vector3[] normals;
    // int[] colors;

    public CuboOrbital(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
    {
      // PrimitivaTipo = PrimitiveType.TriangleFan;
      PrimitivaTipo = PrimitiveType.Triangles;
      PrimitivaTamanho = 12; //sao 12 triangulos para formar o cubo 

      vertices = new Ponto4D[]
      {
        new Ponto4D(-0.6f, -0.6f,  0.6f), // 0: Frente Inferior Esquerda
        new Ponto4D( 0.6f, -0.6f,  0.6f), // 1: Frente Inferior Direita
        new Ponto4D( 0.6f,  0.6f,  0.6f), // 2: Frente Superior Direita
        new Ponto4D(-0.6f,  0.6f,  0.6f), // 3: Frente Superior Esquerda
        new Ponto4D(-0.6f, -0.6f, -0.6f), // 4: Trás Inferior Esquerda
        new Ponto4D( 0.6f, -0.6f, -0.6f), // 5: Trás Inferior Direita
        new Ponto4D( 0.6f,  0.6f, -0.6f), // 6: Trás Superior Direita
        new Ponto4D(-0.6f,  0.6f, -0.6f)  // 7: Trás Superior Esquerda
      };


     
      indices = new int[]
      {
        // Face da Frente (0, 1, 2, 3)
        0, 1, 2, // Triângulo 1: 0-1-2
        0, 2, 3, // Triângulo 2: 0-2-3 (ou 2, 3, 0 dependendo da ordem dos vértices para culling)

        // Face de Trás (4, 7, 6, 5)
        4, 5, 6, // Triângulo 3: 4-5-6
        4, 6, 7, // Triângulo 4: 4-6-7

        // Face de Cima (3, 2, 6, 7)
        3, 2, 6, // Triângulo 5: 3-2-6
        3, 6, 7, // Triângulo 6: 3-6-7

        // Face de Baixo (0, 4, 5, 1)
        0, 4, 5, // Triângulo 7: 0-4-5
        0, 5, 1, // Triângulo 8: 0-5-1

        // Face da Esquerda (0, 3, 7, 4)
        0, 3, 7, // Triângulo 9: 0-3-7
        0, 7, 4, // Triângulo 10: 0-7-4

        // Face da Direita (1, 5, 6, 2)
        1, 5, 6, // Triângulo 11: 1-5-6
        1, 6, 2  // Triângulo 12: 1-6-2
      };


      // indices = new int[]
      // {
      //   0, 6
      // };

      // // // 0, 1, 2, 3 Face da frente
      // base.PontosAdicionar(vertices[0]);
      // base.PontosAdicionar(vertices[1]);
      // base.PontosAdicionar(vertices[2]);
      // base.PontosAdicionar(vertices[3]);

      // // // 3, 2, 6, 7 Face de cima
      // base.PontosAdicionar(vertices[3]);
      // base.PontosAdicionar(vertices[2]);
      // base.PontosAdicionar(vertices[6]);
      // base.PontosAdicionar(vertices[7]);

      // // // 4, 7, 6, 5 Face do fundo
      // base.PontosAdicionar(vertices[4]);
      // base.PontosAdicionar(vertices[7]);
      // base.PontosAdicionar(vertices[6]);
      // base.PontosAdicionar(vertices[5]);

      // // // 0, 3, 7, 4 Face esquerda
      // base.PontosAdicionar(vertices[0]);
      // base.PontosAdicionar(vertices[3]);
      // base.PontosAdicionar(vertices[7]);
      // base.PontosAdicionar(vertices[4]);

      // // // 0, 4, 5, 1 Face de baixo
      // base.PontosAdicionar(vertices[0]);
      // base.PontosAdicionar(vertices[4]);
      // base.PontosAdicionar(vertices[5]);
      // base.PontosAdicionar(vertices[1]);

      // // // 1, 5, 6, 2 Face direita
      // base.PontosAdicionar(vertices[1]);
      // base.PontosAdicionar(vertices[5]);
      // base.PontosAdicionar(vertices[6]);
      // base.PontosAdicionar(vertices[2]);

      // for (int i = 0; i < vertices.Length; i++)
      // {
      //   base.PontosAdicionar(vertices[i]);
      // }

      for (int i = 0; i < indices.Length; i++)
      {
        //Console.WriteLine(indices[i]);
        //Console.WriteLine(vertices[indices[i]]);
        base.PontosAdicionar(vertices[indices[i]]); //usa os 36 pontos do vetor indices para montar cada triangulo
      }

      this.MatrizTranslacaoXYZ(5, 0, 0);

      Atualizar();
    }


    private void Atualizar()
    {
      base.ObjetoAtualizar();
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto CuboOrbital _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif

  }
}
