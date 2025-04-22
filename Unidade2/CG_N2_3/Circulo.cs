using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using System;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        public Circulo(Objeto _paiRef, ref char _rotulo) : this(_paiRef, ref _rotulo, 0.5f)
        {

        }

        public Circulo(Objeto _paiRef, ref char _rotulo, float raio) : base(_paiRef, ref _rotulo)
        {
            base.PrimitivaTipo = PrimitiveType.Points;
            // PrimitivaTamanho = 5;

            base.PontosAdicionar(new Ponto4D());

            int numPoints = 75;

            for (int i = 0; i < numPoints; i++)
            {
                float angulo = (i * 2 * MathF.PI) / numPoints;
                float x = raio * MathF.Cos(angulo);
                float y = raio * MathF.Sin(angulo);
                base.PontosAdicionar(new Ponto4D(x, y));
            }

            Atualizar();
        }

        public void Atualizar()
        {

            base.ObjetoAtualizar();
        }

#if CG_Debug
    public override string ToString()
    {
      System.Console.WriteLine("__________________________________ \n");
      string retorno;
      retorno = "__ Objeto Ponto _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return retorno;
    }
#endif

    }
}
