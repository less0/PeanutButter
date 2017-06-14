﻿using System;
using System.Web.Mvc;

namespace PeanutButter.RandomGenerators.Tests.PerformanceTest
{
    public class LibraryDocumentSearchViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string DocumentType { get; set; }
        public string GeneratedBy { get; set; }
        public SelectList GeneratedBySelectList { get; set; }
        public SelectList DocumentTypeSelectList { get; set; }
    }
}