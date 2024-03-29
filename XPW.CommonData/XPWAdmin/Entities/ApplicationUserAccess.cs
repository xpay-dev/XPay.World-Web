﻿using System;
using XPW.Utilities.BaseContext;
using XPW.Utilities.Enums;

namespace XPW.CommonData.XPWAdmin.Entities {
     public class ApplicationUserAccess : BaseModelInt {
          public Guid AccountId { get; set; }
          public string Username { get; set; }
          public int ApplicationAccessId { get; set; }
          public string ApplicationAccessName { get; set; }
          public Status Tag { get; set; }
     }
}