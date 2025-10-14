# Card Actions Microservice
## Description

A C# (.NET 8) microservice providing an API to generate allowed actions for user cards.
The service supports different card types (Prepaid, Debit, Credit) and card statuses (Ordered, Inactive, Active, Restricted, Blocked, Expired, Closed) and also considers whether a PIN has been set.

## Features

API Endpoint:
```html
GET /api/CardActions/{UserId}/{CardNumber}/allowed
```

Returns a JSON list of allowed actions for the given card.

Action logic is based on configurable rules (CardActionsRules), making it easy to extend with new actions.

Handles edge cases: non-existent user, non-existent card, different card statuses, and cards without a PIN.

## Project Structure

CardService – responsible for generating example data.

AllowedCardActionsService – contains the logic to determine allowed actions based on CardActionsRules.

CardActionsController – API controller that handles requests and returns JSON responses.

## Testing

Comprehensive unit tests using xUnit for all combinations of card types, statuses, and PIN settings.

Each action (ACTION1–ACTION13) is tested to ensure correct behavior.

## Extensibility

Adding a new action is as simple as adding a new rule in CardActionsRules.

New card types or statuses can be supported by updating the conditions in the rules without modifying core logic.

## Getting Started

Clone the repository:
1. git clone https://github.com/DanielJanus/madiff-microservice.git
2. Build the project using .NET 8.
3. Run the service and call the API endpoint to retrieve allowed card actions.
