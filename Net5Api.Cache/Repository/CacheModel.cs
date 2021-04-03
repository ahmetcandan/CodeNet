﻿using Net5Api.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.Cache.Repository
{
    public class CacheModel : BaseMongoModel
    {
        public CacheModel(string key, object value, int time)
        {
            ExpiryDate = DateTime.Now.AddSeconds(time);
            Id = CreateObjectId(key);
            Value = value;
        }

        public DateTime ExpiryDate { get; set; }

        public object Value { get; set; }
    }
}
