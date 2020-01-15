#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.DocIO;
using System;
using System.IO;
using EJ2DocumentEditor = Syncfusion.EJ2.DocumentEditor;

namespace DocumentEditorCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumenteditorController : ControllerBase
    {        
        [Route("Import")]
        // Import file from client side
        public string Import(IFormCollection data)
        {
            if (data.Files.Count == 0)
                return null;
            Stream stream = new MemoryStream();
            IFormFile file = data.Files[0];
            int index = file.FileName.LastIndexOf('.');
            string type = index > -1 && index < file.FileName.Length - 1 ?
                file.FileName.Substring(index) : ".docx";
            file.CopyTo(stream);
            stream.Position = 0;

            EJ2DocumentEditor.WordDocument document = EJ2DocumentEditor.WordDocument.Load(stream, GetFormatType(type.ToLower()));
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(document);
            document.Dispose();
            return json;
        }
        
        internal static EJ2DocumentEditor.FormatType GetFormatType(string fileName)
        {
            int index = fileName.LastIndexOf('.');
            string format = index > -1 && index < fileName.Length - 1 ? fileName.Substring(index + 1) : "";

            if (string.IsNullOrEmpty(format))
                throw new NotSupportedException("EJ2 Document editor does not support this file format.");
            switch (format.ToLower())
            {
                case "dotx":
                case "docx":
                case "docm":
                case "dotm":
                    return EJ2DocumentEditor.FormatType.Docx;
                case "dot":
                case "doc":
                    return EJ2DocumentEditor.FormatType.Doc;
                case "rtf":
                    return EJ2DocumentEditor.FormatType.Rtf;
                case "txt":
                    return EJ2DocumentEditor.FormatType.Txt;
                case "xml":
                    return EJ2DocumentEditor.FormatType.WordML;
                default:
                    throw new NotSupportedException("EJ2 Document editor does not support this file format.");
            }
        }
    }
}
