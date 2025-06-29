# BrewIoT

![logo](https://github.com/user-attachments/assets/9db04f21-6970-43cb-b957-a47754ca8aa9)

This repository covers an attempt to build a fully working home brew operation. It mainly focuses on the temperature control aspect but will likely expand to cover more of the process.

## Structure

The codebase is broken down into the following sections

### `client`

This provides a .NET MAUI based client application that can connect to the server side web API.

### `device`

This provides both Meadow and Raspberry Pi based solutions as well as any common code that can be shared between them.

### `server`

This provides an ASP.NET Core website, web API and Aspire dashboard.

## Resources

Some helpful resources.

### PID Tuning

[PID Tuner](https://pidtuner.github.io/#/)
