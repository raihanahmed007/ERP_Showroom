# ERP Showroom — Architecture

## Stack
- ASP.NET Core 10.0 (Web API + Blazor WASM PWA)
- Clean Architecture (Domain ? Application ? Infrastructure ? Presentation)
- Modular Monolith with schema-per-module DB isolation

## Modules
| Module | Schema | Purpose |
|--------|--------|---------|
| sys    | [sys]  | System settings, users, roles |
| acc    | [acc]  | Chart of accounts, journal entries |
| fin    | [fin]  | Financial reporting, budgets |
| inv    | [inv]  | Products, warehouses, stock |
| prc    | [prc]  | Purchase orders, suppliers |
| crm    | [crm]  | Customers, leads, contacts |
| wrk    | [wrk]  | Workshop, service orders |
| hr     | [hr]   | Employees, departments |
| prl    | [prl]  | Payroll runs, salary slips |
| doc    | [doc]  | Document storage, OCR |
| bank   | [bank] | Bank accounts, reconciliation |
| wf     | [wf]   | Workflow engine, approvals |
