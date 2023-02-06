using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using XNetEx.Guids;
using XNetEx.Guids.Generators;

static Func<int?, object> HandleCount(Func<Guid> newGuid) =>
    (int? count) => (count is null) ? (object)newGuid() : Enumerable.Range(0, (int)count).Select(index => newGuid()).ToArray();
const BindingFlags nsFlags = BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public;
static Guid ParseGuidNs(string ns) => (typeof(GuidNamespaces).GetField(ns, nsFlags)?.GetValue(null) as Guid?) ?? Guid.ParseExact(ns, "D");
static byte[] ParseBase64(string base64) => Convert.FromBase64String(base64.Replace('-', '+').Replace('_', '/') + new string('=', base64.Length % 4));

var app = WebApplication.Create(args);

app.MapGet("/guid/v1", HandleCount(() => GuidGenerator.Version1.NewGuid()));
app.MapGet("/guid/v1r", HandleCount(() => GuidGenerator.Version1R.NewGuid()));

app.MapGet("/guid/v2/person", () => GuidGenerator.Version2.NewGuid(DceSecurityDomain.Person));
app.MapGet("/guid/v2/group", () => GuidGenerator.Version2.NewGuid(DceSecurityDomain.Group));
app.MapGet("/guid/v2/org/{siteId}", (int siteId) => GuidGenerator.Version2.NewGuid(DceSecurityDomain.Org, siteId));

app.MapGet("/guid/v3/{ns}/{name}", (string ns, string name) => GuidGenerator.Version3.NewGuid(ParseGuidNs(ns), name));
app.MapPost("/guid/v3/{ns}", (string ns, [FromBody] string name) => GuidGenerator.Version3.NewGuid(ParseGuidNs(ns), ParseBase64(name)));

app.MapGet("/guid/v4", HandleCount(() => GuidGenerator.Version4.NewGuid()));

app.MapGet("/guid/v5/{ns}/{name}", (string ns, string name) => GuidGenerator.Version5.NewGuid(ParseGuidNs(ns), name));
app.MapPost("/guid/v5/{ns}", (string ns, [FromBody] string name) => GuidGenerator.Version5.NewGuid(ParseGuidNs(ns), ParseBase64(name)));

app.MapGet("/guid/v6", HandleCount(() => GuidGenerator.Version6.NewGuid()));
app.MapGet("/guid/v6p", HandleCount(() => GuidGenerator.Version6P.NewGuid()));

app.MapGet("/guid/v7", HandleCount(() => GuidGenerator.Version7.NewGuid()));

app.MapGet("/guid/v8", HandleCount(() => GuidGenerator.Version8.NewGuid()));

app.Run();
