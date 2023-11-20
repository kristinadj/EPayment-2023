# Law Publishing Agency 
This project aims to create a government registration application for accessing a law issuance agency's platform, allowing governments to choose between one-time access for a year or an annual subscription, 
with payments facilitated through a web store offering card, QR code, PayPal, and Bitcoin payment options, intended for the Systems of Electronic Business course at the Faculty of Technical Sciences, University of Novi Sad.

### Architecture diagram
![alt text](https://github.com/kristinadj/EPayment-2023/blob/main/Arhitektura.png)

## PSP.WebApi

### Description
This API enables manipulation of invoices and management of payment methods.

### Routes

#### /api/Invoice/{paymentMethodId}

- **POST**: Create a new invoice for a specific payment method.
  - Parameters:
    - `paymentMethodId` (integer) - ID of the payment method
  - Request: PspInvoiceIDTO
  - Response: InvoiceODTO

#### /api/Invoice/{invoiceId}/Success

- **PUT**: Mark the invoice with `invoiceId` as successfully paid.
  - Parameters:
    - `invoiceId` (integer) - ID of the invoice
  - Response: RedirectUrlDTO

#### /api/Invoice/{invoiceId}/Failure

- **PUT**: Mark the invoice with `invoiceId` as unsuccessfully paid.
  - Parameters:
    - `invoiceId` (integer) - ID of the invoice
  - Response: RedirectUrlDTO

#### /api/Invoice/{invoiceId}/Error

- **PUT**: Mark the invoice with `invoiceId` as an error during payment.
  - Parameters:
    - `invoiceId` (integer) - ID of the invoice
  - Response: RedirectUrlDTO

#### /api/Merchant

- **POST**: Create a new merchant.
  - Request: MerchantIDTO
  - Response: PaymentMethodODTO

#### /api/PaymentMethod

- **GET**: Get all available payment methods.

- **POST**: Add a new payment method.
  - Request: PaymentMethodIDTO
  - Response: PaymentMethodODTO
