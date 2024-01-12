using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Net.Http;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Layout.Borders;
using iText.Kernel.Colors;

namespace PL.Controllers
{
    public class VentaController : Controller
    {
        // GET: Venta
        
        [HttpPost]
        public ActionResult CompraVenta(ML.Venta venta,float Total)
        {
            venta.Total = Total;
            venta.Usuario=new ML.Usuario();
            venta.ListaProductos = new List<object>();
            venta.Usuario.IdUsuario = (int)Session["IdUsuario"];
            var ListaProductos = Session["ListaProductos"];
            var carritoSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(ListaProductos.ToString());

            foreach (var itemProducto in carritoSession)
            {
                ML.Producto producto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(itemProducto.ToString());
                venta.ListaProductos.Add(producto);
            }
            ML.Result resultVenta = BL.Venta.Add(venta);
            if (resultVenta.Correct)
            {
                ViewBag.Mensaje = " ";
                string rutaPDF = Path.GetTempFileName() + ".pdf";
                using (PdfDocument pdf = new PdfDocument(new PdfWriter(rutaPDF)))
                {
                    using (Document document = new Document(pdf))
                    {
                        document.Add(new Paragraph("Resumen de compra de productos")
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFontSize(18)
           .SetBold());
                        Table table = new Table(6);
                        table.SetWidth(UnitValue.CreatePercentValue(100));

                        AddHeaderCell(table, "Foto");
                        AddHeaderCell(table, "Nombre");
                        AddHeaderCell(table, "Descripción");
                        AddHeaderCell(table, "Precio");
                        AddHeaderCell(table, "Precio Venta");
                        AddHeaderCell(table, "Piezas a comprar");

                        foreach (ML.Producto productoPDF in venta.ListaProductos)
                        {
                            byte[] imageBytes = Convert.FromBase64String(productoPDF.Imagen);
                            ImageData data = ImageDataFactory.Create(imageBytes);
                            Image image = new Image(data);

                            table.AddCell(new Cell().Add(image.SetWidth(50).SetHeight(50)).SetBorder(Border.NO_BORDER));
                            table.AddCell(new Cell().Add(new Paragraph(productoPDF.Nombre)).SetBorder(Border.NO_BORDER));
                            table.AddCell(new Cell().Add(new Paragraph(productoPDF.Descripcion)).SetBorder(Border.NO_BORDER));
                            table.AddCell(new Cell().Add(new Paragraph(productoPDF.Precio.ToString("C"))).SetBorder(Border.NO_BORDER));
                            table.AddCell(new Cell().Add(new Paragraph(productoPDF.Precioventa.ToString("C"))).SetBorder(Border.NO_BORDER));
                            table.AddCell(new Cell().Add(new Paragraph(productoPDF.Piezas.ToString())).SetBorder(Border.NO_BORDER));

                        }
                        table.AddCell(new Cell(1, 5).Add(new Paragraph("Venta Total")));
                        table.AddCell(new Cell().Add(new Paragraph(venta.Total.ToString("C"))));

                        document.Add(table);
                    }
                }
                byte[] fileBytes = System.IO.File.ReadAllBytes(rutaPDF);
                System.IO.File.Delete(rutaPDF);
                Session["ListaProductos"] = null;
                return new FileStreamResult(new MemoryStream(fileBytes), "application/pdf")
                {
                    FileDownloadName = "ReporteProductos"
                };
                void AddHeaderCell(Table table, string headerText)
                {
                    table.AddHeaderCell(new Cell()
                        .Add(new Paragraph(headerText)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBold())
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                }

            }
            else
            {
                ViewBag.Mensaje = "No se pudo registrar la venta";
            }
            return PartialView("Modal");
        }
    }
}