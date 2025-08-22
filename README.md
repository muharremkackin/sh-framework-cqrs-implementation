# SH.Framework.Library.Cqrs.Implementation

[![NuGet Version](https://img.shields.io/nuget/v/SH.Framework.Library.Cqrs.Implementation.svg)](https://www.nuget.org/packages/SH.Framework.Library.Cqrs.Implementation/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SH.Framework.Library.Cqrs.Implementation.svg)](https://www.nuget.org/packages/SH.Framework.Library.Cqrs.Implementation/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A comprehensive implementation layer for the [SH.Framework.Library.Cqrs](https://www.nuget.org/packages/SH.Framework.Library.Cqrs/) package, providing abstract base classes and Result pattern implementation for CQRS operations. This package extends the core CQRS framework with practical base classes that simplify implementation and promote consistent patterns across your application.

## 🚀 Features

- **📋 Abstract Base Classes**: Ready-to-use base classes for requests, handlers, and behaviors
- **✅ Result Pattern**: Standardized Result<T> pattern for consistent error handling and response management
- **🆔 Automatic ID Generation**: Built-in request and notification ID generation
- **🛡️ Type Safety**: Strongly-typed base classes that enforce proper CQRS patterns
- **🔧 Easy Implementation**: Reduces boilerplate code and accelerates development
- **📊 Structured Error Handling**: Comprehensive error tracking with codes, descriptions, and nested error details
- **🎯 Clean Architecture**: Promotes separation of concerns and maintainable code structure

## 📦 Installation

```bash
dotnet add package SH.Framework.Library.Cqrs.Implementation
```

> **Note**: This package automatically includes `SH.Framework.Library.Cqrs` as a dependency.

## 🛠️ Quick Start

### 1. Register Services

```csharp
using SH.Framework.Library.Cqrs;

var builder = WebApplication.CreateBuilder(args);

// Register CQRS library (this also registers the core framework)
builder.Services.AddCqrsLibraryConfiguration(
    Assembly.GetExecutingAssembly()
);

var app = builder.Build();
```

### 2. Create Requests Using Base Classes

**Command with Result Pattern:**
```csharp
using SH.Framework.Library.Cqrs.Implementation;

public class CreateUserRequest : Request<UserDto>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public record UserDto(int Id, string Name, string Email);
```

**Command without Response Data:**
```csharp
public class DeleteUserRequest : Request
{
    public int UserId { get; set; }
}
```

### 3. Implement Request Handlers

**Handler with Response Data:**
```csharp
public class CreateUserRequestHandler : RequestHandler<CreateUserRequest, UserDto>
{
    public override async Task<Result<UserDto>> HandleAsync(
        CreateUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate input
            if (string.IsNullOrEmpty(request.Name))
            {
                return Result.Failure<UserDto>(
                    ResultCode.Failure,
                    new Dictionary<string, Dictionary<string, string>>
                    {
                        ["Name"] = new() { ["Required"] = "Name is required" }
                    },
                    request.RequestId
                );
            }

            // Business logic
            var user = await CreateUserAsync(request.Name, request.Email);
            var userDto = new UserDto(user.Id, user.Name, user.Email);

            return Result.Success(userDto, request.RequestId);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserDto>(
                ResultCode.Exception,
                new Dictionary<string, Dictionary<string, string>>
                {
                    ["Exception"] = new() { ["Message"] = ex.Message }
                },
                request.RequestId
            );
        }
    }
}
```

**Handler without Response Data:**
```csharp
public class DeleteUserRequestHandler : RequestHandler<DeleteUserRequest>
{
    public override async Task<Result> HandleAsync(
        DeleteUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await DeleteUserAsync(request.UserId);
            return Result.Success(request.RequestId);
        }
        catch (Exception ex)
        {
            return Result.Failure(
                ResultCode.Exception, 
                new Dictionary<string, Dictionary<string, string>>
                {
                    ["Exception"] = new() { ["Message"] = ex.Message }
                },
                request.RequestId
            );
        }
    }
}
```

### 4. Create Notifications

```csharp
public class UserCreatedNotification : Notification
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

### 5. Implement Notification Handlers

```csharp
public class UserCreatedEmailNotificationHandler : NotificationBehavior<UserCreatedNotification>
{
    public override async Task HandleAsync(
        UserCreatedNotification notification, 
        CancellationToken cancellationToken = default)
    {
        // Send welcome email
        await SendWelcomeEmailAsync(notification.Email);
    }
}
```

### 6. Use in Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IProjector _projector;

    public UsersController(IProjector projector)
    {
        _projector = projector;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(
        CreateUserRequest request, 
        CancellationToken cancellationToken)
    {
        var result = await _projector.SendAsync(request, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(new
            {
                result.Code,
                result.Description,
                result.Errors,
                result.RequestId
            });
        }

        return Ok(new
        {
            result.Data,
            result.RequestId
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(
        int id, 
        CancellationToken cancellationToken)
    {
        var request = new DeleteUserRequest { UserId = id };
        var result = await _projector.SendAsync(request, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(new
            {
                result.Code,
                result.Description,
                result.Errors,
                result.RequestId
            });
        }

        return NoContent();
    }
}
```

## 🏗️ Architecture Components

### Result Pattern

The Result pattern provides a standardized way to handle success and failure scenarios:

```csharp
public class Result<TResponse> : Result
{
    public virtual TResponse? Data { get; set; }
}

public class Result
{
    public virtual int Code { get; init; }
    public virtual string? Description { get; init; }
    public virtual bool IsSuccess => Code == 0;
    public virtual bool IsFailure => !IsSuccess;
    public virtual Dictionary<string, Dictionary<string, string>>? Errors { get; init; }
    public Guid? RequestId { get; init; }
}
```

### Result Codes

Built-in result codes for common scenarios:

```csharp
public static class ResultCode
{
    public static ResultCode Success => Instance(0, "Success");
    public static ResultCode Exception => Instance(1, "Exception");
    public static ResultCode Failure => Instance(2, "Failure");
    
    public static ResultCode Instance(int code, string? description) => new(code, description);
}
```

### Base Classes

**Request Base Classes:**
```csharp
// For requests that return data
public abstract class Request<TResponse> : IRequest<Result<TResponse>>, IHasRequestId

// For requests without return data
public abstract class Request : IRequest<Result>, IHasRequestId
```

**Handler Base Classes:**
```csharp
// For handlers that return data
public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>

// For handlers without return data
public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest, Result>
```

**Notification Base Classes:**
```csharp
public abstract class Notification : INotification, IHasNotificationId

public abstract class NotificationBehavior<TNotification> : INotificationHandler<TNotification>
```

**Behavior Base Classes:**
```csharp
public abstract class RequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IHasRequestId, IRequest<TResponse> 
    where TResponse : Result, new()
```

## 🔧 Advanced Usage

### Custom Result Codes

```csharp
public static class CustomResultCodes
{
    public static ResultCode ValidationError => ResultCode.Instance(100, "Validation Error");
    public static ResultCode NotFound => ResultCode.Instance(404, "Not Found");
    public static ResultCode Unauthorized => ResultCode.Instance(401, "Unauthorized");
}

// Usage in handler
return Result.Failure<UserDto>(
    CustomResultCodes.NotFound,
    new Dictionary<string, Dictionary<string, string>>
    {
        ["User"] = new() { ["NotFound"] = $"User with ID {request.UserId} not found" }
    },
    request.RequestId
);
```

### Custom Pipeline Behavior

```csharp
public class ValidationBehavior<TRequest, TResponse> : RequestBehavior<TRequest, TResponse>
    where TRequest : IHasRequestId, IRequest<TResponse>
    where TResponse : Result, new()
{
    public override async Task<TResponse> HandleAsync(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken = default)
    {
        // Pre-processing validation logic
        var validationResult = await ValidateRequestAsync(request);
        if (validationResult.Any())
        {
            var result = new TResponse();
            // Set validation errors
            return result;
        }

        // Continue to next behavior/handler
        var response = await next(cancellationToken);
        
        // Post-processing logic
        return response;
    }
}
```

### Error Handling Patterns

```csharp
// Structured error handling
var errors = new Dictionary<string, Dictionary<string, string>>
{
    ["Email"] = new()
    {
        ["Required"] = "Email is required",
        ["Format"] = "Email format is invalid"
    },
    ["Password"] = new()
    {
        ["MinLength"] = "Password must be at least 8 characters",
        ["Complexity"] = "Password must contain special characters"
    }
};

return Result.Failure<UserDto>(ResultCode.Failure, errors, request.RequestId);
```

## 📊 Response Patterns

### Success Response
```json
{
    "data": {
        "id": 1,
        "name": "John Doe",
        "email": "john@example.com"
    },
    "code": 0,
    "description": "Success",
    "isSuccess": true,
    "requestId": "123e4567-e89b-12d3-a456-426614174000"
}
```

### Error Response
```json
{
    "code": 2,
    "description": "Validation Error",
    "isSuccess": false,
    "errors": {
        "Email": {
            "Required": "Email is required",
            "Format": "Email format is invalid"
        }
    },
    "requestId": "123e4567-e89b-12d3-a456-426614174000"
}
```

## 🎯 Best Practices

1. **Always use the Result pattern** for consistent error handling
2. **Include RequestId** in all responses for tracking and debugging
3. **Structure your errors** using the nested dictionary pattern
4. **Use meaningful result codes** and descriptions
5. **Handle exceptions** gracefully and return appropriate error results
6. **Leverage base classes** to reduce boilerplate code
7. **Follow CQRS principles** - separate commands from queries

## 🔗 Dependencies

- **SH.Framework.Library.Cqrs**: Core CQRS framework (automatically included)
- **.NET 9.0**: Target framework

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🏢 Company

**Strawhats Company**  
Created by Muharrem Kaçkın

---

⭐ If you find this library helpful, please consider giving it a star on GitHub!