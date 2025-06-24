#define CG_Debug
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Drawing;

namespace gcgcg
{
    internal class CuboMenor : Objeto
    {
        Ponto4D[] vertices;
        CuboFaces[] faces;

        public CuboMenor(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.TriangleFan;
            PrimitivaTamanho = 10;

            vertices = new[]
            {
                new Ponto4D(-0.3f, -0.3f,  0.3f),
                new Ponto4D( 0.3f, -0.3f,  0.3f),
                new Ponto4D( 0.3f,  0.3f,  0.3f),
                new Ponto4D(-0.3f,  0.3f,  0.3f),
                new Ponto4D(-0.3f, -0.3f, -0.3f),
                new Ponto4D( 0.3f, -0.3f, -0.3f),
                new Ponto4D( 0.3f,  0.3f, -0.3f),
                new Ponto4D(-0.3f,  0.3f, -0.3f)
            };

            var faceFrente = new CuboFaces(
                this,
                ref _rotulo,
                vertices: new[]
                {
                    vertices[0],
                    vertices[1],
                    vertices[2],
                    vertices[2],
                    vertices[3],
                    vertices[0]
                }
            );

            var faceDireita = new CuboFaces(
                this,
                ref _rotulo,
                vertices: new[]
                {
                    vertices[1],
                    vertices[5],
                    vertices[6],
                    vertices[6],
                    vertices[2],
                    vertices[1]
                }
            );

            var faceFundo = new CuboFaces(
                this,
                ref _rotulo,
                vertices: new[]
                {
                    vertices[5],
                    vertices[4],
                    vertices[7],
                    vertices[7],
                    vertices[6],
                    vertices[5]
                }
            );

            var faceEsquerda = new CuboFaces(
                this,
                ref _rotulo,
                vertices: new[]
                {
                    vertices[4],
                    vertices[0],
                    vertices[3],
                    vertices[3],
                    vertices[7],
                    vertices[4]
                }
            );

            var faceCima = new CuboFaces(
                this,
                ref _rotulo,
                vertices: new[]
                {
                    vertices[3],
                    vertices[2],
                    vertices[6],
                    vertices[6],
                    vertices[7],
                    vertices[3]
                }
            );

            var faceBaixo = new CuboFaces(
                this,
                ref _rotulo,
                vertices: new[]
                {
                    vertices[0],
                    vertices[4],
                    vertices[5],
                    vertices[5],
                    vertices[1],
                    vertices[0]
                }
            );

            faces = new[]
            {
                faceFrente,
                faceDireita,
                faceFundo,
                faceEsquerda,
                faceCima,
                faceBaixo
            };

            base.MatrizTranslacaoXYZ(3.0, 0.0, 0.0);

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
            retorno = "__ Objeto Cubo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return (retorno);
        }
#endif
    }
}