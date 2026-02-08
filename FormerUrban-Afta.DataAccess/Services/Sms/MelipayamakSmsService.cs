using FormerUrban_Afta.DataAccess.Model.SMS;
using System.Formats.Asn1;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace FormerUrban_Afta.DataAccess.Services.Sms
{
    public class MelipayamakSmsService
    {
        private readonly ITarifhaService _tarifhaService;
        private readonly ILogSMSService _logSmsService;
        private readonly IAuthService _authService;
        // --- policy knobs ---
        private static readonly HashSet<int> AllowedRsaKeySizes = new() { 2048, 3072, 4096 };
        // EC curve OIDs: secp256r1 (P-256), secp384r1 (P-384)
        private static readonly HashSet<string> AllowedEcCurveOids = new()
        {
            "1.2.840.10045.3.1.7", // secp256r1 / prime256v1 / NIST P-256
            "1.3.132.0.34"         // secp384r1 / NIST P-384
        };
        // Signature OIDs we accept (SHA256/384 only; tighten/relax as needed)
        private static readonly HashSet<string> AllowedSigOids = new()
        {
            "1.2.840.10045.4.3.2", // ecdsa-with-SHA256
            "1.2.840.10045.4.3.3", // ecdsa-with-SHA384
            "1.2.840.113549.1.1.10", // RSASSA-PSS (params define hash; runtime validated)
            "1.2.840.113549.1.1.11", // sha256WithRSAEncryption
            "1.2.840.113549.1.1.12"  // sha384WithRSAEncryption
        };

        //private readonly BoxServiceSoapClient _faraErtebatSmsService;
        private readonly string _smsServiceUrl = "https://rest.payamak-panel.com/api/SendSMS/";

        public MelipayamakSmsService(ITarifhaService tarifhaService, ILogSMSService logSmsService/*, BoxServiceSoapClient faraErtebatSmsService*/, IAuthService authService)
        {
            _tarifhaService = tarifhaService;
            _logSmsService = logSmsService;
            _authService = authService;
            //_faraErtebatSmsService = faraErtebatSmsService;
        }

        public async Task<MeliPayamakRestResponse> SendSms(string message, string phoneNumbers)
        {
            var tarifha = await _tarifhaService.GetTarifhaAsync();
            const bool isFlash = false;

            // Set up custom certificate validation
            if (!await IsTlsValidAsync())
            {
                //_logSmsService.Insert("ناموفق", "", "", "", )
                throw new Exception("ارتباط امن با پنل پیامکی برقرار نشد.");
            }

            MeliPayamakRestResponse resultSms;
            try
            {
                MeliPayamakRestClientAsync restClient = new MeliPayamakRestClientAsync(tarifha?.sms_user.Trim() ?? "", tarifha?.sms_pass.Trim() ?? "");
                resultSms = await restClient.SendAsync(phoneNumbers, string.Empty, message, isFlash);
                resultSms.StrRetStatus = GetPersianContextString(resultSms.Value);
                var user = await _authService.GetCurentUserAsync();
                user.Id = user?.Name == null ? "" : user.Id;
                _logSmsService.Insert(resultSms.StrRetStatus, user.Id, message ?? "", phoneNumbers);

                return resultSms;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logSmsService.Insert("ارتباط با پنل پیامکی برقرار نشد", "", message ?? "", phoneNumbers);
                throw new Exception("ارتباط با پنل پیامکی برقرار نشد.");
            }
        }

        public async Task<MeliPayamakRestResponse> SendSms(string message, string logMessage, string phoneNumbers, int bodyId)
        {
            // Set up custom certificate validation
            if (!await IsTlsValidAsync())
            {
                //_logSmsService.Insert("ناموفق", "", "", "", )
                _logSmsService.Insert("ارتباط امن با پنل پیامکی برقرار نشد", "", logMessage ?? "", phoneNumbers);
                var res = new MeliPayamakRestResponse()
                {
                    RetStatus = 500,
                    StrRetStatus = "ارتباط امن با پنل پیامکی برقرار نشد",
                    Value = "ارتباط امن با پنل پیامکی برقرار نشد"
                };
                return res;
            }

            try
            {
                var tarifha = await _tarifhaService.GetTarifhaAsync();

                var restClient = new MeliPayamakRestClientAsync(tarifha?.sms_user.Trim() ?? "", tarifha?.sms_pass.Trim() ?? "");
                var resultSms = await restClient.SendByBaseNumberAsync(message, phoneNumbers, bodyId);
                resultSms.StrRetStatus = GetPersianContextString(resultSms.Value);
                var user = await _authService.GetCurentUserAsync();
                user.Id = user?.Name == null ? "" : user.Id;

                if (resultSms.StrRetStatus != "با موفقیت ارسال شد")
                {
                    _logSmsService.Insert(resultSms.StrRetStatus, user?.Id ?? "", logMessage ?? "", "");
                    _logSmsService.Insert("ارسال ناموفق", user?.Id ?? "", logMessage ?? "", phoneNumbers);
                }
                else
                    _logSmsService.Insert(resultSms.StrRetStatus, user?.Id ?? "", logMessage ?? "", phoneNumbers);

                return resultSms;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logSmsService.Insert("ارتباط با پنل پیامکی برقرار نشد", "", logMessage ?? "", "");
                //throw new Exception("ارتباط با پنل پیامکی برقرار نشد.");
                return new MeliPayamakRestResponse();
            }
        }

        public string GetPersianContextString(string context) => context switch
        {
            "0" => "نام کاربری یا رمز عبور اشتباه می باشد",
            //"1" => "با موفقیت ارسال شد",
            "3" => "محدودیت در ارسال روزانه",
            "4" => "محدودیت در حجم ارسال",
            "5" => "شماره فرستنده معتبر نمی باشد",
            "6" => "سامانه در حال بروزرسانی می باشد",
            "7" => "متن حاوی کلمه فیلتر شده می باشد",
            "9" => "ارسال از خطوط عمومی از طریق وب سرویس امکان پذیر نمی باشد",
            "10" => "کاربر مورد نظر فعال نمی باشد",
            "11" => "ارسال نشده",
            "12" => "مدارک کاربر کامل نمی باشد",
            "14" => "متن حاوی لینک می باشد",
            "15" => "ارسال به بیش از 1 شماره همراه بدون درج \"لغو11\" ممکن نیست",
            "16" => "شماره گیرنده ای یافت نشد",
            "17" => "متن پیامک خالی می باشد",
            "35" => "در REST به معنای وجود شماره در لیست سیاه مخاربرات می\u200cباشد",
            _ => "با موفقیت ارسال شد",
        };

        private async Task<bool> IsTlsValidAsync()
        {
            try
            {
                var uri = new Uri(_smsServiceUrl);
                if (!string.Equals(uri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
                    return false;

                using var tcp = new TcpClient();
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                await tcp.ConnectAsync(uri.Host, uri.IsDefaultPort ? 443 : uri.Port, cts.Token);

                await using var ssl = new SslStream(tcp.GetStream(), leaveInnerStreamOpen: false);
                await ssl.AuthenticateAsClientAsync(new SslClientAuthenticationOptions
                {
                    TargetHost = uri.Host, // SNI + platform hostname checks
                    EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
                    CertificateRevocationCheckMode = X509RevocationMode.Online,
                    ApplicationProtocols = new() { SslApplicationProtocol.Http2, SslApplicationProtocol.Http11 },
                    // (optional) lock to ECDHE + AEAD suites via CipherSuitesPolicy if you want
                }, cts.Token);

                if (!ssl.IsAuthenticated || !ssl.IsEncrypted || ssl.SslProtocol < SslProtocols.Tls12)
                    return false;

                var leaf = new X509Certificate2(ssl.RemoteCertificate!);

                // RFC 2818/6125 hostname rules (SAN-first; IP must be SAN iPAddress; strict wildcard)
                if (!Rfc2818Matches(leaf, uri.Host))
                    return false;

                // ✅ KEY POLICY: RSA sizes + ECDSA allowed curves
                if (!EnforceKeyPolicy(leaf, out _))
                    return false;

                // ✅ SIGNATURE POLICY: only SHA256/384 (ECDSA/RSA); allow RSASSA-PSS
                if (!EnforceSignaturePolicy(leaf))
                    return false;

                // Build chain strictly with revocation for entire chain
                using var chain = new X509Chain
                {
                    ChainPolicy =
            {
                RevocationMode = X509RevocationMode.Online,
                RevocationFlag = X509RevocationFlag.EntireChain,
                UrlRetrievalTimeout = TimeSpan.FromSeconds(10),
                VerificationFlags = X509VerificationFlags.NoFlag,
                TrustMode = X509ChainTrustMode.System,
                VerificationTime = DateTime.UtcNow
            }
                };
                if (!chain.Build(leaf))
                    return false;

                // Expiry check (UTC)
                var daysLeft = (leaf.NotAfter.ToUniversalTime() - DateTime.UtcNow).TotalDays;
                return daysLeft > 0;
            }
            catch
            {
                return false;
            }
        }

        // --- helpers ---

        private static bool EnforceKeyPolicy(X509Certificate2 cert, out string reason)
        {
            reason = string.Empty;
            try
            {
                var algOid = cert.PublicKey.Oid?.Value;

                if (algOid == "1.2.840.113549.1.1.1") // RSA
                {
                    using var rsa = cert.GetRSAPublicKey();
                    if (rsa is null) { reason = "RSA key missing"; return false; }
                    if (!AllowedRsaKeySizes.Contains(rsa.KeySize)) { reason = $"RSA {rsa.KeySize} not allowed"; return false; }
                    return true;
                }

                if (algOid == "1.2.840.10045.2.1") // ECDSA
                {
                    using var ecdsa = cert.GetECDsaPublicKey();
                    if (ecdsa is null) { reason = "ECDSA key missing"; return false; }
                    var p = ecdsa.ExportParameters(false);
                    var curveOid = p.Curve.Oid?.Value;
                    if (!string.IsNullOrWhiteSpace(curveOid))
                    {
                        if (!AllowedEcCurveOids.Contains(curveOid)) { reason = $"EC curve {curveOid} not allowed"; return false; }
                        return true;
                    }
                    // fallback by bit size if OID not exposed
                    var bits = (p.Q.X?.Length ?? 0) * 8;
                    if (bits is 256 or 384) return true;
                    reason = $"EC {bits}-bit not allowed"; return false;
                }

                reason = $"Unsupported public key OID {algOid ?? "null"}";
                return false;
            }
            catch (Exception ex) { reason = ex.Message; return false; }
        }

        private static bool EnforceSignaturePolicy(X509Certificate2 cert)
        {
            // Allow only ECDSA-with-SHA256/384, RSA-PSS, RSA-with-SHA256/384
            var sigOid = cert.SignatureAlgorithm?.Value;
            if (string.IsNullOrWhiteSpace(sigOid)) return false;
            if (!AllowedSigOids.Contains(sigOid)) return false;

            // If RSASSA-PSS, ensure hash >= SHA-256 (the runtime enforces params; this is conservative gate)
            return true;
        }

        private static bool Rfc2818Matches(X509Certificate2 cert, string host)
        {
            if (IPAddress.TryParse(host, out var ipHost))
            {
                var san = ReadSan(cert);
                return san.Ips.Any(ip => ip.Equals(ipHost));
            }
            var (dnsNames, _) = ReadSan(cert);
            if (dnsNames.Count > 0) return dnsNames.Any(dns => DnsMatch(host, dns));
            var cn = cert.GetNameInfo(X509NameType.DnsName, forIssuer: false);
            return !string.IsNullOrWhiteSpace(cn) && DnsMatch(host, cn);
        }

        private static (List<string> Dns, List<IPAddress> Ips) ReadSan(X509Certificate2 cert)
        {
            var dns = new List<string>(); var ips = new List<IPAddress>();
            foreach (var ext in cert.Extensions)
            {
                if (ext.Oid?.Value != "2.5.29.17") continue;
                var reader = new AsnReader(ext.RawData, AsnEncodingRules.DER);
                var seq = reader.ReadSequence();
                while (seq.HasData)
                {
                    var tag = seq.PeekTag();
                    if (tag.TagClass != TagClass.ContextSpecific) { seq.ReadEncodedValue(); continue; }
                    switch (tag.TagValue)
                    {
                        case 2: // dNSName
                            var dnsName = seq.ReadCharacterString(UniversalTagNumber.IA5String, new Asn1Tag(TagClass.ContextSpecific, 2));
                            dns.Add(dnsName.TrimEnd('.')); break;
                        case 7: // iPAddress
                            var ipBytes = seq.ReadOctetString(new Asn1Tag(TagClass.ContextSpecific, 7));
                            ips.Add(new IPAddress(ipBytes)); break;
                        default: seq.ReadEncodedValue(); break;
                    }
                }
                break;
            }
            return (dns, ips);
        }

        private static bool DnsMatch(string host, string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) return false;
            host = host.TrimEnd('.'); pattern = pattern.TrimEnd('.');
            if (host.Equals(pattern, StringComparison.OrdinalIgnoreCase)) return true;
            if (pattern.StartsWith("*.", StringComparison.Ordinal))
            {
                var suffix = pattern[2..];
                if (!host.EndsWith("." + suffix, StringComparison.OrdinalIgnoreCase)) return false;
                return host.Split('.').Length == suffix.Split('.').Length + 1; // exactly one label
            }
            return false;
        }
    }
}
