using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using XNetEx.Guids;
using XNetEx.Guids.Generators;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
var app = builder.Build();

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

app.MapGet("/", (int? count) => NewGuidCount(GuidGenerator.Version4, count));

app.MapGet("/v1", (int? count) => NewGuidCount(GuidGenerator.Version1, count));
app.MapGet("/v1r", (int? count) => NewGuidCount(GuidGenerator.Version1R, count));

app.MapGet("/v2/person", () => GuidGenerator.Version2.NewGuid(DceSecurityDomain.Person));
app.MapGet("/v2/group", () => GuidGenerator.Version2.NewGuid(DceSecurityDomain.Group));
app.MapGet("/v2/org/{siteId}", (uint siteId) =>
    GuidGenerator.Version2.NewGuid(DceSecurityDomain.Org, (int)siteId));
app.MapGet("/v2/{domain}/{localId}", (byte domain, uint localId) =>
    GuidGenerator.Version2.NewGuid((DceSecurityDomain)domain, (int)localId));

app.MapGet("/v3/{ns}/{name}", (string ns, string name) =>
    GuidGenerator.Version3.NewGuid(ParseGuidNs(ns), name));
app.MapPost("/v3/{ns}", (string ns, [FromBody] string name) =>
    GuidGenerator.Version3.NewGuid(ParseGuidNs(ns), ParseBase64(name)));

app.MapGet("/v4", (int? count) => NewGuidCount(GuidGenerator.Version4, count));

app.MapGet("/v5/{ns}/{name}", (string ns, string name) =>
    GuidGenerator.Version5.NewGuid(ParseGuidNs(ns), name));
app.MapPost("/v5/{ns}", (string ns, [FromBody] string name) =>
    GuidGenerator.Version5.NewGuid(ParseGuidNs(ns), ParseBase64(name)));

#if !UUIDREV_DISABLE
app.MapGet("/v6", (int? count) => NewGuidCount(GuidGenerator.Version6, count));
app.MapGet("/v6p", (int? count) => NewGuidCount(GuidGenerator.Version6P, count));
app.MapGet("/v6r", (int? count) => NewGuidCount(GuidGenerator.Version6R, count));

app.MapGet("/v7", (int? count) => NewGuidCount(GuidGenerator.Version7, count));

app.MapGet("/v8", (int? count) => NewGuidCount(GuidGenerator.Version8, count));
app.MapGet("/v8n/{hash}/{ns}/{name}", (string hash, string ns, string name) =>
    ParseHashName(hash).NewGuid(ParseGuidNs(ns), name));
app.MapPost("/v8n/{hash}/{ns}", (string hash, string ns, [FromBody] string name) =>
    ParseHashName(hash).NewGuid(ParseGuidNs(ns), ParseBase64(name)));
#endif

app.Run();

static object NewGuidCount(IGuidGenerator guidGen, int? count) => (count is null) ?
    (object)guidGen.NewGuid() : Enumerable.Range(0, (int)count).Select(index => guidGen.NewGuid());

static Guid ParseGuidNs(string nsNameOrId) => nsNameOrId.ToUpperInvariant() switch
{
    "DNS" => GuidNamespaces.Dns, "URL" => GuidNamespaces.Url,
    "OID" => GuidNamespaces.Oid, "X500" => GuidNamespaces.X500,
#if !UUIDREV_DISABLE
    "MAX" => Uuid.MaxValue,
#endif
    "NIL" => Guid.Empty, _ => Guid.ParseExact(nsNameOrId, "D"),
};

static byte[] ParseBase64(string base64) =>
    Convert.FromBase64String(base64.Replace('-', '+').Replace('_', '/') + new string('=', base64.Length % 4));

#if !UUIDREV_DISABLE
static INameBasedGuidGenerator ParseHashName(string hashingName)
{
    var guidGen = GuidGenerator.OfHashAlgorithm(hashingName.ToUpperInvariant());
    return (guidGen.Version == GuidVersion.Version8) ? guidGen :
        throw new ArgumentOutOfRangeException(nameof(hashingName));
}
#endif

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(Guid))]
[JsonSerializable(typeof(IEnumerable<Guid>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext { }
