using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XNetEx.Guids;
using XNetEx.Guids.Generators;

static Func<int?, object> HandleCount(Func<Guid> newGuid) =>
    (int? count) => (count is null) ? (object)newGuid() : Enumerable.Range(0, (int)count).Select(index => newGuid());
const BindingFlags nsFlags = BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public;
static Guid ParseGuidNs(string ns) => (typeof(GuidNamespaces).GetField(ns, nsFlags)?.GetValue(null) as Guid?) ?? Guid.ParseExact(ns, "D");
static byte[] ParseBase64(string base64) => Convert.FromBase64String(base64.Replace('-', '+').Replace('_', '/') + new string('=', base64.Length % 4));

#if !FEATURE_DISABLE_UUIDREV
static INameBasedGuidGenerator ParseHashName(string hashingName)
{
    var guidGen = GuidGenerator.OfHashAlgorithm(hashingName.ToUpperInvariant());
    return (guidGen.Version == GuidVersion.Version8) ? guidGen : throw new ArgumentOutOfRangeException(nameof(hashingName));
}
#endif

var app = WebApplication.Create(args);

GuidGenerator.StateStorageException += (sender, e) =>
{
    var isFirstLoad = (e.OperationType == FileAccess.Read) && (e.Exception is FileNotFoundException);
    var logLevel = isFirstLoad ? LogLevel.Information : LogLevel.Warning;
    app.Logger.Log(logLevel, e.Exception, "{exception}: {message}", e.Exception.GetType(), e.Exception.Message);
};
var storageDir = Environment.GetFolderPath(
    Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create);
var storagePath = Path.Combine(storageDir, "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24.state.bin");
_ = GuidGenerator.SetStateStorageFile(storagePath);

app.MapGet("/guid/v1", HandleCount(() => GuidGenerator.Version1.NewGuid()));
app.MapGet("/guid/v1r", HandleCount(() => GuidGenerator.Version1R.NewGuid()));

app.MapGet("/guid/v2/person", () => GuidGenerator.Version2.NewGuid(DceSecurityDomain.Person));
app.MapGet("/guid/v2/group", () => GuidGenerator.Version2.NewGuid(DceSecurityDomain.Group));
app.MapGet("/guid/v2/org/{siteId}", (uint siteId) => GuidGenerator.Version2.NewGuid(DceSecurityDomain.Org, (int)siteId));
app.MapGet("/guid/v2/{domain}/{localId}", (byte domain, uint localId) => GuidGenerator.Version2.NewGuid((DceSecurityDomain)domain, (int)localId));

app.MapGet("/guid/v3/{ns}/{name}", (string ns, string name) => GuidGenerator.Version3.NewGuid(ParseGuidNs(ns), name));
app.MapPost("/guid/v3/{ns}", (string ns, [FromBody] string name) => GuidGenerator.Version3.NewGuid(ParseGuidNs(ns), ParseBase64(name)));

app.MapGet("/guid/v4", HandleCount(() => GuidGenerator.Version4.NewGuid()));

app.MapGet("/guid/v5/{ns}/{name}", (string ns, string name) => GuidGenerator.Version5.NewGuid(ParseGuidNs(ns), name));
app.MapPost("/guid/v5/{ns}", (string ns, [FromBody] string name) => GuidGenerator.Version5.NewGuid(ParseGuidNs(ns), ParseBase64(name)));

#if !FEATURE_DISABLE_UUIDREV
app.MapGet("/guid/v6", HandleCount(() => GuidGenerator.Version6.NewGuid()));
app.MapGet("/guid/v6p", HandleCount(() => GuidGenerator.Version6P.NewGuid()));
app.MapGet("/guid/v6r", HandleCount(() => GuidGenerator.Version6R.NewGuid()));

app.MapGet("/guid/v7", HandleCount(() => GuidGenerator.Version7.NewGuid()));

app.MapGet("/guid/v8", HandleCount(() => GuidGenerator.Version8.NewGuid()));
app.MapGet("/guid/v8n/{hash}/{ns}/{name}", (string hash, string ns, string name) => ParseHashName(hash).NewGuid(ParseGuidNs(ns), name));
app.MapPost("/guid/v8n/{hash}/{ns}", (string hash, string ns, [FromBody] string name) => ParseHashName(hash).NewGuid(ParseGuidNs(ns), ParseBase64(name)));
#endif

app.Run();
