# Moses

[![Build status](https://ci.appveyor.com/api/projects/status/7t1alxxjoy9ujtwq?svg=true)](https://ci.appveyor.com/project/ExodusSistemas/moses)

**Moses** is a framework by [Exodus Sistemas](https://github.com/ExodusSistemas) for building SaaS web services and Web APIs using ASP.NET Core.

## Installation

```bash
dotnet add package Moses
```

## Features

- **Data Access** — Generic `Manager<T, K>` / `ManagerBase<T>` base classes for data access patterns, database-provider agnostic.
- **Cryptography** — `SecurityHelper` with AES encryption, MD5 hashing, and PBKDF2 key derivation.
- **Extensions** — Parsing, formatting, validation, date utilities, enum helpers, OFX support, and more.
- **Serialization** — JSON-based property serialization via `IPropertyValueHolder`.
- **Exceptions** — Domain-specific exception hierarchy (`MosesException`, `MosesSecurityException`, `MosesRuntimeException`, etc.).
- **Networking** — HTTP and OFX client helpers.
- **Reflection** — Runtime type utilities.

## Quick Start

```csharp
using Moses.Extensions;

// Encrypt / Decrypt
string encrypted = "my-secret".Encrypt();
string decrypted = encrypted.Decrypt();

// MD5 hashing
string hash = "hello".GetMd5Hex();

// Validation
bool valid = "test@email.com".IsValidEmail();
```

### Custom Data Manager

```csharp
using Moses.Data;

public class MyManager : Manager<MyEntity, MyDbContext>
{
    public override IQueryable<MyEntity> GetAll() => Context.Set<MyEntity>();
    public override MyEntity Get(int id) => Context.Set<MyEntity>().Find(id);
    public override MyEntity Create(MyEntity item) => Context.Set<MyEntity>().Add(item).Entity;
    public override void AttachBase(MyEntity item, bool asModified) { /* ... */ }
    public override void DeleteBase(MyEntity item) => Context.Set<MyEntity>().Remove(item);
    public override void SubmitChanges() => Context.SaveChanges();
}
```

## Target Framework

- **.NET 10.0**

## License

MIT
