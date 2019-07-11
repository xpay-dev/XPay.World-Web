﻿namespace XPW.Utilities.Enums {
     public enum Status {
          Active,
          Inactive
     }
     public enum APIRequestMethod {
          None,
          Post,
          Get
     }
     public enum CodeStatus {
          Success = 200,
          Failed = 501,
          Error = 502,
          SystemError = 503,
          Unauthorized = 504,
          Invalid = 505,
          DBError = 506,
          InvalidInput = 507
     }
}
