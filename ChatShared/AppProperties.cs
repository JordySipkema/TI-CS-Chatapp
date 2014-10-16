﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatShared
{
    public class AppProperties
    {
        public static AppProperties _instance = null;

        public int PortNumber { get; private set; }

        private AppProperties()
        {


        }

        public static AppProperties GetInstance()
        {
            return _instance ?? (_instance = new AppProperties());

        }


    }
}
