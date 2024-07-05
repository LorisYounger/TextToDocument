using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextToDocument;

namespace DEMO
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TextBoxIn.Text = "|h1|TextToDocument|p|\n" +
                "TextToDocument 是一个简单的文本转富文本的帮助类库\n" +
                "|h2|使用方法|p|\n" +
                "直接调用 `TtD.TextToDocument(\"文本\")` 即可\n" +
                "|h2|TtD语法|p|\n" +
                "详细语法参见 `TextState`\nTtD 使用 `\\|` 标记富文本, 标记后将会对后续的文本生效" +
                "|h3|例子:|p|\n" +
                "`这是\\|b\\|加粗\\|-b\\|文本` 这是|b|加粗|-b|文本\n" +
                "|h2|标签累加|p|\n" +
                "标签可以进行累加\n例如同时显示字体为红色和斜体则写为\n`\\|R\\|\\|I\\|红色和斜体\\|p\\|` |R||I|红色和斜体|p|\n" +
                "|h2|标签移除|p|\n" +
                "在标签前面 添加 `-` 符号即可移除 之前应用的相应标签, `p` 则移除全部标签\n例如 `\\|R\\|红色\\|-R\\|黑色` |R|红色|-R|黑色\n" +
                "|h2|颜色混合|p|\n" +
                "标签颜色支持混合成其他颜色\n例如 `|R3|颜色R3|G3|+G3|B3|+B3|P|`";
            timer.Elapsed += (s, e) =>
            {
                if (FO?.CanNext() == true)
                {
                    Dispatcher.Invoke(() =>
                    {
                        FO.Next();
                        TextBoxOut.ScrollToEnd();
                    });
                    timer.Start();
                }
            };
        }

        private void TextBoxIn_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxOut.Document = TtD.TextToDocument(TextBoxIn.Text);
        }
        Timer timer = new Timer(50);
        TtD.FlowOutPut FO;
        private void TextBoxIn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBoxOut.Document.Blocks.Clear();
            FO = new TtD.FlowOutPut(TextBoxOut.Document, TextBoxIn.Text);
            FO.Next();
            timer.Start();
        }
    }
}
