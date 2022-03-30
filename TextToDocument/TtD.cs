using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TextToDocument
{
    public static class TtD
    {
        /// <summary>
        /// 默认的格式
        /// </summary>
        public static TextFormat DefaultFormart = new TextFormat();
        /// <summary>
        /// 文本样式
        /// </summary>
        public enum TextState
        {
            Nomal = 0,
            P = 0,
            H1 = 1,
            H2 = 2,
            H3 = 3,
            H4 = 4,
            H5 = 5,
            H6 = 6,
            B = 8,
            Bold = 8,
            I = 16,
            Italic = 16,
            Underline = 32,
            U = 32,
            Delete = 64,
            D = 64,

            DIY1 = 4096,
            DIY2 = 8192,
            DIY3 = 16384,
            DIY4 = 32768,

            R = 65536,
            Red = 65536,
            R4 = 65536,
            R3 = 131072,
            R2 = 262144,
            R1 = 524288,
            G = 1048576,
            Green = 1048576,
            G4 = 1048576,
            G3 = 2097152,
            G2 = 4194304,
            G1 = 8388608,
            Bu = 16777216,
            Blue = 16777216,
            B4 = 16777216,
            B3 = 33554432,
            B2 = 67108864,
            B1 = 134217728,
        }
        /// <summary>
        /// 文本和状态转换为Run
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="text">文本</param>
        /// <param name="format">格式</param>
        /// <returns>Run</returns>
        public static Run TextToRun(TextState[] state, string text, TextFormat format = null)
        {
            Run textRun = new Run(text);
            if (format == null)
                format = DefaultFormart;
            format.StatesFormat(state).ToInLine(textRun);
            return textRun;
        }
        /// <summary>
        /// 将文本转换为富文本 Document
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="format">样式</param>
        /// <returns>Document</returns>
        public static FlowDocument TextToDocument(string text, TextFormat format = null)
        {
            FlowDocument document = new FlowDocument();
            Paragraph para = new Paragraph();
            List<TextState> states = new List<TextState>();
            bool isstate = false;
            if (format == null)
                format = DefaultFormart;
            foreach (string str in text.Replace("\\|", "\\split").Split('|'))
            {
                if (isstate)
                {
                    if (str.ToLower() == "p")
                    {
                        states.Clear();
                    }
                    else if (str.StartsWith("-"))
                    {
                        if (Enum.TryParse<TextState>(str.Substring(1), true, out var res))
                            states.Remove(res);
                        else//容错
                            goto TAGstate;
                    }
                    else
                    {
                        if (Enum.TryParse<TextState>(str, true, out var res))
                            states.Add(res);
                        else//容错
                            goto TAGstate;
                    }
                    isstate = false;
                    continue;
                }
            TAGstate:
                string value = str.Replace("\\split", "|");
                if (value.Length > 0)
                    if (states.Contains(TextState.H1) || states.Contains(TextState.H2) || states.Contains(TextState.H3)
                        || states.Contains(TextState.H4) || states.Contains(TextState.H5) || states.Contains(TextState.H6))
                    {
                        if (para.Inlines.Count > 0)
                        {
                            ((Run)para.Inlines.Last()).Text = ((Run)para.Inlines.Last()).Text.TrimEnd('\n');
                            document.Blocks.Add(para);
                        }
                        para = new Paragraph(TextToRun(states.ToArray(), value, format));
                    }
                    else
                        para.Inlines.Add(TextToRun(states.ToArray(), value, format));
                isstate = true;

            }
            document.Blocks.Add(para);
            return document;
        }
        /// <summary>
        /// 将文本转换为富文本 Paragraph 显示效果并没有 Document 好
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="format">样式</param>
        /// <returns>Paragraph</returns>
        public static Paragraph TextToParagraph(string text, TextFormat format = null)
        {
            Paragraph para = new Paragraph();
            List<TextState> states = new List<TextState>();
            bool isstate = false;
            if (format == null)
                format = DefaultFormart;
            foreach (string str in text.Replace("\\|", "\\split").Split('|'))
            {
                if (isstate)
                {
                    if (str.ToLower() == "p")
                    {
                        states.Clear();
                    }
                    else if (str.StartsWith("-"))
                    {
                        states.Remove((TextState)Enum.Parse(typeof(TextState), str.Substring(1), true));
                    }
                    else
                    {
                        states.Add((TextState)Enum.Parse(typeof(TextState), str, true));
                    }
                    isstate = false;
                }
                else
                {
                    if (str.Length > 0)
                        para.Inlines.Add(TextToRun(states.ToArray(), str, format));
                    isstate = true;
                }
            }
            return para;
        }
    }
}
