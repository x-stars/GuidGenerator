#ifndef __XSTARS_GUIDGEN_NATIVELIB_H__
#define __XSTARS_GUIDGEN_NATIVELIB_H__

#ifdef _WIN32

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

#include <windows.h>
#include <guiddef.h>

#pragma comment(lib, "Xstars.GuidGen.NativeLib.lib")

#ifdef __cplusplus
extern "C" {
#endif

typedef UCHAR DCE_SECURITY_DOMAIN;
const DCE_SECURITY_DOMAIN DCE_SECURITY_DOMAIN_PERSON = 0;
const DCE_SECURITY_DOMAIN DCE_SECURITY_DOMAIN_GROUP = 1;
const DCE_SECURITY_DOMAIN DCE_SECURITY_DOMAIN_ORG = 2;

const GUID GUID_NAMESPACE_DNS = {
    0x6ba7b810, 0x9dad, 0x11d1,
    {0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8} };
const GUID GUID_NAMESPACE_URL = {
    0x6ba7b811, 0x9dad, 0x11d1,
    {0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8} };
const GUID GUID_NAMESPACE_OID = {
    0x6ba7b812, 0x9dad, 0x11d1,
    {0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8} };
const GUID GUID_NAMESPACE_X500 = {
    0x6ba7b814, 0x9dad, 0x11d1,
    {0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8} };

__declspec(dllimport)
HRESULT GuidCreateV1(_Out_ GUID *Guid);
__declspec(dllimport)
HRESULT GuidCreateV1R(_Out_ GUID *Guid);

__declspec(dllimport)
HRESULT GuidCreateV2(_Out_ GUID *Guid, DCE_SECURITY_DOMAIN Domain);
__declspec(dllimport)
HRESULT GuidCreateV2Org(_Out_ GUID *Guid, UINT32 LocalId);
__declspec(dllimport)
HRESULT GuidCreateV2Other(_Out_ GUID *Guid,
    DCE_SECURITY_DOMAIN Domain, UINT32 LocalId);

__declspec(dllimport)
HRESULT GuidCreateV3(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const UCHAR *Name, SIZE_T NameLen);

__declspec(dllimport)
HRESULT GuidCreateV4(_Out_ GUID *Guid);

__declspec(dllimport)
HRESULT GuidCreateV5(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const UCHAR *Name, SIZE_T NameLen);

#ifndef FEATURE_DISABLE_UUIDREV
__declspec(dllimport)
HRESULT GuidCreateV6(_Out_ GUID *Guid);
__declspec(dllimport)
HRESULT GuidCreateV6P(_Out_ GUID *Guid);

__declspec(dllimport)
HRESULT GuidCreateV7(_Out_ GUID *Guid);

__declspec(dllimport)
HRESULT GuidCreateV8(_Out_ GUID *Guid);
__declspec(dllimport)
HRESULT GuidCreateV8NSha256(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const UCHAR *Name, SIZE_T NameLen);
__declspec(dllimport)
HRESULT GuidCreateV8NSha384(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const UCHAR *Name, SIZE_T NameLen);
__declspec(dllimport)
HRESULT GuidCreateV8NSha512(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const UCHAR *Name, SIZE_T NameLen);
#endif

#ifdef __cplusplus
}
#endif

#endif /* _WIN32 */

#endif /* __XSTARS_GUIDGEN_NATIVELIB_H__ */
