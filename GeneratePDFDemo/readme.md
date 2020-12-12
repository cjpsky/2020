# .net Core 采用iTestSharp生成PDF（添加文字水印）
> 最近手上有个项目，需要导出pdf并添加水印，网上可找的资料也有很多，我这里采用了iTextSharp.LGPLv2.Core版本库，代码整理了下分享给有需要的小伙伴。

    demo采用.net core 3.1

### 1.Nuget包

> iTextSharp.LGPLv2.Core

    PM> Install-Package iTextSharp.LGPLv2.Core -Version 1.7.0
* 可以在项目中直接引用iTextSharp.LGPLv2.Core.dll就能使用；

```c#
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneratePDFDemo
{
    public class GeneratePDF
    {
        /// <summary>
        /// 转换成pdf
        /// </summary>
        /// <param name="model"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string ObjectToPdf(ComparisonQuotaValue model, string filePath)
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            var uploadPath = filePath + $"/" + currentDate + "/";//>>>相当于HttpContext.Current.Server.MapPath("")
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            //输出的文件名称
            var _temp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            string fileName = string.Format("{0}.pdf", $"{model.QuotaName}_" + _temp, Encoding.UTF8);
            //string newFileName = string.Format("{0}_1.pdf", $"{obj.QuotaName}_" + _temp, Encoding.UTF8);
            string pdfPath = uploadPath + "\\" + fileName;//自定义生成的pdf名
            string newFilePath = "";

            try
            {
                #region 定义字体样式

                BaseFont bfChinese = BaseFont.CreateFont($"C://WINDOWS//fonts//simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font fontChinese_12 = new Font(bfChinese, 18, Font.BOLD, new BaseColor(0, 0, 0));
                Font fontChinese_11 = new Font(bfChinese, 14, Font.BOLD, new BaseColor(0, 0, 0));
                Font fontChinese_10 = new Font(bfChinese, 10, Font.NORMAL, new BaseColor(248, 248, 255));
                Font fontChinese_bold = new Font(bfChinese, 8, Font.BOLD, new BaseColor(0, 0, 0));
                Font fontChinese_8 = new Font(bfChinese, 8, Font.NORMAL, new BaseColor(0, 0, 0));
                Font fontChinese = new Font(bfChinese, 7, Font.NORMAL, new BaseColor(0, 0, 0));
                //黑体
                BaseFont bf_ht = BaseFont.CreateFont("C://WINDOWS//Fonts//simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font ht_7 = new Font(bf_ht, 7, Font.NORMAL, new BaseColor(0, 0, 0));

                #endregion 定义字体样式

                //行高
                float cellHeight = 20;
                //初始化一个目标文档类
                Document document = new Document(PageSize.A4, 5f, 5f, 30f, 0f);
                //调用PDF的写入方法流
                //注意FileMode-Create表示如果目标文件不存在，则创建，如果已存在，则覆盖。
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

                // 页眉页脚
                string space1 = " ".PadLeft(19, ' ');
                string space2 = " ".PadLeft(120, ' ');

                Chunk chunk1 = new Chunk(space1 + "中国XX企业对标提升数据报表\r\n\r\n", fontChinese_12);   //页眉主题
                Chunk chunk2 = new Chunk(space2 + "日期: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "\r\n", fontChinese_8);      //导出时间
                Phrase ph = new Phrase(9)
                    {
                        chunk1,
                        chunk2
                    };
                HeaderFooter header = new HeaderFooter(ph, false)
                {
                    Border = Rectangle.NO_BORDER
                };
                document.Header = header;
                //页码
                HeaderFooter footer = new HeaderFooter(new Phrase(4, "页码: ", fontChinese_8), true)
                {
                    Border = Rectangle.NO_BORDER,
                    Alignment = Element.ALIGN_CENTER,
                    Bottom = 20
                };
                document.Footer = footer;

                //打开目标文档对象
                document.Open();
                // 创建第一页（如果只有一页的话，这一步可以省略）
                document.NewPage();

                PdfPTable table = new PdfPTable(1)
                {
                    TotalWidth = 550,
                    LockedWidth = true
                };

                table.SetWidths(new int[] { 550 });
                PdfPCell cell;
                //自定义title
                cell = new PdfPCell(new Phrase($"{model.QuotaName}({model.Stage})", fontChinese_11))
                {
                    Colspan = 1,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = 50
                };
                table.AddCell(cell);
                document.Add(table);

                //自定义创建第一行表头
                table = new PdfPTable(model.Attributes.Count + 2)
                {
                    TotalWidth = 550,
                    LockedWidth = true
                };

                cell = new PdfPCell(new Phrase("单位名称", fontChinese_10))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.DarkGray,
                    FixedHeight = cellHeight
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("排名", fontChinese_10))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.DarkGray,
                    FixedHeight = cellHeight
                };
                table.AddCell(cell);

                model.Attributes.ForEach(x =>
                {
                    cell = new PdfPCell(new Phrase(x.AttributeName, fontChinese_10))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        BackgroundColor = BaseColor.DarkGray,
                        FixedHeight = cellHeight
                    };
                    table.AddCell(cell);
                });

                //数据渲染
                model.EnterpriseComparisonQuotaValue.ForEach(x =>
                {
                    cell = new PdfPCell(new Phrase(x.OrgName, fontChinese_8))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        FixedHeight = cellHeight
                    };
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(x.Rank, fontChinese_8))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        FixedHeight = cellHeight
                    };
                    table.AddCell(cell);
                    x.Values.ToList().ForEach(m =>
                    {
                        cell = new PdfPCell(new Phrase(m.Value.ToString(), fontChinese_8))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            FixedHeight = cellHeight
                        };
                        table.AddCell(cell);
                    });
                });

                document.Add(table);
                //关闭目标文件
                document.Close();
                //关闭写入流
                pdfWriter.Close();

                //添加水印
                AddWatermark(pdfPath, "机密文件，请勿泄露", out newFilePath);
            }
            catch (Exception)
            {
                throw;
            }

            return newFilePath;
        }

        /// <summary>
        /// 添加普通偏转角度文字水印
        /// </summary>
        public static void AddWatermark(string inputFilePath, string text, out string outFilePath)
        {
            Console.WriteLine(inputFilePath);
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            string tempPath = Path.GetDirectoryName(inputFilePath) + "\\" + Path.GetFileNameWithoutExtension(inputFilePath) + "_temp.pdf";
            outFilePath = Path.GetDirectoryName(inputFilePath) + "\\" + Path.GetFileNameWithoutExtension(inputFilePath) + "_watermark.pdf";
            File.Copy(inputFilePath, tempPath);
            try
            {
                pdfReader = new PdfReader(tempPath);
                pdfStamper = new PdfStamper(pdfReader, new FileStream(outFilePath, FileMode.Create));
                int total = pdfReader.NumberOfPages + 1;
                Rectangle psize = pdfReader.GetPageSize(1);
                //float width = psize.Width;
                //float height = psize.Height;
                PdfContentByte content;
                BaseFont font = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\SIMFANG.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                PdfGState gs = new PdfGState();
                for (int i = 1; i < total; i++)
                {
                    //content = pdfStamper.GetOverContent(i);//在内容上方加水印
                    content = pdfStamper.GetUnderContent(i);//在内容下方加水印
                    gs.FillOpacity = 0.3f;   //透明度
                    content.SetGState(gs);
                    //content.SetGrayFill(0.3f);
                    //开始写入文本
                    content.BeginText();
                    content.SetColorFill(BaseColor.Gray);
                    content.SetFontAndSize(font, 30);
                    content.SetTextMatrix(0, 0);

                    //重复添加水印
                    for (int j = 0; j < 5; j++)
                    {
                        content.ShowTextAligned(Element.ALIGN_CENTER, text, (50.5f + i * 250), (40.0f + j * 150), (total % 2 == 1 ? -45 : 45));
                    }
                    content.EndText();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();
                File.Delete(tempPath);
            }
        }
    }
}
```

> 方法调用

```c#
 var outPath = GeneratePDF.ObjectToPdf(comparisonQuotaValue, path);
```
