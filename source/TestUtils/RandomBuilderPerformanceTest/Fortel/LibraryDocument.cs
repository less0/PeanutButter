﻿using System;

namespace RandomBuilderPerformanceTest.Fortel
{
    public class LibraryDocument : EntityBase
    {
        public string FileId { get; set; }
        public DateTime? DateGenerated { get; set; }
        public string GeneratedBy { get; set; }
        public string DocumentType { get; set; }

        public LibraryDocumentDetails Details { get; set; }
    }
}