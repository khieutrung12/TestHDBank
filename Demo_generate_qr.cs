  [HttpGet]
  [Route("generete-qr")]
  public IActionResult GenerateSign()
  {
      var transactionRequest = new
      {
          merchantId = "1001074687",
          invoiceId = "BN-0011",
          type = "vietqr",
          transactionAmount = 10000,
          additionalData = new
          {
              serviceCode = "KDAUBUNG",
              patientCode = "TEST123",
              patientName = "Võ Duy Cương",
              description = "Mô tả"
          },
          transactionTime = 20230929103111
      };

      string jsonString = JsonConvert.SerializeObject(transactionRequest, Formatting.None);

      string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));

      string secretKeyHex = "81C8BEF5B43D88408CFD5401ED38E45FF4C682946AE227ACF04E014492B618AF";
      byte[] secretKeyBytes = Convert.FromHexString(secretKeyHex);

      byte[] base64Bytes = Encoding.UTF8.GetBytes(base64Data);

      string hmacBase64;
      string hmacHex;

      using (var hmac = new HMACSHA256(secretKeyBytes))
      {
          byte[] hashBytes = hmac.ComputeHash(base64Bytes);

          hmacHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
      }

      return Ok(new
      {
          data = base64Data,
          sign = hmacHex
      });

      //"data": "eyJtZXJjaGFudElkIjoiMTAwMTA3NDY4NyIsImludm9pY2VJZCI6IkJOLTAwMTEiLCJ0eXBlIjoidmlldHFyIiwidHJhbnNhY3Rpb25BbW91bnQiOjEwMDAwLCJhZGRpdGlvbmFsRGF0YSI6eyJzZXJ2aWNlQ29kZSI6IktEQVVCVU5HIiwicGF0aWVudENvZGUiOiJURVNUMTIzIiwicGF0aWVudE5hbWUiOiJWw7UgRHV5IEPGsMahbmciLCJkZXNjcmlwdGlvbiI6Ik3DtCB04bqjIn0sInRyYW5zYWN0aW9uVGltZSI6MjAyMzA5MjkxMDMxMTF9",
      //"sign": "0376f56fc5ae9a38c0552b4763d41d14dac0f1929c2dc1e050fc2148710312fe"
  }
