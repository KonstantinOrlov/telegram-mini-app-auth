## Telegram Mini App Auth

[Telegram Mini Apps Official Documentation](https://core.telegram.org/bots/webapps#validating-data-received-via-the-mini-app)


## Install
Install Telegram.MiniApp.Authentication using NuGet Package Manager:
```bash
Install-Package Telegram.MiniApp.Authentication
```
Or via the .NET CLI:
```bash
dotnet add package Telegram.MiniApp.Authentication
```
---
## Usage

### Backend

- Add Authentication:
```csharp
builder.Services.AddTelegramMiniAppInHeader(options =>
{
	options.BotToken = builder.Configuration.GetValue<string>("BotToken")!;
});
```

- Add Authorization:
```csharp
  // Library provides default policy:
  builder.Services.AddAuthorization(options =>
  {
      // Simple TMA Policy. Only checks tma_user_id claims
	  options.AddDefaultTmaPolicy();
	  
	  // For users with Telegram Premium. Checks tma_user_id and tma_is_premium claims
      options.AddTmaPremiumPolicy();
  });


  // Or you can customize the policy the way you want:
  builder.Services.AddAuthorization(options =>
  {
      options.AddPolicy("MyPolicy", policy =>
      {
          policy.AddAuthenticationSchemes(TmaDefaults.AuthenticationScheme);
          // your settings
      });
  });
```

After successful authorization, records the following data in the Claims(if not null):
- **tma_user_id** - A unique identifier for the telegram user.
- **tma_username** - Username of the telegram user.
- **tma_first_name** - First name of the telegram user.
- **tma_last_name** - Last name of the telegram user.
- **tma_chat_instance** -  Global identifier, uniquely corresponding to the chat.
- **tma_is_premium** - user is a Telegram Premium user or not.

You can get claims in the usual way, or use extension methods provided by the library:
```csharp

// Get TMA claims as TmaUserPrincipals
if (HttpContext.User.TryGetTmaUser(out var tmaUser))
{
    //Using
}

// Get all initData as TmaInitData
if (HttpContext.TryGetTmaInitData(out var initData))
{
    //Using
}
```

### Frontend

Set **window.Telegram.WebApp.initData** with prefix 'Tma ' in **Authorization Header** (Or another header, what you set in TmaAuthenticationOptions.TokenHeaderName)

example:
```javascript
        fetch('endpoint-url', {
            headers: {
                'Authorization': `TMA ${window.Telegram.WebApp.initData}`
            }
        })
```
---
## Solution

- **Telegram.MiniApp.Authentication** - Nuget package
- **tests/TestWebApi** - .NET Web Api app for auth testing
