# Law Publishing Agency 
This project aims to create a government registration application for accessing a law issuance agency's platform, allowing governments to choose between one-time access for a year or an annual subscription, 
with payments facilitated through a web store offering card, QR code, PayPal, and Bitcoin payment options, intended for the Systems of Electronic Business course at the Faculty of Technical Sciences, University of Novi Sad.

### Architecture diagram
![alt text](https://github.com/kristinadj/EPayment-2023/blob/main/Arhitektura.png)

## Payment Service Provider - PSP.WebApi

### Description
This API enables manipulation of invoices and management of payment methods.

### Routes

#### /api/Invoice/{paymentMethodId}

- **POST**: Create a new invoice for a specific payment method.
  - Parameters:
    - `paymentMethodId` (integer) - ID of the payment method
  - Request: PspInvoiceIDTO
    - ```json
      {
          "externalInvoiceId": 123456,
          "merchantId": 789,
          "issuedToUserId": "user123",
          "totalPrice": 99.99,
          "currencyCode": "USD",
          "timestamp": "2023-11-20T12:00:00Z",
          "redirectUrl": "https://example.com/invoice/123456"
      }  
  - Response: InvoiceODTO
    - ```json
      {
        "externalInvoiceId": 123456,
        "merchantId": 789,
        "issuedToUserId": "user123",
        "totalPrice": 99.99,
        "currencyCode": "USD",
        "redirectUrl": "https://example.com/invoice/123456"
      }  

#### /api/Invoice/{invoiceId}/Success

- **PUT**: Mark the invoice with `invoiceId` as successfully paid.
  - Parameters:
    - `invoiceId` (integer) - ID of the invoice
  - Response: RedirectUrlDTO
    - ```json
      {
        "redirectUrl": "https://example.com/success"
      }

#### /api/Invoice/{invoiceId}/Failure

- **PUT**: Mark the invoice with `invoiceId` as unsuccessfully paid.
  - Parameters:
    - `invoiceId` (integer) - ID of the invoice
  - Response: RedirectUrlDTO
     - ```json
       {
         "redirectUrl": "https://example.com/failure"
       }

#### /api/Invoice/{invoiceId}/Error

- **PUT**: Mark the invoice with `invoiceId` as an error during payment.
  - Parameters:
    - `invoiceId` (integer) - ID of the invoice
  - Response: RedirectUrlDTO
    -   ```json
        {
          "redirectUrl": "https://example.com/error"
        }     

#### /api/Merchant

- **POST**: Create a new merchant.
  - Request: MerchantIDTO
    -   ```json
        {
            "merchantExternalId": "123456789",
            "name": "Merchant1",
            "address": "123 Main St",
            "phoneNumber": "123-456-7890",
            "email": "merchant@example.com",
            "serviceName": "ServiceName1",
            "transactionSuccessUrl": "https://example.com/success",
            "transactionFailureUrl": "https://example.com/failure",
            "transactionErrorUrl": "https://example.com/error"
        }
        

#### /api/PaymentMethod

- **GET**: Get all available payment methods.

- **POST**: Add a new payment method.
  - Request: PaymentMethodIDTO
    -   ```json
        {
            "name": "Bank Payment Service",
            "serviceName": "bank-payment-service",
            "serviceApiSufix": "api/bank"
        }
  - Response: PaymentMethodODTO
    -   ```json
        {
          "paymentMethodId": 1,
          "name": "Bank Payment Service",
          "serviceName": "bank-payment-service",
          "serviceApiSufix": "api/bank"
        }
