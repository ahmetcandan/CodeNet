﻿using ServiceStack.Text;

namespace StokTakip.Model
{
    [RuntimeSerializable]
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal TaxRate { get; set; }
    }
}
