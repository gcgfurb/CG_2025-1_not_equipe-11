using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Drawing;

namespace gcgcg
{
    internal class CuboFaces : Objeto
    {
        public Texture TexturaDaFace { get; set; }
        public CuboFaces(Objeto _paiRef, ref char _rotulo, Ponto4D[] vertices)
            : this(_paiRef, ref _rotulo, vertices, null, null)
        {

        }

        public CuboFaces(Objeto _paiRef, ref char _rotulo, Ponto4D[] vertices, int[] texturas, int[] normais)
            : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.TriangleFan;
            PrimitivaTamanho = 10;

            Ponto4D[] vertTextura = new[]
            {
                new Ponto4D(0, 0),
                new Ponto4D(1, 0),
                new Ponto4D(1, 1),
                new Ponto4D(0, 1)
            };

            Ponto4D[] vertNormal = new[]
            {
                new Ponto4D(0, 0, 1),
                new Ponto4D(1, 0, 0),
                new Ponto4D(0, 0, -1),
                new Ponto4D(-1, 0, 0),
                new Ponto4D(0, 1, 0),
                new Ponto4D(0, -1, 0)
            };

            foreach (Ponto4D vertice in vertices)
            {
                base.PontosAdicionar(vertice);
            }

            if (texturas != null)
            {
                foreach (int textura in texturas)
                {
                    texturasLista.Add(vertTextura[textura]);
                }
            }

            if (normais != null)
            {
                foreach (int normal in normais)
                {
                    normaisLista.Add(vertNormal[normal]);
                }
            }

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
        retorno = "__ Objeto CuboFaces _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
        retorno += base.ImprimeToString();
        return (retorno);
        }
#endif
    }
}