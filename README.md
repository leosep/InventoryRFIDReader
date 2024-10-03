# InventoryRFIDReader

This is an example of an Inventory Management System designed to work with the **Nordic ID Merlin RFID Reader**, running a Windows CE application that interacts with a web service to manage asset data. The system performs asset lookup, barcode scanning, and asset group management via the web service.

## Project Overview

This system consists of two key components:
1. **Windows CE Application** (InventoryApp)
2. **Web Service** (Inventory Web Service)

### Windows CE Application (InventoryApp)
- A .NET Compact Framework application that runs on the **Nordic ID Merlin RFID Reader**.
- Uses barcode and RFID scanning to look up assets.
- Fetches asset details and asset groups by communicating with the web service.

### Web Service (Inventory Web Service)
- A backend service that provides asset data, authentication, and asset group management.
- Responds to requests from the Windows CE app for retrieving and updating inventory data.

## Features

- **Barcode & RFID Scanning**: Supports hardware barcode/RFID scanning via the Nordic ID Merlin.
- **Asset Lookup**: Scans barcodes or manually inputs data to retrieve asset information.
- **Asset Group Management**: Fetches available asset groups from the web service.
- **Web Service Integration**: Communicates with the web service for all data-related actions, ensuring up-to-date asset information.

## System Requirements

### Windows CE Application
- **Nordic ID Merlin RFID Reader**
- .NET Compact Framework installed on the device

### Web Service
- .NET Framework (or compatible version) for the web service
- Hosting environment for running the web service (IIS, etc.)
  
