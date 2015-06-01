using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace XPlaneWPF.Effects
{
    public class TransparencyEffect : ShaderEffect
    {
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register("Opacity", typeof(double), typeof(TransparencyEffect), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(TransparencyEffect), 0);

        private static PixelShader shader = new PixelShader();

        static TransparencyEffect()
        {
            shader.UriSource = EffectHelper.MakePackUri("transparency.ps");
        }

        public TransparencyEffect()
        {
            PixelShader = shader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(OpacityProperty);
        }

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }
    }
}