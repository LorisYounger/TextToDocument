using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using static TextToDocument.TtD;

namespace TextToDocument
{
    public class TextFormat
    {
        public Format H1;
        public Format H2;
        public Format H3;
        public Format H4;
        public Format H5;
        public Format H6;
        public Format P;
        public Format B;
        public Format I;
        public Format U;
        public Format D;

        public Format DIY1;
        public Format DIY2;
        public Format DIY3;
        public Format DIY4;

        public TextFormat()
        {
            H1 = new Format() { FontSize = 32, FontWeight = FontWeights.Bold };
            H2 = new Format() { FontSize = 24, FontWeight = FontWeights.Bold };
            H3 = new Format() { FontSize = 16, FontWeight = FontWeights.Bold };
            H4 = new Format() { FontSize = 12, FontWeight = FontWeights.Bold };
            H5 = new Format() { FontSize = 12 };
            H6 = new Format() { FontSize = 9 };
            P = new Format() { FontSize = 12 };
            B = new Format() { FontWeight = FontWeights.Bold };
            I = new Format() { FontStyle = FontStyles.Italic };
            U = new Format() { TextDecorations = TextDecorations.Underline };
            D = new Format() { TextDecorations = TextDecorations.Strikethrough };
        }
        /// <summary>
        /// 为Base应用相应格式
        /// </summary>
        /// <param name="Base">格式</param>
        /// <param name="Formats">要被应用的格式</param>
        public static void ApplyFormats(Format Base, params Format[] Formats)
        {
            foreach (Format TE in Formats)
            {
                Base.ApplyFormat(TE);
            }
        }
        /// <summary>
        /// 从states中获取相应格式
        /// </summary>
        /// <param name="states">文本状态</param>
        /// <returns>格式</returns>
        public Format StatesFormat(params TextState[] states)
        {
            Format Base = P.Clone();
            foreach (TextState state in states)
                switch (state)
                {
                    case TextState.Nomal:
                        Base = P.Clone();
                        break;
                    case TextState.H1:
                        Base.ApplyFormat(H1);
                        break;
                    case TextState.H2:
                        Base.ApplyFormat(H2);
                        break;
                    case TextState.H3:
                        Base.ApplyFormat(H3);
                        break;
                    case TextState.H4:
                        Base.ApplyFormat(H4);
                        break;
                    case TextState.H5:
                        Base.ApplyFormat(H5);
                        break;
                    case TextState.H6:
                        Base.ApplyFormat(H6);
                        break;
                    case TextState.B:
                        Base.ApplyFormat(B);
                        break;
                    case TextState.I:
                        Base.ApplyFormat(I);
                        break;
                    case TextState.Underline:
                        Base.ApplyFormat(U);
                        break;
                    case TextState.Delete:
                        Base.ApplyFormat(D);
                        break;
                    case TextState.DIY1:
                        Base.ApplyFormat(DIY1);
                        break;
                    case TextState.DIY2:
                        Base.ApplyFormat(DIY2);
                        break;
                    case TextState.DIY3:
                        Base.ApplyFormat(DIY3);
                        break;
                    case TextState.DIY4:
                        Base.ApplyFormat(DIY4);
                        break;
                    case TextState.R:
                        Base.ForeColorR = 255;
                        break;
                    case TextState.R3:
                        Base.ForeColorR = 192;
                        break;
                    case TextState.R2:
                        Base.ForeColorR = 128;
                        break;
                    case TextState.R1:
                        Base.ForeColorR = 64;
                        break;
                    case TextState.G:
                        Base.ForeColorG = 255;
                        break;
                    case TextState.G3:
                        Base.ForeColorG = 192;
                        break;
                    case TextState.G2:
                        Base.ForeColorG = 128;
                        break;
                    case TextState.G1:
                        Base.ForeColorG = 64;
                        break;
                    case TextState.Bu:
                        Base.ForeColorB = 255;
                        break;
                    case TextState.B3:
                        Base.ForeColorB = 192;
                        break;
                    case TextState.B2:
                        Base.ForeColorB = 128;
                        break;
                    case TextState.B1:
                        Base.ForeColorB = 64;
                        break;
                }
            return Base;
        }

        /// <summary>
        /// 文本格式
        /// </summary>
        public class Format
        {
            public short ForeColorA = 255;
            public short ForeColorR;
            public short ForeColorG;
            public short ForeColorB;

            public TextDecorationCollection TextDecorations;
            public FontStyle FontStyle;
            public FontWeight FontWeight;
            public double FontSize = double.NaN;
            public FontFamily FontFamily;
            public FontStretch FontStretch;
            public Brush Foreground;
            public Brush Background;
            /// <summary>
            /// 应用其他格式到该格式
            /// </summary>
            /// <param name="f">从其他格式应用到该格式</param>
            public void ApplyFormat(Format f)
            {
                if (!double.IsNaN(f.FontSize))
                    FontSize = f.FontSize;
                if (f.FontWeight != null)
                    FontWeight = f.FontWeight;
                if (f.TextDecorations != null)
                    TextDecorations = f.TextDecorations;
                if (f.FontStyle != null)
                    FontStyle = f.FontStyle;
                if (f.FontFamily != null)
                    FontFamily = f.FontFamily;
                if (f.FontStretch != null)
                    FontStretch = f.FontStretch;
                if (f.Foreground != null)
                    Foreground = f.Foreground;
                if (f.Background != null)
                    Background = f.Background;
                ForeColorR += f.ForeColorR;
                ForeColorG += f.ForeColorG;
                ForeColorB += f.ForeColorB;
                ForeColorA += f.ForeColorA;
            }
            /// <summary>
            /// 获得前景色 若已有前景色则应用前景色,否则应用Color
            /// </summary>
            public Brush ForegroundBrush
            {
                get
                {
                    if (Foreground != null)
                        return Foreground;
                    return new SolidColorBrush(Color.FromArgb(ShortColorToByte(ForeColorA),
                        ShortColorToByte(ForeColorR), ShortColorToByte(ForeColorG), ShortColorToByte(ForeColorB)));
                }
            }
            public static byte ShortColorToByte(short color)
            {
                if (color >= 255)
                    return 255;
                else if (color <= 0)
                    return 0;
                return (byte)color;
            }
            /// <summary>
            /// 克隆一个相同的格式
            /// </summary>
            public Format Clone()
            {
                Format f = new Format();
                f.ForeColorA = ForeColorA;
                f.ForeColorG = ForeColorG;
                f.ForeColorB = ForeColorB;
                f.ForeColorR = ForeColorR;
                f.Background = Background;
                f.FontFamily = FontFamily;
                f.FontSize = FontSize;
                f.FontStyle = FontStyle;
                f.FontWeight = FontWeight;
                f.FontStretch = FontStretch;
                f.TextDecorations = TextDecorations;
                f.Foreground = Foreground;
                return f;
            }
            /// <summary>
            /// 将颜色应用到inline组件
            /// </summary>
            /// <param name="inline">ToInline</param>
            public void ToInLine(Inline inline)
            {
                inline.Background = Background;
                if (FontFamily != null)
                    inline.FontFamily = FontFamily;
                if (!double.IsNaN(FontSize))
                    inline.FontSize = FontSize;
                if (FontStyle != null)
                    inline.FontStyle = FontStyle;
                if (FontWeight != null)
                    inline.FontWeight = FontWeight;
                if (FontStretch != null)
                    inline.FontStretch = FontStretch;
                if (TextDecorations != null)
                    inline.TextDecorations = TextDecorations;
                inline.Foreground = ForegroundBrush;
            }
        }
    }

}
