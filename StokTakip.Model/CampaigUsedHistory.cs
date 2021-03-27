﻿using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;

#nullable disable

namespace StokTakip.Model
{
    public partial class CampaigUsedHistory : IEntity
    {
        public int CampaignId { get; set; }
        public int SalesOrderId { get; set; }
        public DateTime UsedDate { get; set; }
        public bool IsCancel { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }
    }
}
