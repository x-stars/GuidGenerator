/*
 * XstarS GUID Generator native library header.
 *
 * Copyright (c) 2023 XstarS
 * This file is released under the MIT License.
 * https://opensource.org/licenses/MIT
 */

#pragma once

#ifndef __ATTR_SAL
#define _In_
#define _Out_
#endif

#ifdef _WIN32
#define DLL_IMPORT \
    __declspec(dllimport)
#else
#define DLL_IMPORT
#endif

#include <stdint.h>

#ifndef _HRESULT_DEFINED
#define _HRESULT_DEFINED
typedef int32_t HRESULT;
#endif

#ifndef GUID_DEFINED
#define GUID_DEFINED
typedef struct _GUID {
    uint32_t Data1;
    uint16_t Data2;
    uint16_t Data3;
    uint8_t  Data4[8];
} GUID;
#endif

#define UUIDREV_DISABLE

#ifdef __cplusplus
extern "C" {
#endif

typedef uint8_t DCE_SECURITY_DOMAIN;
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

DLL_IMPORT HRESULT GuidCreateV1(_Out_ GUID *Guid);
DLL_IMPORT HRESULT GuidCreateV1R(_Out_ GUID *Guid);

DLL_IMPORT HRESULT GuidCreateV2(_Out_ GUID *Guid,
    DCE_SECURITY_DOMAIN Domain);
DLL_IMPORT HRESULT GuidCreateV2Org(_Out_ GUID *Guid,
    uint32_t LocalId);
DLL_IMPORT HRESULT GuidCreateV2Other(_Out_ GUID *Guid,
    DCE_SECURITY_DOMAIN Domain, uint32_t LocalId);

DLL_IMPORT HRESULT GuidCreateV3(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const uint8_t *Name, size_t NameLen);

DLL_IMPORT HRESULT GuidCreateV4(_Out_ GUID *Guid);

DLL_IMPORT HRESULT GuidCreateV5(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const uint8_t *Name, size_t NameLen);

#ifndef UUIDREV_DISABLE
DLL_IMPORT HRESULT GuidCreateV6(_Out_ GUID *Guid);
DLL_IMPORT HRESULT GuidCreateV6P(_Out_ GUID *Guid);
DLL_IMPORT HRESULT GuidCreateV6R(_Out_ GUID *Guid);

DLL_IMPORT HRESULT GuidCreateV7(_Out_ GUID *Guid);

DLL_IMPORT HRESULT GuidCreateV8(_Out_ GUID *Guid);
DLL_IMPORT HRESULT GuidCreateV8NSha256(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const uint8_t *Name, size_t NameLen);
DLL_IMPORT HRESULT GuidCreateV8NSha384(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const uint8_t *Name, size_t NameLen);
DLL_IMPORT HRESULT GuidCreateV8NSha512(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const uint8_t *Name, size_t NameLen);
DLL_IMPORT HRESULT GuidCreateV8NSha3D256(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const uint8_t *Name, size_t NameLen);
DLL_IMPORT HRESULT GuidCreateV8NSha3D384(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const uint8_t *Name, size_t NameLen);
DLL_IMPORT HRESULT GuidCreateV8NSha3D512(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const uint8_t *Name, size_t NameLen);
DLL_IMPORT HRESULT GuidCreateV8NShake128(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const uint8_t *Name, size_t NameLen);
DLL_IMPORT HRESULT GuidCreateV8NShake256(_Out_ GUID *Guid,
    _In_ const GUID *NsId, _In_ const uint8_t *Name, size_t NameLen);
#endif

#ifdef __cplusplus
}
#endif
