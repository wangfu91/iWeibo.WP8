using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace iWeibo.Utils
{
    public class ConvertContentBehavior : Behavior<RichTextBox>
    {
        public static DependencyProperty ContentProperty = DependencyProperty.RegisterAttached("Content", typeof(string), typeof(ConvertContentBehavior), new PropertyMetadata(null, new PropertyChangedCallback(ConvertContentBehavior.OnContentChanged)));
        public string Content
        {
            get
            {
                return (string)base.GetValue(ConvertContentBehavior.ContentProperty);
            }
            set
            {
                base.SetValue(ConvertContentBehavior.ContentProperty, value);
            }
        }
        public bool IsEnable
        {
            get;
            set;
        }
        public ConvertContentBehavior()
        {
            this.IsEnable = true;
        }
        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string text = e.NewValue as string;
            ConvertContentBehavior convertContentBehavior = d as ConvertContentBehavior;
            if (!string.IsNullOrWhiteSpace(text) && d != null)
            {
                convertContentBehavior.ConvertFanfouContent(text);
            }
        }
        private void ConvertFanfouContent(string content)
        {
            try
            {
                Regex regex = new Regex("(((http://)|(ftp://)|(https://)|(zune://)+)[^(\\s)]{1,})|(#[^\\s]{1,}#)|(((@{1})[^(\\s|@)]{1,}))");
                string text = content.Replace("<b>", string.Empty);
                text = text.Replace("</b>", string.Empty);
                text = HttpUtility.HtmlDecode(text);
                Paragraph paragraph = new Paragraph();
                MatchCollection matchCollection = regex.Matches(text);
                if (matchCollection.Count != 0)
                {
                    int num = 0;
                    for (int i = 0; i < matchCollection.Count; i++)
                    {
                        if (matchCollection[i].Index != 0)
                        {
                            Run run = new Run();
                            run.Text = text.Substring(num, matchCollection[i].Index - num);
                            num += run.Text.Length;
                            paragraph.Inlines.Add(run);
                        }
                        if (matchCollection[i].Value.StartsWith("#"))
                        {
                            if (matchCollection[i].Value.Length > 2)
                            {
                                Hyperlink hyperlink = new Hyperlink();
                                hyperlink.Foreground = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                                string text2 = matchCollection[i].Value.Remove(0, 1);
                                text2 = text2.Remove(text2.Length - 1, 1);
                                dictionary.Add("query", text2);
                                Run run = new Run();
                                run.Text = matchCollection[i].Value;
                                hyperlink.Inlines.Add(run);
                                paragraph.Inlines.Add(hyperlink);
                                num += matchCollection[i].Length;
                            }
                        }
                        else
                        {
                            if (matchCollection[i].Value.StartsWith("@"))
                            {
                                string text3 = matchCollection[i].Value;
                                string text4 = text3;
                                if (text4.Contains(":"))
                                {
                                    text3 = text4.Substring(0, text4.IndexOf(":"));
                                }
                                Hyperlink hyperlink = new Hyperlink();
                                hyperlink.Foreground = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                                Run run = new Run();
                                run.Text = text3;
                                hyperlink.Inlines.Add(run);
                                paragraph.Inlines.Add(hyperlink);
                                num += matchCollection[i].Length;
                                if (text4.Contains(":"))
                                {
                                    run = new Run();
                                    run.Text = text4.Substring(text4.IndexOf(":"));
                                    paragraph.Inlines.Add(run);
                                }
                            }
                            else
                            {
                                Hyperlink hyperlink = new Hyperlink();
                                hyperlink.Foreground = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                                hyperlink.TargetName = "_blank";
                                hyperlink.NavigateUri = new Uri(matchCollection[i].Value);
                                Run run = new Run();
                                run.Text = matchCollection[i].Value;
                                hyperlink.Inlines.Add(run);
                                paragraph.Inlines.Add(hyperlink);
                                num += matchCollection[i].Length;
                            }
                        }
                    }
                    if (num < text.Length - 1)
                    {
                        Run run = new Run();
                        run.Text = text.Substring(num, text.Length - num);
                        num += run.Text.Length;
                        paragraph.Inlines.Add(run);
                    }
                }
                else
                {
                    Run run = new Run();
                    run.Text = text;
                    paragraph.Inlines.Add(run);
                }
                base.AssociatedObject.Blocks.Clear();
                base.AssociatedObject.Blocks.Add(paragraph);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in Custom.RichTextBox.OnContentChanged:" + ex.Message);
            }
        }
    }
}
