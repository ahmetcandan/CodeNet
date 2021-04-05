﻿using System;

#nullable disable

namespace StokTakip.EntityFramework.Models
{
    public partial class CampaignUsedHistory
    {
        public int CampaignId { get; set; }
        public int SalesOrderId { get; set; }
        public DateTime UsedDate { get; set; }
        public bool IsCancel { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
