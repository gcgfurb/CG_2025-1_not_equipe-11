#define CG_Debug
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace gcgcg
{
    internal class Cubo : Objeto
    {

    Ponto4D[] vertices;
    int[] indices;
    // Vector3[] normals;
    // int[] colors;

        public Cubo(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.Triangles;
            PrimitivaTamanho = 1; 

            Ponto4D[] vertices = new Ponto4D[]
            {
                new Ponto4D(-1.0f, -1.0f,  1.0f), // 0: Frente Inferior Esquerda
                new Ponto4D( 1.0f, -1.0f,  1.0f), // 1: Frente Inferior Direita
                new Ponto4D( 1.0f,  1.0f,  1.0f), // 2: Frente Superior Direita
                new Ponto4D(-1.0f,  1.0f,  1.0f), // 3: Frente Superior Esquerda
                new Ponto4D(-1.0f, -1.0f, -1.0f), // 4: Trás Inferior Esquerda
                new Ponto4D( 1.0f, -1.0f, -1.0f), // 5: Trás Inferior Direita
                new Ponto4D( 1.0f,  1.0f, -1.0f), // 6: Trás Superior Direita
                new Ponto4D(-1.0f,  1.0f, -1.0f)  // 7: Trás Superior Esquerda
            };

            int[] indices = new int[]
            {
                // Face da Frente
                0, 1, 2,   2, 3, 0,
                // Face de Trás
                4, 5, 6,   6, 7, 4,
                // Face de Cima
                3, 2, 6,   6, 7, 3,
                // Face de Baixo
                0, 4, 5,   5, 1, 0,
                // Face da Esquerda
                0, 3, 7,   7, 4, 0,
                // Face da Direita
                1, 5, 6,   6, 2, 1
            };

            for (int i = 0; i < indices.Length; i++)
            {
                base.PontosAdicionar(vertices[indices[i]]);
            }

            // base.ObjetoAtualizar();
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