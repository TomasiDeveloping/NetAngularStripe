# Stripe Subscription Demo App

This is a small demo application demonstrating how to offer subscriptions via Stripe and allow customers to subscribe to them.

## Features

- Display available subscription plans
- Select and pay for a subscription using Stripe

## Technology Stack

- **Backend**: .NET 8
- **Frontend**: Angular 17

## Installation

### Backend

1. Clone the repository: `git clone https://github.com/TomasiDeveloping/NetAngularStripe.git`
2. Navigate to the backend directory: `cd backend`
3. Install dependencies: `dotnet restore`
4. Configure your Stripe API keys in the `appsettings.json` file
5. Start the server: `dotnet run`

### Frontend

1. Navigate to the frontend directory: `cd frontend`
2. Install dependencies: `npm install`
3. Start the application: `ng serve`

## Configuration

To use this app with your own Stripe integration, you need to customize the Stripe API keys and other configurations. Follow the instructions in the respective configuration files (`appsettings.json` for the backend.

## Contribution

This demo application is open to contributions. If you find an issue or would like to propose an improvement, please open an issue or submit a pull request.

## Subscription Process Guide

1. **Starting the Application**:
   - Ensure both the backend and frontend are up and running as per the previous instructions.

2. **Displaying Available Subscriptions**:
   - Upon application startup, a company and two subscriptions are automatically created.
   - If the company lacks a StripeCustomerId, the two subscriptions are displayed in the frontend.

3. **Selecting a Subscription**:
   - Choose the desired subscription by clicking on the respective button.

4. **Initiating Payment**:
   - Upon selecting the subscription, the PriceId is sent to the backend.
   - The backend creates a session ID for payment via Stripe and stores it along with the license ID and customer ID in the database.
   - Success and cancel URLs are also sent to the backend, containing the corresponding session ID as a parameter.

5. **Redirecting to Payment Page**:
   - The frontend receives the session ID and Stripe public key from the backend.
   - Using this information, the payment page is opened via the Stripe.js package, redirecting the user to enter payment details.

6. **Payment Confirmation**:
   - Upon successful payment, Stripe redirects the user back to the backend according to the success URL.
   - The backend retrieves the corresponding session ID from the database and updates the entry with the received StripeCustomerId.
   - The company receives the license ID of the paid subscription and the StripeCustomerId.

7. **Payment Cancellation**:
   - If the payment is canceled, Stripe redirects the user back to the backend according to the cancel URL.
   - The backend retrieves the corresponding session ID from the database and updates the entry to mark the payment without creating a license.

8. **Managing the Subscription**:
   - Upon successful payment and receipt of the StripeCustomerId, a button to manage the subscription is displayed to the user in the frontend.
   - Clicking this button sends the StripeCustomerId to the backend from the frontend.
   - The backend generates a URL for the Stripe customer portal, which is sent to the frontend.
   - The frontend redirects the user to the provided URL, allowing them to manage their subscription.

9. **Webhook for Subscription Changes**:
   - Subscription changes are captured via a webhook to appropriately update the database.


