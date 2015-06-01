using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace XPlaneWPF.Effects
{
    internal static class EffectHelper
    {
        public static Uri MakePackUri(string relativeFile)
        {
            string uriString = "pack://application:,,,/" + AssemblyShortName + ";component/" + relativeFile;
            return new Uri(uriString);
        }

        private static string AssemblyShortName
        {
            get
            {
                if (_assemblyShortName == null)
                {
                    Assembly a = typeof(EffectHelper).Assembly;

                    // Pull out the short name.
                    _assemblyShortName = a.ToString().Split(',')[0];
                }

                return _assemblyShortName;
            }
        }

        private static string _assemblyShortName;
    }

    public class NegativeEffect : ShaderEffect
    {
        private static PixelShader shader = new PixelShader();

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(NegativeEffect), 0);

        static NegativeEffect()
        {
            shader.UriSource = EffectHelper.MakePackUri("Effects/Shaders/negative.ps");
        }

        public NegativeEffect()
        {
            this.PixelShader = shader;
            UpdateShaderValue(InputProperty);
        }

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
    }
}