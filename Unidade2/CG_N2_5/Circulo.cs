using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using System;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        public Ponto4D ptoDeslocamento;
        public float raio;
        private int numPoints = 70;

        public Circulo(Objeto _paiRef, ref char _rotulo) : this(_paiRef, ref _rotulo, 0.5f, new Ponto4D(0, 0))
        {

        }

        public Circulo(Objeto _paiRef, ref char _rotulo, float raio, Ponto4D ptoDeslocamento) : base(_paiRef, ref _rotulo)
        {
            this.ptoDeslocamento = ptoDeslocamento;
            this.raio = raio;
            base.PrimitivaTipo = PrimitiveType.LineStrip;
            base.PrimitivaTamanho = 4;

            GerarPontos();
            Atualizar(ptoDeslocamento);
        }

        private void GerarPontos()
        {
            for (int i = 0; i < numPoints + 1; i++)
            {
                float angulo = (i * 2 * MathF.PI) / numPoints;
                float x = raio * MathF.Cos(angulo);
                float y = raio * MathF.Sin(angulo);
                base.PontosAdicionar(new Ponto4D(x + ptoDeslocamento.X, y + ptoDeslocamento.Y));
            }
        }

        public void Atualizar(Ponto4D ptoDeslocamento)
        {
            this.PontosApagar();
            this.ptoDeslocamento = ptoDeslocamento;
            GerarPontos();
            base.ObjetoAtualizar();
        }

        public Ponto4D getPontoAngulo(double angulo)
        {
            double angleRad = angulo * Math.PI / 180.0;
            double x = ptoDeslocamento.X + raio * Math.Cos(angleRad);
            double y = ptoDeslocamento.Y + raio * Math.Sin(angleRad);
            return new Ponto4D(x, y);
        }
    }
}