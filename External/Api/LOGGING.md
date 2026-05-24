Security notes for logs

- Log location: `logs/` by default. Move to a runtime writable directory (e.g. `%PROGRAMDATA%/Funnelica/logs` or configurable via env var) for production.
- Rotation: configured to daily files with `retainedFileCountLimit: 14`, `fileSizeLimitBytes: 10MB`, and `rollOnFileSizeLimit: true` to avoid disk exhaustion.
- Permissions: restrict access to the logs folder to the service account and administrators only.
- Sensitive data: do not log request/response bodies, authorization headers, or connection strings. Use redaction filters or a middleware layer to remove/mask keys such as `password`, `authorization`, `token`, `connectionString`, `ssn`, `creditCard` before writing logs.
- Centralized logging: if sending logs to Seq/Elasticsearch, store sink credentials in a secret store (Key Vault, AWS Secrets Manager, or env vars) and use secure TLS endpoints.
- Exception handling: avoid exposing full exception stacks to end-users; use correlation IDs and keep full traces in secured diagnostics only.
- Further hardening: consider adding `Serilog.Filters.Expressions` to drop or mask sensitive events and `Serilog.Enrichers` to add environment/correlation metadata.

Examples and middleware snippets are intentionally omitted; ask if you want them added.
