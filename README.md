# AzureInsuranceClaimHandlingApp
Multi-tier asp .net core web application for Insurance Claim Handling. The use case is to maintain a list of claims, the user should be able to create/update and delete claims. The application is hosted as an App service in Azure.

Prerequisites
-Azure Subscription (for this example free subscription is being used)
-Visual Studio 
-Javascript

API
App service for Azure written in .net core. This is a REST API, the resource is a Claim. A claim has the following properties:
• Id (int)
• Year (int)
• Name (string)
• Type (string/enum, possible values: Collision, Grounding, Bad Weather, Fire).
• DamageCost (decimal)

The API supports GET, POST, and DELETE.

Business rules:
• Claim with DamageCost exceeding 100.000 cannot be created.
• Validate year, it can’t be in the future and more than 10 years back.
The API returns the appropriate http code and error message when validation fail or some other error occurs.

Storage
The storage medium for a Claim entity is Cosmos.

Audit

Every change to the list of Claims are audited.ClaimAudit table is created for this purpose. A message is create with the claim id, timestamp and operation/http request method type information and pushed to the Azure Servis Bus by subscribing to the topic.
