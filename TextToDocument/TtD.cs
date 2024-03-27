using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TextToDocument
{
#nullable enable
    /// <summary>
    /// 文本转富文本
    /// </summary>
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
            /// <summary>
            /// 普通样式
            /// </summary>
            Nomal = 0,
            /// <summary>
            /// 普通样式
            /// </summary>
            P = 0,
            /// <summary>
            /// 标题1
            /// </summary>
            H1 = 1,
            /// <summary>
            /// 标题2
            /// </summary>
            H2 = 2,
            /// <summary>
            /// 标题3
            /// </summary>
            H3 = 3,
            /// <summary>
            /// 标题4
            /// </summary>
            H4 = 4,
            /// <summary>
            /// 标题5
            /// </summary>
            H5 = 5,
            /// <summary>
            /// 标题6
            /// </summary>
            H6 = 6,
            /// <summary>
            /// 加粗
            /// </summary>
            B = 8,
            /// <summary>
            /// 加粗
            /// </summary>
            Bold = 8,
            /// <summary>
            /// 斜体
            /// </summary>
            I = 16,
            /// <summary>
            /// 斜体
            /// </summary>
            Italic = 16,
            /// <summary>
            /// 下划线
            /// </summary>
            Underline = 32,
            /// <summary>
            /// 下划线
            /// </summary>
            U = 32,
            /// <summary>
            /// 删除线
            /// </summary>
            Delete = 64,
            /// <summary>
            /// 删除线
            /// </summary>
            D = 64,

            /// <summary>
            /// 自定义1
            /// </summary>
            DIY1 = 4096,
            /// <summary>
            /// 自定义2
            /// </summary>
            DIY2 = 8192,
            /// <summary>
            /// 自定义3
            /// </summary>
            DIY3 = 16384,
            /// <summary>
            /// 自定义4
            /// </summary>
            DIY4 = 32768,

            /// <summary>
            /// 红色
            /// </summary>
            R = 65536,
            /// <summary>
            /// 红色
            /// </summary>
            Red = 65536,
            /// <summary>
            /// 红色
            /// </summary>
            R4 = 65536,
            /// <summary>
            /// 红色 3/4
            /// </summary>
            R3 = 131072,
            /// <summary>
            /// 红色 2/4
            /// </summary>
            R2 = 262144,
            /// <summary>
            /// 红色 1/4
            /// </summary>
            R1 = 524288,
            /// <summary>
            /// 绿色
            /// </summary>
            G = 1048576,
            /// <summary>
            /// 绿色
            /// </summary>
            Green = 1048576,
            /// <summary>
            /// 绿色
            /// </summary>
            G4 = 1048576,
            /// <summary>
            /// 绿色 3/4
            /// </summary>
            G3 = 2097152,
            /// <summary>
            /// 绿色 2/4
            /// </summary>
            G2 = 4194304,
            /// <summary>
            /// 绿色 1/4
            /// </summary>
            G1 = 8388608,
            /// <summary>
            /// 蓝色
            /// </summary>
            Bu = 16777216,
            /// <summary>
            /// 蓝色
            /// </summary>
            Blue = 16777216,
            /// <summary>
            /// 蓝色
            /// </summary>
            B4 = 16777216,
            /// <summary>
            /// 蓝色 3/4
            /// </summary>
            B3 = 33554432,
            /// <summary>
            /// 蓝色 2/4
            /// </summary>
            B2 = 67108864,
            /// <summary>
            /// 蓝色 1/4
            /// </summary>
            B1 = 134217728,
        }
        /// <summary>
        /// 文本和状态转换为Run
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="text">文本</param>
        /// <param name="format">格式</param>
        /// <returns>Run</returns>
        public static Run TextToRun(TextState[] state, string text, TextFormat? format = null)
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
        public static FlowDocument TextToDocument(string text, TextFormat? format = null)
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
        public static Paragraph TextToParagraph(string text, TextFormat? format = null)
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
